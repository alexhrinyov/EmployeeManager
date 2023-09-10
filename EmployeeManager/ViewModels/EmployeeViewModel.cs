using EmployeeManager.Commands;
using EmployeeManager.Data;
using EmployeeManager.Data.Entities;
using EmployeeManager.Data.Repositories;
using EmployeeManager.Models;
using EmployeeManager.ViewModels.Base;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace EmployeeManager.ViewModels
{
    /// <summary>
    /// Модель, связаная с основным окном
    /// </summary>
    internal class EmployeeViewModel : ViewModel
    {
        private MainConnector connector;
        private EmployeeRepository employeeRepository;
        /// <summary>
        /// Все должности
        /// </summary>
        private List<Position> positionsFromDB;
        /// <summary>
        /// Конструктор класса EmployeeViewModel
        /// </summary>
        public EmployeeViewModel()
        {
            Commands = new List<string> {
            "Поиск сотрудника по Id" ,
            "Обновить информацию о сотруднике",
            "Удалить сотрудника по Id",
            "Поиск сотрудника по фамилии",
            "Показать всех сотрудников",
            "Показать сотрудников выбранного отдела",
            "Добавить нового сотрудника"
            };

            //свойство-объект команды инициализируется, передаются параметры методов(исполняющий метод и разрешающий)
            ExecuteCommand = new LambdaCommand(OnExecuteCommandExecuted, CanExecuteCommandExecuted);
            CloseWindowCommand = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecuted);
            this.GetData();
        }


        #region Методы
        //Подключение к базе
        internal void Connect()
        {
            var result = connector.Connect();

            if (result)
            {

                employeeRepository = new EmployeeRepository(connector);
            }
            else
            {
                MessageBox.Show("Ошибка подключения к базе!");
            }
        }
        /// <summary>
        /// Привести dataTable к списку EmployeesInfo
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<EmployeeInfo> MapEmployees(DataTable table)
        {
            var employeesData = new List<EmployeeInfo>();

            foreach (DataRow employee in table.Rows)
            {
                employeesData.Add(new EmployeeInfo()
                {
                    Id = (int)employee["ID"],
                    FirstName = (string)employee["Имя"],
                    LastName = (string)employee["Фамилия"],
                    DateOfBirth = (DateTime)employee["Дата рождения"],
                    Department = (string)employee["Отдел"],
                    Email = (string)employee["Email"],
                    Position = (string)employee["Должность"]
                });
            }
            return employeesData;
        }
        /// <summary>
        /// Показать всех сотрудников
        /// </summary>
        public void GetEmployeesInfo()
        {
            var empls = employeeRepository.SelectAll("EmployeesFullInfo");

            EmployeesInfo = this.MapEmployees(empls);
        }
        /// <summary>
        /// Вытащить все данные
        /// </summary>
        public void GetData()
        {
            connector = new MainConnector();
            this.Connect();
            this.GetEmployeesInfo();
            
            //Вызываем метод репозитория
            var deps = employeeRepository.SelectAll("Departments");
            //Промежуточный список
            var depsData = new List<Department>();
            foreach (DataRow dep in deps.Rows)
            {
                depsData.Add(new Department() { Id = (int)dep["id"], Name = (string)dep["Name"] });
            }
            Departments = depsData;

            var poss = employeeRepository.SelectAll("Positions");
            var possData = new List<Position>();
            foreach (DataRow pos in poss.Rows)
            {
                possData.Add(new Position() { Id = (int)pos["id"], Name = (string)pos["Name"], DepartmentId = (int)pos["DepartmentId"] });
            }
            positionsFromDB = possData;
            Positions = possData;
            this.Disconnect();
        }
        //Отключение от базы
        internal void Disconnect()
        {
            connector.Disconnect();
        }
        #endregion

        //Свойства с реализацией интерфейса INotifyPropertyChanged для привязки к элементам формы. См. базовый класс ViewModel
        #region Свойства
        //Свойство - сущность из базы данных
        private List<Employee> employees;
        public List<Employee> Employees
        {
            get { return employees; }
            set => Set(ref employees, value);
        }

        // Свойство для отображения информации о сотрудниках
        private List<EmployeeInfo> employeesInfo;
        public List<EmployeeInfo> EmployeesInfo
        {
            get { return employeesInfo; }
            set => Set(ref employeesInfo, value);
        }

        private List<Department> departments;
        public List<Department> Departments
        {
            get { return departments; }
            set => Set(ref departments, value);
        }

        private List<Position> positions;
        public List<Position> Positions
        {
            get { return positions; }
            set => Set(ref positions, value);
        }

        private List<string> commands;
        /// <summary>
        /// Список команд
        /// </summary>
        public List<string> Commands
        {
            get { return commands; }
            set => Set(ref commands, value);
        }

        private string employeeLastName;
        /// <summary>
        /// Фамилия сотрудника
        /// </summary>
        public string EmployeeLastName
        {
            get { return employeeLastName; }
            set => Set(ref employeeLastName, value);
        }

        private int employeeId;
        public int EmployeeId
        {
            get { return employeeId; }
            set => Set(ref employeeId, value);
        }

        private string employeeFirstName;
        public string EmployeeFirstName
        {
            get { return employeeFirstName; }
            set => Set(ref employeeFirstName, value);
        }

        private string employeeEmail;
        public string EmployeeEmail
        {
            get { return employeeEmail; }
            set => Set(ref employeeEmail, value);
        }

        private DateTime employeeDate;
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime EmployeeDate
        {
            get { return employeeDate; }
            set => Set(ref employeeDate, value);
        }

        #endregion

        // Свойства, связанные с выбраными или введенными элементами в ComboBox, TextBox формы
        #region Selected Свойства
        private Department selectedDepartment;
        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                Set(ref selectedDepartment, value);
                // Автоматическое отображение только должностей, существующих в отделе
                Positions = positionsFromDB.Where(p => p.DepartmentId == SelectedDepartment.Id).ToList();
                SelectedPosition = Positions[0];
            }
        }

        private Position selectedPosition;
        public Position SelectedPosition
        {
            get { return selectedPosition; }
            set => Set(ref selectedPosition, value);
        }

        private string selectedCommand;
        public string SelectedCommand
        {
            get { return selectedCommand; }
            set => Set(ref selectedCommand, value);
        }



        #endregion

        #region Команды

        /// <summary>
        /// Основная команда для всех действий
        /// </summary>
        public ICommand ExecuteCommand { get; }
        public void OnExecuteCommandExecuted(object p)
        {
            connector.Connect();
            switch (SelectedCommand)
            {
                case "Поиск сотрудника по Id":
                    
                    if (EmployeeId != 0)
                    {
                        var empls = employeeRepository.GetEmployeeById(EmployeeId);
                        EmployeesInfo = this.MapEmployees(empls);

                    }
                    connector.Disconnect();
                    break;
                case "Обновить информацию о сотруднике":
                    if (EmployeeId != 0 && SelectedDepartment != null && SelectedPosition != null)
                    {
                        int resultCount = employeeRepository.UpdateEmployee(EmployeeId, SelectedDepartment.Id, selectedPosition.Id);
                        MessageBox.Show("Обновлено записей сотрудников:" + resultCount);
                        this.GetEmployeesInfo();
                        connector.Disconnect();
                    }
                    else
                        MessageBox.Show("Не выбран Id, отдел или должность");
                    break;
                case "Удалить сотрудника по Id":
                    if (EmployeeId != 0)
                    {
                        int resultCount = employeeRepository.DeleteEmployeeById(EmployeeId);
                        MessageBox.Show("Удалено сотрудников:" + resultCount);
                        this.GetEmployeesInfo();
                        connector.Disconnect();
                    }
                    else
                        MessageBox.Show("Не выбран Id");

                    break;
                case "Показать всех сотрудников":
                    this.GetEmployeesInfo();
                    break;
                case "Поиск сотрудника по фамилии":
                    if (!string.IsNullOrEmpty(EmployeeLastName))
                    {
                        var empls = employeeRepository.GetEmployeeByLastName(EmployeeLastName);

                        EmployeesInfo = this.MapEmployees(empls);
                        connector.Disconnect();
                    }
                    else
                        MessageBox.Show("Введите фамилию");
                    break;

                case "Показать сотрудников выбранного отдела":
                    if (SelectedDepartment != null)
                    {
                        var empls = employeeRepository.GetEmployeesByDepartment(SelectedDepartment.Name);
                        
                        EmployeesInfo = this.MapEmployees(empls);
                        connector.Disconnect();
                    }
                    else
                        MessageBox.Show("Не выбран Id, отдел или должность");
                    break;
                case "Добавить нового сотрудника":
                    if (!string.IsNullOrEmpty(EmployeeFirstName)&& !string.IsNullOrEmpty(EmployeeLastName))
                    {
                        Employee employee;
                        if (SelectedDepartment != null)
                        {
                            employee = new Employee() { FirstName = EmployeeFirstName, LastName = EmployeeLastName, Email = EmployeeEmail, DateOfBirth = EmployeeDate, DepartmentId = SelectedDepartment.Id, PositionId = SelectedPosition.Id };
                        }
                        else
                        {
                            employee = new Employee() { FirstName = EmployeeFirstName, LastName = EmployeeLastName, Email = EmployeeEmail, DateOfBirth = EmployeeDate};
                        }
                        
                        int resultCount = employeeRepository.AddEmployee(employee);
                        MessageBox.Show("Добавлено сотрудников:" + resultCount);
                        this.GetEmployeesInfo();
                        connector.Disconnect();
                    }
                    else
                        MessageBox.Show("Введите имя и фамилию сотрудника");
                    break;

            }

        }
        private bool CanExecuteCommandExecuted(object p) => true;
        /// <summary>
        /// Выход из приложения
        /// </summary>
        public ICommand CloseWindowCommand { get; }
        public void OnCloseWindowCommandExecuted(object p)
        {
            System.Windows.Application.Current.Shutdown();

        }
        private bool CanCloseWindowCommandExecuted(object p) => true;
        #endregion
    }
}
