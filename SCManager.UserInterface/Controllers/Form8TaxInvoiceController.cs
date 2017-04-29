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

namespace SCManager.UserInterface.Controllers
{
    public class Form8TaxInvoiceController : Controller
    {
        //
        // GET: /Form8TaxInvoice/
         #region Constructor_Injection

        IForm8TaxInvoiceBusiness _form8TaxInvoiceBusiness;

        public Form8TaxInvoiceController(IForm8TaxInvoiceBusiness form8TaxInvoiceBusiness)
        {
            _form8TaxInvoiceBusiness = form8TaxInvoiceBusiness;           
            
        }
        #endregion Constructor_Injection

        public ActionResult Index()
        {
            Form8ViewModel dummy = new Form8ViewModel();
            dummy.ID = Guid.Empty;
            return View(dummy);
        }
        Const c = new Const();

        #region GetAllForm8
        [HttpGet]
        public string GetAllForm8(Form8ViewModel dataObj)
        {
            try
            {
                UA ua=new UA();
                List<Form8ViewModel> Form8List = Mapper.Map<List<Form8>, List<Form8ViewModel>>(_form8TaxInvoiceBusiness.GetAllForm8(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Form8List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetAllForm8

        [HttpPost]
        public string InsertUpdateForm8(Form8ViewModel Form8Obj)
        {
            string result = "";
           
            try
            {
                if (ModelState.IsValid) {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(Form8Obj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    Form8Obj.Form8Detail=JsonConvert.DeserializeObject<List<Form8DetailViewModel>>(ReadableFormat);
                    Form8ViewModel r = Mapper.Map < Form8,  Form8ViewModel > (_form8TaxInvoiceBusiness.InsertUpdate(Mapper.Map<Form8ViewModel, Form8>(Form8Obj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess , Records = r});
                }

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
            return result;
        }

        [HttpGet]
        public string DeleteForm8(Form8ViewModel Form8Obj) {
           
            try
            {
                if (Form8Obj.ID.GetValueOrDefault() == Guid.Empty) {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _form8TaxInvoiceBusiness.DeleteForm8(Form8Obj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            
        }

        [HttpGet]
        public string DeleteForm8Detail(Form8DetailViewModel Form8Obj)
        {

            try
            {
                UA ua = new UA();
                Guid ID= Form8Obj.ID.GetValueOrDefault();
                Guid HeaderID= Form8Obj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {                
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _form8TaxInvoiceBusiness.DeleteForm8Detail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }
                

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }

        [HttpGet]
        public string GetForm8(Form8ViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                Form8ViewModel Frm8 = Mapper.Map<Form8, Form8ViewModel>(_form8TaxInvoiceBusiness.GetForm8(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Frm8 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

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
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                     ToolboxViewModelObj.savebtn.Text = "Save";
                     ToolboxViewModelObj.savebtn.Title = "Save Invoice";
                     ToolboxViewModelObj.savebtn.Event = "save();";

                     ToolboxViewModelObj.deletebtn.Visible = true;
                     ToolboxViewModelObj.deletebtn.Text = "Delete";
                     ToolboxViewModelObj.deletebtn.Title = "Delete Invoice";
                     ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                     ToolboxViewModelObj.resetbtn.Visible = true;
                     ToolboxViewModelObj.resetbtn.Text = "Reset";
                     ToolboxViewModelObj.resetbtn.Title = "Reset";
                     ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";

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
                     ToolboxViewModelObj.savebtn.Title = "Save Invoice";
                     ToolboxViewModelObj.savebtn.Event = "save();";

                     ToolboxViewModelObj.deletebtn.Visible = true;
                     ToolboxViewModelObj.deletebtn.Disable = true;
                     ToolboxViewModelObj.deletebtn.Text = "Delete";
                     ToolboxViewModelObj.deletebtn.Title = "Delete Invoice";
                     ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new item";
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
