
USE Accounting
GO

IF EXISTS(SELECT * FROM SYSOBJECTS WHERE name = 'pr_storePrintingDevice' AND xtype = 'P')
BEGIN
    DROP PROCEDURE pr_storePrintingDevice
END
GO

/************************************************************************

     Procedure: pr_storePrintingDevice
     Descrição: Armazena/atualiza os dados de um dispositivo SNMP

     Autor: Renato R. Sanseverino
     Data: 06/08/2013

     Observação: Não é possivel mudar o tenantId nem o serialNumber ao atualizar pois isso
     destruiria a integridade dos dados

************************************************************************/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE pr_storePrintingDevice(
    @tenantId        int,
    @ipAddress       varchar(100),
    @description     varchar(100),
    @serialNumber    varchar(100),
    @counter         int
)
AS
BEGIN

	SET NOCOUNT OFF
	
	
	IF EXISTS(SELECT 1 FROM tb_printingDevice WHERE serialNumber = @serialNumber)
	BEGIN
		-- Atualiza dados existentes no banco
		UPDATE
			tb_printingDevice
		SET
			ipAddress = @ipAddress,
			description = @description,
			counter = @counter,
			lastUpdated = GETDATE()
		WHERE
		    serialNumber = @serialNumber
			AND
			tenantId = @tenantId
		
		SELECT id AS deviceId FROM tb_printingDevice WHERE serialNumber = @serialNumber -- Retorna o id do dispositivo atualizado
	END
	ELSE
	BEGIN
		-- Insere um novo registro no banco
		INSERT INTO
			tb_printingDevice
		VALUES
			(@tenantId, @ipAddress, @description, @serialNumber, @counter, GETDATE())
		
		SELECT SCOPE_IDENTITY() deviceId -- Retorna o id do dispositivo inserido no banco
	END


	SET NOCOUNT ON

END
GO

GRANT EXEC ON pr_storePrintingDevice TO FrameworkUser
GO
