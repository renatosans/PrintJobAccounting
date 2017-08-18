
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_login' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_login(
		id int not null identity(1,1),
		tenantId int not null,
		username varchar(100) not null,
		password varchar(100) not null,
		userGroup int not null,
		removed bit not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_loginId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_login
	ADD CONSTRAINT pk_loginId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_loginUsername' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_login
	ADD CONSTRAINT uq_loginUsername UNIQUE ([tenantId], [username])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_loginRefToTenant' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_login
	ADD CONSTRAINT fk_loginRefToTenant FOREIGN KEY ([tenantId])
	REFERENCES tb_tenant([id])
END
GO

