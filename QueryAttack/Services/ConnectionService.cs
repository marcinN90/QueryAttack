using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    public class ConnectionService
    {
        public SqlConnection conn;
        private SqlConnectionStringBuilder ConnectionString = new SqlConnectionStringBuilder();

        public bool Connect(string ServerName, string DatabaseName, string User, string Password)
        {
            ConnectionString.DataSource = ServerName;
            ConnectionString.InitialCatalog = DatabaseName;
            ConnectionString.IntegratedSecurity = false;
            ConnectionString.UserID = User;
            ConnectionString.Password = Password;

            conn = new SqlConnection(ConnectionString.ConnectionString);
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)    
            {
                // TODO
                return false;
            }
        }
        public bool IsConnected()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                return false;
            }
            if (conn.State == ConnectionState.Open)
            {
                return true;
            }
            return false;
        }
    }

}

