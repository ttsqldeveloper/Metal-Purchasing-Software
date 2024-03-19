using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPA.Data
{
    public class AlloyOrder
    {
        [Key]
        public int Id { get; set; }
        public decimal PricePerMass { get; set; }
        public decimal Mass { get; set; }
        public decimal RullingLME { get; set; }
        public Status Status { get; set; }
        public string Area { get; set; }
        [ForeignKey("AlloyId")]
        public Alloy Alloy { get; set; }
        [ForeignKey("OrderNumber")]
        public Order Order { get; set; }
        
    }
}
