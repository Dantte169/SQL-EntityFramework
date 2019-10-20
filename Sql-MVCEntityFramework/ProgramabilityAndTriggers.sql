--Problem 1
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
              AS
          SELECT e.FirstName,
	             e.LastName
            FROM Employees AS e
           WHERE e.Salary > 35000
			  
EXEC usp_GetEmployeesSalaryAbove35000

GO

--Problem 2
 CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber (@TargetSalary DECIMAL(18,4))
			   AS
		   SELECT e.FirstName,e.LastName
		     FROM Employees AS E
			WHERE e.Salary >= @TargetSalary

EXEC usp_GetEmployeesSalaryAboveNumber @TargetSalary = 48100

GO
			 
--Problem 3
CREATE PROCEDURE usp_GetTownsStartingWith (@TargetCity NVARCHAR(30))
			  AS
	      SELECT t.[Name]
	        FROM Towns AS t
 WHERE SUBSTRING(t.[Name],1,LEN(@TargetCity)) = @TargetCity


EXEC usp_GetTownsStartingWith @TargetCity = b

GO
--Problem 4
CREATE PROCEDURE usp_GetEmployeesFromTown (@TownName NVARCHAR(30))
              AS
          SELECT e.FirstName, e.LastName
            FROM Employees AS e
            JOIN Addresses AS a
              ON a.AddressID = e.AddressID
            JOIN Towns AS t
              ON t.TownID = a.TownID
           WHERE t.[Name] = @TownName 

EXEC usp_GetEmployeesFromTown @TownName = 'Sofia'

GO

--Problem 5
CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4))
RETURNS VARCHAR(7)
AS
BEGIN
	DECLARE @salaryLevel VARCHAR(7) = CASE
	      WHEN @salary < 30000 THEN 'Low'
		  WHEN @salary BETWEEN 30000 AND 50000 THEN 'Average'
		  ELSE 'High'
		END
	RETURN @SalaryLevel
END

GO
--Problem 6
CREATE PROCEDURE usp_EmployeesBySalaryLevel(@salaryLevel VARCHAR(7))
AS
BEGIN
	SELECT e.FirstName,e.LastName
	FROM Employees AS e
	WHERE dbo.ufn_GetSalaryLevel(e.Salary) = @salaryLevel
END

EXEC dbo.usp_EmployeesBySalaryLevel 'High'

GO

--Probelm 7
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX))
RETURNS BIT
AS
BEGIN
	DECLARE @counter INT = 1;
	DECLARE @currentLetter CHAR(1);

	WHILE(@counter <= LEN(@word))
	BEGIN
		SET @currentLetter = SUBSTRING(@word,@counter,1);

		DECLARE @charIndex INT = CHARINDEX(@currentLetter, @setOfLetters);
		IF(@charIndex <= 0)
		BEGIN
			RETURN 0;
		END

		SET @counter += 1
	END
	RETURN 1
END

GO

--PROBLEM 8
CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
--
	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (
		SELECT EmployeeID
		FROM Employees
		WHERE DepartmentID = @departmentId)
--
	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (
		SELECT EmployeeID
		FROM Employees
		WHERE DepartmentID = @departmentId)
--
	ALTER TABLE DEPARTMENTS
	ALTER COLUMN ManagerID INT
--
	UPDATE Departments
	SET ManagerID = NULL
	WHERE DepartmentID = @departmentId
--
	DELETE FROM Employees
	WHERE DepartmentID = @departmentId
--
	DELETE FROM Departments
	WHERE DepartmentID = @departmentID
--
	SELECT COUNT(*) FROM Employees
	WHERE DepartmentID = @departmentId
END

USE Bank
GO
--Problem 9
CREATE PROCEDURE usp_GetHoldersFullName
    AS
SELECT CONCAT( FirstName, ' ', LastName) AS  [Full Name]
  FROM AccountHolders

EXECUTE usp_GetHoldersFullName
GO

--Problem 10
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan(@Amount DECIMAL(15, 2))
AS
  SELECT ah.[FirstName], ah.[LastName]
    FROM AccountHolders AS ah
    JOIN Accounts AS a ON ah.Id = a.AccountHolderId
GROUP BY ah.[FirstName], ah.[LastName]
  HAVING SUM(a.[Balance]) > @Amount
ORDER BY ah.[FirstName], ah.[LastName];

EXECUTE usp_GetHoldersWithBalanceHigherThan 100000
GO

--Problem 11

CREATE FUNCTION ufn_CalculateFutureValue(@Sum DECIMAL(15, 2), @Rate FLOAT, @Years INT)
RETURNS DECIMAL(15, 4)
AS
BEGIN
	RETURN @sum * (POWER(1 + @Rate, @Years))
END
GO

--Problem 12

CREATE PROCEDURE usp_CalculateFutureValueForAccount(@AccountId INT, @Rate FLOAT)
AS
BEGIN
	SELECT [a].[Id] AS [Account Id],
	       [ah].[FirstName] AS [First Name],
		   [ah].[LastName] AS [Last Name],
		   [a].[Balance] AS [Current Balance],
		   dbo.ufn_CalculateFutureValue(a.[Balance], @Rate, 5) AS [Balance in 5 years]
	  FROM [dbo].[Accounts] AS a
	  JOIN [dbo].[AccountHolders] AS [ah] ON [a].[AccountHolderId] = [ah].[Id]
	 WHERE [a].[Id] = @AccountId
END
GO

--Problem 13
CREATE FUNCTION ufn_CashInUsersGames(@Game VARCHAR(50))
RETURNS TABLE
AS
RETURN
SELECT SUM([k].[Cash]) AS [SumCash]
  FROM (
SELECT [ug].[Cash] AS [Cash],
       ROW_NUMBER() OVER (PARTITION BY [g].[Name] ORDER BY [ug].[Cash] DESC) AS [Row]
  FROM [dbo].[Games] AS g
  JOIN [dbo].[UsersGames] AS [ug] ON [g].[Id] = [ug].[GameId]
 WHERE [g].[Name] = @Game) AS k
 WHERE [k].[Row] % 2 = 1
 GO