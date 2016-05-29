using System;


namespace DocMageFramework.Reporting
{
    public class PageControl
    {
        private const int recordsPerPage = 30;

        private int recordCount;


        public PageControl(int recordCount)
        {
            this.recordCount = recordCount;
        }

        public int GetPageCount()
        {
            // Todos os registros cabem na primeira página ( ou relatório sem nenhum registro )
            if (recordCount < recordsPerPage)
                return 1;

            int pageCount = (int)recordCount / recordsPerPage;

            // Caso a quantidade de páginas seja um número quebrado, adiciona 1. Acontece quando
            // a última página possui quantidade de registros diferente de "recordsPerPage"
            if ((recordCount % recordsPerPage) != 0)
                pageCount++;

            return pageCount;
        }

        public int GetStartRecord(int currentPage)
        {
            // Todos os registros cabem na primeira página ( ou relatório sem nenhum registro )
            if (recordCount < recordsPerPage)
                return 0;

            int startRecord = (currentPage-1) * recordsPerPage;
            return startRecord;
        }

        public int GetEndRecord(int currentPage)
        {
            // Todos os registros cabem na primeira página ( ou relatório sem nenhum registro )
            if (recordCount < recordsPerPage)
                return (recordCount - 1);

            // Caso a página seja a última o cálculo é diferente, o método retorna o último registro
            int lastPage = GetPageCount();
            if (currentPage == lastPage)
                return (recordCount - 1);

            int startRecord = (currentPage - 1) * recordsPerPage;
            int endRecord = startRecord + (recordsPerPage-1);
            return endRecord;
        }

        /// <summary>
        /// Muda a página baseando-se na ação desejada (MoveFirst, MovePrevious, MoveNext, MoveLast)
        /// </summary>
        public int Perform(String action, int currentPage)
        {
            int gotoPage;
            int lastPage = GetPageCount();

            switch (action)
            {
                case "MoveFirst":
                    gotoPage = 1;
                    break;
                case "MovePrevious":
                    gotoPage = currentPage <= 1 ? 1 : --currentPage;
                    break;
                case "MoveNext":
                    gotoPage = currentPage >= lastPage ? lastPage : ++currentPage;
                    break;
                case "MoveLast":
                    gotoPage = lastPage;
                    break;
                default:
                    gotoPage = currentPage;
                    break;
            }

            return gotoPage;
        }
    }

}
