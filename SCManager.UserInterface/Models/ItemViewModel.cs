using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class ItemViewModel
    {
        
        public string SCCode { get; set; }
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please enter Item Code")]
        [Display(Name = "Item Code")]
        [StringLength(50)]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        [Display(Name = "Description")]
        [StringLength(1000)]
        public string Description { get; set; }
        public Guid CategoryID { get; set; }
        public string Category { get; set; }
        public Guid SubcategoryID { get; set; }
        public string Subcategory { get; set; }
        [Display(Name = "Stock Qty (₹)")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "Please enter UOM")]
        [Display(Name = "UOM")]
        [StringLength(5)]
        public string UOM { get; set; }

        [Display(Name = "Reorder Qty (₹)")]
        public string ReorderQty { get; set; }

        [Display(Name = "Product Commission (₹)")]
        public float? ProductCommission { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "Selling Rate (₹)")]
        public float? SellingRate { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public List<SelectListItem> SubCategoryList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }

    public class ItemDropdownViewModel
    {
        
        public Guid ID { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }        
        public string UOM { get; set; }
        

    }
}