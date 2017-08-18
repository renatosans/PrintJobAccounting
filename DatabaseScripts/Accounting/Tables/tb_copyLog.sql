
USE Accounting
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_copyLog' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_copyLog(
		id int not null identity(1,1),
		tenantId int not null,
		jobTime datetime not null,
		userId int not null,
		printerId int not null,
		pageCount int not null,
		duplex bit not null,
		color bit not null,
		jobCost money not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_copyLogId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_copyLog
	ADD CONSTRAINT pk_copyLogId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_copyLogRefToUser' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_copyLog
	ADD CONSTRAINT fk_copyLogRefToUser FOREIGN KEY ([userId])
	REFERENCES tb_user([id])
END
GO
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_copyLogRefToPrinter' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_copyLog
	ADD CONSTRAINT fk_copyLogRefToPrinter FOREIGN KEY ([printerId])
	REFERENCES tb_printer([id])
END
GO
