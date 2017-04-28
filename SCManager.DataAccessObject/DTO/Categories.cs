using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Categories
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
        public string SCCode { get; set; }
        public string Category { get; set; }
    }
}