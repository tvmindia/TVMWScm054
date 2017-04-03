using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace SCManager.RepositoryServices.Services
{
    public class DatabaseFactory : IDatabaseFactory
    {

        private SqlConnection SQLCon = null;


        public SqlConnection GetDBConnection()
        {
            try
            {
                SQLCon = new SqlConnection(ConfigurationManager.ConnectionStrings["SCManagerConnection"].ConnectionString);
                //if (SQLCon.State == ConnectionState.Closed)
                //{

                //    SQLCon.Open();
                //}

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return SQLCon;
        }


        public Boolean DisconectDB()
        {
            try
            {
                if (SQLCon.State == ConnectionState.Open)
                {
                    SQLCon.Close();
                    SQLCon.Dispose();
                    return true;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}