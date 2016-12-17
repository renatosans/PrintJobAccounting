
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveUserCostDetails' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveUserCostDetails
END
GO

/************************************************************************

     Procedure: pr_retrieveUserCostDetails
     Descrição: Busca custos de impressão/cópia de um usuário por período
     
     Autor: Renato R. Sanseverino
     Data: 07/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveUserCostDetails(
    @tenantId     int,
    @userId       int,
    @startDate    datetime,
    @endDate      datetime,
    @detailType   varchar(50)
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF @startDate IS NULL OR @endDate IS NULL
	BEGIN
		RAISERROR('É necessário fornecer uma faixa de datas', 16, 1)
		RETURN -1
	END
	
	
	IF (@detailType = 'PrintingCosts')
	BEGIN
	    -- Busca os detalhes de impressão do usuário
	    SELECT
	        PRN_LOG.jobTime,
	        PRN_LOG.documentName,
	        PRN.alias printerName,
	        PRN_LOG.pageCount * PRN_LOG.copyCount  pageAmount,
	        PRN_LOG.jobCost cost
	    FROM
	        tb_printLog PRN_LOG
	        INNER JOIN tb_printer PRN WITH (NOLOCK)
	            ON PRN_LOG.printerId = PRN.id
	    WHERE
	        PRN_LOG.tenantId = @tenantId
	        AND
	        PRN_LOG.userId = @userId
	        AND
	        PRN_LOG.jobTime BETWEEN @startDate AND @endDate
	END
	
	IF (@detailType = 'CopyingCosts')
	BEGIN
	    -- Busca os detalhes de cópia do usuário
	    SELECT
	        CPY_LOG.jobTime,
	        '' documentName,
	        PRN.alias printerName,
	        CPY_LOG.pageCount pageAmount,
	        CPY_LOG.jobCost cost
	    FROM
	        tb_copyLog CPY_LOG
	        INNER JOIN tb_printer PRN WITH (NOLOCK)
	            ON CPY_LOG.printerId = PRN.id
	    WHERE
	        CPY_LOG.tenantId = @tenantId
	        AND
	        CPY_LOG.userId = @userId
	        AND
	        CPY_LOG.jobTime BETWEEN @startDate AND @endDate
	END
	
	
	SET NOCOUNT ON
	
END
GO

GRANT EXEC ON pr_retrieveUserCostDetails TO FrameworkUser
GO
