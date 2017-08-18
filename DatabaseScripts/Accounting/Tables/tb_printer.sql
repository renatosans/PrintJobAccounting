
USE Accounting
GO


-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_printer' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_printer(
		id int not null identity(1,1),
		tenantId int not null,
		name varchar(100) not null,
		alias varchar(100) not null,
		pageCost money not null default 0.00,
		colorCostDiff money not null default 0.00,
		duplexCostDiff money not null default 0.00,
		bwPrinter bit not null default 1,
		removed bit not null default 0
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_printerId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_printer
	ADD CONSTRAINT pk_printerId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_printerName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_printer
	ADD CONSTRAINT uq_printerName UNIQUE ([tenantId], [name])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
-- IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_printerRefToTenant' AND xtype = 'F')
-- BEGIN
	-- Verificar como distribuir as entidades entre os bancos Accouting e AppCommon
	--
	-- ALTER TABLE tb_printer
	-- ADD CONSTRAINT fk_printerRefToTenant FOREIGN KEY ([tenantId])
	-- REFERENCES AppCommon..tb_tenant([id])
-- END
-- GO

