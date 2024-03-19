using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPA.Data
{
    public class ReceivedOrder
    {
        [Key]
        public int Id { get; set; }
        public string TruckRegistration { get; set; }
        public string MaterialPacked { get; set; }
        public string CopalcorGin { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public Boolean isPaymentAproved { get; set; }
        [ForeignKey("OrderNumber")]
        public Order Order { get; set; }

        public ReceivedOrder()
        {
            this.Date = DateTime.Now;
            
        }
    }
}
