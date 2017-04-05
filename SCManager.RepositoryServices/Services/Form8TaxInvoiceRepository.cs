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
    public class Form8TaxInvoiceRepository : IForm8TaxInvoiceRepository
    {

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public Form8TaxInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region Methods

        public List<Form8> GetAllForm8(UA UA)
        {
            List<Form8> Form8list = null;
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
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar,5).Value = UA.SCCode;
                        cmd.CommandText = "[GetAllForm8]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Form8list = new List<Form8>();
                                while (sdr.Read())
                                {
                                    Form8 _Form8Obj = new Form8();
                                    {
                                        _Form8Obj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _Form8Obj.ID);
                                        _Form8Obj.SCCode = (sdr["SCCode"].ToString() != "" ?  (sdr["SCCode"].ToString()) : _Form8Obj.SCCode);
                                        _Form8Obj.ChallanDate = (sdr["ChallanDate"].ToString() != "" ? DateTime.Parse(sdr["ChallanDate"].ToString()) : _Form8Obj.ChallanDate);
                                        _Form8Obj.ChallanNo = (sdr["ChallanNo"].ToString() != "" ? (sdr["ChallanNo"].ToString()) : _Form8Obj.ChallanNo);
                                        _Form8Obj.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : _Form8Obj.InvoiceDate);
                                        _Form8Obj.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? (sdr["InvoiceNo"].ToString()) : _Form8Obj.InvoiceNo);
                                        _Form8Obj.PODate = (sdr["PODate"].ToString() != "" ? DateTime.Parse(sdr["PODate"].ToString()) : _Form8Obj.PODate);
                                        _Form8Obj.PONo = (sdr["PONo"].ToString() != "" ? (sdr["PONo"].ToString()) : _Form8Obj.PONo);
                                        _Form8Obj.SaleOrderNo = (sdr["SaleOrderNo"].ToString() != "" ? (sdr["SaleOrderNo"].ToString()) : _Form8Obj.SaleOrderNo);
                                        _Form8Obj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _Form8Obj.Remarks);
                                        _Form8Obj.VATAmount = (sdr["VATAmount"].ToString() != "" ? decimal.Parse(sdr["VATAmount"].ToString()) : _Form8Obj.VATAmount);
                                        _Form8Obj.TotalItemsValue = (sdr["TotalValue"].ToString() != "" ? decimal.Parse(sdr["TotalValue"].ToString()) : _Form8Obj.TotalItemsValue);
                                        _Form8Obj.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : _Form8Obj.Discount);
                                    }

                                    Form8list.Add(_Form8Obj);
                                }
                            }//if
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Form8list;
        }
        #endregion Methods

    }
}