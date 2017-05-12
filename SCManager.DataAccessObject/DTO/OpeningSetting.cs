using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class OpeningSetting
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public decimal Cash { get; set; }
        public decimal Bank { get; set; }
        public DateTime WithEffectDate { get; set; }
        public List<OpeningDetail> OpeningDetails { get; set; }
        public LogDetails logDetails { get; set; }
        public string DetailXML { get; set; }
    }

    public class OpeningDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
       
    }
}