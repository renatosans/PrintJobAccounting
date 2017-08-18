
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeAssociate' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeAssociate
END
GO

/************************************************************************

     Procedure: pr_storeAssociate
     Descrição: Armazena os dados de um associado no banco ( usuário 
     associado a um centro de custo )

     Autor: Renato R. Sanseverino
     Data: 25/05/2010

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeAssociate(
    @tenantId         int,
    @costCenterId     int,
    @userId           int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	-- Insere um novo registro no banco
	INSERT INTO
	    tb_costCenterAssociate
	VALUES
	    (@tenantId, @costCenterId, @userId)
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storeAssociate TO FrameworkUser
GO
