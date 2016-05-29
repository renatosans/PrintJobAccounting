using System;


namespace SPLViewer
{
    public class Page
    {
        /// <summary>
        /// Indica o número da página dentro do documento
        /// </summary>
        public int PageNumber;

        /// <summary>
        /// O conteúdo da página, pode ser EMFPage ou PCL6Page
        /// </summary>
        public Object Contents;

        public Page(int pageNumber, Object contents)
        {
            this.PageNumber = pageNumber;
            this.Contents = contents;
        }
    }

}
