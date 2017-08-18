
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_retrievePrintedDocuments' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_retrievePrintedDocuments
END
GO

/************************************************************************

     Procedure: pr_retrievePrintedDocuments
     Descrição: Busca impressões/documentos por faixa de data, usuario e impressora
     
     Autor: Renato R. Sanseverino
     Data: 13/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_retrievePrintedDocuments(
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
	    PRN_LOG.id jobId,
	    PRN_LOG.tenantId,
	    PRN_LOG.jobTime,
	    USR.alias userName,
	    PRN.alias printerName,
	    PRN_LOG.documentName name,
	    PRN_LOG.pageCount,
	    PRN_LOG.copyCount,
	    PRN_LOG.duplex,
	    PRN_LOG.color
	FROM
	    tb_printLog PRN_LOG
	    INNER JOIN tb_printer PRN WITH (NOLOCK)
	        ON PRN_LOG.printerId = PRN.id
	    INNER JOIN tb_user USR WITH (NOLOCK)
	        ON PRN_LOG.userId = USR.id
	WHERE
	    PRN_LOG.tenantId = @tenantId
	    AND
	    PRN_LOG.jobTime BETWEEN @startDate AND @endDate
	    AND
	    PRN_LOG.userId = ISNULL(@userId, PRN_LOG.userId)
	    AND
	    PRN_LOG.printerId = ISNULL(@printerId, PRN_LOG.printerId)
	
	
	SET NOCOUNT ON
		
END
GO

GRANT EXEC ON pr_retrievePrintedDocuments TO FrameworkUser
GO
