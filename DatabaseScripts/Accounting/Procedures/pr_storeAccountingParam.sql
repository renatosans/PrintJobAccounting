
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeAccountingParam' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeAccountingParam
END
GO

/************************************************************************

     Procedure: pr_storeAccountingParam
     Descrição: Armazena/atualiza o valor de um parâmetro de Accounting no banco
     
     Autor: Renato R. Sanseverino
     Data: 01/09/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeAccountingParam(
    @paramId     int,
    @name        varchar(100),
    @value       varchar(255),
    @ownerTask   varchar(100)
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	---------------------------------------------------------------------------------------
	-- Só permite alterações em parâmetros de Accounting
	---------------------------------------------------------------------------------------
	DECLARE @applicationId INT
	SELECT @applicationId = id FROM AppCommon..tb_application WHERE name = 'Print Accounting'
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_applicationParam WHERE id = @paramId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
			AppCommon..tb_applicationParam
		SET
			name = @name,	
			value = @value,
			ownerTask = @ownerTask
		WHERE
			id = @paramId
			AND
			applicationId = @applicationId
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_applicationParam
		VALUES
		    (@name, @value, @applicationId, @ownerTask)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeAccountingParam TO FrameworkUser
GO
