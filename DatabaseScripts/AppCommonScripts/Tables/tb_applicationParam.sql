
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_applicationParam' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_applicationParam(
		id int not null identity(1,1),
		name varchar(100) not null,
		value varchar(255) not null,
		applicationId int not null,
		ownerTask varchar(100) not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_applicationParamId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_applicationParam
	ADD CONSTRAINT pk_applicationParamId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_applicationParamName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_applicationParam
	ADD CONSTRAINT uq_applicationParamName UNIQUE ([name], [applicationId], [ownerTask])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_applicationParamRefToApplication' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_applicationParam
	ADD CONSTRAINT fk_applicationParamRefToApplication FOREIGN KEY ([applicationId])
	REFERENCES tb_application([id])
END
GO


-----------------------------------------------------------------------------------
--  Insere na tabela os dados iniciais (População de dados)
-----------------------------------------------------------------------------------
--INSERT INTO tb_applicationParam
--VALUES ('logDirectory', '\\vs3\printlog\csv\daily', 1, 'printLogImport')
--INSERT INTO tb_applicationParam
--VALUES ('importPreviousLogs', 'true', 1, 'printLogImport')
--INSERT INTO tb_applicationParam
--VALUES ('interval', '599000', 1, 'printLogImport') -- 10 minutos aproximadamente
--INSERT INTO tb_applicationParam
--VALUES ('lastAccess', '01/01/2009', 1, 'printLogImport')

--INSERT INTO tb_applicationParam
--VALUES ('logDirectory', '\\vs3\datacount', 1, 'copyLogImport')
--INSERT INTO tb_applicationParam
--VALUES ('interval', '599000', 1, 'copyLogImport') -- 10 minutos aproximadamente
--INSERT INTO tb_applicationParam
--values ('lastAccess', '01/01/2009', 1, 'copyLogImport')
