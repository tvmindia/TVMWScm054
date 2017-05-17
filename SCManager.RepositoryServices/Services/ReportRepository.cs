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
    public class ReportRepository: IReportRepository
    {
        private IDatabaseFactory _databaseFactory;
        private Const constObj = new Const();
        public ReportRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }


        #region GetAllItems
        public List<Item> GetItemsSummary(UA UA, string fromdate = null, string todate = null)
        {
            List<Item> Itemlist = null;
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
                        cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = fromdate;
                        cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = todate;
                        cmd.CommandText = "[RPT_StockSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Itemlist = new List<Item>();
                                while (sdr.Read())
                                {
                                    Item _ItemObj = new Item();
                                    {
                                        _ItemObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : _ItemObj.ID);
                                        _ItemObj.SCCode = (sdr["SCCode"].ToString() != "" ? (sdr["SCCode"].ToString()) : _ItemObj.SCCode);
                                        _ItemObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : _ItemObj.ItemCode);
                                        _ItemObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : _ItemObj.Description);
                                        _ItemObj.CategoryID = (sdr["CategoryID"].ToString() != "" ? Guid.Parse(sdr["CategoryID"].ToString()) : _ItemObj.CategoryID);
                                        _ItemObj.Category = (sdr["Category"].ToString() != "" ? (sdr["Category"].ToString()) : _ItemObj.Category);
                                        _ItemObj.SubcategoryID = (sdr["SubcategoryID"].ToString() != "" ? Guid.Parse(sdr["SubcategoryID"].ToString()) : _ItemObj.SubcategoryID);
                                        _ItemObj.Subcategory = (sdr["Subcategory"].ToString() != "" ? (sdr["Subcategory"].ToString()) : _ItemObj.Subcategory);
                                        _ItemObj.Stock = (sdr["StockQty"].ToString() != "" ? (sdr["StockQty"].ToString()) : _ItemObj.Stock);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? (sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);
                                        _ItemObj.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? float.Parse(sdr["ProductCommission"].ToString()) : _ItemObj.ProductCommission);
                                        _ItemObj.Remarks = (sdr["Remarks"].ToString() != "" ?(sdr["Remarks"].ToString()) : _ItemObj.Remarks);
                                        _ItemObj.SellingRate= (sdr["SellingRate"].ToString() != "" ? float.Parse(sdr["SellingRate"].ToString()) : _ItemObj.SellingRate);
                                    }

                                    Itemlist.Add(_ItemObj);
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
            return Itemlist;
        }
        #endregion  GetAllItems
    }
}