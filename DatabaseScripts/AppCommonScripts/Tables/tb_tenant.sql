
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_tenant' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_tenant(
		id int not null identity(1,1),
		name varchar(100) not null,
		alias varchar(100) not null,
        removed bit not null default 0
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_tenantId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_tenant
	ADD CONSTRAINT pk_tenantId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_tenantName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_tenant
	ADD CONSTRAINT uq_tenantName UNIQUE ([name])
END
GO

