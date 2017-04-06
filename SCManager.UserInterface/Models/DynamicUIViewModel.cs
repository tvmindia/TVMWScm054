using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class DynamicUIViewModel
    {
        public List<MenuViewModel> MenuViewModelList { get; set; }
        public List<ReorderAlertViewModel> ReorderAlertViewModelList { get; set; }
    }
}