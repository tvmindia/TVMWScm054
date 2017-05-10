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
    public class CreditNotesController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection

        ICreditNotesBusiness _iCreditNotesBusiness;
       
        public CreditNotesController(ICreditNotesBusiness iCreditNotesBusiness)
        {
            _iCreditNotesBusiness = iCreditNotesBusiness;
           
        }
        #endregion Constructor_Injection

        // GET: CreditNotes
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllCreditNotes
        [HttpGet]
        public string GetAllCreditNotes(string showAllYN)
        {
            UA ua = new UA();
            List<CreditNotesViewModel> CreditNotesList = Mapper.Map<List<CreditNotes>, List<CreditNotesViewModel>>(_iCreditNotesBusiness.GetAllCreditNotes(ua,showAllYN));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetAllCreditNotes
        #region GetCreditNotesBetweenDates
        [HttpGet]
        public string GetCreditNotesBetweenDates(string fromDate, string toDate)
        {
            UA ua = new UA();
            List<CreditNotesViewModel> CreditNotesList = Mapper.Map<List<CreditNotes>, List<CreditNotesViewModel>>(_iCreditNotesBusiness.GetCreditNotesBetweenDates(ua,fromDate,toDate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetCreditNotesBetweenDates

        #region GetCreditNotesByID
        [HttpGet]
        public string GetCreditNotesByID(string ID)
        {
            UA ua = new UA();
            List<CreditNotesViewModel> CreditNotesList = Mapper.Map<List<CreditNotes>, List<CreditNotesViewModel>>(_iCreditNotesBusiness.GetCreditNotesByID(ua, ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetCreditNotesByID

        #region DeleteCreditNote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteCreditNote(string ID)
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
                        status = _iCreditNotesBusiness.DeleteCreditNote(ID, ua);
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
        #endregion DeleteCreditNote

        #region InsertUpdateCreditNotes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateCreditNotes(CreditNotesViewModel creditNotesViewModelViewModelObj)
        {
            object result = null;
            if (creditNotesViewModelViewModelObj.ID != Guid.Empty)
            {
                ModelState.Remove("CreditNoteNo");                
            }
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    creditNotesViewModelViewModelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    creditNotesViewModelViewModelObj.logDetails.CreatedBy = ua.UserName;
                    creditNotesViewModelViewModelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    creditNotesViewModelViewModelObj.logDetails.UpdatedBy = creditNotesViewModelViewModelObj.logDetails.CreatedBy;
                    creditNotesViewModelViewModelObj.logDetails.UpdatedDate = creditNotesViewModelViewModelObj.logDetails.CreatedDate;
                    creditNotesViewModelViewModelObj.SCCode = ua.SCCode;

                    result = _iCreditNotesBusiness.InsertUpdateCreditNotes(Mapper.Map<CreditNotesViewModel, CreditNotes>(creditNotesViewModelViewModelObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    if (ex.Message == "CreditNote No already exist")
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
        #endregion InsertUpdateCreditNotes

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
                    ToolboxViewModelObj.deletebtn.Title = "Delete CreditNotes";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new CreditNotes";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";


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
                    ToolboxViewModelObj.savebtn.Title = "Save CreditNotes";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete CreditNotes";
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