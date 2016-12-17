
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveSmtpServer' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveSmtpServer
END
GO

/************************************************************************

     Procedure: pr_retrieveSmtpServer
     Descrição: Busca os servidores de smtp cadastrados para o tenant
     
     Autor: Renato R. Sanseverino
     Data: 05/08/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveSmtpServer(
    @tenantId     int,
    @smtpServerId int = NULL    
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    name,
	    address,
	    port,
	    username,
	    password,
	    hash
	FROM
	    AppCommon..tb_smtpServer
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@smtpServerId, id)
	    AND
	    removed = 0
	ORDER BY id
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveSmtpServer TO FrameworkUser
GO
