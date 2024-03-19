using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPA.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MPA.Contracts;
using MPA.Data;

namespace MPA.Controllers
{
    [Authorize]
        public class AlloysController : Controller
        {
            
        private readonly ILogger<AlloysController> _logger;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AlloysController(
                ILogger<AlloysController> logger,
                IMapper mapper,
                IUnitOfWork unitOfWork
                )
        {
            
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IActionResult> IndexAsync()
        {
            var alloys = await _unitOfWork.Alloys.FindAll();
            var model = _mapper.Map<List<Alloy>, List<AlloyVM>>(alloys.ToList());

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        //POST: Tenants/Create
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Create(AlloyVM model)
        {
            try
            {
                var alloys = _mapper.Map<Alloy>(model);
                await _unitOfWork.Alloys.Create(alloys);
                await _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("Hello World","Why");
                return View();
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var isExists = await _unitOfWork.Alloys.isExists(q => q.Id == id);

            if (!isExists)
            {
                return NotFound();
            }

            var tenant = await _unitOfWork.Alloys.Find(q => q.Id == id);
            var model = _mapper.Map<AlloyVM>(tenant);
            return View(model);
        }


            
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AlloyVM model)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var materialGrade = _mapper.Map<Alloy>(model);
                _unitOfWork.Alloys.Update(materialGrade);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong...");
                return View(model);
            }
        }


        // POST: LeaveTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var tenant = await _unitOfWork.Alloys.Find(expression: q => q.Id == id);
                if (tenant == null)
                {
                    return NotFound();
                }
                _unitOfWork.Alloys.Delete(tenant);
                await _unitOfWork.Save();

            }
            catch
            {

            }
            return RedirectToAction(nameof(Index));
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        }

}
