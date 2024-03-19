using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MPA.Data;

namespace MPA.Models
{
    public class SupplierVM
    {
        [Display(Name = "Supplier")]
        public int Id { get; set; }
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }
        [Display(Name = "Spoke Person")]
        public string SpokePerson { get; set; }
        [Display(Name = "Payment Terms")]
        public int PaymentTerms { get; set; }
    }
    
    public class EmailItemVM
    {
        [Display(Name = "Email")]
        public string Email { get; set; }
        public Boolean isMain { get; set; }
        public Supplier Supplier { get; set; }
    }

    public class CreateSupplierPageVM
    {
        public EmailItemVM EmailItemVM { get; set; }
        public SupplierVM SupplierVM { get; set; }
        public OrderVM OrderVM { get; set; }
        public string Emails { get; set; }

        public List<EmailItemVM> EmailList { get; set; }

        public CreateSupplierPageVM()
        {
            this.EmailList = new List<EmailItemVM>();
        }

    }
}
