using System;
using System.Collections.Generic;


namespace DocMageFramework.AppUtils
{
    public interface IReportBuilder
    {
        /// <summary>
        /// Abre a mídia para geração do relatório. A mídia pode ser
        /// qualquer objeto que possa renderizar o relatório
        /// </summary>
        void OpenMedia(Object media);

        /// <summary>
        /// Fecha a mídia ao final da geração do relatório
        /// </summary>
        void CloseMedia();

        /// <summary>
        /// Indica se o ReportBuilder permite a paginação e navegação do relatório
        /// </summary>
        Boolean IsNavigable();

        /// <summary>
        /// Define as informações no cabeçalho do relatório (título, empresa, período)
        /// </summary>
        void SetReportHeadings(String reportTitle, String tenantAlias, Dictionary<String, Object> reportFilter);

        /// <summary>
        /// Define os dados de navegação ( utilizados pelos links imprimir/exportar e o pageControl)
        /// </summary>
        void SetNavigationData(String reportClass, int recordCount, Dictionary<String, Object> exportOptions);

        /// <summary>
        /// Muda para a página de relatório escolhida, action indica se é a próxima página, a anterior,
        /// a última ou a primeira
        /// </summary>
        void SetReportPage(String action, int currentPage);

        /// <summary>
        /// Cria a tabela de dados do relatório, rowCount nem sempre é o mesmo que recordCount
        /// </summary>
        void CreateDataTable(String[] columnNames, int[] columnWidths, int rowCount);

        /// <summary>
        /// Insere uma linha na tabela de dados do relatório, não exceder a quantidade
        /// de linhas definida na criação da tabela
        /// </summary>
        void InsertRow(int rowIndex, Object[] cells);

        /// <summary>
        /// Insere o rodapé na tabela de dados do relatório, não exceder a quantidade
        /// de linhas definida na criação da tabela
        /// </summary>
        void InsertFooter(Object[] cells);
    }

}
