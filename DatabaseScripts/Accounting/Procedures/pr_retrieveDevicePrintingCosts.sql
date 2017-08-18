
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveDevicePrintingCosts' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveDevicePrintingCosts
END
GO

/************************************************************************

     Procedure: pr_retrieveDevicePrintingCosts
     Descrição: Busca custos de impressão dos esquipamentos por período
     
     Autor: Renato R. Sanseverino
     Data: 08/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveDevicePrintingCosts(
    @tenantId    int,
    @startDate   datetime,
    @endDate     datetime
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF @startDate IS NULL OR @endDate IS NULL
	BEGIN
		RAISERROR('É necessário fornecer uma faixa de datas', 16, 1)
		RETURN -1
	END
	
	
	DECLARE @tb_deviceLog TABLE(printerId int, colorPageCount int, totalPageCount int, colorCost money, totalCost money)
	
	INSERT INTO
	    @tb_deviceLog
	SELECT
	    PRN_LOG.printerId,
	    SUM(PRN_LOG.pageCount * PRN_LOG.copyCount * PRN_LOG.color), -- ignora páginas pb (multiplicação por zero)
	    SUM(PRN_LOG.pageCount * PRN_LOG.copyCount),
	    SUM(PRN_LOG.jobCost * PRN_LOG.color), -- ignora páginas pb (multiplicação por zero)
	    SUM(PRN_LOG.jobCost)
	FROM
	    tb_printLog PRN_LOG
	WHERE
	    PRN_LOG.tenantId = @tenantId
	    AND
	    PRN_LOG.jobTime BETWEEN @startDate AND @endDate
	GROUP BY
	    PRN_LOG.printerId
	
	
	SELECT
	    PRN.id printerId,
	    PRN.alias printerName,
	    (DVC_LOG.totalPageCount - DVC_LOG.colorPageCount) bwPageCount,
	    DVC_LOG.colorPageCount,
	    DVC_LOG.totalPageCount,
	    (DVC_LOG.totalCost - DVC_LOG.colorCost) bwCost,
	    DVC_LOG.colorCost,
	    DVC_LOG.totalCost
	FROM
	    @tb_deviceLog DVC_LOG
	    INNER JOIN tb_printer PRN WITH (NOLOCK)
	        ON DVC_LOG.printerId = PRN.id
	ORDER BY
	    printerName
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveDevicePrintingCosts TO FrameworkUser
GO
