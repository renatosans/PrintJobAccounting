
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeLogin' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeLogin
END
GO

/************************************************************************

     Procedure: pr_storeLogin
     Descrição: Armazena/atualiza os dados de um login no banco
     
     Autor: Renato R. Sanseverino
     Data: 26/08/2009
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeLogin(
    @loginId      int,
    @tenantId     int,
    @username     varchar(100),
    @password     varchar(100),
    @userGroup    int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_login WHERE id = @loginId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_login
		SET
			username = @username,
		    password = @password,
		    userGroup = @userGroup
		WHERE
			id = @loginId
			AND
			tenantId = @tenantId
			AND
			removed = 0
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_login
		VALUES
		    (@tenantId, @username, @password, @userGroup, 0)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeLogin TO FrameworkUser
GO
