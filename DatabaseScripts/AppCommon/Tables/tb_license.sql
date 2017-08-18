
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_license' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_license(
		id int not null identity(1,1),
		tenantId int not null,
		installationKey varchar(255), -- null enquanto a licença não for utilizada
		installationDate datetime, -- null enquanto a licença não for utilizada
		computerName varchar(100), -- null enquanto a licença não for utilizada
		removed bit not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_licenseId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_license
	ADD CONSTRAINT pk_licenseId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_licenseRefToTenant' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_license
	ADD CONSTRAINT fk_licenseRefToTenant FOREIGN KEY ([tenantId])
	REFERENCES tb_tenant([id])
END
GO
