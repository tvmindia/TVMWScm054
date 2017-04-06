using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class ReorderAlert
    {
        public string Item { get; set; }
        public int qty { get; set; }
        public int ReorderQty { get; set; }
        public decimal ApproachPercentage { get; set; }
    }
}