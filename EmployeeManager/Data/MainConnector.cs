using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace EmployeeManager.Data
{
    /// <summary>
    /// Класс, управляющий подключениями
    /// </summary>
    public class MainConnector
    {
        private SqlConnection connection;
        public async Task<bool> ConnectAsync()
        {
            bool result;
            try
            {
                connection = new SqlConnection(ConnectionString.MsSqlConnection);
                await connection.OpenAsync();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }


        public  void Disconnect()
        {
            if (connection.State == ConnectionState.Open)
            {
                 connection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                return connection;
            }
            else
            {
                throw new Exception("error!");
            }
            
        }

    }
}