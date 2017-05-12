using System;
using System.Collections.Generic;
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
}