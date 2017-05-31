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
    public class ICRBillEntryController : Controller
    {
        #region Constructor_Injection


        ITCRBillEntryBusiness _iTCRBillEntryBusiness;
        IEmployeesBusiness _iEmployeesBusiness;
        IICRBillEntryBusiness _iICRBillEntryBusiness;

        public ICRBillEntryController(ITCRBillEntryBusiness iTCRBillEntryBusiness, IEmployeesBusiness iEmployeesBusiness, IICRBillEntryBusiness iICRBillEntryBusiness)
        {
            _iTCRBillEntryBusiness = iTCRBillEntryBusiness;
            _iEmployeesBusiness = iEmployeesBusiness;
            _iICRBillEntryBusiness = iICRBillEntryBusiness;

        }
        #endregion Constructor_Injection
        // GET: ICRBillEntry
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            ICRBillEntryViewModel iCRBillEntryViewModel = null;
            try
            {
                iCRBillEntryViewModel = new ICRBillEntryViewModel();
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
                iCRBillEntryViewModel.TechniciansList = selectListItem;

                //------------------ UOM Dropdown Bind--------------------//
                selectListItem = new List<SelectListItem>();
                //Categories Drop down bind
                List<TCRBillEntryViewModel> jobNoList = Mapper.Map<List<TCRBillEntry>, List<TCRBillEntryViewModel>>(_iTCRBillEntryBusiness.GetAllJobNo(ua));
                jobNoList = jobNoList == null ? null : jobNoList.OrderBy(attset => attset.JobNo).ToList();
                foreach (TCRBillEntryViewModel tcrvm in jobNoList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = tcrvm.JobNo,
                        Value = tcrvm.JobNo,
                        Selected = false
                    });
                }
                iCRBillEntryViewModel.JobNoList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(iCRBillEntryViewModel);
        }
        Const c = new Const();

        #region GetAllICRBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetAllICRBillEntry()
        {
            UA ua = new UA();
            List<ICRBillEntryViewModel> ItemList = Mapper.Map<List<ICRBillEntry>, List<ICRBillEntryViewModel>>(_iICRBillEntryBusiness.GetAllICRBillEntry(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #endregion GetAllICRBillEntry


        #region RebindJobNo
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string RebindJobNo()
        {
            UA ua = new UA();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            //Categories Drop down bind
            List<TCRBillEntryViewModel> jobNoList = Mapper.Map<List<TCRBillEntry>, List<TCRBillEntryViewModel>>(_iTCRBillEntryBusiness.GetAllJobNo(ua));
            jobNoList = jobNoList == null ? null : jobNoList.OrderBy(attset => attset.JobNo).ToList();
            foreach (TCRBillEntryViewModel tcrvm in jobNoList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = tcrvm.JobNo,
                    Value = tcrvm.JobNo,
                    Selected = false
                });
            }
          //  iCRBillEntryViewModel.JobNoList = selectListItem;
            return JsonConvert.SerializeObject(new { Result = "OK", Records = selectListItem });

        }
        #endregion RebindJobNo

        #region InsertUpdateICRBillEntry
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateICRBillEntry(ICRBillEntryViewModel ICRBillEntryViewModelObj)
        {
            //string result = "";

            try
            {
                //if (ModelState.IsValid)
                //{
                UA ua = new UA();
                ICRBillEntryViewModelObj.STAmount = ICRBillEntryViewModelObj.Subtotal;
                object ResultFromJS = JsonConvert.DeserializeObject(ICRBillEntryViewModelObj.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                ICRBillEntryViewModelObj.ICRBillEntryDetail = JsonConvert.DeserializeObject<List<ICRBillEntryDetailViewModel>>(ReadableFormat);
                ICRBillEntryViewModel r = Mapper.Map<ICRBillEntry, ICRBillEntryViewModel>(_iICRBillEntryBusiness.InsertUpdate(Mapper.Map<ICRBillEntryViewModel, ICRBillEntry>(ICRBillEntryViewModelObj), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
                //}
                //else
                //{
                //    List<string> modelErrors = new List<string>();
                //    foreach (var modelState in ModelState.Values)
                //    {
                //        foreach (var modelError in modelState.Errors)
                //        {
                //            modelErrors.Add(modelError.ErrorMessage);
                //        }
                //    }
                //    return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
                //    //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
                //}

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
            // return result;
        }
        #endregion InsertUpdateICRBillEntry

        #region GetICRBillHeaderByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetICRBillHeaderByID(ICRBillEntryViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                ICRBillEntryViewModel icrBillEntryVM = Mapper.Map<ICRBillEntry, ICRBillEntryViewModel>(_iICRBillEntryBusiness.GetICRBillHeaderByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = icrBillEntryVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetICRBillHeaderByID

        #region DeleteICRBillDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteICRBillDetail(ICRBillEntryDetailViewModel icrDObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = icrDObj.ID.GetValueOrDefault();
                Guid HeaderID = icrDObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _iICRBillEntryBusiness.DeleteICRBillDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteICRBillDetail

        #region DeleteICRBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteICRBillEntry(ICRBillEntryViewModel icrObj)
        {

            try
            {
                if (icrObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _iICRBillEntryBusiness.DeleteICRBillEntry(icrObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }
        #endregion DeleteICRBillEntry

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
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New ICR Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save ICR Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete ICR Bill";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for new ICR Entry";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save ICR Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete ICR";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new ICR Entry";
                    ToolboxViewModelObj.deletebtn.Event = "";

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