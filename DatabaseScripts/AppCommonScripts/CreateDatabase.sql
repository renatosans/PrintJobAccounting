
CREATE DATABASE AppCommon
GO


USE AppCommon
GO


sp_grantdbaccess FrameworkUser
GO
sp_addrolemember db_datareader, FrameworkUser
GO
sp_addrolemember db_datawriter, FrameworkUser
GO
