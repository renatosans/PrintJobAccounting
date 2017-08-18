
USE AppCommon
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_mailing' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_mailing(
		id int not null identity(1,1),
		tenantId int not null,
		smtpServer int not null,
		frequency int not null,
		reportType int not null,
		recipients varchar(255) not null,
		lastSend Datetime not null,
		removed bit not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_mailingId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_mailing
	ADD CONSTRAINT pk_mailingId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_mailingRefToTenant' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_mailing
	ADD CONSTRAINT fk_mailingRefToTenant FOREIGN KEY ([tenantId])
	REFERENCES tb_tenant([id])
END
GO
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_mailingRefToSmtpServer' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_mailing
	ADD CONSTRAINT fk_mailingRefToSmtpServer FOREIGN KEY ([smtpServer])
	REFERENCES tb_smtpServer([id])
END
GO

