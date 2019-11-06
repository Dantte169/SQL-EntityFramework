--Problem 1
CREATE TABLE Passports(
	PassportID INT PRIMARY KEY,
	PassportNumber NVARCHAR(30)
)

CREATE TABLE Persons(
	PersonID INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30),
	Salary DECIMAL(8,2),
	PassportID INT,
	FOREIGN KEY(PassportID) REFERENCES Passports(PassportID)
)

INSERT INTO Passports(PassportID,PassportNumber) VALUES
(101, 'N34FG21B'),
(102, 'K65LO4R7'),
(103, 'ZE657QP2')

INSERT INTO Persons(FirstName,Salary,PassportID) VALUES
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00,101)

SELECT *
  FROM Persons AS p
  JOIN Passports As pass ON pass.PassportID = p.PassportID;

  --Problem 2

CREATE TABLE Manufacturers(
	ManufacturerID INT PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
	EstablishedOn DATE NOT NULL
)

CREATE TABLE Models(
	ModelID INT PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
	ManufacturerID INT FOREIGN KEY(ManufacturerID) REFERENCES Manufacturers(ManufacturerID) NOT NULL	
)

INSERT INTO Manufacturers (ManufacturerID,[Name],EstablishedOn) VALUES
(1,'BMW','07/03/1916'),
(2,'Tesla','01/01/2003'),
(3,'Lada','01/05/1996')

INSERT INTO Models(ModelID,[Name],ManufacturerID) VALUES
(101,'X1',1),
(102,'I6',1),
(103,'Model S',2),
(104,'Model X', 2),
(105,'Model 3', 2),
(106,'Nova',3)

SELECT [ModelID],mo.[Name], mo.ManufacturerID,ma.[Name],ma.EstablishedOn
FROM Models as mo
JOIN Manufacturers AS ma ON mo.ManufacturerID = ma.ManufacturerID

--Problem 3

CREATE TABLE Students(
	StudentID INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL
)

CREATE TABLE Exams(
	ExamID INT PRIMARY KEY NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
)

CREATE TABLE StudentsExams(
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
	ExamID INT FOREIGN KEY REFERENCES Exams(ExamID),
	CONSTRAINT PK_CompositeStudentIDExamID
	PRIMARY KEY(StudentID, ExamID)
)

INSERT INTO Students([Name]) VALUES
('Mila'), ('Toni'), ('Ron')

INSERT INTO Exams(ExamID,[Name]) VALUES
(101, 'SpringMVC'),
(102, 'Neo4j'),
(103,'Oracle 11g')

INSERT INTO StudentsExams(StudentID,ExamID) VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--Problem 4

CREATE TABLE Teachers
(
	TeacherID INT PRIMARY KEY IDENTITY(101, 1) NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES Teachers(TeacherID)
);

INSERT INTO Teachers([Name], ManagerID) VALUES
('John', NULL),
('Maya', 106),
('Silvia', 106),
('Ted', 105),
('Mark', 101),
('Greta', 101);

--Problem 5
CREATE DATABASE OnlineStoreDatabase
USE OnlineStoreDatabase

CREATE TABLE Cities(
	CityID INT PRIMARY KEY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
)

CREATE TABLE Customers(
	CustomerID INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	Birthday DATE NOT NULL,
	CityID INT FOREIGN KEY REFERENCES Cities(CityID) NOT NULL
)

CREATE TABLE Orders(
	OrderID INT PRIMARY KEY NOT NULL,
	CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID) NOT NULL
)


CREATE TABLE ItemTypes
(
	ItemTypeID INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE Items
(
	ItemID INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	ItemTypeID INT FOREIGN KEY REFERENCES ItemTypes(ItemTypeID) NOT NULL,
);

CREATE TABLE OrderItems
(
	OrderID INT FOREIGN KEY REFERENCES Orders(OrderID) NOT NULL,
	ItemID INT FOREIGN KEY REFERENCES Items(ItemID) NOT NULL,
	CONSTRAINT PK_OrderItems PRIMARY KEY (OrderID, ItemID)
);

--Problem 6
CREATE DATABASE University
Use University

CREATE TABLE Majors
(
	MajorID INT PRIMARY KEY IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE Students
(
	StudentID INT PRIMARY KEY IDENTITY NOT NULL,
	StudentNumber VARCHAR(50) NOT NULL,
	StudentName VARCHAR(50) NOT NULL,
	MajorID INT FOREIGN KEY REFERENCES Majors(MajorID) NOT NULL
);

CREATE TABLE Payments
(
	PaymentID INT PRIMARY KEY IDENTITY NOT NULL,
	PaymentDate DATE NOT NULL,
	PaymentAmount DECIMAL(15, 2) NOT NULL,
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID) NOT NULL
);

CREATE TABLE Subjects
(
	SubjectID INT PRIMARY KEY IDENTITY NOT NULL,
	SubjectName VARCHAR(50) NOT NULL
);

CREATE TABLE Agenda
(
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID) NOT NULL,
	SubjectID INT FOREIGN KEY REFERENCES Subjects(SubjectID) NOT NULL,
	CONSTRAINT PK_Agenda PRIMARY KEY (StudentID, SubjectID)
);