using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class SystemReport
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
    public class StockLedger
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
        
        public LogDetails logDetails { get; set; }
    }
    public class TechnicianStock
    {
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal Stock { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
    }
    public class IncomeExpense
    {
        public string ReferenceNo { get; set; }
        public string Description { get; set; }
        public string AccountHead { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Balance { get; set; }
        public LogDetails logDetails { get; set; }
    }

    public class ServiceRegistrySummary
    {
        public string ServiceDate { get; set; }
        public string Technician { get; set; }
        public int TotalCalls { get; set; }
        public int MinorCalls { get; set; }
        public int MajorCalls { get; set; }
        public int MandatoryCalls { get; set; }
        public int DemoCalls { get; set; }
        public int RepeatCalls { get; set; }
        public string EmpID { get; set; }

    }

    public class AmcReport
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

    public class AmcBaseValueSummary
    {
        public string ICRDate { get; set; }
        public string ICRNo { get; set; }
        public string AMCNo { get; set; }
        public string AMCValidFromDate { get; set; }
        public string AMCValidToDate { get; set; }
        public string Technician { get; set; }
        public string CustomerName { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
    public class ProfitAndLossReport
    {
      
        public string SCCode { get; set; }
        public string BaseType { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
       
    }


}