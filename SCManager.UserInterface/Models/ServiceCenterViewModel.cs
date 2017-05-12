using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class ServiceCenterViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}