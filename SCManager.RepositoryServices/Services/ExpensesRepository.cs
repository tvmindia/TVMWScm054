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
    public class ExpensesRepository: IExpensesRepository
    {
        Const constobj = new Const();
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public ExpensesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<ExpenseType> GetAllExpenseTypes(UA UA)
        {
            List<ExpenseType> ExpenseTypelist = null;
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
                      //  cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = UA.SCCode;
                        cmd.CommandText = "[GetAllExpenseType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ExpenseTypelist = new List<ExpenseType>();
                                while (sdr.Read())
                                {
                                    ExpenseType expenseType = new ExpenseType();

                                    {
                                        expenseType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : expenseType.Code);
                                        expenseType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : expenseType.Description);

                                    };

                                    ExpenseTypelist.Add(expenseType);
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
            return ExpenseTypelist;
        }

        public object InsertExpenses(Expenses ExpensesObj)
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
                        cmd.CommandText = "[InsertExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ExpensesObj.SCCode;
                        if(ExpensesObj.EmpID != Guid.Empty)
                        cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = ExpensesObj.EmpID;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = ExpensesObj.RefNo;
                        cmd.Parameters.Add("@RefDate", SqlDbType.SmallDateTime).Value = ExpensesObj.RefDate;
                        cmd.Parameters.Add("@ExpenseTypeCode", SqlDbType.NVarChar,5).Value = ExpensesObj.ExpenseTypeCode;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar,20).Value = ExpensesObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = ExpensesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ExpensesObj.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = ExpensesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = ExpensesObj.logDetails.CreatedDate;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter2 = cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier);
                        outParameter2.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        ExpensesObj.ID = new Guid(outParameter2.Value.ToString());

                        return ExpensesObj;
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
                Message = constobj.InsertSuccess
            };
        }
        public object UpdateExpenses(Expenses ExpensesObj)
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
                        cmd.CommandText = "[UpdateExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ExpensesObj.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = ExpensesObj.SCCode;
                        if (ExpensesObj.EmpID != Guid.Empty)
                            cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = ExpensesObj.EmpID;
                        cmd.Parameters.Add("@RefDate", SqlDbType.DateTime).Value = ExpensesObj.RefDate;
                        cmd.Parameters.Add("@RefNo", SqlDbType.NVarChar, 20).Value = ExpensesObj.RefNo;
                        cmd.Parameters.Add("@ExpenseTypeCode", SqlDbType.NVarChar, 5).Value = ExpensesObj.ExpenseTypeCode;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.NVarChar, 20).Value = ExpensesObj.PaymentMode;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = ExpensesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = ExpensesObj.Description;
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
            return new
            {
                Status = outParameter.Value.ToString(),
                Message = constobj.UpdateSuccess
            };
        }

        public List<Expenses> GetAllExpenses(UA UA, string FromDate, string ToDate, bool showAllYN)
        {
            List<Expenses> Expenseslist = null;
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
                        cmd.Parameters.Add("@showAllYN", SqlDbType.NVarChar, 5).Value = showAllYN;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate==""?null:FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate == "" ? null :ToDate;
                        cmd.CommandText = "[GetAllExpenses]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Expenseslist = new List<Expenses>();
                                while (sdr.Read())
                                {
                                    Expenses expensesObj = new Expenses();
                                    { 
                                        expensesObj.ExpenseTypeCode = (sdr["ExpenseTypeCode"].ToString() != "" ? (sdr["ExpenseTypeCode"].ToString()) : expensesObj.ExpenseTypeCode);
                                        expensesObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()) : expensesObj.RefDate);
                                        expensesObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : expensesObj.RefNo);
                                        expensesObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? (sdr["EntryNo"].ToString()) : expensesObj.EntryNo);
                                        expensesObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : expensesObj.PaymentMode);
                                        expensesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : expensesObj.Description);
                                        expensesObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : expensesObj.Amount);
                                        expensesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : expensesObj.ID);
                                        expensesObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : expensesObj.EmpID);
                                        expensesObj.EmpName = (sdr["EmpName"].ToString() != "" ? (sdr["EmpName"].ToString()) : expensesObj.EmpName);
                                        expensesObj.ExpenseType = (sdr["ExpenseType"].ToString() != "" ? (sdr["ExpenseType"].ToString()) : expensesObj.ExpenseType);
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

        public Expenses GetExpensesByID(UA UA, string ID)
        {
            Expenses expensesObj = new Expenses();
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
                        cmd.CommandText = "[GetExpensesByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                        expensesObj.ExpenseTypeCode = (sdr["ExpenseTypeCode"].ToString() != "" ? (sdr["ExpenseTypeCode"].ToString()) : expensesObj.ExpenseTypeCode);
                                        expensesObj.RefDate = (sdr["RefDate"].ToString() != "" ? DateTime.Parse(sdr["RefDate"].ToString()) : expensesObj.RefDate);
                                        expensesObj.RefNo = (sdr["RefNo"].ToString() != "" ? (sdr["RefNo"].ToString()) : expensesObj.RefNo);
                                        expensesObj.EntryNo = (sdr["EntryNo"].ToString() != "" ? (sdr["EntryNo"].ToString()) : expensesObj.EntryNo);
                                        expensesObj.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? (sdr["PaymentMode"].ToString()) : expensesObj.PaymentMode);
                                        expensesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : expensesObj.Description);
                                        expensesObj.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : expensesObj.Amount);
                                        expensesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : expensesObj.ID);
                                        expensesObj.EmpID = (sdr["EmpID"].ToString() != "" ? Guid.Parse(sdr["EmpID"].ToString()) : expensesObj.EmpID);
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

        public string DeleteExpenses(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteExpenses]";
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

        public Expenses GetOutStandingPayment(UA UA)
        {
            Expenses expensesObj = new Expenses();
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
                      
                        cmd.CommandText = "[GetOutstandingPayment]";
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