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
    public class IssueToTechnicianRepository : IIssueToTechnicianRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public IssueToTechnicianRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetIssueSheets
        public List<IssueToTechnician> GetIssueSheets(string empID,string transferDate, UA UA)
        {
            List<IssueToTechnician> issueToTechnicianList = null;
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
                        if(!string.IsNullOrEmpty(empID))
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        }
                        if (!string.IsNullOrEmpty(transferDate))
                        {
                            cmd.Parameters.Add("@transferDate", SqlDbType.DateTime).Value = DateTime.Parse(transferDate);
                        }
                        cmd.CommandText = "[GetIssueSheets]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                issueToTechnicianList = new List<IssueToTechnician>();
                                while (sdr.Read())
                                {
                                    IssueToTechnician _IssueToTechnicianObj = new IssueToTechnician();
                                    {
                                        _IssueToTechnicianObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _IssueToTechnicianObj.ID);
                                        _IssueToTechnicianObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _IssueToTechnicianObj.SCCode);
                                        _IssueToTechnicianObj.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : _IssueToTechnicianObj.MaterialID);
                                        _IssueToTechnicianObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);                                  
                                        _IssueToTechnicianObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _IssueToTechnicianObj.Material);
                                        _IssueToTechnicianObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _IssueToTechnicianObj.Description);
                                        _IssueToTechnicianObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _IssueToTechnicianObj.UOM);
                                    }

                                    issueToTechnicianList.Add(_IssueToTechnicianObj);
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
            return issueToTechnicianList;
        }
        #endregion GetIssueSheets

        #region GetAllIssueToTechnician
        public List<IssueToTechnician> GetAllIssueToTechnician(string empID,string fromDate,string toDate, UA UA)
        {
            List<IssueToTechnician> issueToTechnicianList = null;
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
                        if(!string.IsNullOrEmpty(empID) && empID!="All")
                        {
                            cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        }
                       if(empID=="All")
                        {
                            cmd.Parameters.Add("@AllTech",SqlDbType.NVarChar,5).Value=empID;
                        }
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DateTime.Parse(fromDate);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DateTime.Parse(toDate);
                        }
                        cmd.CommandText = "[GetAllIssueToTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                issueToTechnicianList = new List<IssueToTechnician>();
                                while (sdr.Read())
                                {
                                    IssueToTechnician _IssueToTechnicianObj = new IssueToTechnician();
                                    {
                                        _IssueToTechnicianObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _IssueToTechnicianObj.ID);
                                        _IssueToTechnicianObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _IssueToTechnicianObj.SCCode);
                                        _IssueToTechnicianObj.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : _IssueToTechnicianObj.MaterialID);
                                        _IssueToTechnicianObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _IssueToTechnicianObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _IssueToTechnicianObj.Material);
                                        _IssueToTechnicianObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _IssueToTechnicianObj.Description);
                                        _IssueToTechnicianObj.empName = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : _IssueToTechnicianObj.empName);
                                        _IssueToTechnicianObj.IssueDate = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()).ToString("dd-MMM-yyyy") : _IssueToTechnicianObj.IssueDate);
                                        _IssueToTechnicianObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _IssueToTechnicianObj.UOM);
                                    }

                                    issueToTechnicianList.Add(_IssueToTechnicianObj);
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
            return issueToTechnicianList;
        }
        #endregion GetAllIssueToTechnician

        #region InsertIssueToTechnician

        public List<IssueToTechnician> InsertIssueToTechnician(IssueToTechnician issueToTechnician, Guid? empID, string issueDate, UA UA)
        {
            List<IssueToTechnician> issueToTechList = null;
            try
            {
                
                SqlParameter outputStatus, outputID = null;
               // issueToTechnician=new List<IssueToTechnician>();
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[InsertIssueToTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = empID;
                        cmd.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = issueDate;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = issueToTechnician.MaterialID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = issueToTechnician.Qty;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputID = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                                               

                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        issueToTechnician.ID = new Guid(outputID.Value.ToString());
                        issueToTechList = GetIssueSheets(issueToTechnician.ID.ToString(), "", UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return issueToTechList;
        }

        #endregion InsertIssueToTechnician

        #region UpdateIssueToTechnician

        public List<IssueToTechnician> UpdateIssueToTechnician(IssueToTechnician issueToTechnician, Guid? empID, string issueDate, UA UA)
        {
            List<IssueToTechnician> issueToTechList = null;
            try
            {

                SqlParameter outputStatus = null;
                // issueToTechnician=new List<IssueToTechnician>();
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[UpdateIssueToTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = issueToTechnician.ID;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = issueToTechnician.MaterialID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = issueToTechnician.Qty;

                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.InsertFailure);
                    case "1":
                        issueToTechList = GetIssueSheets(issueToTechnician.ID.ToString(), "", UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return issueToTechList;
        }

        #endregion UpdateIssueToTechnician

        #region DeleteIssueToTechnician
        public string DeleteIssueToTechnician(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteIssueToTechnician]";
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
        #endregion DeleteIssueToTechnician

    }
}