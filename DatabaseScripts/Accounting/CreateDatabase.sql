
CREATE DATABASE Accounting
GO


USE Accounting
GO


-- sp_grantdbaccess FrameworkUser
CREATE USER FrameworkUser FOR LOGIN FrameworkUser
GO
sp_addrolemember db_datareader, FrameworkUser
GO
sp_addrolemember db_datawriter, FrameworkUser
GO

