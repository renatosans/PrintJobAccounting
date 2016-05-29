using System;
using System.IO;


namespace AccountingLib.Spool.RAW
{
    public class PCL6SpoolFile: JobSpoolFile
    {
        public PCL6SpoolFile(BinaryReader fileReader): base(fileReader)
        {
            PCL6Page newPage = new PCL6Page(fileReader);
            Pages.Add(newPage);
        }
    }

}
