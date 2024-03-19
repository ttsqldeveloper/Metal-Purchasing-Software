using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPA.Data
{
   public  enum Status
    {
        Not_Received,
        Incomplete,
        Complete,
        Over_Delivered
    }

    public class Order
    {
        [Key]
        public int OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpectedDate { get; set; }
        public Status Status { get; set; }
        public int PaymentTerms { get; set; }
        public bool isCustomPaymentTerms { get; set; }
        public string LMERule { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public List<ReceivedOrder> ReceivedOrders { get; set; }
        public List<AlloyOrder> AlloyOrders { get; set; }

        public Order()
        {
            this.Date = DateTime.Now;

        }

    }
}
