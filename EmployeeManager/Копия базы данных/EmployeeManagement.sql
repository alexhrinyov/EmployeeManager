insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('Максим', 'Маковецкий', 'macovetsky@mail.ru', '25.04.1975', 3, 3); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('Кирилл', 'Левданский', 'kirlev@mail.ru', '25.04.1976', 3, 3); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('Ирина', 'Сидорова', 'sidorova@mail.ru', '05.04.2001', 1, 1); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('Владимир', 'Войтов', 'voitov@mail.ru', '05.08.1992', 1, 1); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('Евгений', 'Мовдян', 'emovd@mail.ru', '05.08.1993', 2, 2); 

--Список сотрудников

select FirstName as 'Имя', LastName as 'Фамилия', Positions.Name as 'Должность', Departments.Name as 'Отдел'
from Employees 
left join Positions
on (Employees.PositionId = Positions.Id)
left join Departments
on (Employees.DepartmentId = Departments.Id);

--Представление
Go
create view EmployeesFullInfo as
select Employees.id as ID, FirstName as 'Имя', DateOfBirth as 'Дата рождения', Email as Email, LastName as 'Фамилия', Positions.Name as 'Должность', Departments.Name as 'Отдел'
from Employees 
left join Positions
on (Employees.PositionId = Positions.Id)
left join Departments
on (Employees.DepartmentId = Departments.Id);

--Сотрудник по айди
go
select * from EmployeesFullInfo
where EmployeesFullInfo.ID = 1;

--Сотрудники по департаменту
go
select * from EmployeesFullInfo
where EmployeesFullInfo.Отдел = 'Отдел продаж';

-- Изменить сотрудника
go
update Employees
set DepartmentId = 2, PositionId = 2
where id = 4;

--Удалить сотрдуника
go
delete from Employees where id = 5;


--Список департаментов и должностей

go
select Departments.Name,Positions.Name from Departments
left join Positions
on (Departments.id = Positions.DepartmentId);


--Запрос для процедуры

select *from EmployeesFullInfo where
EmployeesFullInfo.Фамилия = 'Левданский';

--Процедура
go
create procedure GetEmployeeByLastName
@LastName nvarchar(50)
as
select *from EmployeesFullInfo where
EmployeesFullInfo.Фамилия = @LastName;

--Вызов процедуры

execute GetEmployeeByLastName @LastName = N'Левданский';