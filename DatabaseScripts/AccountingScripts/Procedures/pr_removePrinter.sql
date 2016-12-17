
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removePrinter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removePrinter
END
GO

/************************************************************************

     Procedure: pr_removePrinter
     Descrição: Marca uma impressora como excluída na tabela

     Autor: Renato R. Sanseverino
     Data: 25/07/2012

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removePrinter(
    @printerId  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		UPDATE
		    Accounting..tb_printer
		SET
		    removed = 1
		WHERE
			id = @printerId
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removePrinter TO FrameworkUser
GO
