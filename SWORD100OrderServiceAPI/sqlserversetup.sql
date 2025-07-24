CREATE DATABASE OrderServiceDB;
GO

USE OrderServiceDB;
GO

CREATE TABLE Swords (
    SwordGuid UNIQUEIDENTIFIER NOT NULL,
    OrderGuid UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    ForgedDate DATETIME NOT NULL,
    PRIMARY KEY (SwordGuid, OrderGuid)   
);
GO

CREATE TABLE Orders (
    OrderGuid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    UserGuid UNIQUEIDENTIFIER NOT NULL,
    CartGuid UNIQUEIDENTIFIER NOT NULL,
    CreatedDate DATETIME NOT NULL CONSTRAINT DF_Orders_CreatedDate DEFAULT getdate()
);
GO

CREATE TABLE Users (
    UserGuid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),    
    Username NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,   
    CreatedDate DATETIME NOT NULL CONSTRAINT DF_Users_CreatedDate DEFAULT getdate()
);
GO

ALTER TABLE dbo.Swords ADD CONSTRAINT FK_Swords_Orders FOREIGN KEY (OrderGuid) REFERENCES dbo.Orders (OrderGuid);
GO

ALTER TABLE dbo.Orders ADD CONSTRAINT FK_Orders_Users FOREIGN KEY (UserGuid) REFERENCES dbo.Users (UserGuid);
GO

INSERT INTO dbo.Users (UserGuid, Username, Email, Password, CreatedDate) VALUES ('{E8E369C0-960B-4584-9A81-F9FF9F98DBD6}', 'will', 'realcars3@hotmail.com', 'abc123', '2023-03-31 13:56:55.483');
GO