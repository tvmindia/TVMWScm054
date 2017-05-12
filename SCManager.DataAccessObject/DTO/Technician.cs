using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Technician
    {
    }

    public class TechnicianSummary
    {
        public string Name { get; set; }
        public int Calls { get; set; }
        public decimal StockValue { get; set; }
        public string StockValueFormatted { get; set; }
    }
}