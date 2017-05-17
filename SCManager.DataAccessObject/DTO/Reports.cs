using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class SystemReport
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int Order { get; set; }
    }
    public class StockLedger
    {
        public int Order { get; set; }
        public string SCCode { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string RefNo { get; set; }
        public decimal Qty { get; set; }
        public string Location { get; set; }
        public LogDetails logDetails { get; set; }
    }

}