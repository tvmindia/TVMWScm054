using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ReceiveFromTechnicianController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        IEmployeesBusiness _iEmployeesBusiness;
        IReceiveFromTechnicianBusiness _iReceiveFromTechnicianBusiness;

        public ReceiveFromTechnicianController(IEmployeesBusiness iEmployeesBusiness, IReceiveFromTechnicianBusiness iReceiveFromTechnicianBusiness)
        {
            _iEmployeesBusiness = iEmployeesBusiness;
            _iReceiveFromTechnicianBusiness = iReceiveFromTechnicianBusiness;
        }
        #endregion Constructor_Injection
        // GET: ReceiveFromTechnician
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            UA ua = new UA();
            DateTime dt = ua.CurrentDatetime();
            ViewBag.fromdate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            ReceiveFromTechnicianViewModel receiveFromTechnicianViewModel = null;
            try
            {
                receiveFromTechnicianViewModel = new ReceiveFromTechnicianViewModel();
               
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
                receiveFromTechnicianViewModel.TechniciansList = selectListItem;
                receiveFromTechnicianViewModel.TechniciansListItems = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(receiveFromTechnicianViewModel);
        }


        #region GetReceiptsSheet
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetReceiptsSheet(string empID, string transferDate)
        {
            UA ua = new UA();
            List<ReceiveFromTechnicianViewModel> ReceiptFromTechList = Mapper.Map<List<ReceiveFromTechnician>, List<ReceiveFromTechnicianViewModel>>(_iReceiveFromTechnicianBusiness.GetReceiptsSheet(empID, transferDate, ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ReceiptFromTechList });

        }
        #endregion GetReceiptsSheet

        #region GetAllReceiptsFromTechnician
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllReceiptsFromTechnician(string empID, string fromDate, string toDate)
        {
            UA ua = new UA();
            List<ReceiveFromTechnicianViewModel> ReceiptFromTechList = Mapper.Map<List<ReceiveFromTechnician>, List<ReceiveFromTechnicianViewModel>>(_iReceiveFromTechnicianBusiness.GetAllReceiptsFromTechnician(empID, fromDate, toDate, ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ReceiptFromTechList });

        }
        #endregion GetAllReceiptsFromTechnician

        #region InsertUpdateReceiveFromTechnician
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateReceiveFromTechnician(ReceiveFromTechnicianViewModel ReceiveFromTechnicianViewModelObj)
        {
            // string result = "";

            try
            {
                if (ReceiveFromTechnicianViewModelObj.HiddenEmpID != Guid.Empty)
                {
                    ModelState.Remove("EmpID");
                    ReceiveFromTechnicianViewModelObj.EmpID = ReceiveFromTechnicianViewModelObj.HiddenEmpID;
                }
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(ReceiveFromTechnicianViewModelObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    List<ReceiveFromTechnicianViewModel> obj = new List<ReceiveFromTechnicianViewModel>();
                    obj = JsonConvert.DeserializeObject<List<ReceiveFromTechnicianViewModel>>(ReadableFormat);
                    List<ReceiveFromTechnician> DTOobj = new List<ReceiveFromTechnician>();
                    DTOobj = Mapper.Map<List<ReceiveFromTechnicianViewModel>, List<ReceiveFromTechnician>>(obj);
                    List<ReceiveFromTechnicianViewModel> r = Mapper.Map<List<ReceiveFromTechnician>, List<ReceiveFromTechnicianViewModel>>(_iReceiveFromTechnicianBusiness.InsertUpdateReceiveFromTechnician(DTOobj, ReceiveFromTechnicianViewModelObj.HiddenEmpID, ReceiveFromTechnicianViewModelObj.ReceiveDate, ua));
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
        #endregion InsertUpdateReceiveFromTechnician

        #region DeleteReceiveFromTechnician
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteReceiveFromTechnician(string ID)
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
                        status = _iReceiveFromTechnicianBusiness.DeleteReceiveFromTechnician(ID, ua);
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
        #endregion DeleteReceiveFromTechnician

        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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