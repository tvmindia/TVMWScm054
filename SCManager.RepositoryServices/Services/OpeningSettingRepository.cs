using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;


namespace SCManager.RepositoryServices.Services
{
    public class OpeningSettingRepository:IOpeningSettingRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public OpeningSettingRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

      

        public OpeningSetting InsertOpeningSetting(OpeningSetting opn, UA UA)
        {
            //OpeningSetting Result = null;
            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[InsertOpeningSetting]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@Bank", SqlDbType.Decimal).Value = opn.Bank;
                        cmd.Parameters.Add("@Cash", SqlDbType.Decimal).Value = opn.Cash;
                        cmd.Parameters.Add("@WithEffectDate", SqlDbType.SmallDateTime).Value = opn.WithEffectDate;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = opn.DetailXML;

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
                        opn.ID = new Guid(outputID.Value.ToString());
                        opn.OpeningDetails = GetOpeningSettingDetail(UA);

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return opn;
        }

        public OpeningSetting UpdateOpeningSetting(OpeningSetting opn, UA UA)
        {
            OpeningSetting Result = null;
            try
            {
                SqlParameter outputStatus, outputID = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[UpdateOpeningSetting]";
                        cmd.CommandType = CommandType.StoredProcedure;
                      
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@Bank", SqlDbType.Decimal).Value = opn.Bank;
                        cmd.Parameters.Add("@Cash", SqlDbType.Decimal).Value = opn.Cash;
                        cmd.Parameters.Add("@WithEffectDate", SqlDbType.SmallDateTime).Value = opn.WithEffectDate;
                        
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = opn.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = UA.CurrentDatetime();

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();



                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.UpdateFailure);

                    case "1":
                        opn.OpeningDetails = GetOpeningSettingDetail(UA);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return opn;
        }

        public OpeningSetting GetOpeningSettingHeader( UA UA)
        {
            OpeningSetting OpeningSetting = null;
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
                        cmd.CommandText = "[GetOpeningSetting]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                if (sdr.Read())
                                {
                                    OpeningSetting _OpeningSettingObj = new OpeningSetting();
                                    {
                                        _OpeningSettingObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _OpeningSettingObj.ID);
                                        _OpeningSettingObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _OpeningSettingObj.SCCode);
                                        _OpeningSettingObj.Cash = (sdr["Cash"].ToString() != "" ? decimal.Parse(sdr["Cash"].ToString()) : _OpeningSettingObj.Cash);
                                        _OpeningSettingObj.Bank = (sdr["Bank"].ToString() != "" ? decimal.Parse(sdr["Bank"].ToString()) : _OpeningSettingObj.Bank);
                                        _OpeningSettingObj.WithEffectDate = (sdr["WithEffectDate"].ToString() != "" ? DateTime.Parse(sdr["WithEffectDate"].ToString()).Date : _OpeningSettingObj.WithEffectDate);

                                        
                                    }

                                    OpeningSetting = _OpeningSettingObj;
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
            return OpeningSetting;
        }

        public List<OpeningDetail> GetOpeningSettingDetail(  UA UA)
        {
            List<OpeningDetail> OpeningSettingDetailList = null;
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
                        cmd.CommandText = "[GetOpening_Detail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                OpeningSettingDetailList = new List<OpeningDetail>();
                                while (sdr.Read())
                                {
                                    OpeningDetail _OpeningSettingDetailObj = new OpeningDetail();
                                    {
                                        _OpeningSettingDetailObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _OpeningSettingDetailObj.ID);
                                        _OpeningSettingDetailObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _OpeningSettingDetailObj.SCCode);
                                        _OpeningSettingDetailObj.MaterialID = (sdr["ItemID"].ToString() != "" ? Guid.Parse(sdr["ItemID"].ToString()) : _OpeningSettingDetailObj.MaterialID);
                                        _OpeningSettingDetailObj.Quantity = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : 0);                                       
                                        _OpeningSettingDetailObj.Material = (sdr["Material"].ToString() != "" ? (sdr["Material"].ToString()) : _OpeningSettingDetailObj.Material);
                                        _OpeningSettingDetailObj.MaterialDescription = (sdr["MaterialDescription"].ToString() != "" ? (sdr["MaterialDescription"].ToString()) : _OpeningSettingDetailObj.MaterialDescription);
                                        _OpeningSettingDetailObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _OpeningSettingDetailObj.UOM);
                                    }

                                    OpeningSettingDetailList.Add(_OpeningSettingDetailObj);
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
            return OpeningSettingDetailList;
        }

        

        public bool DeleteOpeningSettingDetail(Guid ID, UA UA)
        {
            bool result = false;
            try
            {
                SqlParameter outputStatus;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[DeleteOpeningDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;                    
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 250).Value = UA.UserName;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();


                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        Const Cobj = new Const();
                        throw new Exception(Cobj.DeleteFailure);
                    case "1":
                        return true;

                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        #endregion Methods

    }
}