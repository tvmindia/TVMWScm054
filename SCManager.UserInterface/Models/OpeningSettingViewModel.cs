﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCManager.UserInterface.Models
{
    public class OpeningSettingViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Opening Cash Balance is missing")]
        [Display(Name = "Cash Balance (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal Cash { get; set; }

        [Required(ErrorMessage = "Opening Bank Balance  missing")]
        [Display(Name = "Bank Balance (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal Bank { get; set; }


        [Required(ErrorMessage = "With effect Date is missing")]
        [Display(Name = "With Effect Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime WithEffectDate { get; set; }

        public List<OpeningDetailViewModel> OpeningDetails { get; set; }
        public String DetailJSON { get; set; }

    }

    public class OpeningDetailViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }

    }
}