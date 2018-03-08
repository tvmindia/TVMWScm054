using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IImportBusiness
    {
        List<UploadedFiles> GetAllUploadedFile();
        UploadedFiles InsertAttachment(UploadedFiles uploadedFile);
        UploadedFiles ValidateImportData(UploadedFiles uploadedFileObj, string filePath,UA ua);
        UploadedFiles ImportDataToDB(UploadedFiles uploadedFileObj, string filePath, UA ua);
    }
}
