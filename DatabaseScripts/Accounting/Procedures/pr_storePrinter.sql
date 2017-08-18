
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storePrinter' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storePrinter
END
GO

/************************************************************************

     Procedure: pr_storePrinter
     Descrição: Armazena/atualiza os dados de uma impressora no banco
     
     Autor: Renato R. Sanseverino
     Data: 27/04/2010
     
     Observação: Não é possivel mudar o tenantId ao atualizar pois isso
     destruiria a integridade dos dados

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storePrinter(
    @printerId       int,
    @tenantId        int,
    @name            varchar(100),
    @alias           varchar(100),
    @pageCost        money,
    @colorCostDiff   money,
    @duplexCostDiff  money,
    @bwPrinter       bit
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM tb_printer WHERE id = @printerId)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
		    tb_printer
		SET
		    alias = @alias,
		    pageCost = @pageCost,
		    colorCostDiff = @colorCostDiff,
		    duplexCostDiff = @duplexCostDiff,
		    bwPrinter = @bwPrinter
		WHERE
		    id = @printerId
			AND
			tenantId = @tenantId
			AND
			removed = 0
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco ou recupera um registro removido
		IF EXISTS(SELECT 1 FROM tb_printer WHERE name = @name AND tenantId = @tenantId AND removed = 1)
		BEGIN
			UPDATE tb_printer SET removed = 0 WHERE name = @name AND tenantId = @tenantId AND removed = 1
		END
		ELSE
		BEGIN
			INSERT INTO tb_printer VALUES (@tenantId, @name, @alias, @pageCost, @colorCostDiff, @duplexCostDiff, @bwPrinter, 0)
		END
	END
	
	
	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storePrinter TO FrameworkUser
GO
