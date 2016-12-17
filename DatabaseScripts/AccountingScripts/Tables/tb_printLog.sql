
USE Accounting
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_printLog' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_printLog(
		id int not null identity(1,1),
		tenantId int not null,
		jobTime datetime not null,
		userId int not null,
		printerId int not null,
		documentName varchar(100) not null,
		pageCount int not null,
		copyCount int not null,
		duplex bit not null,
		color bit not null,
		jobCost money not null
--		workstation(estacao_trabalho) varchar(50),
--		paperFormat(formato_papel) varchar(20),
--		printerLanguage(linguagem_impressora) varchar(30),
--		height(altura) int,
--		width(largura) int,
--		fileSize(tamanho_arquivo) int
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_printLogId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_printLog
	ADD CONSTRAINT pk_printLogId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
-- IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_printLogRefToTenant' AND xtype = 'F')
-- BEGIN
	-- Verificar como distribuir as entidades entre os bancos Accouting e AppCommon
	--
	-- ALTER TABLE tb_printLog
	-- ADD CONSTRAINT fk_printLogRefToTenant FOREIGN KEY ([tenantId])
	-- REFERENCES AppCommon..tb_tenant([id])
-- END
-- GO
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_printLogRefToUser' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_printLog
	ADD CONSTRAINT fk_printLogRefToUser FOREIGN KEY ([userId])
	REFERENCES tb_user([id])
END
GO
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_printLogRefToPrinter' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_printLog
	ADD CONSTRAINT fk_printLogRefToPrinter FOREIGN KEY ([printerId])
	REFERENCES tb_printer([id])
END
GO
