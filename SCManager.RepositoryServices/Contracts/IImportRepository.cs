using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IImportRepository
    {
        List<UploadedFiles> GetAllUploadedFile();
        UploadedFiles InsertAttachment(UploadedFiles uploadedFile);
        DataTable GetExcelDataToTable(UploadedFiles uploadedFileObj, string fileName, int flag);
        UploadedFiles UpdateUploadedFileDetail(UploadedFiles uploadedFileObj, UA ua, int rowCount);
    }
}
