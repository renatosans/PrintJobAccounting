
USE Accounting
GO


-----------------------------------------------------------------------------------
--  Cria a tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'tb_costCenter' AND xtype = 'U')
BEGIN
	CREATE TABLE tb_costCenter(
	    id int not null identity(1,1),
		tenantId int not null,
		name varchar(100) not null,
		parentId int null -- nós raiz possuem parent "null"
	)
END
GO


-----------------------------------------------------------------------------------
--  Cria a PK da tabela caso ela não exista
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pk_costCenterId' AND xtype = 'PK')
BEGIN
	ALTER TABLE tb_costCenter
	ADD CONSTRAINT pk_costCenterId PRIMARY KEY ([id])
END
GO

-----------------------------------------------------------------------------------
--  Adiciona uma self-referencing foreign key (centro de custo pai)
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'fk_costCenterRefToParent' AND xtype = 'F')
BEGIN
	ALTER TABLE tb_costCenter
	ADD CONSTRAINT fk_costCenterRefToParent FOREIGN KEY ([parentId])
	REFERENCES tb_costCenter([id])
END
GO

-----------------------------------------------------------------------------------
--  Adiciona constraints aos campos da tabela
-----------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_costCenterName' AND xtype = 'UQ')
BEGIN
	ALTER TABLE tb_costCenter
	ADD CONSTRAINT uq_costCenterName UNIQUE ([tenantId], [name])
END
GO
IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'udf_rootCount' AND xtype = 'FN')
BEGIN
    DROP FUNCTION udf_rootCount
END
GO
CREATE FUNCTION udf_rootCount(@tenantId INT) RETURNS INT AS
BEGIN
    RETURN (SELECT COUNT(*) FROM tb_costCenter WHERE tenantId = @tenantId AND parentId IS NULL)
END
GO
IF NOT EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'uq_rootCostCenter' AND xtype = 'C')
BEGIN
    ALTER TABLE tb_costCenter
    ADD CONSTRAINT uq_rootCostCenter CHECK (dbo.udf_rootCount(tenantId) = 1)
END
