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
    public class DepositAndWithdrawalRepository: IDepositAndWithdrawalRepository
    {
        private IDatabaseFactory _databaseFactory;
        Const constObj = new Const();
        public DepositAndWithdrawalRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region InsertDepositAndWithdrawal
        
        public object InsertDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal)
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
                        cmd.CommandText = "[InsertDepositAndWithdrawal]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = depositAndWithdrawal.SCCode;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.NVarChar,20).Value = depositAndWithdrawal.TransactionType;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar,20).Value = depositAndWithdrawal.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = depositAndWithdrawal.RefDate;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = depositAndWithdrawal.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = depositAndWithdrawal.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = depositAndWithdrawal.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = depositAndWithdrawal.logDetails.CreatedDate;
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
            switch (outParameter.Value.ToString())
            {

                case "1":
                    return new
                    {
                        ID = Guid.Parse(outParameter1.Value.ToString()),
                        Status = outParameter.Value.ToString(),
                        Message = constObj.InsertSuccess
                    };

                default:
                    return new
                    {
                        Status = outParameter.Value.ToString(),
                        Message = constObj.InsertFailure
                    };

            }


        }
        #endregion InsertJob

        #region UpdateDepositAndWithdrawal
        public object UpdateDepositAndWithdrawal(DepositAndWithdrawal depositAndWithdrawal)
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
                        cmd.CommandText = "[UpdateDepositAndWithdrawal]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = depositAndWithdrawal.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = depositAndWithdrawal.SCCode;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.NVarChar, 20).Value = depositAndWithdrawal.TransactionType;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = depositAndWithdrawal.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = depositAndWithdrawal.RefDate;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = depositAndWithdrawal.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = depositAndWithdrawal.Description;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = depositAndWithdrawal.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = depositAndWithdrawal.logDetails.UpdatedDate;
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
            if (outParameter.Value.ToString() == "1")
            {
                return new
                {
                    Message = constObj.UpdateSuccess
                };
            }
            else
            {
                return new
                {
                    Message = constObj.UpdateFailure
                };
            }

        }
        #endregion UpdateDepositAndWithdrawal


        #region GetAllDepositAndWithdrawal
        public List<DepositAndWithdrawal> GetAllDepositAndWithdrawal(string SCCode)
        {
            List<DepositAndWithdrawal> depositAndWithdrawalList = null;
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
                        cmd.CommandText = "[GetAllDepositsAndWithdrawals]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = SCCode;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                depositAndWithdrawalList = new List<DepositAndWithdrawal>();
                                while (sdr.Read())
                                {
                                    DepositAndWithdrawal _depositAndWithdrawal = new DepositAndWithdrawal();
                                    {
                                        _depositAndWithdrawal.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _depositAndWithdrawal.SCCode);
                                        _depositAndWithdrawal.ID = (sdr["ID"].ToString() != "" ? (Guid.Parse(sdr["ID"].ToString())) : Guid.Empty);
                                        _depositAndWithdrawal.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : _depositAndWithdrawal.TransactionType);
                                        _depositAndWithdrawal.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : _depositAndWithdrawal.RefNo);
                                        _depositAndWithdrawal.RefDate = (sdr["RefDate"].ToString() != "" ? (sdr["RefDate"].ToString()) : _depositAndWithdrawal.RefDate);
                                        _depositAndWithdrawal.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : _depositAndWithdrawal.Amount);
                                        _depositAndWithdrawal.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _depositAndWithdrawal.Description);
                                     }
                                    depositAndWithdrawalList.Add(_depositAndWithdrawal);
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
            return depositAndWithdrawalList;
        }
        #endregion GetAllDepositAndWithdrawal

    }
}