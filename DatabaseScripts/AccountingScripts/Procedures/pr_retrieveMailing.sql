
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveMailing' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveMailing
END
GO

/************************************************************************

     Procedure: pr_retrieveMailing
     Descrição: Busca os mailings cadastrados para o tenant
     
     Autor: Renato R. Sanseverino
     Data: 06/08/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveMailing(
    @tenantId     int,
    @mailingId    int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    smtpServer,
	    frequency,
	    reportType,
	    recipients,
	    lastSend
	FROM
	    AppCommon..tb_mailing
	WHERE
	    tenantId = @tenantId
	    AND
	    id = ISNULL(@mailingId, id)
	    AND
	    removed = 0
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveMailing TO FrameworkUser
GO
