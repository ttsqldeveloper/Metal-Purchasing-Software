using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPA.Data
{
    public class SupplierEmail
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public Boolean isMain { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
