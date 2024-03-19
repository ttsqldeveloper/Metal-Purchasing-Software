using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MPA.Data;

namespace MPA.Models
{
    public class OrderVM
    {
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Expected Date")]
        public DateTime ExpectedDate { get; set; }
        [Display(Name = "Date")]
        public Status Status { get; set; }
        [Display(Name = "Payment Terms")]
        public int PaymentTerms { get; set; }
        [Display(Name = "Copalcor Gin")]
        public int CopalcorGin { get; set; }
        [Display(Name = "LME Rule")]
        public string LMERule { get; set; }
        [Display(Name = "Supplier")]
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
        [Display(Name = "Received Orders")]
        public List<ReceivedOrder> ReceivedOrders { get; set; }
        [Display(Name = "Alloys")]
        public List<AlloyOrderVM> AlloyOrders { get; set; }
        public int NumberOfAlloy { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal TotalMass { get; set; }

        [Display(Name = "Payment Plan")]
        public bool isCustomPaymentTerms { get; set; }
    }

    public class ReceivedOrderVM
    {
        public int Id { get; set; }
        [Display(Name = "Truck Registration")]
        public string TruckRegistration { get; set; }
        [Display(Name = "Material Packed")]
        public string MaterialPacked { get; set; }
        [Display(Name = "Delivery Note")]
        public string Note { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }
        public Order Order { get; set; }
        [Display(Name = "Copalcor Gin")]
        public string CopalcorGin { get; set; }
    }

    public class AlloyOrderVM
    {
        public int Id { get; set; }
        [Display(Name = "Price Per Mass"),
        DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal PricePerMass { get; set; }
        // [Display(Name = "Mass (Kg)")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true),
        Display(Name = "Mass (Tons)")]
        public decimal Mass { get; set; }
        [Display(Name = "Rulling LME"), DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal RullingLME { get; set; }
        [Display(Name = "Which Area for this alloy?")]
        public string Area { get; set; }


        [Display(Name = "Outstanding Mass")]
        public decimal OutstandingMass { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Alloy")]
        public Alloy Alloy { get; set; }
        public int AlloyId { get; set; }

        [Display(Name = "Order")]
        public int OrderNumber { get; set; }
        public Order Order { get; set; }

        public string AlloyName { get; set; }


        public IEnumerable<SelectListItem> AlloyList { get; set; }

    }

    public class ReceivedAlloyOrderVM
    {
        public int Id { get; set; }
        // Measured by the Supplier | claimed mass

        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true),
        Display(Name = "Supplier Mass (Tons)")]
        public decimal SupplierMass { get; set; }
        // Measured at Copalcor Weight bridge
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true),
        Display(Name = "Copalcor Mass (Tons)")]
        public decimal Mass { get; set; }
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true),
        Display(Name = "Mass Returned (Tons)")]
        public decimal ReturnedMass { get; set; }

        [Display(Name = "AlloyOrder")]
        public int AlloyOrderId { get; set; }
        public AlloyOrder AlloyOrder { get; set; }

        [Display(Name = "ReceivedOrderId")]
        public ReceivedOrder ReceivedOrder { get; set; }
    }

}
