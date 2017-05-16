using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class ServiceCenter
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public LogDetails logDetails { get; set; }
    }
}