using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Display(Name = "Project Name")]
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Year { get; set; }
        [Display(Name = "Project Type")]
        [Required]
        public string ProjectType { get; set; }
        [Display(Name = "Finished")]
        [Required]
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        [NotMapped]
        public List<SelectListItem> drpStudents { get; set; }
        [Display(Name = "Select Students")]
        [Required]
        [NotMapped]
        public long[] StudentIds { get; set; }
    }
}
