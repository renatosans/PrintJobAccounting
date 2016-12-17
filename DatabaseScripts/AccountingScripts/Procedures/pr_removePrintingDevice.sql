
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_removePrintingDevice' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_removePrintingDevice
END
GO

/************************************************************************

     Procedure: pr_removePrintingDevice
     Descrição: Remove um dispositivo SNMP da tabela

     Autor: Renato R. Sanseverino
     Data: 06/08/2013

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_removePrintingDevice(
    @id  int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
		DELETE FROM
		    Accounting..tb_printingDevice
		WHERE
			id = @id
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_removePrintingDevice TO FrameworkUser
GO
