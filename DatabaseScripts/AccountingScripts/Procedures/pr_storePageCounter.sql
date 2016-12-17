
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storePageCounter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storePageCounter
END
GO

/************************************************************************

     Procedure: pr_storePageCounter
     Descrição: Armazena o contador para um dispositivo cadastrado

     Autor: Renato R. Sanseverino
     Data: 13/08/2013

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storePageCounter(
    @deviceId        int,
    @counter         int
)
AS
BEGIN

	SET NOCOUNT OFF
	
		INSERT INTO
			tb_pageCounter
		VALUES
			(@deviceId, @counter, GETDATE())
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storePageCounter TO FrameworkUser
GO
