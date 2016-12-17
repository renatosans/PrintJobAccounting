
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveAccountingParams' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveAccountingParams
END
GO

/************************************************************************

     Procedure: pr_retrieveAccountingParams
     Descrição: Busca parametros de configuração de Accounting no banco
     
     Autor: Renato R. Sanseverino
     Data: 14/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveAccountingParams
AS
BEGIN

	SET NOCOUNT OFF
	

	SELECT
	    APP_PARAM.id,
	    APP_PARAM.name,
	    APP_PARAM.value,
	    APP_PARAM.ownerTask
	FROM
	    AppCommon..tb_applicationParam APP_PARAM
	    INNER JOIN AppCommon..tb_application APP WITH (NOLOCK)
	        ON APP_PARAM.applicationId = APP.id
	WHERE
	    APP.name = 'Print Accounting'
	
	
	SET NOCOUNT ON
	
END
GO

GRANT EXEC ON pr_retrieveAccountingParams TO FrameworkUser
GO
