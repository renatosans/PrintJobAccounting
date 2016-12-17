
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveTenantPreference' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveTenantPreference
END
GO

/************************************************************************

     Procedure: pr_retrieveTenantPreference
     Descrição: Busca preferências do tenant no banco

     Autor: Renato R. Sanseverino
     Data: 03/08/2009

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveTenantPreference(
    @tenantId           int,
    @preferenceName     varchar(100)  =  NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    tenantId,
	    id,
	    name,
	    value,
	    type
	FROM
	    AppCommon..tb_tenantPreference
	WHERE
	    tenantId = @tenantId
	    AND
	    name = ISNULL(@preferenceName, name)
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveTenantPreference TO FrameworkUser
GO
