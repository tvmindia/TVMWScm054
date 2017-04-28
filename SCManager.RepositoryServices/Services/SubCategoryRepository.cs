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
    public class SubCategoryRepository : ISubCategoryRepository
    {
      //  LogDetails logDetailsObj = new LogDetails();

        #region DataBaseFactory
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implemented object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SubCategoryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion DataBaseFactory

        #region GetAllSubCategories
        public List<SubCategories> GetAllSubCategories(UA UA,string categoryID)
        {
            List<SubCategories> SubCategorieslist = null;
            SubCategories subCategoryObj = new SubCategories();
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
                        cmd.Parameters.Add("@CategoryID", SqlDbType.UniqueIdentifier).Value =Guid.Parse(categoryID.ToString());
                        cmd.CommandText = "[GetAllSubCategories]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SubCategorieslist = new List<SubCategories>();
                                while (sdr.Read())
                                {
                                    

                                    {
                                        subCategoryObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : subCategoryObj.ID);
                                        subCategoryObj.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : subCategoryObj.Description);

                                    };

                                    SubCategorieslist.Add(subCategoryObj);
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
            return SubCategorieslist;
        }
        #endregion GetAllSubCategories
    }
}