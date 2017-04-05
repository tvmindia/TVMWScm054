using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
        public class Menu
        {
            /// <summary>
            /// get or set menu ID
            /// </summary>
            public Int16 ID { get; set; }
            /// <summary>
            /// get or set ParentID
            /// </summary>
            public Int16 ParentID { get; set; }
            /// <summary>
            /// get or set MenuText
            /// </summary>
            public string MenuText { get; set; }
            /// <summary>
            /// get or set Controller
            /// </summary>
            public string Controller { get; set; }
            /// <summary>
            /// get or set Action
            /// </summary>
            public string Action { get; set; }
            /// <summary>
            /// get or set Parameter
            /// </summary>
            public string Parameters { get; set; }
        }
     
}