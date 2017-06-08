using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Item
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
        public string UOMDesc { get; set; }
        public string ReorderQty { get; set; }
        public float ProductCommission { get; set; }
        public string Remarks { get; set; }
        public float SellingRate { get; set; }
        public float Value { get; set; }
        public string SalesReturnPendingQty { get; set; }
        public LogDetails logDetails { get; set; }
        public string DefDamgStockQty { get; set; }
        public string SCQty { get; set; }
        public string TechnicianQty { get; set; }
    }
}