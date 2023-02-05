using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
namespace IntegrationWebApp
{
    public class BalHelper
    {
        #region Private Variables
        private static readonly object connectionStringLock = new object();

        #endregion     


        /// <summary>
        /// Gets the connection string for AppDB use only in mysql data base.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionStringMySql()
        {
            string connectionString = string.Empty;

            lock (connectionStringLock)
            {
                connectionString = ConfigurationManager.ConnectionStrings["CONNECTIONSTRINGMySql"].ToString();
              
            }
            return connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionLive()
        {
            string connectionString = string.Empty;

            lock (connectionStringLock)
            {
                connectionString = ConfigurationManager.ConnectionStrings["CONNECTIONSTRINGLIVE"].ToString();
                
            }
            return connectionString;
        }

       
    }
}
