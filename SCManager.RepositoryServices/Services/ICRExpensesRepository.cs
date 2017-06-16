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
    public class ICRExpensesRepository: IICRExpensesRepository
    {
        Const constobj = new Const();
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public ICRExpensesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public object InsertICRExpenses(ICRExpenses ExpensesObj)
        {
            SqlParameter outParameter = null;
            SqlParameter outParameter2 = null;
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
                        cmd.CommandText = "[InsertICRExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ExpensesObj.SCCode;
                 
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = ExpensesObj.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = ExpensesObj.RefDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = ExpensesObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = ExpensesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ExpensesObj.Description;
                        cmd.Parameters.Add("@ChequeType", SqlDbType.NVarChar, 20).Value = ExpensesObj.ChequeType;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = ExpensesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = ExpensesObj.logDetails.CreatedDate;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter2 = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outParameter2.Direction = ParameterDirection.Output;
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
                        ID = Guid.Parse(outParameter2.Value.ToString()),
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
        public object UpdateICRExpenses(ICRExpenses ExpensesObj)
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
                        cmd.CommandText = "[UpdateICRExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ExpensesObj.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ExpensesObj.SCCode;
                        cmd.Parameters.Add("@RefDate", SqlDbType.DateTime).Value = ExpensesObj.RefDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = ExpensesObj.RefNo;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = ExpensesObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = ExpensesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ExpensesObj.Description;
                        cmd.Parameters.Add("@ChequeType", SqlDbType.NVarChar, 20).Value = ExpensesObj.ChequeType;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = ExpensesObj.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = ExpensesObj.logDetails.UpdatedDate;

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
            switch (outParameter.Value.ToString())
            {
                case "1":
                    return new
                    { 
                        Status = outParameter.Value.ToString(),
                        Message = constObj.UpdateSuccess
                    };
                default:
                    return new
                    {
                        Status = outParameter.Value.ToString(),
                        Message = constObj.UpdateFailure
                    };
            }
        }

        public List<ICRExpenses> GetAllICRExpenses(UA UA, string FromDate, string ToDate)
        {
            List<ICRExpenses> Expenseslist = null;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate == "" ? null : FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate == "" ? null : ToDate;
                        cmd.CommandText = "[GetAllICRExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Expenseslist = new List<ICRExpenses>();
                                while (sdr.Read())
                                {
                                    ICRExpenses expensesObj = new ICRExpenses();
                                    {                                      
                                        expensesObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()).ToString("dd-MMM-yyyy") : expensesObj.RefDate);
                                        expensesObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : expensesObj.RefNo);
                                        expensesObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? (sdr["EntryNo"].ToString()) : expensesObj.EntryNo);
                                        expensesObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : expensesObj.PaymentMode);
                                        expensesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : expensesObj.Description);
                                        expensesObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : expensesObj.Amount);
                                        expensesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : expensesObj.ID);
                                    };
                                    Expenseslist.Add(expensesObj);
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
            return Expenseslist;

        }

        public ICRExpenses GetICRExpensesByID(UA UA, string ID)
        {
            ICRExpenses expensesObj = new ICRExpenses();
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
                        cmd.CommandText = "[GetICRExpensesByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    expensesObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()).ToString("dd-MMM-yyyy") : expensesObj.RefDate);
                                    expensesObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : expensesObj.RefNo);
                                    expensesObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? (sdr["EntryNo"].ToString()) : expensesObj.EntryNo);
                                    expensesObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : expensesObj.PaymentMode);
                                    expensesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : expensesObj.Description);
                                    expensesObj.ChequeType = (sdr["ChequeType"].ToString() != "" ? (sdr["ChequeType"].ToString()) : expensesObj.ChequeType);
                                    expensesObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : expensesObj.Amount);
                                    expensesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : expensesObj.ID);
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
            return expensesObj;
        }

        public string DeleteICRExpenses(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteICRExpenses]";
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

        public ICRExpenses GetOutStandingICRPayment(UA UA)
        {
            ICRExpenses expensesObj = new ICRExpenses();
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

                        cmd.CommandText = "[GetOutStandingICRPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    expensesObj.OutStandingPayment = (sdr["OutStandingPayments"].ToString() != "" ? Decimal.Parse(sdr["OutStandingPayments"].ToString()) : expensesObj.OutStandingPayment);

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
            return expensesObj;
        }
    }
   
}