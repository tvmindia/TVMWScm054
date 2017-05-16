using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCManager.UserInterface.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter login name")]
        //[Display(Name = "Login Name")]
        [StringLength(250)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        //[Display(Name = "Password")]
        [StringLength(250)]
        public string Password { get; set; }

        ////[Required(ErrorMessage = "Confirmation Password is required.")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        //public string ConfirmPassword { get; set; }
        public bool IsFailure { get; set; }
    }
}