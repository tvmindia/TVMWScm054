using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SCManager.UserInterface.Models;
using SCManager.DataAccessObject.DTO;
using SCManager.BusinessService.Contracts;
using System.Web.Script.Serialization;
using SCManager.UserInterface.CustomAttributes;


namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ReturnBillController : Controller
    {
        // GET: ReturnBill
        #region Constructor_Injection

        IReturnBillBusiness _returnBillBusiness;

        public ReturnBillController(IReturnBillBusiness returnBillBusiness)
        {
            _returnBillBusiness = returnBillBusiness;

        }
        #endregion Constructor_Injection
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            UA ua = new UA();
            ReturnBillViewModel returnBillViewModel = null;
            try
            {
                returnBillViewModel = new ReturnBillViewModel();

                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //TicketNo Drop down bind
                List<ReturnBillViewModel> TicketNoList = Mapper.Map<List<ReturnBill>, List<ReturnBillViewModel>>(_returnBillBusiness.GetAllTicketNo(ua));
                foreach (ReturnBillViewModel clvm in TicketNoList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = clvm.TicketNo,
                        Value = clvm.TicketNo,
                        Selected = false
                    });
                }
                returnBillViewModel.TicketNoList = selectListItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(returnBillViewModel);

        }

        Const c = new Const();

        #region GetAllReturnBill
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllReturnBill()
        {
            try
            {
                UA ua = new UA();
                List<ReturnBillViewModel> ReturnBillList = Mapper.Map<List<ReturnBill>, List<ReturnBillViewModel>>(_returnBillBusiness.GetAllReturnBill(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ReturnBillList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetAllReturnBill

        #region GetFranchiseeDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllFranchiseeDetail()
        {
            try
            {
                UA ua = new UA();
                List<ReturnBillViewModel> ReturnBillList = Mapper.Map<List<ReturnBill>, List<ReturnBillViewModel>>(_returnBillBusiness.GetAllFranchiseeDetail(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ReturnBillList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetFranchiseeDetail


        #region GetSupplierDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetSupplierDetail()
        {
            try
            {
                UA ua = new UA();
                List<ReturnBillViewModel> ReturnBillList = Mapper.Map<List<ReturnBill>, List<ReturnBillViewModel>>(_returnBillBusiness.GetSupplierDetail(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ReturnBillList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetSupplierDetail

        #region GetMaterialsFromDefectiveDamaged
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetMaterialsFromDefectiveDamaged(string TicketNo)
        {
            try
            {
                UA ua = new UA();
                List<ReturnBillDetailViewModel> ReturnBillTicketNoList = Mapper.Map<List<ReturnBillDetail>, List<ReturnBillDetailViewModel>>(_returnBillBusiness.GetMaterialsFromDefectiveDamaged(TicketNo, ua.SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ReturnBillTicketNoList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion GetMaterialsFromDefectiveDamaged

        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateReturnBill(ReturnBillViewModel ReturnBillObj)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(ReturnBillObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    ReturnBillObj.ReturnBillDetail = JsonConvert.DeserializeObject<List<ReturnBillDetailViewModel>>(ReadableFormat);
                    ReturnBillViewModel r = Mapper.Map<ReturnBill, ReturnBillViewModel>(_returnBillBusiness.InsertUpdate(Mapper.Map<ReturnBillViewModel, ReturnBill>(ReturnBillObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
                }

                else
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(ReturnBillObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    ReturnBillObj.ReturnBillDetail = JsonConvert.DeserializeObject<List<ReturnBillDetailViewModel>>(ReadableFormat);
                    ReturnBillViewModel r = Mapper.Map<ReturnBill, ReturnBillViewModel>(_returnBillBusiness.InsertUpdate(Mapper.Map<ReturnBillViewModel, ReturnBill>(ReturnBillObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.UpdateSuccess, Records = r });
                }

            }
            catch (Exception ex)
            {

                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            return result;
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteReturnBill(ReturnBillViewModel ReturnBillObj)
        {

            try
            {
                if (ReturnBillObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _returnBillBusiness.DeleteReturnBill(ReturnBillObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteReturnBillDetail(ReturnBillDetailViewModel ReturnBillObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = ReturnBillObj.ID.GetValueOrDefault();
                Guid HeaderID = ReturnBillObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _returnBillBusiness.DeleteReturnBillDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetReturnBill(ReturnBillViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                ReturnBillViewModel Rtb = Mapper.Map<ReturnBill, ReturnBillViewModel>(_returnBillBusiness.GetReturnBill(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Rtb.ReturnBillDetail });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }


        #region GetReturnBillHeaderByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetReturnBillHeaderByID(ReturnBillViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                ReturnBillViewModel result = Mapper.Map<ReturnBill, ReturnBillViewModel>(_returnBillBusiness.GetReturnBillHeaderByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetReturnBillHeaderByID


        #region ReturnDefectiveDamaged
        [HttpGet]
        //[ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string ReturnDefectiveDamaged(string HeaderID,string TicketNo)
        {
            string status = null;
            string msg = null;

            UA ua = new UA();
            try
            {
                status = _returnBillBusiness.ReturnDefectiveDamaged(HeaderID.ToString(), ua, TicketNo);
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
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill";                    
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";
                   
                    ToolboxViewModelObj.savebtn.Visible = true;                   
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";                    
                    ToolboxViewModelObj.savebtn.Event = "save();";
                   

                    ToolboxViewModelObj.deletebtn.Visible = true;                    
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";                   
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";
                    

                    ToolboxViewModelObj.resetbtn.Visible = true;                    
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";           
                    ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";
                   

                    ToolboxViewModelObj.returnBtn.Visible = true;                   
                    ToolboxViewModelObj.returnBtn.Disable = false;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return Bill";                   
                    ToolboxViewModelObj.returnBtn.Event = "ReturnClick();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Print";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToPdf();";



                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new item";
                    ToolboxViewModelObj.deletebtn.Event = "";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Return Bill";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new item";
                    ToolboxViewModelObj.deletebtn.Event = "";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Disable = false;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return Bill";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnToCompany();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Print";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToPdf();";

                    break;

                case "Return":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill";
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Disable = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";
                    ToolboxViewModelObj.savebtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Disable = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Disable = false;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return Bill";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnClick();";
                    ToolboxViewModelObj.returnBtn.Disable = true;
                    ToolboxViewModelObj.returnBtn.DisableReason = "Bill Returned";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Print";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToPdf();";

                    break;

                case "EditReturn":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill";
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Disable = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";
                    ToolboxViewModelObj.savebtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Disable = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.DisableReason = "N/A for Return Bill";
                    ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";

                    ToolboxViewModelObj.returnBtn.Visible = true;
                    ToolboxViewModelObj.returnBtn.Disable = true;
                    ToolboxViewModelObj.returnBtn.Text = "Return";
                    ToolboxViewModelObj.returnBtn.Title = "Return Bill";
                    ToolboxViewModelObj.returnBtn.Event = "ReturnClick();";
                    ToolboxViewModelObj.returnBtn.Disable = true;
                    ToolboxViewModelObj.returnBtn.DisableReason = "Bill Returned";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Print";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToPdf();";

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

       


    
