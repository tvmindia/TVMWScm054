using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SCManager.RepositoryServices.Services
{
    public class AssignBillBookRepository : IAssignBillBookRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public AssignBillBookRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllBillBook
        public List<AssignBillBook> GetAllBillBook(UA UA)
        {
            List<AssignBillBook> AssignBillBooklist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;                       
                        cmd.CommandText = "[GetAllBillBook]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                AssignBillBooklist = new List<AssignBillBook>();
                                while (sdr.Read())
                                {
                                    AssignBillBook assignBillBookObj = new AssignBillBook();

                                    {
                                        assignBillBookObj.LastUsed = (sdr["LastUsed"].ToString() != "" ? (sdr["LastUsed"].ToString()) : assignBillBookObj.LastUsed);
                                        assignBillBookObj.Status = (sdr["Status"].ToString() != "" ? (sdr["Status"].ToString()) : assignBillBookObj.Status);
                                        assignBillBookObj.Technician = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : assignBillBookObj.Technician);
                                        assignBillBookObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : assignBillBookObj.Remarks);
                                        assignBillBookObj.BillNo = (sdr["BookNo"].ToString() != "" ? (sdr["BookNo"].ToString()) : assignBillBookObj.BillNo);
                                        assignBillBookObj.SeriesStart = (sdr["SeriesStart"].ToString() != "" ? (sdr["SeriesStart"].ToString()) : assignBillBookObj.SeriesStart);
                                        assignBillBookObj.SeriesEnd = (sdr["SeriesEnd"].ToString() != "" ? (sdr["SeriesEnd"].ToString()) : assignBillBookObj.SeriesEnd);
                                        assignBillBookObj.BillBookType = (sdr["BillBookType"].ToString() != "" ? (sdr["BillBookType"].ToString()) : assignBillBookObj.BillBookType);
                                        assignBillBookObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : assignBillBookObj.ID);

                                    };

                                    AssignBillBooklist.Add(assignBillBookObj);
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
            return AssignBillBooklist;
        }
        #endregion GetAllBillBook

        #region GeBillBookByID
        public List<AssignBillBook> GeBillBookByID(UA UA, string ID)
        {
            List<AssignBillBook> AssignBillBooklist = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.CommandText = "[GetBillBookByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                AssignBillBooklist = new List<AssignBillBook>();
                                while (sdr.Read())
                                {
                                    AssignBillBook assignBillBookObj = new AssignBillBook();

                                    {
                                        assignBillBookObj.LastUsed = (sdr["LastUsed"].ToString() != "" ? (sdr["LastUsed"].ToString()) : assignBillBookObj.LastUsed);
                                        assignBillBookObj.Status = (sdr["Status"].ToString() != "" ? (sdr["Status"].ToString()) : assignBillBookObj.Status);
                                        assignBillBookObj.EmpID = (sdr["TechID"].ToString() != "" ? Guid.Parse(sdr["TechID"].ToString()) : assignBillBookObj.EmpID);
                                        assignBillBookObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : assignBillBookObj.Remarks);
                                        assignBillBookObj.BillNo = (sdr["BookNo"].ToString() != "" ? (sdr["BookNo"].ToString()) : assignBillBookObj.BillNo);
                                        assignBillBookObj.SeriesStart = (sdr["SeriesStart"].ToString() != "" ? (sdr["SeriesStart"].ToString()) : assignBillBookObj.SeriesStart);
                                        assignBillBookObj.BillBookType = (sdr["BillBookType"].ToString() != "" ? (sdr["BillBookType"].ToString()) : assignBillBookObj.BillBookType);
                                        assignBillBookObj.SeriesEnd = (sdr["SeriesEnd"].ToString() != "" ? (sdr["SeriesEnd"].ToString()) : assignBillBookObj.SeriesEnd);
                                        assignBillBookObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : assignBillBookObj.ID);

                                    };

                                    AssignBillBooklist.Add(assignBillBookObj);
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
            return AssignBillBooklist;
        }
        #endregion GeBillBookByID
             

        #region GetMissingSerials
        public DataSet GetMissingSerials(string seriesStart, string seriesEnd,string BillBookType,UA UA)
        {
           
            DataSet ds = null;
            SqlDataAdapter sda = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        if(! string.IsNullOrEmpty(seriesStart))
                        {
                            cmd.Parameters.Add("@start", SqlDbType.Int).Value = int.Parse(seriesStart);
                        }
                       if(! string.IsNullOrEmpty(seriesEnd))
                        {
                            cmd.Parameters.Add("@end", SqlDbType.Int).Value = int.Parse(seriesEnd);
                        }
                        
                        cmd.Parameters.Add("@BillBookType", SqlDbType.NVarChar, 15).Value = BillBookType;
                        cmd.CommandText = "[GetMissingSerials]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        ds = new DataSet();
                        sda.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        #endregion GetMissingSerials

        #region InsertBillBook
        public object InsertBillBook(AssignBillBook assignBillBook)
        {
            SqlParameter outputStatus, outputID = null;
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
                        cmd.CommandText = "[InsertBillBook]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = assignBillBook.SCCode;
                        cmd.Parameters.Add("@SeriesStart", SqlDbType.NVarChar, 50).Value = assignBillBook.SeriesStart;
                        cmd.Parameters.Add("@SeriesEnd", SqlDbType.NVarChar, 50).Value = assignBillBook.SeriesEnd;
                        cmd.Parameters.Add("@LastUsed", SqlDbType.NVarChar, 50).Value = assignBillBook.LastUsed;
                        cmd.Parameters.Add("@TechID", SqlDbType.UniqueIdentifier).Value = assignBillBook.EmpID;
                        cmd.Parameters.Add("@BookStatus", SqlDbType.Bit).Value = assignBillBook.Status;                       
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = assignBillBook.Remarks;
                        cmd.Parameters.Add("@BillBookType", SqlDbType.NVarChar, 15).Value = assignBillBook.BillBookType;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = assignBillBook.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = assignBillBook.logDetails.CreatedDate;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }              

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                ID = Guid.Parse(outputID.Value.ToString()),
                Status = outputStatus.Value.ToString(),
                Message = c.InsertSuccess
            };
        }
        #endregion InsertBillBook

        #region UpdateBillBook
        public object UpdateBillBook(AssignBillBook assignBillBook)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[UpdateBillBook]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = assignBillBook.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = assignBillBook.SCCode;
                        cmd.Parameters.Add("@SeriesStart", SqlDbType.NVarChar, 50).Value = assignBillBook.SeriesStart;
                        cmd.Parameters.Add("@SeriesEnd", SqlDbType.NVarChar, 50).Value = assignBillBook.SeriesEnd;
                        cmd.Parameters.Add("@LastUsed", SqlDbType.NVarChar, 50).Value = assignBillBook.LastUsed;
                        cmd.Parameters.Add("@TechID", SqlDbType.UniqueIdentifier).Value = assignBillBook.EmpID;
                        cmd.Parameters.Add("@BookStatus", SqlDbType.Bit).Value = assignBillBook.Status;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = assignBillBook.Remarks;
                        cmd.Parameters.Add("@BillBookType", SqlDbType.NVarChar, 15).Value = assignBillBook.BillBookType;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = assignBillBook.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = assignBillBook.logDetails.CreatedDate;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                       
                        cmd.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {              
                Status = outputStatus.Value.ToString(),
                Message = c.InsertSuccess
            };
        }
        #endregion UpdateBillBook

        #region DeleteBillBook
        public string DeleteBillBook(string ID, UA ua)
        {
            SqlParameter outParameter = null;
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
                        cmd.CommandText = "[DeleteBillBook]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outParameter.Value.ToString();
        }
        #endregion DeleteBillBook

        #region BillBookRangeValidation
        public string BillBookRangeValidation(string seriesStart,string seriesEnd,string BillNo,string BillBookType, UA UA)
        {
            SqlParameter outParameter = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        if(! string.IsNullOrEmpty(seriesStart))
                        {
                            cmd.Parameters.Add("@start", SqlDbType.Int).Value = int.Parse(seriesStart);
                        }
                        if(! string.IsNullOrEmpty(seriesEnd))
                        {
                            cmd.Parameters.Add("@end", SqlDbType.Int).Value = int.Parse(seriesEnd);
                        }
                                             
                        cmd.Parameters.Add("@BookNo", SqlDbType.NVarChar, 50).Value = BillNo;
                        cmd.Parameters.Add("@BillBookType", SqlDbType.NVarChar, 15).Value = BillBookType;
                        cmd.CommandText = "[BillBookRangeValidation]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outParameter.Value.ToString();
        }
        #endregion BillBookRangeValidation
    }
}