CREATE DATABASE OutOfOffice
ON (NAME = OutOfOffice_data, FILENAME = 'C:\SQL Databases\OutOfOffice.mdf')
LOG 
ON (NAME = OutOfOffice_log, FILENAME = 'C:\SQL Databases\OutOfOffice.ldf');
GO

USE OutOfOffice;

-- DROP DATABASE OutOfOffice;

CREATE TABLE Employee (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    Subdivision NVARCHAR(255) NOT NULL,
    Position NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    PeoplePartnerId INT,
	ProjectId INT,
    OutOfOfficeBalance INT NOT NULL,
    Photo VARBINARY(MAX),
    FOREIGN KEY (PeoplePartnerId) REFERENCES Employee(Id)
);

CREATE TABLE Project (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProjectType NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE,
	ProjectManagerId INT,
    Comment NVARCHAR(MAX),
    Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (ProjectManagerId) REFERENCES Employee(Id)
);

CREATE TABLE LeaveRequest (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT,
    AbsenceReason NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Comment NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'New',
    FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
);

CREATE TABLE ApprovalRequest (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ApproverId INT,
    LeaveRequestId INT,
    Status NVARCHAR(50) DEFAULT 'New',
    Comment NVARCHAR(MAX),
    FOREIGN KEY (ApproverId) REFERENCES Employee(Id),
    FOREIGN KEY (LeaveRequestId) REFERENCES LeaveRequest(Id)
);