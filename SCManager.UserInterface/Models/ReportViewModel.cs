using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class SystemReportViewModel
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

    public class StockSummaryViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
        public string Category { get; set; }
        public Guid SubcategoryID { get; set; }
        public string Subcategory { get; set; }
        public string Stock { get; set; }
        public string UOM { get; set; }
        public string ReorderQty { get; set; }
        public float ProductCommission { get; set; }
        public string Remarks { get; set; }
        public float SellingRate { get; set; }
        public float Value { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }

}