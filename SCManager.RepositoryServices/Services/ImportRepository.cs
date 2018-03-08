using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace SCManager.RepositoryServices.Services
{
    public class ImportRepository: IImportRepository
    {
        #region Constructor Injection
        Const Cobj = new Const();
        private IDatabaseFactory _databaseFactory;
        IItemRepository _itemRepository;
        public ImportRepository(IDatabaseFactory databaseFactory, IItemRepository itemRepository)
        {
            _databaseFactory = databaseFactory;
            _itemRepository = itemRepository;


        }
        #endregion Constructor Injection

        #region GetAllUploadedFile
        public List<UploadedFiles> GetAllUploadedFile()
        {
            List<UploadedFiles> uploadedFileList = new List<UploadedFiles>();
            try
            {
                SCManagerSettings setting = new SCManagerSettings();
                string excessPath = "/Content/Uploads/";
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[dbo].[GetAllUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    UploadedFiles uploadedFile = new UploadedFiles();
                                    {
                                        uploadedFile.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : uploadedFile.ID);
                                        uploadedFile.FileType = (sdr["FileType"].ToString() != "" ? (sdr["FileType"].ToString()) : uploadedFile.FileType);
                                        uploadedFile.FilePath = (sdr["FilePath"].ToString() != "" ? sdr["FilePath"].ToString().Replace(excessPath, "") : uploadedFile.FilePath);
                                        uploadedFile.RecordCount = (sdr["RecordCount"].ToString() != "" ? int.Parse(sdr["RecordCount"].ToString()) : uploadedFile.RecordCount);
                                        uploadedFile.FileStatus = (sdr["FileStatus"].ToString() != "" ? sdr["FileStatus"].ToString() : uploadedFile.FileStatus);
                                        uploadedFile.CreatedDate = (sdr["CreatedDate"]).ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(setting.dateformat) : sdr["CreatedDate"].ToString();
                                    }
                                    uploadedFileList.Add(uploadedFile);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uploadedFileList;
        }
        #endregion GetAllUploadedFile

        #region InsertAttachment
        public UploadedFiles InsertAttachment(UploadedFiles uploadedFile)
        {
            try
            {
                SqlParameter outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        cmd.Connection = con;
                        cmd.CommandText = "[dbo].[InsertUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 250).Value = uploadedFile.FilePath;
                        cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 50).Value = uploadedFile.FileType;
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Value = uploadedFile.RecordCount;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = uploadedFile.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = uploadedFile.logDetails.CreatedDate;
                        cmd.Parameters.Add("@FileStatus", SqlDbType.NVarChar, 50).Value = uploadedFile.FileStatus;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    uploadedFile.ID = Guid.Parse(outputID.Value.ToString());
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return uploadedFile;
        }
        #endregion InsertAttachment

        #region GetExcelDataToTable
        public DataTable GetExcelDataToTable(UploadedFiles fileUploadObj, string fname, int flag)
        {
            List<ImportForm8> importForm8List = new List<ImportForm8>();
            DataTable ExcelData = null;
            try
            {
                //Insert all values from excel to datatable
                using (OleDbConnection excel_con = _databaseFactory.GetOleDBConnection(flag, fname))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    ExcelData = new DataTable();

                    //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                    OleDbCommand cmdExcel = new OleDbCommand();
                    cmdExcel.Connection = excel_con;
                    cmdExcel.CommandText = "SELECT * From [" + sheet1 + "]";
                    OleDbDataAdapter oda = new OleDbDataAdapter();
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(ExcelData);
                    excel_con.Close();
                }
                //To remove all Empty Row from DataTable
                ExcelData = ExcelData
                    .Rows.Cast<DataRow>()
                    .Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string)))
                    .CopyToDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ExcelData;
        }
        #endregion GetExcelDataToTable
        
        #region UpdateUploadedFileDetail
        public UploadedFiles UpdateUploadedFileDetail(UploadedFiles uploadedFile, UA ua, int RowCount)
        {
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }

                        cmd.Connection = con;
                        cmd.CommandText = "[dbo].[UpdateUploadedFile]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = uploadedFile.ID;
                        cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 250).Value = uploadedFile.FilePath;
                        cmd.Parameters.Add("@FileType", SqlDbType.NVarChar, 50).Value = uploadedFile.FileType;
                        cmd.Parameters.Add("@RecordCount", SqlDbType.Int).Value = uploadedFile.RecordCount = RowCount;
                        cmd.Parameters.Add("@FileStatus", SqlDbType.NVarChar, 50).Value = uploadedFile.FileStatus;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = ua.UserName;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = ua.GetCurrentDateTime();
                        cmd.ExecuteNonQuery();
                    }
                }
                return uploadedFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion UpdateUploadedFileDetail
        
    }
}