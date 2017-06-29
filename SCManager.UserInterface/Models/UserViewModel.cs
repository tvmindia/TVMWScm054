using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class UserViewModel
    {
        public ServiceCenterViewModel serviceCenter { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter login name")]
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }
       
        [Display(Name = "Active(Yes/No)")]
        public bool Active { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public RoleViewModel role { get; set; }
        [Required(ErrorMessage = "Please Select Service Center")]
        [Display(Name = "Service Center")]
        public string SCCode { get; set; }
        public List<SelectListItem> SCList { get; set; }


        [Required(ErrorMessage = "Please Select Role")]
        [Display(Name = "Role")]
        public string RoleList { get; set; }
        //public Guid? RoleID { get; set; }
        public List<RoleViewModel> RoleVMList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }

        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string ConfirmPassword { get; set; }
        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeDate { get; set; }
    }
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "Name")]
        public string UserName { get; set; }
    
        
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

       
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string NewPassword { get; set; }

      
        [Display(Name = "Confirm Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password and Confirmation Password must match.")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string ConfirmPassword { get; set; }

        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}