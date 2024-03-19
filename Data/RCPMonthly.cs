using System.ComponentModel.DataAnnotations;

namespace MPA.Data
{
    public class RCPMonthly
    {
        [Key]
        public int Id { get; set; }
        public string Month { get; set; }
        public decimal Value { get; set; }
        
    }
    
}
