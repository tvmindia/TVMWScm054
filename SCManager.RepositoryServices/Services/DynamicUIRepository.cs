using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SCManager.DataAccessObject.DTO;
using System.Data;
using System.Data.SqlClient;
using SCManager.RepositoryServices.Contracts;

namespace SCManager.RepositoryServices.Services
{
    public class DynamicUIRepository : IDynamicUIRepository
    {

        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DynamicUIRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<Menu> GetAllMenues()
        {
            List<Menu> menuList = null;
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
                        cmd.CommandText = "[GetAllMenuItems]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                menuList = new List<Menu>();
                                while (sdr.Read())
                                {
                                    Menu menuObj = new Menu();
                                    {
                                        menuObj.ID = (sdr["ID"].ToString() != "" ? Int16.Parse(sdr["ID"].ToString()) : menuObj.ID);
                                        menuObj.ParentID = (sdr["ParentID"].ToString() != "" ? Int16.Parse(sdr["ParentID"].ToString()) : menuObj.ParentID);
                                        menuObj.MenuText = sdr["MenuText"].ToString();
                                        menuObj.Controller = sdr["Controller"].ToString();
                                        menuObj.Action = sdr["Action"].ToString();
                                        menuObj.Parameters = sdr["Parameters"].ToString();
                                    }
                                    menuList.Add(menuObj);
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

            return menuList;
        }


        public List<ReorderAlert> GetReorderAlertITems(UA UA)
        {
            List<ReorderAlert> ReorderAlertList = null;
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
                        cmd.CommandText = "[GetReorderQtyItems]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ReorderAlertList = new List<ReorderAlert>();
                                while (sdr.Read())
                                {
                                    ReorderAlert reorderAlertObj = new ReorderAlert();
                                    {
                                        reorderAlertObj.Item = sdr["item"].ToString();
                                        reorderAlertObj.qty = (sdr["Qty"].ToString() != "" ? Int16.Parse(sdr["Qty"].ToString()) : reorderAlertObj.qty);
                                        reorderAlertObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? Int16.Parse(sdr["ReorderQty"].ToString()) : reorderAlertObj.ReorderQty);
                                        reorderAlertObj.ApproachPercentage = (sdr["ApproachPercentage"].ToString() != "" ? decimal.Parse(sdr["ApproachPercentage"].ToString()) : reorderAlertObj.ApproachPercentage);
                                      
                                    }
                                    ReorderAlertList.Add(reorderAlertObj);
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

            return ReorderAlertList;
        }

        public List<StockValueSummary> GetStockValueSummary(UA UA) {
           List<StockValueSummary> ResultList = null;

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
                        cmd.CommandText = "[GetStockValueSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("SCCode", UA.SCCode);
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ResultList = new List<StockValueSummary>();


                                while (sdr.Read())
                                {
                                    StockValueSummary Result = new StockValueSummary();
                                    {

                                        Result.value = int.Parse(sdr["value"].ToString());
                                        Result.color = sdr["color"].ToString();
                                        Result.label = sdr["label"].ToString();
                                        Result.totalValue = int.Parse(sdr["totalValue"].ToString());

                                    }
                                    ResultList.Add(Result);
                                }
                            } 
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return ResultList;
        }

    }
}