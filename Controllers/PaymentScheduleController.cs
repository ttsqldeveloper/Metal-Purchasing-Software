using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPA.Contracts;
using MPA.Data;
using MPA.Models;

namespace MPA.Controllers
{
    [Authorize]
    public class PaymentScheduleController : Controller
    {
        private readonly ILogger<PaymentScheduleController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public PaymentScheduleController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<PaymentScheduleController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var receivedOrders = await _unitOfWork.ReceivedOrders.FindAll(includes: q => q.Include(x => x.Order).ThenInclude(x => x.Supplier));

            var model = new PaymentSchedulePageVM
            {
                ReceivedOrders = _mapper.Map<List<ReceivedOrder>, List<ReceivedOrderVM>>(receivedOrders.ToList()),
            };
            return View(model);
        }
    }

}
