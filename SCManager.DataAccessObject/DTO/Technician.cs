﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Technician
    {
    }

    public class TechnicianSummary
    {
        public string Name { get; set; }
        public int Calls { get; set; }
        public decimal StockValue { get; set; }
        public string StockValueFormatted { get; set; }
    }

    public class TechnicianSalary
    {
        public string Name { get; set; }
        public string SCCode { get; set; }
        public Guid EmpID { get; set; }
        public Int16 Month { get; set; }
        public Int16 Year { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal SalaryAdvance { get; set; }
        public decimal TotalPayable { get; set; }
        public int? MajorCalls { get; set; }
        public decimal MajorCommission { get; set; }
        public int? DemoCalls { get; set; }
        public decimal DemoCommission { get; set; }
        public int? MandatoryCalls { get; set; }
        public decimal MandatoryCommission { get; set; }
        public int? MinorCalls { get; set; }
        public decimal MinorCommission { get; set; }
        public int? RepeatCalls { get; set; }
        public decimal RepeatCommission { get; set; }
        public int? RepeatDeductCalls { get; set; }
        public decimal RepeatDeductCommission { get; set; }
        public decimal SpecialCommission { get; set; }
        public decimal ServiceChargeCommission { get; set; }
        public decimal ProductCommission { get; set; }
        public decimal AMCCommission { get; set; }
    }

    public class TechnicianSalaryJobBreakUp
    {
        public string ServiceDate { get; set; }
        public string JobNo { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public decimal Commission { get; set; }
        public decimal SpecialCommission { get; set; }
        public decimal Total { get; set; }
    }

    public class TechnicianSalaryTCRBreakUp
    {
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public decimal ServiceChargeCommission { get; set; }
        public decimal ProductCommission { get; set; }
        public decimal Total { get; set; }
    }
    public class TechnicianSalaryAMCBreakUp
    {
        public string ICRDate { get; set; }
        public string ICRNo { get; set; }
        public string CustomerName { get; set; }
        public decimal AMCCommission { get; set; }
        
    }
    public class TechnicianSalaryAdvanceBreakUp
    {
        public string RefDate { get; set; }
        public decimal Advance { get; set; }
        public string Note { get; set; }
       

    }

}