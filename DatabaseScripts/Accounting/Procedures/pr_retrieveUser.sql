
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveUser' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveUser
END
GO

/************************************************************************

     Procedure: pr_retrieveUser
     Descrição: Busca usuarios cadastrados no sistema de Accounting
     
     Autor: Renato R. Sanseverino
     Data: 07/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveUser(
    @tenantId     int,
    @userId       int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    name,
	    alias,
	    quota
	FROM
	    tb_user
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@userId, id)
	    AND
	    removed = 0
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveUser TO FrameworkUser
GO
