using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class OtherIncome
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public string EntryNo { get; set; }
        public string IncomeTypeCode { get; set; }
        public string IncomeTypeDescription { get; set; }
        public string RefNo { get; set; }
        public string HiddenRefNo { get; set; }
        public string RefDate { get; set; }
        public String RefDateFormatted { get; set; }
        public float Amount { get; set; }
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public LogDetails logDetails { get; set; }
    }
}