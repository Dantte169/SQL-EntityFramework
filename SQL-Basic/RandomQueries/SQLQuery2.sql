CREATE TABLE Towns(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20),
)

CREATE TABLE Adresses(
	Id INT PRIMARY KEY IDENTITY,
	AdressText NVARCHAR(50) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Departments(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50),
)

CREATE TABLE Employees (
  Id INT PRIMARY KEY IDENTITY,
  FirstName NVARCHAR(30) NOT NULL,
  MiddleName NVARCHAR(30) NOT NULL,
  LastName NVARCHAR(30) NOT NULL,
  JobTitle NVARCHAR(30) NOT NULL,
  DepartmentId INT FOREIGN KEY REFERENCES DEPARTMENTS(Id),
  HireDate DATE NOT NULL,
  Salary DECIMAL(10, 4) NOT NULL,
  AddressId INT FOREIGN KEY REFERENCES Adresses(Id)
)


INSERT INTO Towns 
			([Name])
VALUES
			('Sofia'),
			('Plovdiv'),
			('Varna'),
			('Burgas')

INSERT INTO Departments
			([Name])
VALUES
			('Engineering'),
			('Sales'),
			('Marketing'),
			('Software Development'),
			('Quality Assurance')

INSERT INTO Employees(FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary) VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '2013-02-01', 3500),
('Petar', 'Petrov', 'Petrov', 'Senior Engineer', 1, '2004-03-02', 4000),
('Maria', 'Petrova', 'Ivanova', 'Intern', 5, '2016-08-28', 525.25),
('Georgi', 'Terzeiv', 'Ivanov', 'CEO', 2, '2007-12-09', 3000),
('Peter', 'Pan', 'Pan', 'Intern', 2, '2016-08-28', 599.88)

SELECT * FROM Towns
ORDER BY [Name] ASC

SELECT * FROM Departments
ORDER BY [Name] ASC

SELECT * FROM Employees
ORDER BY [Salary] DESC

SELECT [Name] FROM Towns
ORDER BY [Name] ASC

SELECT [Name] FROM Departments
ORDER BY [Name] ASC

SELECT [FirstName], [LastName], [JobTitle],[Salary] FROM Employees
ORDER BY [Salary] DESC
