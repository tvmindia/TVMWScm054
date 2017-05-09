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
    public class CreditNotesRepository : ICreditNotesRepository
    {
        Const c = new Const();
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public CreditNotesRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        #region GetAllCreditNotes
        public List<CreditNotes> GetAllCreditNotes(UA UA,string showAllYN)
        {
            List<CreditNotes> CreditNoteslist = null;
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
                        cmd.Parameters.Add("@showAllYN", SqlDbType.NVarChar,5).Value = showAllYN;
                        cmd.CommandText = "[GetAllCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CreditNoteslist = new List<CreditNotes>();
                                while (sdr.Read())
                                {
                                    CreditNotes creditNotesObj = new CreditNotes();

                                    {
                                        creditNotesObj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : creditNotesObj.Date);
                                        creditNotesObj.CreditNoteNo = (sdr["CreditNoteNo"].ToString() != "" ? (sdr["CreditNoteNo"].ToString()) : creditNotesObj.CreditNoteNo);
                                        creditNotesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : creditNotesObj.Description);
                                        creditNotesObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : creditNotesObj.Amount);
                                        creditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : creditNotesObj.ID);

                                    };

                                    CreditNoteslist.Add(creditNotesObj);
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
            return CreditNoteslist;
        }
        #endregion GetAllCreditNotes

        #region GetCreditNotesBetweenDates
        public List<CreditNotes> GetCreditNotesBetweenDates(UA UA, string fromDate,string toDate)
        {
            List<CreditNotes> CreditNoteslist = null;
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
                       if(!string.IsNullOrEmpty(fromDate))
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DateTime.Parse(fromDate);
                        }
                       if(!string.IsNullOrEmpty(toDate))
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = DateTime.Parse(toDate);
                        }
                        
                        cmd.CommandText = "[GetCreditNotesBetweenDates]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CreditNoteslist = new List<CreditNotes>();
                                while (sdr.Read())
                                {
                                    CreditNotes creditNotesObj = new CreditNotes();

                                    {
                                        creditNotesObj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : creditNotesObj.Date);
                                        creditNotesObj.CreditNoteNo = (sdr["CreditNoteNo"].ToString() != "" ? (sdr["CreditNoteNo"].ToString()) : creditNotesObj.CreditNoteNo);
                                        creditNotesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : creditNotesObj.Description);
                                        creditNotesObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : creditNotesObj.Amount);
                                        creditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : creditNotesObj.ID);

                                    };

                                    CreditNoteslist.Add(creditNotesObj);
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
            return CreditNoteslist;
        }
        #endregion GetCreditNotesBetweenDates

        #region GetCreditNotesByID
        public List<CreditNotes> GetCreditNotesByID(UA UA, string ID)
        {
            List<CreditNotes> CreditNoteslist = null;
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
                        cmd.CommandText = "[GetCreditNotesByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CreditNoteslist = new List<CreditNotes>();
                                while (sdr.Read())
                                {
                                    CreditNotes creditNotesObj = new CreditNotes();

                                    {
                                        creditNotesObj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : creditNotesObj.Date);
                                        creditNotesObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : creditNotesObj.ID);
                                        creditNotesObj.CreditNoteNo = (sdr["CreditNoteNo"].ToString() != "" ? (sdr["CreditNoteNo"].ToString()) : creditNotesObj.CreditNoteNo);
                                        creditNotesObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : creditNotesObj.Description);
                                        creditNotesObj.Amount = (sdr["Amount"].ToString() != "" ? float.Parse(sdr["Amount"].ToString()) : creditNotesObj.Amount);

                                    };

                                    CreditNoteslist.Add(creditNotesObj);
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
            return CreditNoteslist;
        }
        #endregion GetCreditNotesByID

        #region InsertCreditNotes
        public object InsertCreditNotes(CreditNotes creditNotesObj)
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
                        cmd.CommandText = "[InsertCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = creditNotesObj.SCCode;
                        cmd.Parameters.Add("@CreditNoteNo", SqlDbType.NVarChar, 50).Value = creditNotesObj.CreditNoteNo;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = creditNotesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = creditNotesObj.Description;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = creditNotesObj.Date;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = creditNotesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = creditNotesObj.logDetails.CreatedDate;

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
        #endregion InsertCreditNotes

        #region UpdateCreditNotes
        public object UpdateCreditNotes(CreditNotes creditNotesObj)
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
                        cmd.CommandText = "[UpdateCreditNotes]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = creditNotesObj.ID;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = creditNotesObj.SCCode;
                        cmd.Parameters.Add("@CreditNoteNo", SqlDbType.NVarChar, 50).Value = creditNotesObj.CreditNoteNo;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = creditNotesObj.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = creditNotesObj.Description;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = creditNotesObj.Date;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = creditNotesObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = creditNotesObj.logDetails.CreatedDate;

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
        #endregion UpdateCreditNotes

        #region DeleteCreditNote
        public string DeleteCreditNote(string ID, UA ua)
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
                        cmd.CommandText = "[DeleteCreditNote]";
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
        #endregion DeleteCreditNote

        #endregion Methods
    }
}