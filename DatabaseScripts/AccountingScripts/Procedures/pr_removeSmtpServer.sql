
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeSmtpServer' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeSmtpServer
END
GO

/************************************************************************

     Procedure: pr_removeSmtpServer
     Descri��o: Marca um servidor smtp como exclu�do na tabela
     
     Autor: Renato R. Sanseverino
     Data: 19/08/2009

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeSmtpServer(
    @smtpServerId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    AppCommon..tb_smtpServer
		SET
		    -- � adicionado texto com a hora de remo��o para evitar conflitos de nomes repetidos, permitindo
		    -- assim excluir v�rias vezes o mesmo nome de servidor (e recri�-lo)
		    name = name + '_REMOVED_' + Convert(varchar(100),GetDate(), 120),
		    removed = 1
		WHERE
			id = @smtpServerId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeSmtpServer TO FrameworkUser
GO
