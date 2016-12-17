
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveDuplexPrintingCosts' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveDuplexPrintingCosts
END
GO

/************************************************************************

     Procedure: pr_retrieveDuplexPrintingCosts
     Descri��o: Busca custos de impress�o simplex/duplex dos usu�rios no per�odo
     
     Autor: Renato R. Sanseverino
     Data: 04/08/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveDuplexPrintingCosts(
    @tenantId    int,
    @startDate   datetime,
    @endDate     datetime
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF @startDate IS NULL OR @endDate IS NULL
	BEGIN
		RAISERROR('� necess�rio fornecer uma faixa de datas', 16, 1)
		RETURN -1
	END
	
	
	DECLARE @tb_userLog TABLE(userId int, duplexPageCount int, totalPageCount int, duplexCost money, totalCost money)
	
	INSERT INTO
	    @tb_userLog
	SELECT
	    PRN_LOG.userId,
	    SUM(PRN_LOG.pageCount * PRN_LOG.copyCount * PRN_LOG.duplex), -- ignora p�ginas simplex (multiplica��o por zero)
	    SUM(PRN_LOG.pageCount * PRN_LOG.copyCount),
	    SUM(PRN_LOG.jobCost * PRN_LOG.duplex), -- ignora p�ginas simplex (multiplica��o por zero)
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
	    (USR_LOG.totalPageCount - USR_LOG.duplexPageCount) simplexPageCount,
	    USR_LOG.duplexPageCount,
	    USR_LOG.totalPageCount,
	    (USR_LOG.totalCost - USR_LOG.duplexCost) simplexCost,
	    USR_LOG.duplexCost,
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

GRANT EXEC ON pr_retrieveDuplexPrintingCosts TO FrameworkUser
GO
