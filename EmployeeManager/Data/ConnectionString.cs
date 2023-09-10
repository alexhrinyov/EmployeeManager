using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Data
{
    /// <summary>
    /// Информация о сервере и базе данных
    /// </summary>
    public static class ConnectionString
    {
        public static string MsSqlConnection => @"Server = DESKTOP-BQ6LNRN\SQLEXPRESS; TrustServerCertificate = true; Database = EmployeeManagement; Trusted_Connection=True";
    }
}
