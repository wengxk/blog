using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace App2
{
    class Config
    {
        public static string GetConnectionString(string connectionStringName)
        {
            var config = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
            if (config == null)
                throw new Exception(string.Format("No ConnectionString with name of {0}", connectionStringName));
            return config.ConnectionString;
        }

        public static OracleConnection GetConnection()
        {
            var connection = new OracleConnection(GetConnectionString("test"));
            connection.ConnectionOpen += connection_ConnectionOpen;
            connection.Disposed += connection_Disposed;
            return connection;
        }

        static void connection_Disposed(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Connection disposing");
        }

        static void connection_ConnectionOpen(OracleConnectionOpenEventArgs eventArgs)
        {
            System.Diagnostics.Trace.WriteLine("Connection openning");
        }
    }
}
