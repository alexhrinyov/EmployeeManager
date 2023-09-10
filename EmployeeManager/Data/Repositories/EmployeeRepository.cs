
using EmployeeManager.Data.Entities;
using EmployeeManager.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace EmployeeManager.Data.Repositories
{
    public class EmployeeRepository
    {
        /// <summary>
        /// Объект класса, управляющего подключениями
        /// </summary>
        private MainConnector connector;
        public EmployeeRepository(MainConnector connector)
        {
            this.connector = connector;
        }
        /// <summary>
        /// Выгрузить данные из базы для выбранной таблицы
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable SelectAll(string table)
        {
            // sql команда
            var selectcommandtext = "select * from " + table;
            try
            {
                // адаптер
                var adapter = new SqlDataAdapter(
              selectcommandtext,
              connector.GetConnection()
            );
                //Получаем объект dataset
                var ds = new DataSet();
                //Заполняем его данными
                adapter.Fill(ds);
                //возвращаем полученную таблицу
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// Получение информации о сотруднике по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetEmployeeById(int id)
        {
            var selectcommandtext = $"select * from EmployeesFullInfo\r\nwhere EmployeesFullInfo.ID = {id}";
            try
            {
                var adapter = new SqlDataAdapter(
              selectcommandtext,
              connector.GetConnection()
            );
                var ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Поиск сотрудников по фамилии
        /// </summary>
        /// <param name="LastName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetEmployeeByLastName(string LastName)
        {
            //Вызов процедуры
            var selectcommandtext = $"execute GetEmployeeByLastName @LastName = N'{LastName}'";
            try
            {
                var adapter = new SqlDataAdapter(
              selectcommandtext,
              connector.GetConnection()
            );
                var ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteEmployeeById(int id)
        {
            try
            {
                var command = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = $"delete from Employees where id = {id}",
                    Connection = connector.GetConnection(),
                };
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
            

        }
        /// <summary>
        /// Получение сотрудников заданного отдела (из представления)
        /// </summary>
        /// <param name="Department"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable GetEmployeesByDepartment(string Department)
        {
            //Вызов из представления
            var selectcommandtext = $"select * from EmployeesFullInfo\r\nwhere EmployeesFullInfo.Отдел = '{Department}'";
            try
            {
                var adapter = new SqlDataAdapter(
              selectcommandtext,
              connector.GetConnection()
            );
                var ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Обновление информации о сотруднике(отдела и/или должности)
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="DepartmentId"></param>
        /// <param name="PositionId"></param>
        /// <returns></returns>
        public int UpdateEmployee(int EmployeeId, int DepartmentId, int PositionId)
        {
            try
            {
                var command = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = $"update Employees\r\nset DepartmentId = {DepartmentId}, PositionId = {PositionId}\r\nwhere id = {EmployeeId}",
                    Connection = connector.GetConnection(),
                };
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

        }
        /// <summary>
        /// Добавление нового сотрудника в базу данных
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public int AddEmployee(Employee employee)
        {
            try
            {
                var command = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = $"insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)\r\nvalues ('{employee.FirstName}', '{employee.LastName}', '{employee.Email}', '{employee.DateOfBirth}', {employee.DepartmentId}, {employee.PositionId})",
                    Connection = connector.GetConnection(),
                };
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

        }




    }
}
