using System;
using System.IO;
using System.Text;
using System.Drawing;


namespace AccountingLib.Spool.EMF
{
    public class EMFPageHeader
    {
        private Int32 type;
        private Int32 size;

        private Int32 boundsLeft;
        private Int32 boundsTop;
        private Int32 boundsRight;
        private Int32 boundsBottom;

        private Int32 frameLeft;
        private Int32 frameTop;
        private Int32 frameRight;
        private Int32 frameBottom;

        private Byte signature1; // space
        private Byte signature2; // E
        private Byte signature3; // M
        private Byte signature4; // F

        private UInt32 version;
        private Int32 byteCount;
        private Int32 recordCount;
        private Int16 handleCount;
        private Int16 reserved;
        private Int32 descriptionLength;
        private Int32 descriptionOffset;
        private Int32 palEntries;
        private Int32 deviceWidth;
        private Int32 deviceHeight;
        private Int32 deviceWidthMilimeters;
        private Int32 deviceHeightMilimeters;
        private Int32 pixelFormatSize;
        private Int32 pixelFormatOffset;
        private Boolean openGL;
        private Int32 deviceWidthMicrometers;
        private Int32 deviceHeightMicrometers;
        private String description;
        private String signature;


        public Int32 Size
        {
            get { return size; }
        }

        /// <summary>
        /// Obtem os limites de página (dimensões do papel) 
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(boundsLeft, boundsTop, boundsRight, boundsBottom); }
        }

        /// <summary>
        /// Obtem o quadro contendo elementos de impressão. Este pode ser menor que o papel, pois
        /// muitas impressoras tem uma margem não imprimível nas extremidades.
        /// </summary>
        public Rectangle Frame
        {
            get { return new Rectangle(frameLeft, frameTop, frameRight, frameBottom); }
        }

        /// <summary>
        /// Retorna a dimensão do metafile device em pixels
        /// </summary>
        public Size DevicePixelDimensions
        {
            get { return new Size(deviceWidth, deviceHeight); }
        }

        /// <summary>
        /// Retorna a dimensão do metafile device em milimetros
        /// </summary>
        public Size DeviceMilimeterDimensions
        {
            get { return new Size(deviceWidthMilimeters, deviceHeightMilimeters); }
        }

        /// <summary>
        /// Retorna a dimensão do metafile device em micrometros
        /// </summary>
        public Size DeviceMicrometerDimensions
        {
            get { return new Size(deviceWidthMicrometers, deviceHeightMicrometers); }
        }

        public Int32 FileSize
        {
            get { return byteCount; }
        }

        /// <summary>
        /// Obtem o número de registros (EMF Records). Estes descrevem elementos de texto
        /// e elementos gráficos que fazem parte da página.
        /// </summary>
        public Int32 RecordCount
        {
            get { return recordCount; }
        }

        /// <summary>
        /// Descrição contendo o nome do aplicativo onde o metafile foi gerado e o nome da figura
        /// </summary>
        public String Description
        {
            get { return description; }
        }

        /// <summary>
        /// Assinatura do arquivo ( " EMF" caso tenha sido gerado corretamente )
        /// </summary>
        public String Signature
        {
            get { return signature; }
        }

        public EMFPageHeader(BinaryReader fileReader)
        {
            // Marca a posição inicial de leitura do fileStream
            Int64 startPos = fileReader.BaseStream.Position;

            type = fileReader.ReadInt32();
            size = fileReader.ReadInt32();

            boundsLeft = fileReader.ReadInt32();
            boundsTop = fileReader.ReadInt32();
            boundsRight = fileReader.ReadInt32();
            boundsBottom = fileReader.ReadInt32();

            frameLeft = fileReader.ReadInt32();
            frameTop = fileReader.ReadInt32();
            frameRight = fileReader.ReadInt32();
            frameBottom = fileReader.ReadInt32();

            signature1 = fileReader.ReadByte();
            signature2 = fileReader.ReadByte();
            signature3 = fileReader.ReadByte();
            signature4 = fileReader.ReadByte();

            version = fileReader.ReadUInt32();
            byteCount = fileReader.ReadInt32();
            recordCount = fileReader.ReadInt32();
            handleCount = fileReader.ReadInt16();
            reserved = fileReader.ReadInt16();
            descriptionLength = fileReader.ReadInt32();
            descriptionOffset = fileReader.ReadInt32();
            palEntries = fileReader.ReadInt32();

            deviceWidth = fileReader.ReadInt32();
            deviceHeight = fileReader.ReadInt32();
            deviceWidthMilimeters = fileReader.ReadInt32();
            deviceHeightMilimeters = fileReader.ReadInt32();

            if (size > 88)
            {
                pixelFormatSize = fileReader.ReadInt32();
                pixelFormatOffset = fileReader.ReadInt32();
                openGL = fileReader.ReadInt32() != 0;
            }

            if (size > 100)
            {
                deviceWidthMicrometers = fileReader.ReadInt32();
                deviceHeightMicrometers = fileReader.ReadInt32();
            }

            if (descriptionLength > 0)
            {
                fileReader.BaseStream.Seek(startPos + descriptionOffset, SeekOrigin.Begin);
                description = new String(StringResource.Get(fileReader));

                if (description.Length+2 < descriptionLength) // Verifica se existe uma outra string
                {
                    fileReader.ReadChars(1);
                    description = description + "#" + new String(StringResource.Get(fileReader));
                }
            }

            signature = Encoding.UTF8.GetString(new Byte[] { signature1, signature2, signature3, signature4 });
        }
    }

}
