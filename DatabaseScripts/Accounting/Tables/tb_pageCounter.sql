

USE Accounting
GO


-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_pageCounter' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_pageCounter(
		id int not null identity(1,1),
		deviceId int not null,
		counter int not null,
		date Datetime not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_pageCounterId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_pageCounter
	ADD CONSTRAINT pk_pageCounterId PRIMARY KEY ([id])
END
GO

