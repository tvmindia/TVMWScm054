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
    public class OtherIncomeController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        IOtherIncomeBusiness _iOtherIncomeBusiness;

        public OtherIncomeController(IOtherIncomeBusiness iOtherIncomeBusiness)
        {
            _iOtherIncomeBusiness = iOtherIncomeBusiness;

        }
        #endregion Constructor_Injection

        // GET: OtherIncome
        public ActionResult Index()
        {
            OtherIncomeViewModel otherIncomeViewModel = null;
            try
            {
                otherIncomeViewModel = new OtherIncomeViewModel();
                UA ua = new UA();
                List<SelectListItem> selectListIncome = new List<SelectListItem>();
                //Technician Drop down bind
                List<OtherIncomeViewModel> IncomeTypeList = Mapper.Map<List<OtherIncome>, List<OtherIncomeViewModel>>(_iOtherIncomeBusiness.GetAllIncomeType());
                IncomeTypeList = IncomeTypeList == null ? null : IncomeTypeList.OrderBy(attset => attset.IncomeTypeDescription).ToList();
                foreach (OtherIncomeViewModel oivm in IncomeTypeList)
                {
                    selectListIncome.Add(new SelectListItem
                    {
                        Text = oivm.IncomeTypeDescription,
                        Value = oivm.IncomeTypeCode,
                        Selected = false
                    });
                }
                otherIncomeViewModel.IncomeTypeList = selectListIncome;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(otherIncomeViewModel);
        }

        #region GetAllOtherIncome
        [HttpGet]
        public string GetAllOtherIncome(string showAllYN)
        {
            UA ua = new UA();
            List<OtherIncomeViewModel> OtherIncomeList = Mapper.Map<List<OtherIncome>, List<OtherIncomeViewModel>>(_iOtherIncomeBusiness.GetAllOtherIncome(ua, showAllYN));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = OtherIncomeList });

        }
        #endregion GetAllOtherIncome

        #region InsertUpdateOtherIncome
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateOtherIncome(OtherIncomeViewModel otherIncomeViewModelObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    otherIncomeViewModelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    otherIncomeViewModelObj.logDetails.CreatedBy = ua.UserName;
                    otherIncomeViewModelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    otherIncomeViewModelObj.logDetails.UpdatedBy = otherIncomeViewModelObj.logDetails.CreatedBy;
                    otherIncomeViewModelObj.logDetails.UpdatedDate = otherIncomeViewModelObj.logDetails.CreatedDate;
                    otherIncomeViewModelObj.SCCode = ua.SCCode;

                    result = _iOtherIncomeBusiness.InsertUpdateOtherIncome(Mapper.Map<OtherIncomeViewModel, OtherIncome>(otherIncomeViewModelObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Reference No. already exist")
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
        #endregion InsertUpdateOtherIncome

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

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete OtherIncome";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new OtherIncome";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";


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
                    ToolboxViewModelObj.savebtn.Title = "Save OtherIncome";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete OtherIncome";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}