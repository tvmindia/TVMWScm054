using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Expenses
    {

        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public string ExpenseTypeCode { get; set; }
        public  string RefNo	{ get; set; }
        public DateTime RefDate { get; set; }
        public Guid EmpID { get; set; }
        public string PaymentMode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public LogDetails logDetails { get; set; }


    }
}