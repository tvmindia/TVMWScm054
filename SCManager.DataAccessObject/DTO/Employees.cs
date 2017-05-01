using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Employees
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        public LogDetails logDetails { get; set; }
    }
}