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
    public class DefectiveDamageRepository : IDefectiveDamageRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DefectiveDamageRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods
        #region GetAllDefectiveDamaged
        public List<DefectiveDamage> GetAllDefectiveDamaged(UA UA)
        {
            List<DefectiveDamage> DefectiveDamagelist = null;
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
                        cmd.CommandText = "[GetAllDefectiveDamaged]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                DefectiveDamagelist = new List<DefectiveDamage>();
                                while (sdr.Read())
                                {
                                    DefectiveDamage _DefectiveDamagelistObj = new DefectiveDamage();
                                    {
                                        _DefectiveDamagelistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _DefectiveDamagelistObj.ID);
                                        _DefectiveDamagelistObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _DefectiveDamagelistObj.ItemCode);
                                        _DefectiveDamagelistObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : _DefectiveDamagelistObj.Type);
                                        _DefectiveDamagelistObj.OpenDate = (sdr["OpenDate"].ToString() != "" ? DateTime.Parse(sdr["OpenDate"].ToString()) : _DefectiveDamagelistObj.OpenDate);
                                        _DefectiveDamagelistObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _DefectiveDamagelistObj.RefNo);
                                        _DefectiveDamagelistObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _DefectiveDamagelistObj.Description);
                                        _DefectiveDamagelistObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : _DefectiveDamagelistObj.Qty);
                                        _DefectiveDamagelistObj.ReturnStatusYN = (sdr["ReturnStatusYN"].ToString() != "" ?bool.Parse (sdr["ReturnStatusYN"].ToString()) : _DefectiveDamagelistObj.ReturnStatusYN);
                                        _DefectiveDamagelistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _DefectiveDamagelistObj.Remarks);
                                    }

                                    DefectiveDamagelist.Add(_DefectiveDamagelistObj);
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
            return DefectiveDamagelist;
        }
        #endregion  GetAllDefectiveDamaged

        #region GetDefectiveDamagedByID
        public List<DefectiveDamage> GetDefectiveDamagedByID(UA UA,string ID)
        {
            List<DefectiveDamage> DefectiveDamagelist = null;
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
                        cmd.CommandText = "[GetDefectiveDamagedByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                DefectiveDamagelist = new List<DefectiveDamage>();
                                while (sdr.Read())
                                {
                                    DefectiveDamage _DefectiveDamagelistObj = new DefectiveDamage();
                                    {
                                        _DefectiveDamagelistObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _DefectiveDamagelistObj.ID);
                                        _DefectiveDamagelistObj.ItemID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _DefectiveDamagelistObj.ItemID);
                                        _DefectiveDamagelistObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : _DefectiveDamagelistObj.EmpID);
                                        _DefectiveDamagelistObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _DefectiveDamagelistObj.ItemCode);
                                        _DefectiveDamagelistObj.Type = (sdr["Type"].ToString() != "" ? (sdr["Type"].ToString()) : _DefectiveDamagelistObj.Type);
                                        _DefectiveDamagelistObj.OpenDate = (sdr["OpenDate"].ToString() != "" ? DateTime.Parse(sdr["OpenDate"].ToString()) : _DefectiveDamagelistObj.OpenDate);
                                        _DefectiveDamagelistObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _DefectiveDamagelistObj.RefNo);
                                        _DefectiveDamagelistObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _DefectiveDamagelistObj.Description);
                                        _DefectiveDamagelistObj.Qty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : _DefectiveDamagelistObj.Qty);
                                        _DefectiveDamagelistObj.ReturnStatusYN = (sdr["ReturnStatusYN"].ToString() != "" ? bool.Parse(sdr["ReturnStatusYN"].ToString()) : _DefectiveDamagelistObj.ReturnStatusYN);
                                        _DefectiveDamagelistObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _DefectiveDamagelistObj.Remarks);
                                    }

                                    DefectiveDamagelist.Add(_DefectiveDamagelistObj);
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
            return DefectiveDamagelist;
        }
        #endregion  GetDefectiveDamagedByID

        #region InsertDefectiveDamaged
        public object InsertDefectiveDamaged(DefectiveDamage defectiveDamageObj)
        {
            SqlParameter outParameter, outParameter1 = null;
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
                        cmd.CommandText = "[InsertDefectiveDamaged]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = defectiveDamageObj.SCCode;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar,15).Value = defectiveDamageObj.Type;
                        cmd.Parameters.Add("@OpenDate", SqlDbType.DateTime).Value =defectiveDamageObj.OpenDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = defectiveDamageObj.RefNo;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = defectiveDamageObj.ItemID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = defectiveDamageObj.Qty;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar,-1).Value = defectiveDamageObj.Remarks;
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = defectiveDamageObj.ReturnStatusYN;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value =defectiveDamageObj.ReturnDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = defectiveDamageObj.EmpID;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = defectiveDamageObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = defectiveDamageObj.logDetails.CreatedDate;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter1 = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outParameter1.Direction = ParameterDirection.Output;
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
                ID = Guid.Parse(outParameter1.Value.ToString()),
                Status = outParameter.Value.ToString(),
                Message = c.InsertSuccess
            };
        }
        #endregion InsertDefectiveDamaged

        #region UpdateDefectiveDamaged
        public object UpdateDefectiveDamaged(DefectiveDamage defectiveDamageObj)
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
                        cmd.CommandText = "[UpdateDefectiveDamaged]";
                        cmd.CommandType = CommandType.StoredProcedure;                     
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = defectiveDamageObj.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = defectiveDamageObj.SCCode;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 15).Value = defectiveDamageObj.Type;
                        cmd.Parameters.Add("@OpenDate", SqlDbType.DateTime).Value = defectiveDamageObj.OpenDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = defectiveDamageObj.RefNo;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = defectiveDamageObj.ItemID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = defectiveDamageObj.Qty;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = defectiveDamageObj.Remarks;
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = defectiveDamageObj.ReturnStatusYN;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = defectiveDamageObj.ReturnDate;
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = defectiveDamageObj.EmpID;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = defectiveDamageObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = defectiveDamageObj.logDetails.CreatedDate;

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
            return new
            {
                Status = outParameter.Value.ToString(),
                Message = c.UpdateSuccess
            };
        }
        #endregion UpdateDefectiveDamaged

        #region DeleteDefectiveDamaged
        public string DeleteDefectiveDamaged(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteDefectiveDamaged]";
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
        #endregion DeleteDefectiveDamaged

        #region ReturnDefectiveDamaged
        public string ReturnDefectiveDamaged(string ID, UA ua)
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
                        cmd.CommandText = "[ReturnDefectiveDamaged]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;
                        cmd.Parameters.Add("@ReturnDate",SqlDbType.DateTime).Value= ua.CurrentDatetime();
                        cmd.Parameters.Add("@ReturnStatusYN", SqlDbType.Bit).Value = true;
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
        #endregion ReturnDefectiveDamaged

        #region DefectiveDamagedValidation
        public string DefectiveDamagedValidation(string itemID,string empID, string type, UA ua)
        {
            
            int result ;
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
                        cmd.CommandText = "[GetItemStockByLocation]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ItemID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(itemID);
                        cmd.Parameters.Add("@TechID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(empID);
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ua.SCCode;
                        if(type== "Defective")
                        {
                            cmd.Parameters.Add("@LocType", SqlDbType.NVarChar, 5).Value = "TECH";
                        }
                        else
                        {
                            cmd.Parameters.Add("@LocType", SqlDbType.NVarChar, 5).Value = "OFFC";
                        }
                                              
                        result=int.Parse( cmd.ExecuteScalar().ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result.ToString();
        }
        #endregion DefectiveDamagedValidation

        #endregion Methods
    }
}