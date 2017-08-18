
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveCostCenter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveCostCenter
END
GO

/************************************************************************

     Procedure: pr_retrieveCostCenter
     Descrição: Busca centros de custo cadastrados no sistema de Accounting
     
     Autor: Renato R. Sanseverino
     Data: 11/05/2010

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveCostCenter(
    @tenantId         int,
    @costCenterId     int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    name,
	    parentId
	FROM
	    tb_costCenter
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@costCenterId, id)
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveCostCenter TO FrameworkUser
GO
