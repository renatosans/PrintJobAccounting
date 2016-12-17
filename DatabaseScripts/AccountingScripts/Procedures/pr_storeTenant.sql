
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeTenant' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeTenant
END
GO

/************************************************************************

     Procedure: pr_storeTenant
     Descrição: Armazena/atualiza dados de um tenant no sistema
     
     Autor: Renato R. Sanseverino
     Data: 07/08/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeTenant(
    @tenantId  int,
    @name      varchar(100),
    @alias     varchar(100)
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_tenant WHERE id = @tenantId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_tenant
		SET
		    name = @name,
		    alias = @alias
		WHERE
		    id = @tenantId
			AND
			removed = 0

		SELECT @tenantId tenantId -- Retorna o id da empresa atualizada ( o mesmo que foi recebido )
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_tenant
		VALUES
		    (@name, @alias, 0)

		SELECT SCOPE_IDENTITY() tenantId -- Retorna o id da empresa inserida no banco
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeTenant TO FrameworkUser
GO
