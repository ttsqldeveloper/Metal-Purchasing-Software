using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MPA.Models
{
    public class CreateOrderPageVM
    {
        public OrderVM OrderVM { get; set; }
        [Display(Name="Alloy")]
        public AlloyOrderVM AlloyOrderVM { get; set; }
        public IEnumerable<SelectListItem> SupplierList { get; set; }
        public IEnumerable<SelectListItem> AlloyList { get; set; }

        public string tempSupplierName { get; set; }
        public string tempAlloy { get; set; }
        public AlloyVM AlloyVM { get; set; }

        public List<AlloyOrderVM> AlloyOrderList { get; set; }
        public List<ReceivedAlloyOrderVM> ReceivedAlloyOrderVMs { get; set; }

        public CreateOrderPageVM()
        {
            this.AlloyOrderList = new List<AlloyOrderVM>();
        }
    }
    public class ViewRecievePageVM
    {
        public int OrderNumber { get; set; }
        public OrderVM OrderVM { get; set; }
        public AlloyOrderVM AlloyOrderVM { get; set; }
        public ReceivedOrderVM ReceivedOrderVM { get; set; }
        public ReceivedAlloyOrderVM ReceivedAlloyOrderVM { get; set; }
        public List<AlloyOrderVM> AlloyOrderList { get; set; }
        public List<ReceivedAlloyOrderVM> ReceivedAlloyOrderList { get; set; }

        public ViewRecievePageVM()
        {
            this.AlloyOrderList = new List<AlloyOrderVM>();
        }

    }

    public class OrderPageVM
    {
        [Display(Name="#Order")]
        public List<OrderVM> AllOrders { get; set; }
        public List<OrderVM> OpenOrders { get; set; }
        public List<OrderVM> Incomplete { get; set; }
    }


    public class ReceivedPageVM
    {
        [Required]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }
    }
    public class PaymentSchedulePageVM
    {
        public List<ReceivedOrderVM> ReceivedOrders { get; set; }
    }
}
