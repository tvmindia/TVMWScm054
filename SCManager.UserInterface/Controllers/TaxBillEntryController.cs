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
    public class TaxBillEntryController : Controller
    {
        #region Constructor_Injection

        ITaxBillEntryBusiness _iTaxBillEntryBusiness;
        IEmployeesBusiness _iEmployeesBusiness;

        public TaxBillEntryController(ITaxBillEntryBusiness iTaxBillEntryBusiness, IEmployeesBusiness iEmployeesBusiness)
        {
            _iTaxBillEntryBusiness = iTaxBillEntryBusiness;
            _iEmployeesBusiness = iEmployeesBusiness;

        }
        #endregion Constructor_Injection


        // GET: TaxBillEntry
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            TaxBillEntryViewModel taxBillEntryViewModel = null;
            try
            {
                taxBillEntryViewModel = new TaxBillEntryViewModel();
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
                taxBillEntryViewModel.TechniciansList = selectListItem;

                //------------------ UOM Dropdown Bind--------------------//
                selectListItem = new List<SelectListItem>();
                //Categories Drop down bind
                //List<TaxBillEntryViewModel> jobNoList = Mapper.Map<List<TaxBillEntry>, List<TaxBillEntryViewModel>>(_iTaxBillEntryBusiness.GetAllJobNo(ua));
                //jobNoList = jobNoList == null ? null : jobNoList.OrderBy(attset => attset.JobNo).ToList();
                //foreach (TaxBillEntryViewModel tcrvm in jobNoList)
                //{
                //    selectListItem.Add(new SelectListItem
                //    {
                //        Text = tcrvm.JobNo,
                //        Value = tcrvm.JobNo,
                //        Selected = false
                //    });
                //}
                //taxBillEntryViewModel.JobNoList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(taxBillEntryViewModel);
        }

        #region GetAllTaxBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllTaxBillEntry()
        {
            UA ua = new UA();          
            List<TaxBillEntryViewModel> ItemList = Mapper.Map<List<TaxBillEntry>, List<TaxBillEntryViewModel>>(_iTaxBillEntryBusiness.GetAllTaxBillEntry(ua));            
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #endregion GetAllTaxBillEntry


        #region GetTaxBillHeaderByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetTaxBillHeaderByID(TaxBillEntryViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                TaxBillEntryViewModel result = Mapper.Map<TaxBillEntry, TaxBillEntryViewModel>(_iTaxBillEntryBusiness.GetTaxBillHeaderByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetTaxBillHeaderByID



        #region UpdateTaxBillEntry
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string UpdateTaxBillEntry(TaxBillEntryViewModel TaxBillEntryViewModel)
        {            
            try
            {              
                UA ua = new UA();
                object ResultFromJS = JsonConvert.DeserializeObject(TaxBillEntryViewModel.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                TaxBillEntryViewModel.TaxBillEntryDetail = JsonConvert.DeserializeObject<List<TaxBillEntryDetailViewModel>>(ReadableFormat);
                TaxBillEntryViewModel result = Mapper.Map<TaxBillEntry, TaxBillEntryViewModel>(_iTaxBillEntryBusiness.UpdateTaxBill(Mapper.Map<TaxBillEntryViewModel, TaxBillEntry>(TaxBillEntryViewModel), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result,Message="Updation Successfull" });
              

            }
            catch (Exception ex)
            {

                //ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
           
        }
        #endregion UpdateTaxBillEntry


        #region GetTaxBill
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]       
        public string GetTaxBill(TaxBillEntryViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                TaxBillEntryViewModel taxBillEntry = Mapper.Map<TaxBillEntry, TaxBillEntryViewModel>(_iTaxBillEntryBusiness.GetTaxBill(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxBillEntry.TaxBillEntryDetail });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
            }
        #endregion GetTaxBill

        #region GetFranchiseeDetails
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllFranchiseeDetail()
        {
            try
            {
                UA ua = new UA();
                List<TaxBillEntryViewModel> taxEntryBillList = Mapper.Map<List<TaxBillEntry>, List<TaxBillEntryViewModel>>(_iTaxBillEntryBusiness.GetAllFranchiseeDetail(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxEntryBillList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetFranchiseeDetails



        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
               case "List":
                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToDoc();";              
                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Tax Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Download Tax Bill";
                    ToolboxViewModelObj.PrintBtn.Event = "DownloadTaxBill();";

                    break;
                case "Add":
                    ToolboxViewModelObj.PrintBtn.Visible = false;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToDoc();";
                    //ToolboxViewModelObj.backbtn.Visible = true;
                    //ToolboxViewModelObj.backbtn.Text = "Back";
                    //ToolboxViewModelObj.backbtn.Title = "Back to list";
                    //ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    //ToolboxViewModelObj.addbtn.Visible = true;
                    //ToolboxViewModelObj.addbtn.Disable = true;
                    //ToolboxViewModelObj.addbtn.DisableReason = "N/A for new Tax Entry";
                    //ToolboxViewModelObj.addbtn.Text = "New";
                    //ToolboxViewModelObj.addbtn.Title = "Add New";
                    //ToolboxViewModelObj.addbtn.Event = "";

                    //ToolboxViewModelObj.savebtn.Visible = true;
                    //ToolboxViewModelObj.savebtn.Text = "Save";
                    //ToolboxViewModelObj.savebtn.Title = "Save Tax Bill";
                    //ToolboxViewModelObj.savebtn.Event = "save();";

                    //ToolboxViewModelObj.deletebtn.Visible = true;
                    //ToolboxViewModelObj.deletebtn.Disable = true;
                    //ToolboxViewModelObj.deletebtn.Text = "Delete";
                    //ToolboxViewModelObj.deletebtn.Title = "Delete Tax";
                    //ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new Tax Entry";
                    //ToolboxViewModelObj.deletebtn.Event = "";

                    //ToolboxViewModelObj.resetbtn.Visible = true;
                    //ToolboxViewModelObj.resetbtn.Text = "Reset";
                    //ToolboxViewModelObj.resetbtn.Title = "Reset";
                    //ToolboxViewModelObj.resetbtn.Event = "reset();";

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