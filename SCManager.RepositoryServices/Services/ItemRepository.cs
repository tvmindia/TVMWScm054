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
    public class ItemRepository:IItemRepository
    {

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ItemRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory


        #region Methods

        public List<Item> GetAllItems(UA UA)
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
                        cmd.CommandText = "[GetAllItems]";
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
                                        _ItemObj.Stock = (sdr["StockQty"].ToString() != "" ? int.Parse(sdr["StockQty"].ToString()) : _ItemObj.Stock);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? int.Parse(sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);

                                         
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
        #endregion Methods
    }
}