
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveAdministratorLogin' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveAdministratorLogin
END
GO

/************************************************************************

     Procedure: pr_retrieveAdministratorLogin
     Descrição: Busca logins de administradores do sistema
     
     Autor: Renato R. Sanseverino
     Data: 02/12/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveAdministratorLogin(
    @loginId     int = NULL,
    @username    varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    username,
	    password
	FROM
	    AppCommon..tb_administratorLogin
	WHERE
	    id = ISNULL(@loginId, id)
	    AND
	    username = ISNULL(@username, username)
	ORDER BY id
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveAdministratorLogin TO FrameworkUser
GO
