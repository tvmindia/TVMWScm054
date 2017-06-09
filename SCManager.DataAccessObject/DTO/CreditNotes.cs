using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class CreditNotes
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string CreditNoteNo { get; set; }
        public float Amount { get; set; }
        public string Description { get; set; }
        public bool showAllYN { get; set; }
        public string Date { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string HiddenCreditNoteNo { get; set; }
        public LogDetails logDetails { get; set; }
        public String DateFormatted { get; set; }
    }
}