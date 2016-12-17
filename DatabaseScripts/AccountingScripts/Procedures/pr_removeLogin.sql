
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeLogin' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeLogin
END
GO

/************************************************************************

     Procedure: pr_removeLogin
     Descrição: Marca um login de acesso como excluído na tabela
     
     Autor: Renato R. Sanseverino
     Data: 26/08/2009

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeLogin(
    @loginId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    AppCommon..tb_login
		SET
		    -- É adicionado texto com a hora de remoção para evitar conflitos de nomes repetidos, permitindo
		    -- assim excluir várias vezes o mesmo username (e recriá-lo)
		    username = username + '_REMOVED_' + Convert(varchar(100),GetDate(), 120),
		    removed = 1
		WHERE
			id = @loginId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeLogin TO FrameworkUser
GO
