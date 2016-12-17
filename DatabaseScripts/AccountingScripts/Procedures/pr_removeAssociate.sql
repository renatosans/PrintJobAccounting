
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeAssociate' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeAssociate
END
GO

/***************************************************************************

     Procedure: pr_removeAssociate
     Descrição: Remove um associado, caso receba apenas o costCenterId remove
     todos os associados do centro de custo

     Autor: Renato R. Sanseverino
     Data: 21/05/2010

****************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeAssociate(
    @associateId      int = NULL,
    @costCenterId     int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		IF @associateId IS NULL
		BEGIN
		    -- Remove todos os associados do centro de custo
		    DELETE FROM
		        tb_costCenterAssociate
		    WHERE
		        costCenterId = @costCenterId
		END
		ELSE
		BEGIN
		    -- Remove o associado
		    DELETE FROM
		        tb_costCenterAssociate
		    WHERE
		        id = @associateId
		END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeAssociate TO FrameworkUser
GO
