
USE AppCommon
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveParams' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveParams
END
GO

/************************************************************************

     Procedure: pr_retrieveParams
     Descrição: Busca parametros de configuração de um aplicativo no banco
     
     Autor: Renato R. Sanseverino
     Data: 15/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveParams(
    @applicationName  varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	

	SELECT
	    APP_PARAM.id,
	    APP_PARAM.name,
	    APP_PARAM.value,
	    APP_PARAM.ownerTask
	FROM
	    tb_applicationParam APP_PARAM
	    INNER JOIN tb_application APP WITH (NOLOCK)
	        ON APP_PARAM.applicationId = APP.id
	WHERE
	    APP.name = ISNULL(@applicationName, APP.name)
	
	
	SET NOCOUNT ON
	
END
GO

GRANT EXEC ON pr_retrieveParams TO FrameworkUser
GO
