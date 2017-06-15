using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class SystemReportViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int Order { get; set; }
    }

    public class StockSummaryViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
        public string Category { get; set; }
        public Guid SubcategoryID { get; set; }
        public string Subcategory { get; set; }
        public string Stock { get; set; }
        public string UOM { get; set; }
        public string ReorderQty { get; set; }
        public float ProductCommission { get; set; }
        public string Remarks { get; set; }
        public float SellingRate { get; set; }
        public float Value { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }

    public class StockLedgerViewModel
    {
        public int Order { get; set; }
        public string SCCode { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string RefNo { get; set; }
        public decimal Qty { get; set; }
        public string Location { get; set; }
        public string GroupCode { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
    public class TechnicianStockViewModel
    {
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal Stock { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
    }
    public class IncomeExpenseViewModel
    {
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public string AccountHead { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }

    public class ServiceRegistrySummaryViewModel
    {
        public string ServiceDate { get; set; }
        public string Technician { get; set; }
        public int TotalCalls { get; set; }
        public int MinorCalls { get; set; }
        public int MajorCalls { get; set; }
        public int MandatoryCalls { get; set; }
        public int DemoCalls { get; set; }
        public int RepeatCalls { get; set; }

    }

    public class AmcReportViewModel
    {
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string ContactNo { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string AmcNo { get; set; }
        public string AmcStartDate { get; set; }
        public string AmcEndDate { get; set; }
        public string Remarks { get; set; }
        public string DueDays { get; set; }


    }
}