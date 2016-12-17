
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storePrintedDocument' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storePrintedDocument
END
GO

/************************************************************************

     Procedure: pr_storePrintedDocument
     Descrição: Insere a impressao no log de impressões do sistema
     
     Autor: Renato R. Sanseverino
     Data: 14/07/2009
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storePrintedDocument(
    @tenantId      int,
    @jobTime       datetime, -- não pode ser através de getdate() pois o processamento é em lote
    @userName      varchar(100),
    @printerName   varchar(100),
    @name          varchar(100),
    @pageCount     int,
    @copyCount     int,
    @duplex        bit,
    @color         bit
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	-- Executa procedimento para garantir a existência do usuário no banco
	IF NOT EXISTS(SELECT 1 FROM tb_user WHERE name = @userName AND tenantId = @tenantId)
	BEGIN
		INSERT INTO tb_user(tenantId, name, alias) VALUES (@tenantId, @userName, @userName)
	END
	-- Recupera os dados do usuário
	DECLARE @userId INT
	
	SELECT @userId = id
	FROM tb_user
	WHERE name = @userName AND tenantId = @tenantId
	
	
	-- Executa procedimento para garantir a existência da impresora no banco
	IF NOT EXISTS(SELECT 1 FROM tb_printer WHERE name = @printerName AND tenantId = @tenantId)
	BEGIN
		INSERT INTO tb_printer(tenantId, name, alias) VALUES (@tenantId, @printerName, @printerName)
	END
	-- Recupera os dados da impressora
	DECLARE @printerId     INT
	DECLARE @pageCost      MONEY
	DECLARE @colorCostDiff MONEY
	DECLARE @bwPrinter     BIT
	
	SELECT @printerId = id, @pageCost = pageCost, @colorCostDiff = colorCostDiff, @bwPrinter = bwPrinter
	FROM tb_printer
	WHERE name = @printerName AND tenantId = @tenantId
	
	
	IF (@bwPrinter = 1) -- caso a impressora esteja definida como Monocromática define a impressão como Pb
	BEGIN
	    SET @color = 0
	END
	
	DECLARE @jobCost MONEY
	SET @jobCost = (@pageCost + (@colorCostDiff * @color) ) * @pageCount * @copyCount
	
	
	INSERT INTO
		tb_printLog(tenantId, jobTime, userId, printerId, documentName, pageCount, copyCount, duplex, color, jobCost)
	VALUES
		(@tenantId, @jobTime, @userId, @printerId, @name, @pageCount, @copyCount, @duplex, @color, @jobCost)
	
	
	SET NOCOUNT ON
		
END
GO

GRANT EXEC ON pr_storePrintedDocument TO FrameworkUser
GO
