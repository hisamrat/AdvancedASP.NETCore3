using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvancedASP.NETCore3.Models
{
   public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Category Name")]
        public string Name { get; set; }
        public string DisplayOrder { get; set; }
    }
}
