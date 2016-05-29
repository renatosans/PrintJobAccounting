using System;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.ReportMailing
{
    public enum ReportTypeEnum
    {
        // Atenção: Não modificar o nome dos items abaixo, cada item da enumeração
        // corresponde a uma classe de relatório.
        [AssociatedText("Impressões por usuário")]
        UserPrintingCostsReport,
        [AssociatedText("Impressões por Grupo (CC)")]
        GroupPrintingCostsReport,
        [AssociatedText("Impressões por equipamento")]
        DevicePrintingCostsReport,
        [AssociatedText("Impressões Simplex/Duplex")]
        DuplexPrintingCostsReport,
        [AssociatedText("Impressões no período")]
        PrintedDocumentsReport,
        [AssociatedText("Cópias por usuário")]
        UserCopyingCostsReport,
        [AssociatedText("Cópias por equipamento")]
        DeviceCopyingCostsReport,
        [AssociatedText("Cópias no período")]
        CopiedDocumentsReport
    }

}
