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
    public class Form8BTaxInvoiceRepository : IForm8BTaxInvoiceRepository
    {
        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public Form8BTaxInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods
        public List<Form8B> GetAllForm8B(UA UA)
        {
            List<Form8B> Form8Blist = null;
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
                        cmd.CommandText = "[GetAllForm8B]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8Blist = new List<Form8B>();
                                while (sdr.Read())
                                {
                                    Form8B _Form8Obj = new Form8B();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()).Date : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).Date : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()).Date : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SPUNo = (sdr["SPUNo"].ToString() != "" ? (sdr["SPUNo"].ToString()) : _Form8Obj.SPUNo);
                                        _Form8Obj.CustomerBillAddrs = (sdr["CustomerBillAddrs"].ToString() != "" ? (sdr["CustomerBillAddrs"].ToString()) : _Form8Obj.CustomerBillAddrs);
                                        _Form8Obj.CustomerDelvAddrs = (sdr["CustomerDelvAddrs"].ToString() != "" ? (sdr["CustomerDelvAddrs"].ToString()) : _Form8Obj.CustomerDelvAddrs);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.Subtotal = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.Subtotal);
                                        _Form8Obj.VATExpense = (sdr["VATExpense"].ToString() != "" ? decimal.Parse(sdr["VATExpense"].ToString()) : _Form8Obj.VATExpense);
                                    }

                                    Form8Blist.Add(_Form8Obj);
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
            return Form8Blist;
        }
        #endregion Methods
    }
}