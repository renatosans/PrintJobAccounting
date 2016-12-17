
USE Accounting
GO


-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_user' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_user(
		id int not null identity(1,1),
		tenantId int not null,
		name varchar(100) not null,
		alias varchar(100) not null,
		quota money null,
		removed bit not null default 0
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_userId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_user
	ADD CONSTRAINT pk_userId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_userName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_user
	ADD CONSTRAINT uq_userName UNIQUE ([tenantId], [name])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
-- IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_userRefToTenant' AND xtype = 'F')
-- BEGIN
	-- Verificar como distribuir as entidades entre os bancos Accouting e AppCommon
	--
	-- ALTER TABLE tb_user
	-- ADD CONSTRAINT fk_userRefToTenant FOREIGN KEY ([tenantId])
	-- REFERENCES AppCommon..tb_tenant([id])
-- END
-- GO

