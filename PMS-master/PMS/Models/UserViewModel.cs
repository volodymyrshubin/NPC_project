using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }
        [Display(Name = "Role")]
        [Required]
        public string ApplicationRoleId { get; set; }
    }
}
