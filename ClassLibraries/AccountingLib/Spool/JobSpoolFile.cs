using System;
using System.IO;
using System.Collections.Generic;


namespace AccountingLib.Spool
{
    public class JobSpoolFile
    {
        protected Int64 startPos;

        public List<Object> Pages = new List<Object>(); // Páginas do documento em spool

        public int FileSize;


        public JobSpoolFile(BinaryReader fileReader)
        {
            // Marca a posição inicial de leitura do fileStream
            startPos = fileReader.BaseStream.Position;
        }
    }

}
