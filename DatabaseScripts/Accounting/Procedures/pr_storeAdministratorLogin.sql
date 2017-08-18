
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeAdministratorLogin' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeAdministratorLogin
END
GO

/************************************************************************

     Procedure: pr_storeAdministratorLogin
     Descrição: Armazena/atualiza os dados de login de um admnistrador no banco
     
     Autor: Renato R. Sanseverino
     Data: 02/12/2010

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeAdministratorLogin(
    @loginId      int,
    @username     varchar(100),
    @password     varchar(100)
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_administratorLogin WHERE id = @loginId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_administratorLogin
		SET
			username = @username,
		    password = @password
		WHERE
			id = @loginId
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_administratorLogin
		VALUES
		    (@username, @password)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeAdministratorLogin TO FrameworkUser
GO
