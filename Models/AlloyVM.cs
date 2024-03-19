using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPA.Models
{
    public class AlloyVM
    {
        public int Id { get; set; }
        [Display(Name = "Material Grade / Alloy")]
        public string Material { get; set; }
        [Display(Name = "Item Code")]
        public int Code { get; set; }

    }


}
