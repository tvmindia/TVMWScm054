using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    public class SalesReturnController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        ISalesReturnBusiness _iSalesReturnBusiness;
        IItemBusiness _iItemBusiness;

        public SalesReturnController(ISalesReturnBusiness iSalesReturnBusiness, IItemBusiness iItemBusiness)
        {
            _iSalesReturnBusiness = iSalesReturnBusiness;
            _iItemBusiness = iItemBusiness;

        }
        #endregion Constructor_Injection

        // GET: SalesReturn
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllSalesReturn
        [HttpGet]
        public string GetAllSalesReturn()
        {
            UA ua = new UA();
            List<SalesReturnViewModel> salesReturnList = Mapper.Map<List<SalesReturn>, List<SalesReturnViewModel>>(_iSalesReturnBusiness.GetAllSalesReturn(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = salesReturnList });

        }
        #endregion GetAllSalesReturn

        #region GetAllItemCode    
        [HttpGet]
        public string GetAllItemCode(ItemDropdownViewModel obj)
        {
            UA ua = new UA();
            List<ItemViewModel> ItemCodeList = Mapper.Map<List<Item>, List<ItemViewModel>>(_iItemBusiness.GetAllItemCode(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemCodeList });

        }
        #endregion GetAllDefectiveDamaged

        #region SalesReturnValidation
        [HttpGet]
        public string SalesReturnValidation(string itemID)
        {
            string status = null;
           
            try
            {
                if (!string.IsNullOrEmpty(itemID))
                {
                    UA ua = new UA();

                    status = _iSalesReturnBusiness.SalesReturnValidation(itemID,ua);


                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = status });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = c.NoItems });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }
        #endregion SalesReturnValidation

        #region InsertUpdateSalesReturn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateSalesReturn(SalesReturnViewModel salesReturnViewModel)
        {
            object result = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    salesReturnViewModel.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    salesReturnViewModel.logDetails.CreatedBy = ua.UserName;
                    salesReturnViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    salesReturnViewModel.logDetails.UpdatedBy = salesReturnViewModel.logDetails.CreatedBy;
                    salesReturnViewModel.logDetails.UpdatedDate = salesReturnViewModel.logDetails.CreatedDate;
                    salesReturnViewModel.SCCode = ua.SCCode;

                    result = _iSalesReturnBusiness.InsertUpdateSalesReturn(Mapper.Map<SalesReturnViewModel, SalesReturn>(salesReturnViewModel));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Item already exist")
                    {
                        ConstMessage cm = c.GetMessage("DIMD2");
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }

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
        #endregion InsertUpdateSalesReturn

        #region GetSalesReturnByID
        [HttpGet]
        public string GetSalesReturnByID(string ID)
        {
            UA ua = new UA();
            List<SalesReturnViewModel> salesReturnList = Mapper.Map<List<SalesReturn>, List<SalesReturnViewModel>>(_iSalesReturnBusiness.GetSalesReturnByID(ua, ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = salesReturnList });

        }
        #endregion GetSalesReturnByID

        #region DeleteSalesReturn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteSalesReturn(string ID)
        {
            string status = null;
            string msg = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    if (!string.IsNullOrEmpty(ID))
                    {
                        status = _iSalesReturnBusiness.DeleteSalesReturn(ID, ua);
                    }
                    switch (status)
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
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = msg });
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
        #endregion DeleteSalesReturn

        #region ReturnSalesToCompany
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ReturnSalesToCompany(SalesReturnViewModel salesReturnViewModelObj)
        {
            string status = null;
            string msg = null;
            

                try
                {
                    UA ua = new UA();
                    if ((salesReturnViewModelObj.ID) != Guid.Empty)
                    {
                        status = _iSalesReturnBusiness.ReturnSalesToCompany(salesReturnViewModelObj.ID.ToString(), ua);
                    }
                    switch (status)
                    {
                        case "0":
                            msg = c.UpdateFailure;
                            break;
                        case "1":
                            msg = c.UpdateSuccess;
                            break;
                    }
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = msg });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            

        }
        #endregion ReturnSalesToCompany

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

                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Sales Return";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new Sales Return";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Disable = true;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return To Company";
                    ToolboxViewModelObj.returnBtn.DisableReason = "Not applicable for new Sales Return";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnToCompany();";

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
                    ToolboxViewModelObj.savebtn.Title = "Save Sales Return";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Sales Return";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return To Company";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnToCompany();";

                    break;
                case "Return":
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}