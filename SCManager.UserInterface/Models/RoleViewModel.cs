using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class RoleViewModel
    {
        public Guid ID { get; set; }
        public string RoleName { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
    public static class RoleContants
    {
        public const string SuperAdminRole = "SA";
        public const string AdministratorRole = "Admin";
        public const string ManagerRole = "Manager";
    }
}