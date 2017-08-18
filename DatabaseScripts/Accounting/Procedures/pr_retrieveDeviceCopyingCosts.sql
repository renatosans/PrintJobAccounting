
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveDeviceCopyingCosts' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveDeviceCopyingCosts
END
GO

/************************************************************************

     Procedure: pr_retrieveDeviceCopyingCosts
     Descrição: Busca custos de cópia dos esquipamentos por período
     
     Autor: Renato R. Sanseverino
     Data: 12/04/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveDeviceCopyingCosts(
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
	
	
	DECLARE @tb_deviceLog TABLE(printerId int, pageAmount int, cost money)
	
	INSERT INTO
	    @tb_deviceLog
	SELECT
	    CPY_LOG.printerId,
	    SUM(CPY_LOG.pageCount),
	    SUM(CPY_LOG.jobCost)
	FROM
	    tb_copyLog CPY_LOG
	WHERE
	    CPY_LOG.tenantId = @tenantId
	    AND
	    CPY_LOG.jobTime BETWEEN @startDate AND @endDate
	GROUP BY
	    CPY_LOG.printerId
	
	
	DECLARE @pageSum FLOAT -- Uso das casas decimais para o calculo do percentual
	DECLARE @costSum FLOAT
	SELECT
	    @pageSum = SUM(pageAmount),
	    @costSum = SUM(cost)
	FROM
	    @tb_deviceLog
	
	-- Evita divisões por zero, caso divida por 1 traz o mesmo valor
	IF @pageSum = 0 SET @pageSum = 1
	IF @costSum = 0 SET @costSum = 1
	
	
	SELECT
	    PRN.id printerId,
	    PRN.alias printerName,
	    DVC_LOG.pageAmount,
	    (DVC_LOG.pageAmount/@pageSum) pagePercentage,
	    DVC_LOG.cost,
	    (DVC_LOG.cost/@costSum) costPercentage
	FROM
	    @tb_deviceLog DVC_LOG
	    INNER JOIN tb_printer PRN WITH (NOLOCK)
	        ON DVC_LOG.printerId = PRN.id
	ORDER BY
	    printerName
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveDeviceCopyingCosts TO FrameworkUser
GO
