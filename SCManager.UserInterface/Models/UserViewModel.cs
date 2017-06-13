using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class UserViewModel
    {
        public ServiceCenterViewModel serviceCenter { get; set; }
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string RoleList { get; set; }
        public string[] Roles { get; set; }
        public RoleViewModel role { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "Name")]
        public string UserName { get; set; }
    
        
        [Display(Name = "Current Password")]
       
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string CurrentPassword { get; set; }

       
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string NewPassword { get; set; }

      
        [Display(Name = "Confirm Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirmation Password must match.")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string ConfirmPassword { get; set; }

        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}