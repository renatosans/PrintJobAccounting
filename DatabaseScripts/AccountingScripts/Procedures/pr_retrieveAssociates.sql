
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveAssociates' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveAssociates
END
GO

/*****************************************************************************

     Procedure: pr_retrieveAssociates
     Descrição: Busca os usuários associados a um centro de custo, caso não seja
	 especificado o centro de custo traz todos os associados ao tenant
     
     Autor: Renato R. Sanseverino
     Data: 19/05/2010

*****************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveAssociates(
    @tenantId         int,
    @costCenterId     int  =  NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    CCA.id,
	    CCA.tenantId,
	    CCA.costCenterId,
	    CCA.userId,
	    USR.alias userName
	FROM
	    tb_costCenterAssociate CCA
	    INNER JOIN tb_user USR WITH (NOLOCK)
	        ON CCA.userId = USR.id
	WHERE
	    CCA.tenantId = @tenantId
	    AND
	    CCA.costCenterId = ISNULL(@costCenterId, costCenterId)
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveAssociates TO FrameworkUser
GO
