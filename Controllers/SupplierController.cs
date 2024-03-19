using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MPA.Contracts;
using MPA.Data;
using MPA.Models;

namespace MPA.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public SupplierController(
                ILogger<SupplierController> logger,
                IMapper mapper,
                IUnitOfWork unitOfWork
                )
        {

            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _unitOfWork.Suppliers.FindAll(q => q.isActive == true);
            var model = _mapper.Map<List<Supplier>, List<SupplierVM>>(suppliers.ToList());
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreateSupplierPageVM model)
        {
            try
            {

                var supplier = await _unitOfWork.Suppliers.Find(q => q.Name == model.SupplierVM.Name);

                if (supplier != null)
                    return View();

                supplier = new Supplier
                {
                    Name = model.SupplierVM.Name,
                    SpokePerson = model.SupplierVM.SpokePerson,
                    isActive = true,
                    PaymentTerms = model.SupplierVM.PaymentTerms
                };

                await _unitOfWork.Suppliers.Create(supplier);
                await _unitOfWork.Save();

                List<string> emails = model.Emails.Split(",").ToList();

                for (int i = 0; i < emails.Count; i++)
                {
                    var email = await _unitOfWork.SupplierEmails.Find(q => q.Email == emails[i]);
                    if (email != null)
                        return View();
                    email = new SupplierEmail
                    {
                        Supplier = supplier,
                        Email = emails[i],
                        isMain = i == 0 ? true : false
                    };
                    await _unitOfWork.SupplierEmails.Create(email);
                    await _unitOfWork.Save();
                }

                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var isExists = await _unitOfWork.Suppliers.isExists(q => q.Id == id);
            if (!isExists)
            {
                return NotFound();
            }

            var supplier = await _unitOfWork.Suppliers.Find(q => q.Id == id);
            var emails = await _unitOfWork.SupplierEmails.FindAll(q => q.Supplier.Id == supplier.Id);

            var model = new CreateSupplierPageVM
            {
                SupplierVM = _mapper.Map<SupplierVM>(supplier),
                Emails = string.Join(',', emails),

            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateSupplierPageVM model)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var supplierModel = new SupplierVM
                {
                    Id = model.SupplierVM.Id,
                    Name = model.SupplierVM.Name,
                    SpokePerson = model.SupplierVM.SpokePerson
                };

                var supplier = _mapper.Map<Supplier>(supplierModel);
                _unitOfWork.Suppliers.Update(supplier);
                await _unitOfWork.Save();

                return RedirectToAction("Index");

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
                var supplier = await _unitOfWork.Suppliers.Find(expression: q => q.Id == id);
                if (supplier == null)
                {
                    return NotFound();
                }
                supplier.isActive = false;

                _unitOfWork.Suppliers.Update(supplier);
                await _unitOfWork.Save();

            }
            catch
            {
                throw;

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(int id)
        {

            var supplierEmails = await _unitOfWork.SupplierEmails.FindAll(q => q.Supplier.Id == id);

            // var emails = supplierEmails.Where(q => q.Supplier.Id == id).ToList();

            var model = _mapper.Map<List<SupplierEmail>, List<EmailItemVM>>(supplierEmails.ToList());

            return View(model);
        }
    }

}
