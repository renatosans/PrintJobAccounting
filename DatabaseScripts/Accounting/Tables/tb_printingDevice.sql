
USE Accounting
GO


-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_printingDevice' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_printingDevice(
		id int not null identity(1,1),
		tenantId int not null,
		ipAddress varchar(100) not null,
		description varchar(100) not null,
		serialNumber varchar(100) not null,
		counter int not null,
		lastUpdated Datetime not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_printingDeviceId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_printingDevice
	ADD CONSTRAINT pk_printingDeviceId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_printingDeviceSerial' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_printingDevice
	ADD CONSTRAINT uq_printingDeviceSerial UNIQUE ([tenantId], [serialNumber])
END
GO

