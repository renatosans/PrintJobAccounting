
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_application' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_application(
		id int not null identity(1,1),
		name varchar(100) not null,
		type int not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_applicationId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_application
	ADD CONSTRAINT pk_applicationId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_applicationName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_application
	ADD CONSTRAINT uq_applicationName UNIQUE ([name])
END
GO
