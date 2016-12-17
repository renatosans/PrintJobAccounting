
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrievePrintingDevice' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrievePrintingDevice
END
GO

/************************************************************************

     Procedure: pr_retrievePrintingDevice
     Descrição: Recupera os dispositivos SNMP cadastrados para o tenant

     Autor: Renato R. Sanseverino
     Data: 06/08/2013

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrievePrintingDevice(
    @tenantId      int,
    @serialNumber  varchar(100) = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	SELECT
	    id,
	    tenantId,
	    ipAddress,
	    description,
	    serialNumber,
	    counter,
	    lastUpdated
	FROM
	    tb_printingDevice
	WHERE
	    tenantId = @tenantId
	    AND	
	    serialNumber = ISNULL(@serialNumber, serialNumber)
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrievePrintingDevice TO FrameworkUser
GO
