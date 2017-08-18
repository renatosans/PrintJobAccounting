
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storeCopiedDocument' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storeCopiedDocument
END
GO

/************************************************************************

     Procedure: pr_storeCopiedDocument
     Descri��o: Insere a c�pia no log de c�pias do sistema
     
     Autor: Renato R. Sanseverino
     Data: 16/03/2010
     
************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storeCopiedDocument(
    @tenantId      int,
    @jobTime       datetime, -- n�o pode ser atrav�s de getdate() pois o processamento � em lote
    @userName      varchar(100),
    @printerName   varchar(100),
    @pageCount     int,
    @duplex        bit,
    @color         bit
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	-- Executa procedimento para garantir a exist�ncia do usu�rio no banco
	IF NOT EXISTS(SELECT 1 FROM tb_user WHERE name = @userName AND tenantId = @tenantId)
	BEGIN
		INSERT INTO tb_user(tenantId, name, alias) VALUES (@tenantId, @userName, @userName)
	END
	-- Recupera os dados do usu�rio
	DECLARE @userId INT
	
	SELECT @userId = id
	FROM tb_user
	WHERE name = @userName AND tenantId = @tenantId
	
	
	-- Executa procedimento para garantir a exist�ncia da impresora no banco
	IF NOT EXISTS(SELECT 1 FROM tb_printer WHERE name = @printerName AND tenantId = @tenantId)
	BEGIN
		INSERT INTO tb_printer(tenantId, name, alias) VALUES (@tenantId, @printerName, @printerName)
	END
	-- Recupera os dados da impressora
	DECLARE @printerId      INT
	DECLARE @pageCost       MONEY
	DECLARE @colorCostDiff  MONEY
	DECLARE @bwPrinter      BIT
	
	SELECT @printerId = id, @pageCost = pageCost, @colorCostDiff = colorCostDiff, @bwPrinter = bwPrinter
	FROM tb_printer
	WHERE name = @printerName AND tenantId = @tenantId
	
		
	IF (@bwPrinter = 1) -- caso a copiadora esteja definida como Monocrom�tica define a c�pia como Pb
	BEGIN
	    SET @color = 0
	END
	
	DECLARE @jobCost MONEY
	SET @jobCost = @pageCost * @pageCount
	
	
	INSERT INTO
		tb_copyLog(tenantId, jobTime, userId, printerId, pageCount, duplex, color, jobCost)
	VALUES
		(@tenantId, @jobTime, @userId, @printerId, @pageCount, @duplex, @color, @jobCost)
	
	
	SET NOCOUNT ON
		
END
GO

GRANT EXEC ON pr_storeCopiedDocument TO FrameworkUser
GO
