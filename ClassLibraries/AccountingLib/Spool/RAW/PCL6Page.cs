using System;
using System.IO;
using System.Drawing;


namespace AccountingLib.Spool.RAW
{
    public class PCL6Page
    {
        private Image thumbnail;

        /// <summary>
        /// Obtem uma imagem reduzida da página.
        /// </summary>
        public Image Thumbnail
        {
            get { return thumbnail; }
        }

        public PCL6Page(BinaryReader fileReader)
        {
        }
    }

}
