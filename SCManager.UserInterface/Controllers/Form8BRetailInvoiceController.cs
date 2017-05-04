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

namespace SCManager.UserInterface.Controllers
{
    public class Form8BRetailInvoiceController : Controller
    {
        #region Constructor_Injection

        IForm8BRetailInvoiceBusiness _form8BRetailInvoiceBusiness;
        Const c = new Const();

        public Form8BRetailInvoiceController(IForm8BRetailInvoiceBusiness form8BRetailInvoiceBusiness)
        {
            _form8BRetailInvoiceBusiness = form8BRetailInvoiceBusiness;

        }
        #endregion Constructor_Injection

        // GET: Form8BRetailInvoice
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllForm8
        [HttpGet]
        public string GetAllForm8B(Form8BViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                List<Form8BViewModel> Form8BList = Mapper.Map<List<Form8B>, List<Form8BViewModel>>(_form8BRetailInvoiceBusiness.GetAllForm8B(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Form8BList });
            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetAllForm8


        [HttpPost]
        public string InsertUpdateForm8B(Form8BViewModel Form8BObj)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(Form8BObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    Form8BObj.Form8BDetail = JsonConvert.DeserializeObject<List<Form8BDetailViewModel>>(ReadableFormat);
                    Form8BViewModel r = Mapper.Map<Form8B, Form8BViewModel>(_form8BRetailInvoiceBusiness.InsertUpdate(Mapper.Map<Form8BViewModel, Form8B>(Form8BObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
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
        public string DeleteForm8B(Form8ViewModel Form8BObj)
        {

            try
            {
                if (Form8BObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _form8BRetailInvoiceBusiness.DeleteForm8B(Form8BObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }

        [HttpGet]
        public string DeleteForm8BDetail(Form8BDetailViewModel Form8BObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = Form8BObj.ID.GetValueOrDefault();
                Guid HeaderID = Form8BObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _form8BRetailInvoiceBusiness.DeleteForm8BDetail(ID, HeaderID, ua);
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
        public string GetForm8B(Form8ViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                Form8BViewModel Frm8 = Mapper.Map<Form8B, Form8BViewModel>(_form8BRetailInvoiceBusiness.GetForm8B(dataObj.ID.GetValueOrDefault(), ua));
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
                    ToolboxViewModelObj.addbtn.Title = "Add New Invoice";
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