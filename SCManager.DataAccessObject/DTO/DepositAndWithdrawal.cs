using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class DepositAndWithdrawal
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDescription { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public LogDetails logDetails { get; set; }
    }
 
}