insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('������', '����������', 'macovetsky@mail.ru', '25.04.1975', 3, 3); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('������', '����������', 'kirlev@mail.ru', '25.04.1976', 3, 3); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('�����', '��������', 'sidorova@mail.ru', '05.04.2001', 1, 1); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('��������', '������', 'voitov@mail.ru', '05.08.1992', 1, 1); 

insert into Employees (FirstName, LastName, Email, DateOfBirth, DepartmentId, PositionId)
values ('�������', '������', 'emovd@mail.ru', '05.08.1993', 2, 2); 

--������ �����������

select FirstName as '���', LastName as '�������', Positions.Name as '���������', Departments.Name as '�����'
from Employees 
left join Positions
on (Employees.PositionId = Positions.Id)
left join Departments
on (Employees.DepartmentId = Departments.Id);

--�������������
Go
create view EmployeesFullInfo as
select Employees.id as ID, FirstName as '���', DateOfBirth as '���� ��������', Email as Email, LastName as '�������', Positions.Name as '���������', Departments.Name as '�����'
from Employees 
left join Positions
on (Employees.PositionId = Positions.Id)
left join Departments
on (Employees.DepartmentId = Departments.Id);

--��������� �� ����
go
select * from EmployeesFullInfo
where EmployeesFullInfo.ID = 1;

--���������� �� ������������
go
select * from EmployeesFullInfo
where EmployeesFullInfo.����� = '����� ������';

-- �������� ����������
go
update Employees
set DepartmentId = 2, PositionId = 2
where id = 4;

--������� ����������
go
delete from Employees where id = 5;


--������ ������������� � ����������

go
select Departments.Name,Positions.Name from Departments
left join Positions
on (Departments.id = Positions.DepartmentId);


--������ ��� ���������

select *from EmployeesFullInfo where
EmployeesFullInfo.������� = '����������';

--���������
go
create procedure GetEmployeeByLastName
@LastName nvarchar(50)
as
select *from EmployeesFullInfo where
EmployeesFullInfo.������� = @LastName;

--����� ���������

execute GetEmployeeByLastName @LastName = N'����������';