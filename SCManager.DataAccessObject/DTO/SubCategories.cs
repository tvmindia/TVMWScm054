using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class SubCategories
    {
        public Guid ID { get; set; }
        public string Subcategory { get; set; }
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
    }
}