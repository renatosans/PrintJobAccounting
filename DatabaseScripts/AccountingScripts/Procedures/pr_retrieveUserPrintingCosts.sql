
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveUserPrintingCosts' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveUserPrintingCosts
END
GO

/************************************************************************

     Procedure: pr_retrieveUserPrintingCosts
     Descrição: Busca custos de impressão dos usuários por período
     
     Autor: Renato R. Sanseverino
     Data: 07/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveUserPrintingCosts(
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
	
	
	DECLARE @tb_userLog TABLE(userId int, colorPageCount int, totalPageCount int, colorCost money, totalCost money)
	
	INSERT INTO
	    @tb_userLog
	SELECT
	    PRN_LOG.userId,
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
	    PRN_LOG.userId
	
	
	SELECT
	    USR.id userId,
	    USR.alias userName,
	    (USR_LOG.totalPageCount - USR_LOG.colorPageCount) bwPageCount,
	    USR_LOG.colorPageCount,
	    USR_LOG.totalPageCount,
	    (USR_LOG.totalCost - USR_LOG.colorCost) bwCost,
	    USR_LOG.colorCost,
	    USR_LOG.totalCost
	FROM
	    @tb_userLog USR_LOG
	    INNER JOIN tb_user USR WITH (NOLOCK)
	        ON USR_LOG.userId = USR.id
	ORDER BY
	    userName
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveUserPrintingCosts TO FrameworkUser
GO
