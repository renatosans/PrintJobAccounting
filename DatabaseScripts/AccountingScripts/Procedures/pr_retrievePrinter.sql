
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrievePrinter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrievePrinter
END
GO

/************************************************************************

     Procedure: pr_retrievePrinter
     Descrição: Busca impressoras cadastradas no sistema de Accounting
     
     Autor: Renato R. Sanseverino
     Data: 08/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrievePrinter(
    @tenantId     int,
    @printerId    int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    name,
	    alias,
	    pageCost,
	    colorCostDiff,
	    duplexCostDiff,
	    bwPrinter
	FROM
	    tb_printer
	WHERE
	    tenantId = @tenantId
	    AND	
	    id = ISNULL(@printerId, id)
	    AND
	    removed = 0
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrievePrinter TO FrameworkUser
GO
