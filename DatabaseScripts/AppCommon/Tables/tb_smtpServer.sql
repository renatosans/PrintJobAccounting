
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_smtpServer' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_smtpServer(
		id int not null identity(1,1),
		tenantId int not null,
		name varchar(100) not null,
		address varchar(100) not null,
		port int not null,
		username varchar(100) not null,
		password varchar(100) not null,
		hash varchar(255) null,
		removed bit not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_smtpServerId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_smtpServer
	ADD CONSTRAINT pk_smtpServerId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_smtpServerName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_smtpServer
	ADD CONSTRAINT uq_smtpServerName UNIQUE ([tenantId], [name])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_smtpServerRefToTenant' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_smtpServer
	ADD CONSTRAINT fk_smtpServerRefToTenant FOREIGN KEY ([tenantId])
	REFERENCES tb_tenant([id])
END
GO

