
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveDeviceCostDetails' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveDeviceCostDetails
END
GO

/************************************************************************

     Procedure: pr_retrieveDeviceCostDetails
     Descrição: Busca custos de impressão/cópia de um esquipamento por período
     
     Autor: Renato R. Sanseverino
     Data: 08/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveDeviceCostDetails(
    @tenantId     int,
    @printerId    int,
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
	    -- Busca os detalhes de impressão do equipamento
	    SELECT
	        PRN_LOG.jobTime,
	        PRN_LOG.documentName,
	        USR.alias userName,
	        PRN_LOG.pageCount * PRN_LOG.copyCount pageAmount,
	        PRN_LOG.jobCost cost
	    FROM
	        tb_printLog PRN_LOG
	        INNER JOIN tb_user USR WITH (NOLOCK)
	            ON PRN_LOG.userId = USR.id
	    WHERE
	        PRN_LOG.tenantId = @tenantId
	        AND
	        PRN_LOG.printerId = @printerId
	        AND
	        PRN_LOG.jobTime BETWEEN @startDate AND @endDate
	END
	
	IF (@detailType = 'CopyingCosts')
	BEGIN
	    -- Busca os detalhes de cópia do equipamento
	    SELECT
	        CPY_LOG.jobTime,
	        '' documentName,
	        USR.alias userName,
	        CPY_LOG.pageCount pageAmount,
	        CPY_LOG.jobCost cost
	    FROM
	        tb_copyLog CPY_LOG
	        INNER JOIN tb_user USR WITH (NOLOCK)
	            ON CPY_LOG.userId = USR.id
	    WHERE
	        CPY_LOG.tenantId = @tenantId
	        AND
	        CPY_LOG.printerId = @printerId
	        AND
	        CPY_LOG.jobTime BETWEEN @startDate AND @endDate
    END	
	
	
	SET NOCOUNT ON
	
END
GO

GRANT EXEC ON pr_retrieveDeviceCostDetails TO FrameworkUser
GO
