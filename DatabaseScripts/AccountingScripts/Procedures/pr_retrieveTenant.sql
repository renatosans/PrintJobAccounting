
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveTenant' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveTenant
END
GO

/************************************************************************

     Procedure: pr_retrieveTenant
     Descrição: Busca os dados de um tenant no banco
     
     Autor: Renato R. Sanseverino
     Data: 03/08/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveTenant(
    @tenantId      int = NULL,
    @tenantName    varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    name,
	    alias
	FROM
	    AppCommon..tb_tenant
	WHERE
	    id = ISNULL(@tenantId, id)
	    AND
	    name = ISNULL(@tenantName, name)
	    AND
	    removed = 0


	SET NOCOUNT ON
		
END
GO

GRANT EXEC ON pr_retrieveTenant TO FrameworkUser
GO
