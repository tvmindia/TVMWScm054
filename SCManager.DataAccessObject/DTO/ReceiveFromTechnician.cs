using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class ReceiveFromTechnician
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? EmpID { get; set; }
        public string ReceiveDate { get; set; }
        public int? Qty { get; set; }
        public Guid? HiddenEmpID { get; set; }
        public LogDetails logDetails { get; set; }
        public Guid? MaterialID { get; set; }
        public string Material { get; set; }
        public int SlNo { get; set; }
        public string UOM { get; set; }
        public string DetailXML { get; set; }
        public string empName { get; set; }
        public String DateFormatted { get; set; }
        public string Description { get; set; }
    }
}