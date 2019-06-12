using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    internal static class Connection
    {
        private static readonly string connectionString =
            @"Data Source=localhost;Initial Catalog=LibraryDB;Integrated Security=True";


        public static SqlConnection GetDbConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
