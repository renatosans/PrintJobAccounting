
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_administratorLogin' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_administratorLogin(
		id int not null identity(1,1),
		username varchar(100) not null,
		password varchar(100) not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_administratorLoginId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_administratorLogin
	ADD CONSTRAINT pk_administratorLoginId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_administratorUsername' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_administratorLogin
	ADD CONSTRAINT uq_administratorUsername UNIQUE ([username])
END
GO
