using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data
{
    class Connection
    {
        // creates a connection and returns it
        public static SqlConnection getInstance()
        {
            // ConfigurationManager requires having System.Configuration reference to be present in References (in Solution explorer)
            string s = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(s);
            conn.Open();
            return conn;
        }
    }
}
