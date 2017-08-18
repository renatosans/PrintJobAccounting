
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removeTenant' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removeTenant
END
GO

/************************************************************************

     Procedure: pr_removeTenant
     Descrição: Marca um inquilino(cliente) como excluído na tabela

     Autor: Renato R. Sanseverino
     Data: 23/10/2012

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removeTenant(
    @tenantId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    AppCommon..tb_tenant
		SET
		    removed = 1
		WHERE
			id = @tenantId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removeTenant TO FrameworkUser
GO
