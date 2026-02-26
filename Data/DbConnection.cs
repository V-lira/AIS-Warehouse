using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Data
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString =
            @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
        public static string ConnectionString => _connectionString;
        public static SqlConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
