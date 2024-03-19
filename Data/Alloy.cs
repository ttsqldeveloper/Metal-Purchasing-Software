using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPA.Data
{
    public class Alloy
    {
        [Key]
        public int Id { get; set; }
        public string Material { get; set; }
        public int Code { get; set; }
        public List<AlloyOrder> AlloyOrders { get; set; }

    }
}
