
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrieveCopiedDocuments' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrieveCopiedDocuments
END
GO

/************************************************************************

     Procedure: pr_retrieveCopiedDocuments
     Descrição: Busca cópias por faixa de data, usuario e impressora
     
     Autor: Renato R. Sanseverino
     Data: 16/03/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrieveCopiedDocuments(
    @tenantId    int,
    @startDate   datetime,
    @endDate     datetime,
    @userId      int = NULL,
    @printerId   int = NULL
)
AS
BEGIN

	SET NOCOUNT OFF
	

	IF @startDate IS NULL OR @endDate IS NULL
	BEGIN
		RAISERROR('É necessário fornecer uma faixa de datas', 16, 1)
		RETURN -1
	END
	
	SELECT
	    COPY_LOG.id jobId,
	    COPY_LOG.tenantId,
	    COPY_LOG.jobTime,
	    USR.alias userName,
	    PRN.alias printerName,
	    COPY_LOG.pageCount,
	    COPY_LOG.duplex,
	    COPY_LOG.color
	FROM
	    tb_copyLog COPY_LOG
	    INNER JOIN tb_printer PRN WITH (NOLOCK)
	        ON COPY_LOG.printerId = PRN.id
	    INNER JOIN tb_user USR WITH (NOLOCK)
	        ON COPY_LOG.userId = USR.id
	WHERE
	    COPY_LOG.tenantId = @tenantId
	    AND
	    COPY_LOG.jobTime BETWEEN @startDate AND @endDate
	    AND
	    COPY_LOG.userId = ISNULL(@userId, COPY_LOG.userId)
	    AND
	    COPY_LOG.printerId = ISNULL(@printerId, COPY_LOG.printerId)
	
	
	SET NOCOUNT ON
		
END
GO

GRANT EXEC ON pr_retrieveCopiedDocuments TO FrameworkUser
GO
