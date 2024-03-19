using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPA.Data
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpokePerson { get; set; }
        public int PaymentTerms { get; set; }
        public bool isActive { get; set; }
        public List<Order> Orders { get; set; }
        public List<SupplierEmail> Emails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Supplier()
        {
            this.CreatedDate = DateTime.Now;
            this.UpdatedDate = null;

            
        }
    }
}
