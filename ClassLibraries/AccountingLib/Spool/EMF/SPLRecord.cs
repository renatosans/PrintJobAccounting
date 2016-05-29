using System;
using System.IO;


namespace AccountingLib.Spool.EMF
{
    public class SPLRecord
    {
        private Int64 seek;

        private Int32 type;

        private Int32 size;


        public Int64 RecSeek
        {
            get { return seek; }
        }

        public SPLRecordTypeEnum RecType
        {
            get { return (SPLRecordTypeEnum) type; }
        }

        public Int32 RecSize
        {
            get { return size; }
        }


        public SPLRecord(BinaryReader fileReader)
        {
            seek = fileReader.BaseStream.Position;
            try
            {
                type = fileReader.ReadInt32();
            }
            catch (EndOfStreamException)
            {
                type = (Int32)SPLRecordTypeEnum.SRT_EOF;
                return;
            }
            size = fileReader.ReadInt32();
        }
    }

}
