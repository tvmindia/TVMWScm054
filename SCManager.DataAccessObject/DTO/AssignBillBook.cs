using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class AssignBillBook
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public string BillNo { get; set; }
        public string SeriesStart { get; set; }
        public string SeriesEnd { get; set; }
        public string LastUsed { get; set; }
        public Guid? EmpID { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Technician { get; set; }
        public string BillBookType { get; set; }
        public string MissingSerials { get; set; }
        public LogDetails logDetails { get; set; }
    }
}