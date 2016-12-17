
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeMailing' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeMailing
END
GO

/************************************************************************

     Procedure: pr_removeMailing
     Descrição: Marca um mailing como excluído na tabela
     
     Autor: Renato R. Sanseverino
     Data: 20/08/2009

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeMailing(
    @mailingId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    AppCommon..tb_mailing
		SET
		    -- A tabela tb_mailing não possui unique constraint, não é adicionado texto com a hora de remoção
		    -- recipients = recipients + '_REMOVED_' + Convert(varchar(100),GetDate(), 120),
		    removed = 1
		WHERE
			id = @mailingId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeMailing TO FrameworkUser
GO
