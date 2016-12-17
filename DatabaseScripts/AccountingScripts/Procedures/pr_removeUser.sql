
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeUser' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeUser
END
GO

/************************************************************************

     Procedure: pr_removeUser
     Descrição: Marca um usuário como excluído na tabela

     Autor: Renato R. Sanseverino
     Data: 12/08/2013

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeUser(
    @userId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    Accounting..tb_user
		SET
		    removed = 1
		WHERE
			id = @userId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeUser TO FrameworkUser
GO
