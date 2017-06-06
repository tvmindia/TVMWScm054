using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class CallandServiceTypes
    {

    }

    public class JobCallTypes
    {
        public string SCCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class CallTypes
    {
        public string SCCode { get; set; }
        public string Code { get; set; }
        public float Commission { get; set; }
        public float MajorCommission { get; set; }
        public float MinorCommission { get; set; }
        public float MandatoryCommission { get; set; }
        public float RepeatCommission { get; set; }
        public float DemoCommission { get; set; }
        public string SubType { get; set; }
        public LogDetails logDetails { get; set; }
    }
    public class ServiceTypes
    {
        public string SCCode { get; set; }
        public string Code { get; set; }
        public float Commission { get; set; }
        public float MajorCommission { get; set; }
        public float MinorCommission { get; set; }
        public float MandatoryCommission { get; set; }
        public float RepeatCommission { get; set; }
        public float DemoCommission { get; set; }
        public float AMC1Commission { get; set; }
        public float AMC2Commission { get; set; }
        public string SubType { get; set; }
        public string Description { get; set; }
        public LogDetails logDetails { get; set; }
    }
}