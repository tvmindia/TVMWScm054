using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    
    public class UA
    {
        public string UserName { get; set; }
        public string SCCode { get; set; }
        public DateTime DateTime { get; set; }


        public UA(){
            SCCode = "SC001";
            UserName = "Suvaneeth";
        }
        public DateTime CurrentDatetime() {
            return DateTime.Now;
        }
    }
}