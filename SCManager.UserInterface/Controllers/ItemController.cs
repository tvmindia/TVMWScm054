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
        ISubCategoriesBusiness _SubCategoriesBusiness;
        ICategoriesBusiness _categoriesBusiness;
     

        public ItemController(IItemBusiness itemBusiness, ISubCategoriesBusiness subCategoriesBusiness,ICategoriesBusiness categoriesBusiness)
        {
            _itemBusiness = itemBusiness;
            _SubCategoriesBusiness = subCategoriesBusiness;
            _categoriesBusiness = categoriesBusiness;
           
        }
        #endregion Constructor_Injection


        // GET: Item  UA ua = new UA();
        public ActionResult Index()
        {
            ItemViewModel itemViewModal = null;
            try
            {
               itemViewModal= new ItemViewModel();
                UA ua = new UA();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Categories Drop down bind
                List<ItemViewModel> CategoryListVM = Mapper.Map<List<Categories>, List<ItemViewModel>>(_categoriesBusiness.GetAllCategories(ua));
                CategoryListVM = CategoryListVM == null ? null : CategoryListVM.OrderBy(attset => attset.Description).ToList();             
                foreach (ItemViewModel clvm in CategoryListVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = clvm.Description,
                        Value = clvm.ID.ToString(),
                        Selected = false
                    });
                }
                itemViewModal.CategoryList = selectListItem;
               
        

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View(itemViewModal);
        }
        Const c = new Const();
        [HttpGet]
        public string ItemsForDropdown(ItemDropdownViewModel obj)
        {
            UA ua = new UA();
            List<ItemDropdownViewModel> ItemList = Mapper.Map<List<Item>, List<ItemDropdownViewModel>>(_itemBusiness.GetAllItems(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });        

        }
        [HttpGet]
        public string GetAllItems()
        {
            UA ua = new UA();
            List<ItemViewModel> ItemList = Mapper.Map<List<Item>, List<ItemViewModel>>(_itemBusiness.GetAllItems(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        [HttpGet]
        public string GetItemByID(string ID)
        {
            UA ua = new UA();
            List<ItemViewModel> ItemList = Mapper.Map<List<Item>, List<ItemViewModel>>(_itemBusiness.GetItemByID(ua,ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #region GetAllSubCategories
        [HttpGet]
        public string GetAllSubCategories(string CategoryID)
        {
           try
            {
                    UA ua = new UA();
                    //SubCategoryList Drop down bind
                    List<ItemViewModel> subCategoryListVM = Mapper.Map<List<SubCategories>, List<ItemViewModel>>(_SubCategoriesBusiness.GetAllSubCategories(ua, CategoryID));

                    return JsonConvert.SerializeObject(new { Result = "OK", Records = subCategoryListVM });
                
               
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
           
        }
        #endregion GetAllSubCategories

        #region InsertItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateItem(ItemViewModel itemViewmodelObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    itemViewmodelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    itemViewmodelObj.logDetails.CreatedBy = ua.UserName;
                    itemViewmodelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    itemViewmodelObj.logDetails.UpdatedBy = itemViewmodelObj.logDetails.CreatedBy;
                    itemViewmodelObj.logDetails.UpdatedDate = itemViewmodelObj.logDetails.CreatedDate;
                    itemViewmodelObj.SCCode = ua.SCCode;
                    if (itemViewmodelObj.ID==Guid.Empty)
                    {

                        result = _itemBusiness.InsertItem(Mapper.Map<ItemViewModel, Item>(itemViewmodelObj));
                        return JsonConvert.SerializeObject(new { Result = "OK", Records = result,Message=c.InsertSuccess });
                    }
                   else
                    {
                        result = _itemBusiness.UpdateItem(Mapper.Map<ItemViewModel, Item>(itemViewmodelObj));
                        return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message=c.UpdateSuccess });
                    }
                   
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
                //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }
            
        }
        #endregion InsertItem
              
        #region DeleteItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteItem(string ID)
        {
            string status = null;
            string msg = null;
            if (ModelState.IsValid)
            {

                try
                {
                    if(!string.IsNullOrEmpty(ID))
                    {
                        status = _itemBusiness.DeleteItem(ID);
                    }
                    switch(status)
                    {
                        case "0":
                            msg = c.DeleteFailure;
                            break;
                        case "1":
                            msg = c.DeleteSuccess;
                            break;
                        case "2":
                            msg = c.FKviolation;
                            break;
                    }
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message= msg });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion DeleteItem

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Item";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Item";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Item";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Item";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new item";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}