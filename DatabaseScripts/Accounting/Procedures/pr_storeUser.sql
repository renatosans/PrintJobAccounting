
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeUser' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeUser
END
GO

/************************************************************************

     Procedure: pr_storeUser
     Descrição: Armazena/atualiza os dados de um usuário no banco
     
     Autor: Renato R. Sanseverino
     Data: 11/06/2010
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeUser(
    @userId        int,
    @tenantId      int,
    @name          varchar(100),
    @alias         varchar(100),
    @quota         money = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM tb_user WHERE id = @userId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    tb_user
		SET
		    name   =  @name,
		    alias  =  @alias,
		    quota  =  @quota
		WHERE
			id = @userId
			AND
			tenantId = @tenantId
			AND
			removed = 0
		
		SELECT @userId userId -- Retorna o id do usuário atualizado ( o mesmo que foi recebido )
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco ou recupera um registro removido
		IF EXISTS(SELECT 1 FROM tb_user WHERE name = @name AND tenantId = @tenantId AND removed = 1)
		BEGIN
			UPDATE tb_user SET removed = 0 WHERE name = @name AND tenantId = @tenantId AND removed = 1
			SELECT id AS userId FROM tb_user WHERE name = @name AND tenantId = @tenantId AND removed = 0 -- Retorna o id do usuário recuperado
		END
		ELSE
		BEGIN
			INSERT INTO tb_user VALUES (@tenantId, @name, @alias, @quota, 0)
			SELECT SCOPE_IDENTITY() userId -- Retorna o id do usuário inserido no banco
		END
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeUser TO FrameworkUser
GO
