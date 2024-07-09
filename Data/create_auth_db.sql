CREATE DATABASE OutOfOffice_Auth
ON (NAME = OutOfOffice_Auth_data, FILENAME = 'C:\SQL Databases\OutOfOffice_Auth.mdf')
LOG 
ON (NAME = OutOfOffice_Auth_log, FILENAME = 'C:\SQL Databases\OutOfOffice_Auth.ldf');
GO

USE OutOfOffice_Auth;

-- DROP DATABASE OutOfOffice_Auth;

CREATE TABLE Users (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DateTimeOffset NULL,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
	FirstName NVARCHAR(MAX) NULL,
    LastName NVARCHAR(MAX) NULL
);

CREATE TABLE Roles (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NULL,
    NormalizedName NVARCHAR(256) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);

CREATE TABLE UserRoles (
    UserId NVARCHAR(450) NOT NULL,
    RoleId NVARCHAR(450) NOT NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserRoles_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

CREATE TABLE UserClaim (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_UserClaim_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE UserLogins (
    LoginProvider NVARCHAR(128) NOT NULL,
    ProviderKey NVARCHAR(128) NOT NULL,
    ProviderDisplayName NVARCHAR(MAX) NULL,
    UserId NVARCHAR(450) NOT NULL,
    CONSTRAINT PK_UserLogins PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_UserLogins_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE UserTokens (
    UserId NVARCHAR(450) NOT NULL,
    LoginProvider NVARCHAR(128) NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Value NVARCHAR(MAX) NULL,
    CONSTRAINT PK_UserTokens PRIMARY KEY (UserId, LoginProvider, Name),
    CONSTRAINT FK_UserTokens_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE RoleClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleId NVARCHAR(450) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_RoleClaims_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
