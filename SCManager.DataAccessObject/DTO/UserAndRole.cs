using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class User
    {
        public ServiceCenter serviceCenter { get; set; }
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string RoleList { get; set; }
        public string[] Roles { get; set; }
        public Role role { get; set; }
        public LogDetails logDetails { get; set; }
    }
    public class Role
    {
        public Guid ID { get; set; }
        public string RoleName { get; set; }
        public LogDetails logDetails { get; set; }
    }
}