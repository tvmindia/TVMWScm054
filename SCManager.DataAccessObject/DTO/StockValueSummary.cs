using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class StockValueSummary
    {
      public  int value;
        public decimal Amount;
        public string AmountConverted;
        public string color;
        public string label;
        public int totalValue;
        public string totalValueConverted;
    }
}