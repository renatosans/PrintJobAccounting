
USE Accounting
GO

-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_costCenterAssociate' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_costCenterAssociate(
		id int not null identity(1,1),
		tenantId int not null,
		costCenterId int not null,
		userId int not null
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_associateId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_costCenterAssociate
	ADD CONSTRAINT pk_associateId PRIMARY KEY ([id])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_associateUserId' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_costCenterAssociate
	ADD CONSTRAINT uq_associateUserId UNIQUE ([tenantId], [userId])
END
GO


-----------------------------------------------------------------------------------
--  Adiciona as Foreign Keys da tabela caso elas não existam
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_associateRefToCostCenter' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_costCenterAssociate
	ADD CONSTRAINT fk_associateRefToCostCenter FOREIGN KEY ([costCenterId])
	REFERENCES tb_costCenter([id])
END
GO

IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_associateRefToUser' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_costCenterAssociate
	ADD CONSTRAINT fk_associateRefToUser FOREIGN KEY ([userId])
	REFERENCES tb_user([id])
END
GO
