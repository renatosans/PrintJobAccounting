
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrievePageCounter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrievePageCounter
END
GO

/************************************************************************

     Procedure: pr_retrievePageCounter
     Descrição: Busca o histórico de contadores para um dispositivo

     Autor: Renato R. Sanseverino
     Data: 13/08/2013

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrievePageCounter(
    @deviceId     int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    deviceId,
	    counter,
	    date
	FROM
	    tb_pageCounter
	WHERE
	    deviceId = @deviceId
	ORDER BY
	    date DESC
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrievePageCounter TO FrameworkUser
GO
