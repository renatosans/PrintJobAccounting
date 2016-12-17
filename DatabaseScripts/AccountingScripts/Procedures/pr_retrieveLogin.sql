
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveLogin' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveLogin
END
GO

/************************************************************************

     Procedure: pr_retrieveLogin
     Descrição: Busca logins cadastrados para o tenant
     
     Autor: Renato R. Sanseverino
     Data: 25/08/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveLogin(
    @tenantId    int,
    @loginId     int = NULL,
    @username    varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    username,
	    password,
	    userGroup
	FROM
	    AppCommon..tb_login
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@loginId, id)
	    AND
	    username = ISNULL(@username, username)
	    AND
	    removed = 0
	ORDER BY id
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveLogin TO FrameworkUser
GO
