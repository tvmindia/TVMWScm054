using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ImportController : Controller
    {
        #region Constructor_injection
        Const c = new Const();
        IImportBusiness _importBusiness;
        Common common = new Common();
        public ImportController(IImportBusiness importBusiness)
        {
            _importBusiness = importBusiness;
        }
        #endregion Constructor_injection

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            return View();
        }


        #region GetAllUploadedFile
        [HttpGet]

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllUploadedFile()
        {
            try
            {
                List<UploadedFiles> uploadedFileList = _importBusiness.GetAllUploadedFile();
                return JsonConvert.SerializeObject(new { Result = "OK", Records = uploadedFileList });
            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllUploadedFile

        #region UploadFile
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult UploadFile()
        {
            UploadedFilesViewModel uploadedFilesVM = new UploadedFilesViewModel();
            UploadedFiles uploadedFilesObj = new UploadedFiles();
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;

            HttpPostedFileBase file = files[0];
            try
            {
                UA ua = new UA();
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (ValidateFileName(fname) != "success")
                    {
                        return Json(new { Result = "WARNING", Message = "Invalid Filename!" });
                    }

                    if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/Uploads/"), fname)) == false)
                    {
                        uploadedFilesVM.FilePath = SetFilePath(fname);
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "File uploaded recently" });
                    }

                    UploadedFilesViewModel uploadedFiles = Mapper.Map<UploadedFiles, UploadedFilesViewModel>((
                        from i in _importBusiness.GetAllUploadedFile()
                        where uploadedFilesVM.FilePath.Equals("/Content/Uploads/" + i.FilePath)
                        select i).ToArray().FirstOrDefault()
                    );
                    uploadedFiles.logDetails = new LogDetailsViewModel();
                    uploadedFiles.logDetails.CreatedBy = ua.UserName;
                    uploadedFiles.logDetails.CreatedDate = ua.GetCurrentDateTime();
                    uploadedFiles.logDetails.UpdatedBy = ua.UserName;
                    uploadedFiles.logDetails.UpdatedDate = ua.GetCurrentDateTime();
                    fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                    file.SaveAs(fname);
                    uploadedFilesObj = _importBusiness.ImportDataToDB(Mapper.Map<UploadedFilesViewModel, UploadedFiles>(uploadedFiles), fname, ua);

                }
                else
                {
                    return Json(new { Result = "WARNING", Message = "No files selected." });
                }
                return Json(new
                {
                    Result = "OK",
                    Message = "File Uploaded Successfully!",
                    ImportForm8List = JsonConvert.SerializeObject(uploadedFilesObj.Form8List),
                    TotalCount = uploadedFilesObj.RecordCount,
                    RemovedCount = uploadedFilesObj.RemovedDataCount,
                    FileName = Path.GetFileName(uploadedFilesObj.FilePath)
                });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "EXCEPTION", Message = ex.Message });
            }
        }
        #endregion UploadFile

        #region ValidateUploadFile
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ValidateUploadFile()
        {
            UploadedFilesViewModel uploadedFilesVM = new UploadedFilesViewModel();
            UploadedFiles uploadFilesObj = new UploadedFiles();
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;

            HttpPostedFileBase file = files[0];
            try
            {
                UA ua = new UA();
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    string fname;
                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    if (ValidateFileName(fname) != "success")
                    {
                        return Json(new { Result = "WARNING", Message = "Invalid File, Either filename or filetype mismatch !" });
                    }

                    if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/Uploads/"), fname)) == false)
                    {
                        uploadedFilesVM.FilePath = SetFilePath(fname);
                    }
                    else
                    {
                        return Json(new { Result = "ERROR", Message = "File uploaded recently" });
                    }
                    uploadedFilesVM.logDetails = new LogDetailsViewModel();
                    uploadedFilesVM.logDetails.CreatedBy = ua.UserName;
                    uploadedFilesVM.logDetails.CreatedDate = ua.GetCurrentDateTime();
                    uploadedFilesVM.logDetails.UpdatedBy = ua.UserName;
                    uploadedFilesVM.logDetails.UpdatedDate = ua.GetCurrentDateTime();
                    uploadedFilesVM.FileType = "Form8";

                    List<UploadedFiles> uploadedFileList = _importBusiness.GetAllUploadedFile();
                    object fileExist = (from i in uploadedFileList where uploadedFilesVM.FilePath.Equals("/Content/Uploads/" + i.FilePath) && i.FileStatus.Equals("Successfully Imported") select i).FirstOrDefault();
                    if (fileExist != null)
                    {
                        return Json(new { Result = "ERROR", Message = "File Already Imported!" });
                    }
                    UploadedFiles uploadedFiles = _importBusiness.InsertAttachment(Mapper.Map<UploadedFilesViewModel, UploadedFiles>(uploadedFilesVM));

                    fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                    file.SaveAs(fname);
                    uploadFilesObj = _importBusiness.ValidateImportData(uploadedFiles, fname, ua);
                    System.IO.File.Delete(fname);
                }
                else
                {
                    return Json(new { Result = "WARNING", Message = "No files selected. Select one and upload" });
                }
                int Count = uploadFilesObj.Form8List != null ? uploadFilesObj.RecordCount - uploadFilesObj.Form8List.Count : uploadFilesObj.RecordCount;
                return Json(new
                {
                    Result = "OK",
                    Message = "File Validated Successfully!",
                    ImportList = JsonConvert.SerializeObject(uploadFilesObj.Form8List),
                    TotalCount = uploadFilesObj.RecordCount,
                    RemovedCount = uploadFilesObj.RemovedDataCount,
                    FileName = Path.GetFileName(uploadFilesObj.FilePath)
                });

            }
            catch (Exception ex)
            {
                System.IO.File.Delete(Server.MapPath("~/Content/Uploads/" + file.FileName));
                return Json(new { Result = "EXCEPTION", Message = ex.Message });
            }
        }
        #endregion ValidateUploadFile


        #region ValidateFileName
        string ValidateFileName(string fname)
        {
            var regEx = new Regex(@"^Form8_([0]?[0-9]|[12][0-9]|[3][01])[.]([0]?[1-9]|[1][0-2])[.]([0-9]{4}|[0-9]{2})_F[0-9]+?.xlsx$");
            if (regEx.IsMatch(fname))
            {
                return ("success");
            }
            return ("failed");
        }
        #endregion ValidateFileName

        #region SetFilePath
        string SetFilePath(string fname)
        {
            string[] allFiles = Directory.GetFiles(Server.MapPath("~/Content/Uploads/"));
            foreach (string aFile in allFiles)
            {
                FileInfo fInfo = new FileInfo(aFile);
                if ((fInfo.CreationTime < DateTime.Now.AddHours(-12)) && fInfo.Extension.Equals(".xlsx"))//Gets only .xlsx files older than 12 hrs. 
                {
                    if (!fInfo.Name.Equals("Form8_00.00.0000_F0.xlsx"))
                    { fInfo.Delete(); }
                }
            }

            return "/Content/Uploads/" + fname;
        }
        #endregion  SetFilePath

        #region DownloadTemplate
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult DownloadTemplate()
        {
            string filename = "Form8_00.00.0000_F0.xlsx";
            string filepath = Path.Combine(Server.MapPath("~/Content/Uploads/Form8_00.00.0000_F0.xlsx"));
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";//web content type of .xlsx files
            return File(filepath, contentType, filename);
        }
        #endregion DownloadTemplate

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.downloadBtn.Visible = true;
                    ToolboxViewModelObj.downloadBtn.Text = "Template";
                    ToolboxViewModelObj.downloadBtn.Title = "Download Template";
                    ToolboxViewModelObj.downloadBtn.Event = "DownloadTemplate();";

                    ToolboxViewModelObj.HistoryBtn.Visible = true;
                    ToolboxViewModelObj.HistoryBtn.Text = "History";
                    ToolboxViewModelObj.HistoryBtn.Title = "Uploaded Files History";
                    ToolboxViewModelObj.HistoryBtn.Event = "FetchHistory();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
    }
}
    