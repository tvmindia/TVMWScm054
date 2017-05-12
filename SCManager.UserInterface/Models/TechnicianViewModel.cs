using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class TechnicianViewModel
    {
        public List<TechnicianSummaryViewModel> TechnicianSummaryViewModel { get; set; }
    }
    public class TechnicianSummaryViewModel
    {
        public string Name { get; set; }
        public int Calls { get; set; }
        public decimal StockValue { get; set; }
        public string StockValueFormatted { get; set; }
    }
}