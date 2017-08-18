
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeSmtpServer' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeSmtpServer
END
GO

/************************************************************************

     Procedure: pr_storeSmtpServer
     Descrição: Armazena/atualiza os dados de um servidor smtp no banco
     
     Autor: Renato R. Sanseverino
     Data: 06/08/2009
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeSmtpServer(
    @smtpServerId  int,
    @tenantId      int,
    @name          varchar(100),
    @address       varchar(100),
    @port          int,
    @username      varchar(100),
    @password      varchar(100),
    @hash          varchar(255) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_smtpServer WHERE id = @smtpServerId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_smtpServer
		SET
		    name     = @name,
		    address  = @address,
		    port     = @port,
		    username = @username,
		    password = @password,
		    hash     = @hash
		WHERE
		    id = @smtpServerId
		    AND
		    tenantId = @tenantId
		    AND
		    removed = 0
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_smtpServer
		VALUES
		    (@tenantId, @name, @address, @port, @username, @password, @hash, 0)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeSmtpServer TO FrameworkUser
GO
