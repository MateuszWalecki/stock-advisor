CREATE DATABASE StockAdvisor

USE StockAdvisor

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    Email nvarchar(100) NOT NULL,
    PasswordHash nvarchar(200) NOT NULL,
    Salt nvarchar(200) NOT NULL,
    FirstName nvarchar(100) NOT NULL,
    SurName nvarchar(100) NOT NULL,
    Role nvarchar(10) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
)

SELECT * FROM Users;