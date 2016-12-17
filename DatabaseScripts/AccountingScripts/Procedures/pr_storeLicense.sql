
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeLicense' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeLicense
END
GO

/************************************************************************

     Procedure: pr_storeLicense
     Descrição: Armazena/atualiza uma licença de uso do sistema no banco
     
     Autor: Renato R. Sanseverino
     Data: 25/10/2010
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeLicense(
    @licenseId         int,
    @tenantId          int,
    @installationKey   varchar(255) = NULL,
    @installationDate  datetime = NULL,
    @computerName      varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_license WHERE id = @licenseId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    AppCommon..tb_license
		SET
			installationKey = @installationKey,
            installationDate = @installationDate,
			computerName = @computerName
		WHERE
			id = @licenseId
			AND
			tenantId = @tenantId
			AND
			removed = 0
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
		    AppCommon..tb_license
		VALUES
		    (@tenantId, null, null, null, 0)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeLicense TO FrameworkUser
GO
