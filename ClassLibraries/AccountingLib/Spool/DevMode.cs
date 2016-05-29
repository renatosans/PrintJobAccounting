using System;
using System.IO;
using DocMageFramework.CustomAttributes;


namespace AccountingLib.Spool
{
    /// <summary>
    /// Representa uma estrutura DEVMODE armazenada em arquivo (SPL ou SHD)
    /// </summary>
    public class DevMode
    {
        private const Int16 MONOCHROME = 1;
        private const Int16 COLOR = 2;
        private const Int16 SIMPLEX = 1;
        private const Int16 VERTICAL_DUPLEX = 2;
        private const Int16 HORIZONTAL_DUPLEX = 3;


        private Char[] deviceName = new Char[64];
        private Int16 specVersion;
        private Int16 driverVersion;
        private Int16 size;
        private Int16 driverExtra;
        private Int32 fields;
        private Int16 orientation;
        private Int16 paperSize;
        private Int16 paperLength;
        private Int16 paperWidth;
        private Int16 scale;
        private Int16 copies;
        private Int16 defaultSource;
        private Int16 printQuality;
        private Int16 color;
        private Int16 duplex;
        private Int16 yResolution;
        private Int16 trueTypeOption;
        private Int16 collate;
        private Char[] formName = new Char[32];
        private Int16 unusedPadding;
        private Int32 bitsPerPixel;
        private Int32 pixelWidth;
        private Int32 pixelHeight;
        private Int32 displayFlags;
        private Int32 displayFrequency;
        private Int32 icmMethod;
        private Int32 icmIntent;
        private Int32 mediaType;
        private Int32 ditherType;
        private Int32 reserved1;
        private Int32 reserved2;
        private Int32 panningWidth;
        private Int32 panningHeight;


        public String DeviceName
        {
            get { return new String(deviceName); }
        }

        public String Paper
        {
            get
            {
                String paper = ((PaperSize)paperSize).ToString();
                paper = AssociatedText.GetFieldDescription(typeof(PaperSize), paper);

                return paper;
            }
        }

        public int Copies
        {
            get { return copies; }
        }

        public Boolean Color
        {
            get { return color != MONOCHROME; }
        }

        public Boolean Duplex
        {
            get { return duplex != SIMPLEX; }
        }

        public Boolean Collate
        {
            get { return collate > 0; }
        }

        public String FormName
        {
            get { return new String(formName); }
        }

        public int BitsPerPixel
        {
            get { return bitsPerPixel; }
        }


        public DevMode(BinaryReader fileReader)
        {
            // Marca a posição de leitura do stream
            Int64 currentPos = fileReader.BaseStream.Position;

            deviceName = StringResource.Get(fileReader);
            fileReader.BaseStream.Seek(currentPos + 64, SeekOrigin.Begin);
            specVersion = fileReader.ReadInt16();
            driverVersion = fileReader.ReadInt16();
            size = fileReader.ReadInt16();
            driverExtra = fileReader.ReadInt16();
            fields = fileReader.ReadInt32();
            orientation = fileReader.ReadInt16();
            paperSize = fileReader.ReadInt16();
            paperLength = fileReader.ReadInt16();
            paperWidth = fileReader.ReadInt16();
            scale = fileReader.ReadInt16();
            copies = fileReader.ReadInt16();
            defaultSource = fileReader.ReadInt16();
            printQuality = fileReader.ReadInt16();
            color = fileReader.ReadInt16();
            duplex = fileReader.ReadInt16();
            yResolution = fileReader.ReadInt16();
            trueTypeOption = fileReader.ReadInt16();
            collate = fileReader.ReadInt16();

            // Marca a posição de leitura do stream
            currentPos = fileReader.BaseStream.Position;

            formName = StringResource.Get(fileReader);
            fileReader.BaseStream.Seek(currentPos + 32, SeekOrigin.Begin);
            unusedPadding = fileReader.ReadInt16();
            bitsPerPixel = fileReader.ReadInt32();
            pixelWidth = fileReader.ReadInt32();
            pixelHeight = fileReader.ReadInt32();
            displayFlags = fileReader.ReadInt32();
            displayFrequency = fileReader.ReadInt32();
            icmMethod = fileReader.ReadInt32();
            icmIntent = fileReader.ReadInt32();
            mediaType = fileReader.ReadInt32();
            ditherType = fileReader.ReadInt32();
            reserved1 = fileReader.ReadInt32();
            reserved2 = fileReader.ReadInt32();
            panningWidth = fileReader.ReadInt32();
            panningHeight = fileReader.ReadInt32();
        }
    }

}
