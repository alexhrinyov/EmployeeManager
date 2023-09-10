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
        public  bool Connect()
        {
            bool result;
            try
            {
                connection = new SqlConnection(ConnectionString.MsSqlConnection);
                connection.Open();
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
                throw new Exception("Подключение уже закрыто!");
            }
        }

    }
}