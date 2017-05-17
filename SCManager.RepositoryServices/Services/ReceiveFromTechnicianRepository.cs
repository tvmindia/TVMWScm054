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
    public class ReceiveFromTechnicianRepository : IReceiveFromTechnicianRepository
    {

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ReceiveFromTechnicianRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetReceiptsSheet
        public List<ReceiveFromTechnician> GetReceiptsSheet(string empID, string transferDate, UA UA)
        {
            List<ReceiveFromTechnician> receiveFromTechnicianList = null;
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
                        if (!string.IsNullOrEmpty(empID))
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        }
                        if (!string.IsNullOrEmpty(transferDate))
                        {
                            cmd.Parameters.Add("@transferDate", SqlDbType.DateTime).Value = DateTime.Parse(transferDate);
                        }
                        cmd.CommandText = "[GetReceiptsSheet]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                receiveFromTechnicianList = new List<ReceiveFromTechnician>();
                                while (sdr.Read())
                                {
                                    ReceiveFromTechnician _ReceiveFromTechnicianObj = new ReceiveFromTechnician();
                                    {
                                        _ReceiveFromTechnicianObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReceiveFromTechnicianObj.ID);
                                        _ReceiveFromTechnicianObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReceiveFromTechnicianObj.SCCode);
                                        _ReceiveFromTechnicianObj.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : _ReceiveFromTechnicianObj.MaterialID);
                                        _ReceiveFromTechnicianObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ReceiveFromTechnicianObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _ReceiveFromTechnicianObj.Material);
                                        _ReceiveFromTechnicianObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReceiveFromTechnicianObj.Description);
                                        _ReceiveFromTechnicianObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ReceiveFromTechnicianObj.UOM);
                                    }

                                    receiveFromTechnicianList.Add(_ReceiveFromTechnicianObj);
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
            return receiveFromTechnicianList;
        }
        #endregion GetReceiptsSheet

        #region GetAllReceiptsFromTechnician
        public List<ReceiveFromTechnician> GetAllReceiptsFromTechnician(string empID, string fromDate, string toDate, UA UA)
        {
            List<ReceiveFromTechnician> receiveFromTechnicianList = null;
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
                        if (!string.IsNullOrEmpty(empID) && empID != "All")
                        {
                            cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        }
                        if (empID == "All")
                        {
                            cmd.Parameters.Add("@AllTech", SqlDbType.NVarChar, 5).Value = empID;
                        }
                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DateTime.Parse(fromDate);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DateTime.Parse(toDate);
                        }
                        cmd.CommandText = "[GetAllReceiptsFromTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                receiveFromTechnicianList = new List<ReceiveFromTechnician>();
                                while (sdr.Read())
                                {
                                    ReceiveFromTechnician _ReceiveFromTechnicianObj = new ReceiveFromTechnician();
                                    {
                                        _ReceiveFromTechnicianObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ReceiveFromTechnicianObj.ID);
                                        _ReceiveFromTechnicianObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ReceiveFromTechnicianObj.SCCode);
                                        _ReceiveFromTechnicianObj.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : _ReceiveFromTechnicianObj.MaterialID);
                                        _ReceiveFromTechnicianObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);
                                        _ReceiveFromTechnicianObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _ReceiveFromTechnicianObj.Material);
                                        _ReceiveFromTechnicianObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ReceiveFromTechnicianObj.Description);
                                        _ReceiveFromTechnicianObj.empName = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : _ReceiveFromTechnicianObj.empName);
                                        _ReceiveFromTechnicianObj.ReceiveDate = (sdr["ReceiveDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiveDate"].ToString()) : _ReceiveFromTechnicianObj.ReceiveDate);
                                        _ReceiveFromTechnicianObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ReceiveFromTechnicianObj.UOM);
                                    }

                                    receiveFromTechnicianList.Add(_ReceiveFromTechnicianObj);
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
            return receiveFromTechnicianList;
        }
        #endregion GetAllReceiptsFromTechnician

        #region InsertReceiveFromTechnician

        public List<ReceiveFromTechnician> InsertReceiveFromTechnician(ReceiveFromTechnician receiveFromTechnician, Guid? empID, DateTime? receiveDate, UA UA)
        {
            List<ReceiveFromTechnician> issueToTechList = null;
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
                        cmd.CommandText = "[InsertReceiveFromTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = empID;
                        cmd.Parameters.Add("@ReceiveDate", SqlDbType.DateTime).Value = receiveDate;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = receiveFromTechnician.MaterialID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = receiveFromTechnician.Qty;

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
                        receiveFromTechnician.ID = new Guid(outputID.Value.ToString());
                        issueToTechList = GetReceiptsSheet(receiveFromTechnician.ID.ToString(), "", UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return issueToTechList;
        }

        #endregion InsertReceiveFromTechnician

        #region UpdateReceiveFromTechnician

        public List<ReceiveFromTechnician> UpdateReceiveFromTechnician(ReceiveFromTechnician receiveFromTechnician, Guid? empID, DateTime? issueDate, UA UA)
        {
            List<ReceiveFromTechnician> receiveFromTechnicianList = null;
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
                        cmd.CommandText = "[UpdateReceiveFromTechnician]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = receiveFromTechnician.ID;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = receiveFromTechnician.MaterialID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = receiveFromTechnician.Qty;

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
                        receiveFromTechnicianList = GetReceiptsSheet(receiveFromTechnician.ID.ToString(), "", UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return receiveFromTechnicianList;
        }

        #endregion UpdateReceiveFromTechnician

        #region DeleteReceiveFromTechnician
        public string DeleteReceiveFromTechnician(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteReceiveFromTechnician]";
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
        #endregion DeleteReceiveFromTechnician
    }
}