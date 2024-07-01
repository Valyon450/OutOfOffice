CREATE DATABASE OutOfOffice
ON (NAME = OutOfOffice_data, FILENAME = 'C:\SQL Test\OutOfOffice.mdf')
LOG 
ON (NAME = OutOfOffice_log, FILENAME = 'C:\SQL Test\OutOfOffice.ldf');
GO

USE OutOfOffice;

CREATE TABLE Employees (
    ID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    Subdivision NVARCHAR(255) NOT NULL,
    Position NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    PeoplePartner INT,
    OutOfOfficeBalance INT NOT NULL,
    Photo VARBINARY(MAX),
    FOREIGN KEY (PeoplePartner) REFERENCES Employees(ID)
);

CREATE TABLE LeaveRequests (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Employee INT,
    AbsenceReason NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Comment NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'New',
    FOREIGN KEY (Employee) REFERENCES Employees(ID)
);

CREATE TABLE ApprovalRequests (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Approver INT,
    LeaveRequest INT,
    Status NVARCHAR(50) DEFAULT 'New',
    Comment NVARCHAR(MAX),
    FOREIGN KEY (Approver) REFERENCES Employees(ID),
    FOREIGN KEY (LeaveRequest) REFERENCES LeaveRequests(ID)
);

CREATE TABLE Projects (
    ID INT PRIMARY KEY IDENTITY(1,1),
    ProjectType NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE,
    ProjectManager INT,
    Comment NVARCHAR(MAX),
    Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (ProjectManager) REFERENCES Employees(ID)
);
