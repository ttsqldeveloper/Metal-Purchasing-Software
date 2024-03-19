using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPA.Data
{
    public class ReceivedAlloyOrder
    {
        [Key]
        public int Id { get; set; }
        // Measured by the Supplier | claimed mass
        public decimal SupplierMass { get; set; }
        // Measured at Copalcor Weight bridge
        public decimal Mass { get; set; }
        public decimal ReturnedMass { get; set; }
        [ForeignKey("AlloyOrderId")]
        public AlloyOrder AlloyOrder { get; set; }
        [ForeignKey("ReceivedOrderId")]
        public ReceivedOrder ReceivedOrder { get; set; }
    }
    
}
