
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeCostCenter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeCostCenter
END
GO

/***************************************************************************

     Procedure: pr_removeCostCenter
     Descrição: Remove um centro de custo da tabela, caso receba apenas o
     tenantId remove todos os centros de custo deste tenant

     Autor: Renato R. Sanseverino
     Data: 17/05/2010

****************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeCostCenter(
    @costCenterId     int = NULL,
    @tenantId         int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		IF @costCenterId IS NULL
		BEGIN
		    -- Remove todos os centros de custo do tenant
		    DELETE FROM
		        tb_costCenter
		    WHERE
		        tenantId = @tenantId
		END
		ELSE
		BEGIN
		    -- Remove o centro de custo
		    DELETE FROM
		        tb_costCenter
		    WHERE
		        id = @costCenterId
		END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeCostCenter TO FrameworkUser
GO
