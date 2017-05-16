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
    public class IssueToTechnicianController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        IEmployeesBusiness _iEmployeesBusiness;
        IIssueToTechnicianBusiness _iIssueToTechnicianBusiness;

        public IssueToTechnicianController(IEmployeesBusiness iEmployeesBusiness, IIssueToTechnicianBusiness iIssueToTechnicianBusiness)
        {
            _iEmployeesBusiness = iEmployeesBusiness;
            _iIssueToTechnicianBusiness = iIssueToTechnicianBusiness;
        }
        #endregion Constructor_Injection
        // GET: IssueToTechnician
        public ActionResult Index()
        {
            IssueToTechnicianViewModel issueToTechnicianViewModel = null;
            try
            {
                issueToTechnicianViewModel = new IssueToTechnicianViewModel();
                UA ua = new UA();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Technician Drop down bind
                List<EmployeesViewModel> TechniciansList = Mapper.Map<List<Employees>, List<EmployeesViewModel>>(_iEmployeesBusiness.GetAllTechnicians(ua));
                TechniciansList = TechniciansList == null ? null : TechniciansList.OrderBy(attset => attset.Name).ToList();
                foreach (EmployeesViewModel clvm in TechniciansList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = clvm.Name,
                        Value = clvm.ID.ToString(),
                        Selected = false
                    });
                }
                issueToTechnicianViewModel.TechniciansList = selectListItem;
                issueToTechnicianViewModel.TechniciansListItems = selectListItem;
           
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(issueToTechnicianViewModel);
        }

        #region GetIssueSheets
        [HttpGet]
        public string GetIssueSheets(string empID, string transferDate)
        {
            UA ua = new UA();
            List<IssueToTechnicianViewModel> IssueToTechList = Mapper.Map<List<IssueToTechnician>, List<IssueToTechnicianViewModel>>(_iIssueToTechnicianBusiness.GetIssueSheets(empID,transferDate , ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = IssueToTechList });

        }
        #endregion GetIssueSheets

        #region GetAllIssueToTechnician
        [HttpGet]
        public string GetAllIssueToTechnician(string empID,string fromDate, string toDate)
        {
            UA ua = new UA();
            List<IssueToTechnicianViewModel> IssueToTechList = Mapper.Map<List<IssueToTechnician>, List<IssueToTechnicianViewModel>>(_iIssueToTechnicianBusiness.GetAllIssueToTechnician(empID,fromDate, toDate,ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = IssueToTechList });

        }
        #endregion GetAllIssueToTechnician

        #region InsertUpdateIssuedSheets
        [HttpPost]
        public string InsertUpdateIssuedSheets(IssueToTechnicianViewModel IssueToTechnicianViewModelObj)
        {
           // string result = "";

            try
            {
                if (IssueToTechnicianViewModelObj.HiddenEmpID != Guid.Empty)
                {                
                    ModelState.Remove("EmpID");
                    IssueToTechnicianViewModelObj.EmpID = IssueToTechnicianViewModelObj.HiddenEmpID;
                }
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(IssueToTechnicianViewModelObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    List<IssueToTechnicianViewModel> obj = new List<IssueToTechnicianViewModel>();
                    obj = JsonConvert.DeserializeObject<List<IssueToTechnicianViewModel>>(ReadableFormat);
                    List<IssueToTechnician> DTOobj = new List<IssueToTechnician>();
                    DTOobj = Mapper.Map<List<IssueToTechnicianViewModel>, List<IssueToTechnician>>(obj);
                    List<IssueToTechnicianViewModel> r = Mapper.Map<List<IssueToTechnician>, List<IssueToTechnicianViewModel>>(_iIssueToTechnicianBusiness.InsertUpdateIssueToTechnician(DTOobj,IssueToTechnicianViewModelObj.HiddenEmpID,IssueToTechnicianViewModelObj.IssueDate, ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
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
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
           // return result;
        }
        #endregion InsertUpdateIssuedSheets

        #region DeleteIssueToTechnician
        [HttpGet]
        public string DeleteIssueToTechnician(string ID)
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
                        status = _iIssueToTechnicianBusiness.DeleteIssueToTechnician(ID, ua);
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
                    
                    if (ex.Message == "Item already exist")
                    {
                        ConstMessage cm = c.GetMessage("DIMD2");
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                    }
                    else
                    {
                        ConstMessage cm = c.GetMessage(ex.Message);
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                    }
                   
                   
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion DeleteIssueToTechnician

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
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";



                    break;
                case "Edit":
                  
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Issue";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                  
                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";

                    break;
                case "Add":
                   

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Issue";
                    ToolboxViewModelObj.savebtn.Event = "save();";
                                      

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