using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class ICRBillEntryViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please select ICR No")]
        [Display(Name = "ICR No")]
        public string ICRNo { get; set; }
        [Required(ErrorMessage = "Please select ICR date")]
        [Display(Name = "ICR Date")]
        public string ICRDate { get; set; }
        [Required(ErrorMessage = "Please Select Technician")]
        [Display(Name = "Technician")]
        public Guid? EmpID { get; set; }
        [Display(Name = "Job No")]
        public string JobNo { get; set; }
        [Display(Name = "Model No")]
        public string ModelNo { get; set; }
        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }
        [Required(ErrorMessage = "Please Enter Customer Name")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Contact")]
        public string CustomerContactNo { get; set; }
        [Display(Name = "Customer Location")]
        public string CustomerLocation { get; set; }
        [Display(Name = "AMC Valid From")]
        public string AMCValidFromDate { get; set; }
        [Display(Name = "AMC Valid To")]
        public string AMCValidToDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name = "Mode Of Payment")]
        [Required(ErrorMessage = "Please Select Payment Mode")]
        public string PaymentMode { get; set; }
        [Display(Name = "Cheque Type")]
        public string ChequeType { get; set; }
        public decimal? STAmount { get; set; }
        [Display(Name = "Discount %")]
        public decimal? Discount { get; set; }
        public String DetailJSON { get; set; }
        public String ICRDateFormatted { get; set; }
        public String AMCFromDateFormatted { get; set; }
        public String AMCToDateFormatted { get; set; }
        public List<SelectListItem> JobNoList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public List<ICRBillEntryDetailViewModel> ICRBillEntryDetail { get; set; }
        [Display(Name = "Base Rate (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }
        [Display(Name = "Payment Reference No")]
        public string PaymentRefNo { get; set; }
        [Display(Name = "Total Service Tax %")]
        [Range(0, 9999999999999999.99)]
        public decimal? TotalServiceTaxAmt { get; set; }
        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }
        [Display(Name = "AMC No")]
        public string AMCNO { get; set; }
        [Display(Name = "Total (₹)")]
        public decimal? Total { get; set; }
        public string Technician { get; set; }
    }

    public class ICRBillEntryDetailViewModel
    {
        public int? SlNo { get; set; }
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public int? ItemNo { get; set; }
        // public string ServiceTypeCode { get; set; }
        //public Guid? MaterialID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public string UOM { get; set; }
        public string Material { get; set; }
        public decimal? NetAmount { get; set; }
        public string Description { get; set; }
    }
}