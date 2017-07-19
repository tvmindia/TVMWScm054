using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Job
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public Employees Employee { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceDateformatted { get; set; }
        public string JobNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLocation { get; set; }
        public string ServiceType { get; set; }
        public string CallType { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string CallStatusCode { get; set; }
        //public string ICRNo { get; set; }
        public string MobileNumber { get; set; }
        public string TechnicianRemark { get; set; }
        public string RepeatEmpName { get; set; }
        public string RepeatJobNo { get; set; }
        public string Repeat_EmpID { get; set; }
        public string CallStatusDescription { get; set; }
        public string ServiceTypeDescription { get; set; }
        public string JobCallTypeDescription { get; set; }
        public decimal? SCCommAmount { get; set; }
        public LogDetails logDetails { get; set; }
    }
}