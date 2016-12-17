
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_tenantPreference' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_tenantPreference(
		id int not null identity(1,1),
		tenantId int not null,
		name varchar(100) not null,
		value varchar(255) not null,
		type varchar(80) not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_tenantPreferenceId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_tenantPreference
	ADD CONSTRAINT pk_tenantPreferenceId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_tenantPreferenceName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_tenantPreference
	ADD CONSTRAINT uq_tenantPreferenceName UNIQUE ([tenantId], [name])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_tenantPreferenceRefToTenant' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_tenantPreference
	ADD CONSTRAINT fk_tenantPreferenceRefToTenant FOREIGN KEY ([tenantId])
	REFERENCES tb_tenant([id])
END
GO

