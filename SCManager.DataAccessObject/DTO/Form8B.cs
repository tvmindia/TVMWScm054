using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Form8B
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }

        public string SaleOrderNo { get; set; }
        public string ChallanNo { get; set; }

        public DateTime? ChallanDate { get; set; }

        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string SPUNo { get; set; }
        public string TicketNo { get; set; }

        public string CustomerDelvAddrs { get; set; }
        public string CustomerBillAddrs { get; set; }

        public string Remarks { get; set; }

        public decimal VATAmount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal VATExpense { get; set; }
        public LogDetails logDetails { get; set; }

        public decimal GrandTotal { get; set; }


        public String ChallanDateFormatted { get; set; }
        public String PODateFormatted { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public List<Form8BDetail> Form8BDetail { get; set; }
        public string DetailXML { get; set; }
    }



    public class Form8BDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public decimal? Rate { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? NetAmount { get; set; }
    }
}