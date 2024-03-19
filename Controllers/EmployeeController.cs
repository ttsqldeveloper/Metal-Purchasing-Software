using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPA.Contracts;

namespace MPA.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(
                ILogger<EmployeeController> logger,
                IMapper mapper,
                IUnitOfWork unitOfWork
                )
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }
        
    }
    
}
