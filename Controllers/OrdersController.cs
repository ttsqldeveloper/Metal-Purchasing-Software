using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Utils;
using MPA.Contracts;
using MPA.Data;
using MPA.Models;
using MPA.Services;
using MPA.Utility;

namespace MPA.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public OrdersController(
                ILogger<OrdersController> logger,
                IUnitOfWork unitOfWork,
                IEmailSender emailSender,
                IMapper mapper
                )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _mapper = mapper;

        }

        public async Task<IActionResult> IndexAsync()
        {

            var orders = await _unitOfWork.Orders.FindAll(
                    includes: q => q.Include(x => x.Supplier)
                    );

            var rOrders = await _unitOfWork.ReceivedOrders.FindAll(
                    includes: q => q.Include(x => x.Order)
                    );

            // var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
            //         hhh: q => q.Include(x => x.Order)

            List<OrderVM> allOrders = new List<OrderVM>();
            List<OrderVM> openOrders = new List<OrderVM>();
            List<OrderVM> incompleteOrders = new List<OrderVM>();

            for (int i = 0; i < orders.Count(); i++)
            {
                var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
                        q => q.Order.OrderNumber == orders[i].OrderNumber,
                        includes: q => q.Include(x => x.Order)
                        );

                decimal totalMass = 0;

                for (int j = 0; j < alloyOrders.Count(); j++)
                {
                    totalMass = totalMass + alloyOrders[j].Mass;
                }

                var order = new OrderVM
                {
                    AlloyOrders = _mapper.Map<List<AlloyOrderVM>>(alloyOrders),
                    Supplier = orders[i].Supplier,
                    PaymentTerms = orders[i].PaymentTerms,
                    Date = orders[i].Date,
                    ExpectedDate = orders[i].ExpectedDate,
                    LMERule = orders[i].LMERule,
                    OrderNumber = orders[i].OrderNumber,
                    NumberOfAlloy = alloyOrders.Count(),
                    TotalMass = totalMass,
                    Status = orders[i].Status
                };

                allOrders.Add(order);

                if (order.Status == Status.Not_Received)
                    openOrders.Add(order);

                if (order.Status == Status.Incomplete)
                    incompleteOrders.Add(order);

            }

            var finalModel = new OrderPageVM
            {
                AllOrders = allOrders,
                OpenOrders = openOrders,
                Incomplete = incompleteOrders
            };

            return View(finalModel);
        }

        //get
        public async Task<IActionResult> Create()
        {
            var suppliers = await _unitOfWork.Suppliers.FindAll();
            var alloys = await _unitOfWork.Alloys.FindAll();

            var suppliersOptionList = suppliers.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var alloyOptionList = alloys.Select(q => new SelectListItem
            {
                Text = $"{q.Material} ({q.Code})",
                Value = q.Id.ToString()
            });

            var model = new CreateOrderPageVM
            {
                SupplierList = suppliersOptionList,
                AlloyList = alloyOptionList,
                AlloyOrderList = new List<AlloyOrderVM>() { }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder(CreateOrderPageVM modelVM)
        {
            try
            {
                var alloyList = new List<AlloyOrderVM>();
                for (int i = 0; i < modelVM.AlloyOrderList.Count(); i++)
                {
                    //NOTE: WARN: this is the bad way to do this... hope in the future I think of a better way.
                    // NOTE: 2022-05-11 Wed 08:36 AM 
                    var selectedAlloy = await _unitOfWork.Alloys.Find(q => q.Id == modelVM.AlloyOrderList[i].AlloyId);

                    var alloyOrder = new AlloyOrderVM
                    {
                        AlloyId = modelVM.AlloyOrderList[i].AlloyId,
                        Mass = modelVM.AlloyOrderList[i].Mass * 1000,
                        RullingLME = modelVM.AlloyOrderList[i].RullingLME,
                        Area = modelVM.AlloyOrderList[i].Area,
                        PricePerMass = modelVM.AlloyOrderList[i].PricePerMass,
                        AlloyName = selectedAlloy.Material
                    };

                    alloyList.Add(alloyOrder);
                }

                //NOTE: WARN: this is the bad way to do this... hope in the future I think of a better way.
                var selectedSupplier = await _unitOfWork.Suppliers.Find(q => q.Id == modelVM.OrderVM.SupplierId);

                if (!modelVM.OrderVM.isCustomPaymentTerms)
                    modelVM.OrderVM.PaymentTerms = selectedSupplier.PaymentTerms;

                // modelVM.OrderVM.PaymentTerms = selectedSupplier.PaymentTerms

                var model = new CreateOrderPageVM
                {
                    OrderVM = modelVM.OrderVM,
                    AlloyOrderList = alloyList,
                    tempSupplierName = selectedSupplier.Name
                };

                return View("OverView", model);

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCreate(CreateOrderPageVM modelVM)
        {
            var alloyList = new List<AlloyOrderVM>();
            var suppliers = await _unitOfWork.Suppliers.FindAll();
            var alloys = await _unitOfWork.Alloys.FindAll();

            var suppliersOptionList = suppliers.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var alloyOptionList = alloys.Select(q => new SelectListItem
            {
                Text = $"{q.Material} ({q.Code})",
                Value = q.Id.ToString()
            });


            for (int i = 0; i < modelVM.AlloyOrderList.Count(); i++)
            {
                //NOTE: WARN: this is the bad way to do this... hope in the future I think of a better way.
                // NOTE: 2022-05-11 Wed 08:36 AM 
                var selectedAlloy = await _unitOfWork.Alloys.Find(q => q.Id == modelVM.AlloyOrderList[i].AlloyId);

                var alloyOrder = new AlloyOrderVM
                {
                    AlloyId = modelVM.AlloyOrderList[i].AlloyId,
                    Mass = modelVM.AlloyOrderList[i].Mass / 1000,
                    RullingLME = modelVM.AlloyOrderList[i].RullingLME,
                    Area = modelVM.AlloyOrderList[i].Area,
                    PricePerMass = modelVM.AlloyOrderList[i].PricePerMass,
                    AlloyName = selectedAlloy.Material
                };

                alloyList.Add(alloyOrder);
            }

            //NOTE: WARN: this is the bad way to do this... hope in the future I think of a better way.
            // 2022-05-11 Wed 10:37 AM Rememe not to call database from this side because int is cost
            var selectedSupplier = await _unitOfWork.Suppliers.Find(q => q.Id == modelVM.OrderVM.SupplierId);

            // modelVM.OrderVM.Date = System.DateTime.Now;

            var model = new CreateOrderPageVM
            {
                OrderVM = modelVM.OrderVM,
                AlloyOrderList = alloyList,
                SupplierList = suppliersOptionList,
                AlloyList = alloyOptionList,
                tempSupplierName = selectedSupplier.Name
            };
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(OrderVM model)
        {
            var suppliers = await _unitOfWork.Suppliers.FindAll();
            var materialGrade = await _unitOfWork.Alloys.FindAll();

            var suppliersOptionList = suppliers.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var materialGradeOptionList = materialGrade.Select(q => new SelectListItem
            {
                // Text = q.Name + " (" + q.ItemCode + ")",
                Value = q.Id.ToString()
            });

            var order = await _unitOfWork.Orders.Find(
                    q => q.OrderNumber == model.OrderNumber,
                    includes: q => q.Include(x => x.Supplier)
                    );

            var modelNew = new OrderVM
            {
                OrderNumber = order.OrderNumber,
                PaymentTerms = order.PaymentTerms,
                LMERule = order.LMERule,
                Supplier = order.Supplier
            };

            return View(modelNew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder(CreateOrderPageVM model)
        {
            //1: Saving The Order Deatals
            var supplier = await _unitOfWork.Suppliers.Find(q => q.Id == model.OrderVM.SupplierId);
            var order = new Order
            {
                Status = Status.Not_Received,
                LMERule = model.OrderVM.LMERule,
                Supplier = supplier,
                PaymentTerms = model.OrderVM.PaymentTerms,
                ExpectedDate = model.OrderVM.ExpectedDate,
                isCustomPaymentTerms = model.OrderVM.isCustomPaymentTerms,
            };
            await _unitOfWork.Orders.Create(order);
            await _unitOfWork.Save();

            var emails = await _unitOfWork.SupplierEmails.FindAll(q => q.Supplier.Id == supplier.Id);

            for (int i = 0; i < model.AlloyOrderList.Count(); i++)
            {
                var alloy = await _unitOfWork.Alloys.Find(q => q.Id == model.AlloyOrderList[i].AlloyId);
                var alloyOrder = new AlloyOrder
                {
                    Order = order,
                    Alloy = alloy,
                    Mass = model.AlloyOrderList[i].Mass,
                    RullingLME = model.AlloyOrderList[i].RullingLME,
                    Area = model.AlloyOrderList[i].Area,
                    PricePerMass = model.AlloyOrderList[i].PricePerMass,
                    Status = Status.Not_Received
                };

                await _unitOfWork.AlloyOrders.Create(alloyOrder);
                await _unitOfWork.Save();
            }

            //2: Query Database for the information i need to send an Email
            var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
                    q => q.Order.OrderNumber == order.OrderNumber,
                    includes: q => q.Include(x => x.Alloy)
                    );

            //3: Send an Email

            var alloysMessage = "";
            var alloysNames = "";

            for (int i = 0; i < alloyOrders.Count(); i++)
            {
                alloysMessage = alloysMessage + $@"
                        Material Grade:             { alloyOrders[i].Alloy.Material } <br /> Brass
                        Material Code:              { alloyOrders[i].Alloy.Code } <br />
                        Order Mass:                 { String.Format("{0:n2}", alloyOrders[i].Mass) } Kg  <br />
                        Price Per Ton (Excl VAT):   <b> R { String.Format("{0:n2}", alloyOrders[i].PricePerMass) } </b>
                        <br /> ----------------------------------------------------------------------- <br />
                        ";
                alloysNames = alloysNames + $" {alloyOrders[i].Alloy.Material},";
            }

            InternetAddressList list = new InternetAddressList();
            //
            // add emails to emails
            for (int i = 0; i < emails.Count; i++)
            {
                list.Add(MailboxAddress.Parse(emails[i].Email));
            }
            list.Add(MailboxAddress.Parse("benson@copalcor.co.za"));
            list.Add(MailboxAddress.Parse("hendry@copalcor.co.za"));
            list.Add(MailboxAddress.Parse("Thembisile@copalcor.co.za "));
            // list.Add(MailboxAddress.Parse("metalpurchasing@copalcor.co.za"));


            try
            {
                // Send Email to supervisor and requesting user
                await _emailSender.SendEmailAsync(list, $" [Testing] Order Placement Confirmation [ { order.OrderNumber } ]",
                        $@"
                        Hello {order.Supplier.SpokePerson} <br />
                        <br />
                        Please see below the order confirmation as per our earlier discussion.
                        <br />

                        Supplier:                   { order.Supplier.Name } <br />
                        Order No:                   { order.OrderNumber } <br />
                        Date of order:              { order.Date.ToString("dd MMMM yyyy") } <br /> June 2022
                        Delivery Date Required:     <b> { order.ExpectedDate.ToString("dd MMMM yyyy") }</b> <br />
                        Payment Terms:              <b> { order.PaymentTerms }  Days Delivery </b> Days Delivery
                        <br /> ----------------------------------------------------------------------- <br />

                        { alloysMessage }

                        <br />
                        <br />

                        ");

            }
            catch (System.Exception)
            {
                return View("EmailErro");
            }

            var modelv = new CreateSupplierPageVM
            {
                OrderVM = _mapper.Map<OrderVM>(order),
                // EmailList = _mapper.Map<List<EmailItemVM>>(emails)
            };

            return View(modelv);


        }

        public IActionResult ReceivedOrder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReceivedOrder(ReceivedPageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // var isExists = await _unitOfWork.Orders.isExists(q => q.OrderNumber == model.OrderNumber);
            var order = await _unitOfWork.Orders.Find(
                    q => q.OrderNumber == model.OrderNumber,
                    includes: q => q.Include(x => x.Supplier)
                    );

            if (order == null)
            {
                return View();
            }

            var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
                    q => q.Order.OrderNumber == model.OrderNumber,
                    includes: q => q.Include(x => x.Alloy)
                    );

            var alloyList = new List<AlloyOrderVM>();

            for (int i = 0; i < alloyOrders.Count(); i++)
            {
                decimal receivedMass = 0;
                var receivedAlloyOrder = await _unitOfWork.ReceivedAlloyOrder.FindAll(
                        q => q.AlloyOrder.Id == alloyOrders[i].Id
                        );

                // Accumalation of received mass
                for (int j = 0; j < receivedAlloyOrder.Count(); j++)
                    receivedMass += receivedAlloyOrder[j].Mass;

                var alloyOrderVM = new AlloyOrderVM
                {
                    Id = alloyOrders[i].Id,
                    PricePerMass = alloyOrders[i].PricePerMass,
                    Mass = alloyOrders[i].Mass,
                    RullingLME = alloyOrders[i].RullingLME,
                    Area = alloyOrders[i].Area,
                    Alloy = alloyOrders[i].Alloy,
                    Status = alloyOrders[i].Status
                };

                // under delivered
                alloyOrderVM.OutstandingMass = alloyOrders[i].Mass - receivedMass;
                alloyList.Add(alloyOrderVM);
            }

            var modelv = new ViewRecievePageVM
            {
                OrderNumber = model.OrderNumber,
                OrderVM = _mapper.Map<OrderVM>(order),
                AlloyOrderList = _mapper.Map<List<AlloyOrderVM>>(alloyList)
            };

            return View("ViewRecieve", modelv);
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            // 1. save data about the received order
            var order = await _unitOfWork.Orders.Find(
                    q => q.OrderNumber == id,
                    includes: q => q.Include(x => x.Supplier)
                    );

            var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
                    q => q.Order.OrderNumber == order.OrderNumber,
                    includes: q => q.Include(x => x.Alloy)
                    );

            var receivedOrder = await _unitOfWork.ReceivedOrders.FindAll(
                    q => q.Order.OrderNumber == id
                    );

            List<ReceivedAlloyOrder> receivedAlloyOrderList = new List<ReceivedAlloyOrder>();

            for (int i = 0; i < receivedOrder.Count; i++)
            {
                var receiveds = await _unitOfWork.ReceivedAlloyOrder.FindAll(
                        q => q.ReceivedOrder.Id == receivedOrder[i].Id,
                        includes: q => q.Include(x => x.AlloyOrder)
                        .Include(x => x.ReceivedOrder)
                        );

                for (int j = 0; j < receiveds.Count; j++)
                {
                    receivedAlloyOrderList.Add(receiveds[j]);
                }

            }

            var model = new CreateOrderPageVM
            {
                ReceivedAlloyOrderVMs = _mapper.Map<List<ReceivedAlloyOrderVM>>(receivedAlloyOrderList),
                OrderVM = _mapper.Map<OrderVM>(order),
                // SupplierList = suppliersOptionList,
                // AlloyList = alloyOptionList,
                AlloyOrderList = _mapper.Map<List<AlloyOrderVM>>(alloyOrders),
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReceivedOrderAsync(ViewRecievePageVM model)
        {
            // 1. save data about the received order
            var order = await _unitOfWork.Orders.Find(
                    q => q.OrderNumber == model.OrderVM.OrderNumber,
                    includes: q => q.Include(x => x.Supplier)
                    );

            model.ReceivedOrderVM.Order = order;
            model.ReceivedOrderVM.Date = DateTime.Now;
            var receivedOrder = _mapper.Map<ReceivedOrder>(model.ReceivedOrderVM);
            await _unitOfWork.ReceivedOrders.Create(receivedOrder);
            await _unitOfWork.Save();

            // Expected Date: { model.OrderVM.Status }
            // isCustomPaymentTerms: { model.OrderVM.isCustomPaymentTerms }
            // Supplier Name: { model.OrderVM.Supplier.Name }
            // LME Rule: { model.OrderVM.LMERule }

            // save received mass detalis to the database while drafting the message
            // that will be sent via emails
            var receivedAlloyMessage = "";
            var receivedAlloysNames = "";
            decimal totalPrice = 0;

            for (int i = 0; i < model.ReceivedAlloyOrderList.Count(); i++)
            {

                var alloyOrder = await _unitOfWork.AlloyOrders.Find(
                        q => q.Id == model.ReceivedAlloyOrderList[i].AlloyOrderId,
                        includes: q => q.Include(x => x.Alloy)
                        );

                model.ReceivedAlloyOrderList[i].ReceivedOrder = receivedOrder;
                model.ReceivedAlloyOrderList[i].AlloyOrder = alloyOrder;
                model.ReceivedAlloyOrderList[i].Mass *= 1000;
                model.ReceivedAlloyOrderList[i].SupplierMass *= 1000;
                model.ReceivedAlloyOrderList[i].ReturnedMass *= 1000;

                var receivedAlloyOrder = _mapper.Map<ReceivedAlloyOrder>(model.ReceivedAlloyOrderList[i]);
                await _unitOfWork.ReceivedAlloyOrder.Create(receivedAlloyOrder);
                await _unitOfWork.Save();

                decimal totalReceivedMasss = 0;
                var receivedAlloyOrders = await _unitOfWork.ReceivedAlloyOrder.FindAll(q => q.AlloyOrder.Id == alloyOrder.Id);
                for (int j = 0; j < receivedAlloyOrders.Count; j++)
                {
                    totalReceivedMasss += receivedAlloyOrders[j].Mass;
                }

                alloyOrder.Status = Helper.GetAlloyOrderStatus(alloyOrder.Mass, totalReceivedMasss);
                _unitOfWork.AlloyOrders.Update(alloyOrder);
                await _unitOfWork.Save();

                // Calculate the amount that needs to be paid
                totalPrice = totalPrice + (alloyOrder.PricePerMass * Math.Abs(receivedAlloyOrder.Mass - receivedAlloyOrder.ReturnedMass));

                receivedAlloyMessage = receivedAlloyMessage + $@"

                        Material Grade:              { alloyOrder.Alloy.Material  } <br /> Brass
                        Item Code:                   { alloyOrder.Alloy.Code } <br />
                        Supplier Mass:               { String.Format("{0:n2}", receivedAlloyOrder.SupplierMass) } Kg <br />
                        Copalcor Mass :              { String.Format("{0:n2}", receivedAlloyOrder.Mass + receivedAlloyOrder.ReturnedMass) } Kg <br /> Kg
                        Returned Mass:               { String.Format("{0:n2}", receivedAlloyOrder.ReturnedMass) } Kg <br /> Kg
                        Weigh Difference:            { String.Format("{0:n2}", Math.Abs(receivedAlloyOrder.SupplierMass - receivedAlloyOrder.Mass)) } Kg <br /> Kg
                        Mass Booked in:              { String.Format("{0:n2}", Math.Abs(receivedAlloyOrder.Mass - receivedAlloyOrder.ReturnedMass)) } Kg <br /> Kg
                        Mass Balance:                { String.Format("{0:n2}", Math.Abs(alloyOrder.Mass - receivedAlloyOrder.Mass)) } Kg <br /> Kg
                        Price Per Ton (Excl VAT):  R { String.Format("{0:n2}", alloyOrder.PricePerMass) }
                        <br /> ----------------------------------------------------------------------- <br />
                        ";

            }

            var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
                    q => q.Order.OrderNumber == order.OrderNumber
                    );

            order.Status = Helper.GetOrderStatus(alloyOrders);
            _unitOfWork.Orders.Update(order);
            await _unitOfWork.Save();

            var emails = await _unitOfWork.SupplierEmails.FindAll(q => q.Supplier.Id == order.Supplier.Id);

            InternetAddressList list = new InternetAddressList();

            // add emails to emails
            for (int i = 0; i < emails.Count; i++)
            {
                list.Add(MailboxAddress.Parse(emails[i].Email));
            }
            list.Add(MailboxAddress.Parse("benson@copalcor.co.za"));
            list.Add(MailboxAddress.Parse("hendry@copalcor.co.za"));
            list.Add(MailboxAddress.Parse("Thembisile@copalcor.co.za "));

            try
            {
                // Send Email to supervisor and requesting user
                await _emailSender.SendEmailAsync(list, $" [Testing] Order Receeived Confirmation [ { order.OrderNumber  } ]",
                        $@"
                        Hello
                        Please see below the order received as per our earlier discussion.
                        <br />

                        Supplier:           { order.Supplier.Name } <br />
                        Order No:           { order.OrderNumber } <br />
                        Date Received:      { order.Date.ToString("dd MMMM yyyy") } <br />
                        Payment Terms:      { order.PaymentTerms } Days Delivery <br />
                        Truck Registration: { receivedOrder.TruckRegistration.ToUpper() } <br />
                        Total Price:        <b> { String.Format("{0:n2}", (totalPrice) / 1000) } </b>
                        Payment Date:       { receivedOrder.Date.AddDays(order.PaymentTerms).ToString("dd MMMM yyyy") } <br />
                        Delivery Notes:     { receivedOrder.Note }
                        <br /> ----------------------------------------------------------------------- <br />

                        { receivedAlloyMessage }

                        <br />
                        ");

            }
            catch (System.Exception)
            {
                return View("EmailErro");
            }

            var modelv = new CreateSupplierPageVM
            {
                OrderVM = _mapper.Map<OrderVM>(order),
                // EmailList = _mapper.Map<List<EmailItemVM>>(emails)
            };

            return View(modelv);
        }

        public IActionResult EditOrderPrice(string parameter)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrderPrice(ReceivedPageVM model)
        {
            // if (!ModelState.IsValid)
            // {
            //     return View();
            // }

            // // var isExists = await _unitOfWork.Orders.isExists(q => q.OrderNumber == model.OrderNumber);
            // var order = await _unitOfWork.Orders.Find(
            //         q => q.OrderNumber == model.OrderNumber,
            //         includes: q => q.Include(x => x.Supplier)
            //         );

            // if (order == null)
            // {
            //     return View();
            // }

            // var alloyOrders = await _unitOfWork.AlloyOrders.FindAll(
            //         q => q.Order.OrderNumber == model.OrderNumber,
            //         includes: q => q.Include(x => x.Alloy)
            //         );

            System.Console.WriteLine("Are you in");

            return View("ViewEditOrderPrice");
        }

    }



}
