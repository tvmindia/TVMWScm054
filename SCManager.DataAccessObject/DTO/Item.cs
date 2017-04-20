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
        public int Stock { get; set; }
        public string UOM { get; set; }
        public int ReorderQty { get; set; }
    }
}