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
                                        _ItemObj.Stock = (sdr["StockQty"].ToString() != "" ? (sdr["StockQty"].ToString()) : _ItemObj.Stock);
                                        _ItemObj.DefDamgStockQty = (sdr["DefDamgQty"].ToString() != "" ? (sdr["DefDamgQty"].ToString()) : _ItemObj.DefDamgStockQty);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? (sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);
                                        _ItemObj.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? float.Parse(sdr["ProductCommission"].ToString()) : _ItemObj.ProductCommission);
                                        _ItemObj.Remarks = (sdr["Remarks"].ToString() != "" ?(sdr["Remarks"].ToString()) : _ItemObj.Remarks);
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

        #region GetAllServiceTypeItems
        public List<Item> GetAllServiceTypeItems(UA UA)
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
                        cmd.CommandText = "[GetAllServiceTypeItems]";
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
                                        _ItemObj.DefDamgStockQty = (sdr["DefDamgQty"].ToString() != "" ? (sdr["DefDamgQty"].ToString()) : _ItemObj.DefDamgStockQty);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? (sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);
                                        _ItemObj.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? float.Parse(sdr["ProductCommission"].ToString()) : _ItemObj.ProductCommission);
                                        _ItemObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ItemObj.Remarks);
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
        #endregion GetAllServiceTypeItems
        #region GetItemByID
        public List<Item> GetItemByID(UA UA,string ID)
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
                        cmd.Parameters.Add("@ID",SqlDbType.UniqueIdentifier).Value=Guid.Parse(ID);
                        cmd.CommandText = "[GetItemByID]";
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
                                        _ItemObj.DefDamgStockQty = (sdr["DefDamgQty"].ToString() != "" ? (sdr["DefDamgQty"].ToString()) : _ItemObj.DefDamgStockQty);
                                        _ItemObj.SCQty = (sdr["SCQty"].ToString() != "" ? (sdr["SCQty"].ToString()) : _ItemObj.SCQty);
                                        _ItemObj.TechnicianQty = (sdr["TechnicianQty"].ToString() != "" ? (sdr["TechnicianQty"].ToString()) : _ItemObj.TechnicianQty);
                                        _ItemObj.UOM = (sdr["UOM"].ToString() != "" ? (sdr["UOM"].ToString()) : _ItemObj.UOM);
                                        _ItemObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? (sdr["ReorderQty"].ToString()) : _ItemObj.ReorderQty);
                                        _ItemObj.SellingRate = (sdr["SellingRate"].ToString() != "" ? float.Parse(sdr["SellingRate"].ToString()) : _ItemObj.SellingRate);
                                        _ItemObj.ProductCommission = (sdr["ProductCommission"].ToString() != "" ? float.Parse(sdr["ProductCommission"].ToString()) : _ItemObj.ProductCommission);
                                        _ItemObj.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : _ItemObj.Remarks);
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
        #endregion GetItemByID

        #region InsertItem
        public object InsertItem(Item itemObj)
        {
            SqlParameter outParameter,outParameter1 = null;
            try
            {
               
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[InsertItem]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = itemObj.SCCode;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = itemObj.Description;
                        cmd.Parameters.Add("@CategoryID", SqlDbType.UniqueIdentifier).Value =itemObj.CategoryID;
                        if(itemObj.SubcategoryID!=Guid.Empty)
                        {
                            cmd.Parameters.Add("@SubCategoryID", SqlDbType.UniqueIdentifier).Value = itemObj.SubcategoryID;
                        }                      
                        cmd.Parameters.Add("@StockQty", SqlDbType.Int).Value = itemObj.Stock;
                        cmd.Parameters.Add("@UOM", SqlDbType.NVarChar, 5).Value = itemObj.UOM;
                        cmd.Parameters.Add("@ReOrderQty", SqlDbType.Int).Value = itemObj.ReorderQty;
                        cmd.Parameters.Add("@ProductCommission", SqlDbType.Decimal).Value = itemObj.ProductCommission;
                        cmd.Parameters.Add("@SellingRate", SqlDbType.Decimal).Value = itemObj.SellingRate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = itemObj.Remarks;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = itemObj.logDetails.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = itemObj.logDetails.CreatedDate;
                        cmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 50).Value = itemObj.ItemCode;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        outParameter1 = cmd.Parameters.Add("@ID",SqlDbType.UniqueIdentifier);
                        outParameter1.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                                          
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                itemID = Guid.Parse(outParameter1.Value.ToString()),
                Status = outParameter.Value.ToString()
            };
        }

        #endregion InsertItem

        #region UpdateItem
        public string UpdateItem(Item itemObj)
        {
             SqlParameter outParameter = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[UpdateItem]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID",SqlDbType.UniqueIdentifier).Value=Guid.Parse(itemObj.ID.ToString());
                        cmd.Parameters.Add("@SCCode", SqlDbType.NVarChar, 5).Value = itemObj.SCCode;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = itemObj.Description;
                        cmd.Parameters.Add("@CategoryID", SqlDbType.UniqueIdentifier).Value = itemObj.CategoryID;
                        if(itemObj.SubcategoryID!=Guid.Empty)
                        {
                            cmd.Parameters.Add("@SubCategoryID", SqlDbType.UniqueIdentifier).Value = itemObj.SubcategoryID;
                        }
                        cmd.Parameters.Add("@StockQty", SqlDbType.Int).Value = itemObj.Stock;
                        cmd.Parameters.Add("@UOM", SqlDbType.NVarChar, 5).Value = itemObj.UOM;
                        cmd.Parameters.Add("@ReOrderQty", SqlDbType.Int).Value = itemObj.ReorderQty;
                        cmd.Parameters.Add("@ProductCommission", SqlDbType.Decimal).Value = itemObj.ProductCommission;
                        cmd.Parameters.Add("@SellingRate", SqlDbType.Decimal).Value = itemObj.SellingRate;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = itemObj.Remarks;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = itemObj.logDetails.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = itemObj.logDetails.UpdatedDate;
                        cmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar, 50).Value = itemObj.ItemCode;

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return outParameter.Value.ToString();
        }
        #endregion UpdateItem

        #region DeleteItem
        public string DeleteItem(string ID)
        {
            SqlParameter outParameter = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd=new SqlCommand())
                    {
                        if(con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[DeleteItem]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);

                        outParameter = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outParameter.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                   
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return outParameter.Value.ToString();
        }
        #endregion DeleteItem

        #region GetAllItemCode
        public List<Item> GetAllItemCode(UA UA)
        {
            List<Item> ItemCodelist = null;
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
                        cmd.CommandText = "[GetAllItemCode]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ItemCodelist = new List<Item>();
                                while (sdr.Read())
                                {
                                    Item itemObj = new Item();

                                    {
                                        itemObj.ItemCode = (sdr["ItemCode"].ToString() != "" ? (sdr["ItemCode"].ToString()) : itemObj.ItemCode);
                                        itemObj.Description = (sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : itemObj.Description);
                                        itemObj.ID = (sdr["ID"].ToString() != "" ?Guid.Parse(sdr["ID"].ToString()) : itemObj.ID);

                                    };

                                    ItemCodelist.Add(itemObj);
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
            return ItemCodelist;
        }
        #endregion GetAllItemCode

        #region GetAllUOMs
        public List<Item> GetAllUOMs()
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
                    
                        cmd.CommandText = "[GetAllUOMs]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                Itemlist = new List<Item>();
                                while (sdr.Read())
                                {
                                    Item itemObj = new Item();

                                    {
                                        itemObj.UOM = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : itemObj.UOM);
                                        itemObj.UOMDesc = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : itemObj.UOMDesc);

                                    };

                                    Itemlist.Add(itemObj);
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
        #endregion GetAllUOMs

        #endregion Methods
    }
}