﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace SCManager.UserInterface.Models
{
    public class TaxBillEntryViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please Enter Bill No")]
        [Display(Name = "Bill No")]
        public string BillNo { get; set; }
        [Required(ErrorMessage = "Please select Bill date")]
        [Display(Name = "Bill Date")]
        public string BillDate { get; set; }
        public string BillDateFormatted { get; set; }
        [Required(ErrorMessage = "Please Select Technician")]
        [Display(Name = "Technician")]
        public Guid? EmpID { get; set; }
        [Required(ErrorMessage = "Please Select Job No")]
        [Display(Name = "Job No")]
        public string JobNo { get; set; }
        [Required(ErrorMessage = "Please Enter Customer Name")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Contact")]
        public string CustomerContactNo { get; set; }
        [Display(Name = "Customer Location")]
        public string CustomerLocation { get; set; }
        [Display(Name = "Payment Reference No")]
        public string PaymentRefNo { get; set; }
        [Required(ErrorMessage = "Please select mode of payment")]
        [Display(Name = "Mode Of Payment")]
        public string PaymentMode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name = "Tax %")]
        public decimal? VATAmount { get; set; }
        [Range(0, 9999999999999999.99)]
        public decimal? Discount { get; set; }
        [Display(Name = "Service Charge")]
        public decimal? ServiceCharge { get; set; }

        [Display(Name = "Service Charge Commission %")]
        public decimal? ServiceChargeComm { get; set; }
        [Display(Name = "Special Commission")]
        public decimal? SpecialComm { get; set; }
        [Display(Name = "SC Commission Amount")]
        public decimal? SCCommAmount { get; set; }
        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }
        [Display(Name = "Sub Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }
        [Display(Name = "Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Total { get; set; }
        public String DetailJSON { get; set; }
        public string Technician { get; set; }
        public Guid? jobNoID { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public List<SelectListItem> JobNoList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public List<TaxBillEntryDetailViewModel> TaxBillEntryDetail { get; set; }
        public int IsActive { get; set; }
        public PDFTools PDFToolsObj { get; set; }
        [Display(Name = "CGST %")]
        public decimal? CGSTAmount { get; set; }
        [Display(Name = "SGST %")]
        public decimal? SGSTAmount { get; set; }
        [Display(Name = "Total Tax Amount")]
        public decimal? TotalTaxAmount { get; set; }
        public decimal? CgstPercentage { get; set; }
        public decimal? SgstPercentage { get; set; }
        public decimal? TotalAmount { get; set; }

        public string ServiceCenterCode { get; set; }
        public string ServiceCenterDescription { get; set; }
        public string ServiceCenterAddress { get; set; }
        public string ServiceCenterEmail { get; set; }
        public string ServiceCenterContactNo { get; set; }
        public string ServiceCenterGstIn { get; set; }
        public string ServiceCenterPanNo { get; set; }
        public string ServiceCenterPlace { get; set; }

        public List<SelectListItem> CustomerList { get; set; }
        public string TaxBillIDs { get; set; }
    }

    public class TaxBillEntryDetailViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public int? ItemNo { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public decimal? ReferralRate { get; set; }
        public decimal? Rate { get; set; }
        public string UOM { get; set; }

        public string Description { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? CgstPercentage { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SgstPercentage { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal? SubTotalAmount { get; set; }
    }
}