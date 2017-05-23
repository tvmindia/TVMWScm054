using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class ServiceTypeViewModel
    {
        public string SCCode { get; set; }
        public string Code { get; set; }
        public decimal Commission { get; set; }
        public string SubType { get; set; }
        public string Description { get; set; }
    }
}