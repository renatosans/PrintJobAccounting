
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeMailing' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeMailing
END
GO

/************************************************************************

     Procedure: pr_storeMailing
     Descrição: Armazena/atualiza os dados de um mailing no banco
     
     Autor: Renato R. Sanseverino
     Data: 07/08/2009
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeMailing(
    @mailingId    int,
    @tenantId     int,
    @smtpServer   int,
    @frequency    int,
    @reportType   int,
    @recipients   varchar(255),
    @lastSend     Datetime
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_mailing WHERE id = @mailingId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_mailing
		SET
			smtpServer = @smtpServer,
			frequency  = @frequency,
			reportType = @reportType,
			recipients = @recipients,
			lastSend   = @lastSend
		WHERE
			id = @mailingId
			AND
			tenantId = @tenantId
			AND
			removed = 0
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_mailing
		VALUES
		    (@tenantId, @smtpServer, @frequency, @reportType, @recipients, @lastSend, 0)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeMailing TO FrameworkUser
GO
