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
    public class DefectiveorDamagedController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        IDefectiveDamageBusiness _iDefectiveDamageBusiness;
        IEmployeesBusiness _iEmployeesBusiness;
        IItemBusiness _iItemBusiness;

        public DefectiveorDamagedController(IDefectiveDamageBusiness iDefectiveDamageBusiness, IEmployeesBusiness iEmployeesBusiness, IItemBusiness iItemBusiness)
        {
            _iDefectiveDamageBusiness = iDefectiveDamageBusiness;
            _iEmployeesBusiness = iEmployeesBusiness;
            _iItemBusiness = iItemBusiness;

        }
        #endregion Constructor_Injection
        // GET: DefectiveorDamaged
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            DefectiveorDamagedViewModel defectiveorDamagedViewModel = null;
            try
            {               
                defectiveorDamagedViewModel = new DefectiveorDamagedViewModel();
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
                defectiveorDamagedViewModel.TechniciansList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(defectiveorDamagedViewModel);
        }

        #region GetAllDefectiveDamaged
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllDefectiveDamaged()
        {
            UA ua = new UA();
            List<DefectiveorDamagedViewModel> defectiveDamagedList = Mapper.Map<List<DefectiveDamage>, List<DefectiveorDamagedViewModel>>(_iDefectiveDamageBusiness.GetAllDefectiveDamaged(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = defectiveDamagedList });

        }
        #endregion GetAllDefectiveDamaged

        #region GetAllItemCode    
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllItemCode(ItemDropdownViewModel obj)
        {
            UA ua = new UA();
            List<ItemViewModel> ItemCodeList = Mapper.Map<List<Item>, List<ItemViewModel>>(_iItemBusiness.GetAllItemCode(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemCodeList });

        }
        #endregion GetAllDefectiveDamaged

        #region GetDefectiveDamagedByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetDefectiveDamagedByID(string ID)
        {
            UA ua = new UA();
            List<DefectiveorDamagedViewModel> defectiveDamagedList = Mapper.Map<List<DefectiveDamage>, List<DefectiveorDamagedViewModel>>(_iDefectiveDamageBusiness.GetDefectiveDamagedByID(ua,ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = defectiveDamagedList });

        }
        #endregion GetDefectiveDamagedByID


        #region InsertUpdateDefectDamaged
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateDefectDamaged(DefectiveorDamagedViewModel defectiveorDamagedViewModelObj)
        {
            object result = null;
            if(defectiveorDamagedViewModelObj.ID!=Guid.Empty)
            {
                ModelState.Remove("Type");
                ModelState.Remove("EmpID");
            }
            if(defectiveorDamagedViewModelObj.Type== "Damaged")
            {
                ModelState.Remove("EmpID");
            }
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    defectiveorDamagedViewModelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    defectiveorDamagedViewModelObj.logDetails.CreatedBy = ua.UserName;
                    defectiveorDamagedViewModelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    defectiveorDamagedViewModelObj.logDetails.UpdatedBy = defectiveorDamagedViewModelObj.logDetails.CreatedBy;
                    defectiveorDamagedViewModelObj.logDetails.UpdatedDate = defectiveorDamagedViewModelObj.logDetails.CreatedDate;
                    defectiveorDamagedViewModelObj.SCCode = ua.SCCode;
                    
                    result = _iDefectiveDamageBusiness.InsertUpdateDefectiveDamaged(Mapper.Map<DefectiveorDamagedViewModel, DefectiveDamage>(defectiveorDamagedViewModelObj));
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
        #endregion InsertUpdateDefectDamaged

        #region DeleteDefectiveDamaged
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteDefectiveDamaged(string ID)
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
                        status = _iDefectiveDamageBusiness.DeleteDefectiveDamaged(ID, ua);
                    }
                    switch (status)
                    {
                        case "0":
                            msg = c.DeleteSuccess;                          
                            break;
                        case "1":
                            msg = c.DeleteFailure;
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
        #endregion DeleteDefectiveDamaged

        #region ReturnDefectiveDamaged
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string ReturnDefectiveDamaged(DefectiveorDamagedViewModel defectiveorDamagedViewModelObj)
        {
            string status = null;
            string msg = null;
            

                try
                {
                    UA ua = new UA();
                    if ((defectiveorDamagedViewModelObj.ID)!=Guid.Empty)
                    {
                        status = _iDefectiveDamageBusiness.ReturnDefectiveDamaged(defectiveorDamagedViewModelObj.ID.ToString(), ua);
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
        #endregion ReturnDefectiveDamaged

        #region DefectiveDamagedValidation
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DefectiveDamagedValidation(string itemID, string empID,string type)
        {
            string status = null;
            //string msg = null;
            
                try
                {
                if (!string.IsNullOrEmpty(itemID) )
                {
                    UA ua = new UA();
                    
                        status = _iDefectiveDamageBusiness.DefectiveDamagedValidation(itemID,empID,type, ua);
                  
                 
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
        #endregion DefectiveDamagedValidation

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
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    break;
               
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.DisableReason= "Not applicable for new Defective/Damaged";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Defective/Damaged";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new Defective/Damaged";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Disable = true;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return To Company";
                    ToolboxViewModelObj.returnBtn.DisableReason = "Not applicable for new Defective/Damaged";
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
                    ToolboxViewModelObj.savebtn.Title = "Save Defective/Damaged";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Defective/Damaged";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return To Company";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnToCompany();";

                    break;
                case "Return":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}