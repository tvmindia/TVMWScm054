using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class CallandServiceTypesViewModel
    {
        //calltype
        public string SCCode { get; set; }
        [Display(Name = "Major")]
        public float MajorCommission { get; set; }
        [Display(Name = "Minor")]
        public float MinorCommission { get; set; }
        [Display(Name = "Mandatory")]
        public float MandatoryCommission { get; set; }
        [Display(Name = "Repeat")]
        public float RepeatCommission { get; set; }
        [Display(Name = "Demo")]
        public float DemoCommission { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        //servicetype
        [Display(Name = "AMC 1 Year")]
        public float AMC1Commission { get; set; }
        [Display(Name = "AMC 2 Year")]
        public float AMC2Commission { get; set; }
    }
}