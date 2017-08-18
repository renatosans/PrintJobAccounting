
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeTenantPreference' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeTenantPreference
END
GO

/************************************************************************

     Procedure: pr_storeTenantPreference
     Descrição: Armazena uma preferência do tenant no banco

     Autor: Renato R. Sanseverino
     Data: 25/05/2010

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeTenantPreference(
    @tenantId         int,
    @preferenceId     int,
    @name             varchar(100),
    @value            varchar(255),
    @type             varchar(80)
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM AppCommon..tb_tenantPreference WHERE id = @preferenceId)
	BEGIN
		-- Atualiza dados existentes no banco
	    UPDATE
	        AppCommon..tb_tenantPreference
	    SET
	        name = @name,
	        value = @value,
	        type = @type
	    WHERE
	        id = @preferenceId
	END
	ELSE
	BEGIN
	    -- Insere um novo registro no banco
	    INSERT INTO
	        AppCommon..tb_tenantPreference
	    VALUES
	        (@tenantId, @name, @value, @type)
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeTenantPreference TO FrameworkUser
GO
