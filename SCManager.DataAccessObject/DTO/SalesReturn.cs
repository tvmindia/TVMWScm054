using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class SalesReturn
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public DateTime? OpenDate { get; set; }
        public string RefNo { get; set; }
        public Guid ItemID { get; set; }
        public int Qty { get; set; }
        public string Remarks { get; set; }
        public bool ReturnStatusYN { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public LogDetails logDetails { get; set; }
        public String OpenDateFormatted { get; set; }
    }
}