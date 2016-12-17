
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeCostCenter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeCostCenter
END
GO

/************************************************************************

     Procedure: pr_storeCostCenter
     Descrição: Armazena os dados de um centro de custo no banco

     Autor: Renato R. Sanseverino
     Data: 17/05/2010

     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeCostCenter(
    @costCenterId     int,
    @tenantId         int,
    @name             varchar(100),
    @parentId         int  =  NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM tb_costCenter WHERE id = @costCenterId)
	BEGIN
	    -- Atualiza dados existentes no banco
	    UPDATE
	        tb_costCenter
	    SET
	        name = @name,
	        parentId = @parentId
	    WHERE
	        id = @costCenterId
	END
	ELSE
	BEGIN
	    -- Insere um novo registro no banco	
	    INSERT INTO
	        tb_costCenter
	    VALUES
	        (@tenantId, @name, @parentId)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeCostCenter TO FrameworkUser
GO
