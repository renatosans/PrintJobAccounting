
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveLicense' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveLicense
END
GO

/************************************************************************

     Procedure: pr_retrieveLicense
     Descrição: Busca licenças de uso do sistema atribuidas ao tenant
     
     Autor: Renato R. Sanseverino
     Data: 25/10/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveLicense(
    @tenantId    int,
    @licenseId   int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
        installationKey,
        installationDate,
        computerName
	FROM
	    AppCommon..tb_license
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@licenseId, id)
	    AND
	    removed = 0
	ORDER BY id
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveLicense TO FrameworkUser
GO
