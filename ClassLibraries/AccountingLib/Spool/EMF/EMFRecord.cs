using System;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace AccountingLib.Spool.EMF
{
    public class EMFRecord
    {
        private Int64 seek;

        private Int32 type;

        private Int32 size;

        private Byte[] data;


        public Int64 RecSeek
        {
            get { return seek; }
        }

        public EmfPlusRecordType RecType
        {
            get { return (EmfPlusRecordType)type; }
        }

        public Int32 RecSize
        {
            get { return size; }
        }

        public Byte[] Data
        {
            get { return data; }
        }

        /// <summary>
        /// Constroi o registro EMF a partir de um arquivo (através de fileReader)
        /// </summary>
        public EMFRecord(BinaryReader fileReader)
        {
            seek = fileReader.BaseStream.Position;
            type = fileReader.ReadInt32();
            size = fileReader.ReadInt32();
            data = fileReader.ReadBytes(size - 8);
        }

        /// <summary>
        /// Constroi o registro EMF a partir de sua representação em memória (recebe ponteiro)
        /// </summary>
        public EMFRecord(IntPtr memoryAddress)
        {
            seek = memoryAddress.ToInt64();

            Int32[] buffer1 = new Int32[1];
            Marshal.Copy(memoryAddress, buffer1, 0, 1);
            type = buffer1[0];

            Int32[] buffer2 = new Int32[1];
            Marshal.Copy(new IntPtr(memoryAddress.ToInt64() + 4), buffer2, 0, 1);
            size = buffer2[0];

            data = new Byte[size - 8];
            Marshal.Copy(new IntPtr(memoryAddress.ToInt64() + 8), data, 0, size - 8);
        }

        /// <summary>
        /// Constroi o registro EMF a partir de um stream, o stream deve estar posicionado para leitura
        /// </summary>
        public EMFRecord(Stream contentStream)
        {
            seek = contentStream.Position;
            Byte[] buffer = new Byte[8];
            contentStream.Read(buffer, 0, 8);
            type = BitConverter.ToInt32(buffer, 0);
            size = BitConverter.ToInt32(buffer, 4);
            data = new Byte[size - 8];
            contentStream.Read(data, 0, size - 8);
        }
    }

}
