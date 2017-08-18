
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveUserCopyingCosts' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveUserCopyingCosts
END
GO

/************************************************************************

     Procedure: pr_retrieveUserCopyingCosts
     Descrição: Busca custos de cópia dos usuários por período
     
     Autor: Renato R. Sanseverino
     Data: 09/04/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveUserCopyingCosts(
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
	
	
	DECLARE @tb_userLog TABLE(userId int, pageAmount int, cost money)
	
	INSERT INTO
	    @tb_userLog
	SELECT
	    CPY_LOG.userId,
	    SUM(CPY_LOG.pageCount),
	    SUM(CPY_LOG.jobCost)
	FROM
	    tb_copyLog CPY_LOG
	WHERE
	    CPY_LOG.tenantId = @tenantId
	    AND
	    CPY_LOG.jobTime BETWEEN @startDate AND @endDate
	GROUP BY
	    CPY_LOG.userId
	
	
	DECLARE @pageSum FLOAT -- Uso das casas decimais para o calculo do percentual
	DECLARE @costSum FLOAT
	SELECT
	    @pageSum = SUM(pageAmount),
	    @costSum = SUM(cost)
	FROM
	    @tb_userLog
	
	-- Evita divisões por zero, caso divida por 1 traz o mesmo valor
	IF @pageSum = 0 SET @pageSum = 1
	IF @costSum = 0 SET @costSum = 1
	
	
	SELECT
	    USR.id userId,
	    USR.alias userName,
	    USR_LOG.pageAmount,
	    (USR_LOG.pageAmount/@pageSum) pagePercentage,
	    USR_LOG.cost,
	    (USR_LOG.cost/@costSum) costPercentage
	FROM
	    @tb_userLog USR_LOG
	    INNER JOIN tb_user USR WITH (NOLOCK)
	        ON USR_LOG.userId = USR.id
	ORDER BY
	    userName
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_retrieveUserCopyingCosts TO FrameworkUser
GO
