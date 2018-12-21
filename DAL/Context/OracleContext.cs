using Models.DataBaseModels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class OracleContext: DbContext
    {
        private static OracleContext connection;
        private static object obj = new object();       

        private string _connectionString = "DATA SOURCE=filecloud-test.taximaxim.local:1521/xe;PASSWORD=qwe321;USER ID=CODERLINK_DIV";
        // ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;

        public OracleContext() { }

        public IDbConnection Connection
        {
            get
            {
                return new OracleConnection(_connectionString);
            }
        }


        public string getCnnectionString
        {
            get
            {
                return _connectionString;
            }
        }



        public static OracleContext getContext()
        {
            lock (obj)
            {
                if (connection == null)
                {
                    connection = new OracleContext();
                }
            }

            return connection;
        }

    }
}
