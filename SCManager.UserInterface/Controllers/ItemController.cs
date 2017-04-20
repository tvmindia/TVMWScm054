using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using SCManager.BusinessService.Contracts;
using AutoMapper;
using Newtonsoft.Json;

namespace SCManager.UserInterface.Controllers
{
    public class ItemController : Controller
    {

        #region Constructor_Injection

        IItemBusiness _itemBusiness;
         

        public ItemController(IItemBusiness itemBusiness)
        {
            _itemBusiness = itemBusiness;
            
        }
        #endregion Constructor_Injection


        // GET: Item  UA ua = new UA();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string ItemsForDropdown(ItemDropdownViewModel obj)
        {
            UA ua = new UA();
            List<ItemDropdownViewModel> ItemList = Mapper.Map<List<Item>, List<ItemDropdownViewModel>>(_itemBusiness.GetAllItems(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });
          

        }
    }
}