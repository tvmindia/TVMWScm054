using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class TechnicianJob
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public Guid EmpID { get; set; }
        public string ServiceDate { get; set; }
        public string JobNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLocation { get; set; }
        public string ServiceTypeCode { get; set; }
        public string CallTypeCode { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string CallStatusCode { get; set; }
        public string ICRNo { get; set; }
        public string TechnicianRemarks { get; set; }
        public Guid Repeat_EmpID { get; set; }
        public string Repeat_JobNo { get; set; }
        public LogDetails logDetails { get; set; }
    }
}