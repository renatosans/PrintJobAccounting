using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using DocMageFramework.AppUtils;


namespace AccountingLib.Spool.EMF
{
    public class EMFPage
    {
        private IListener listener;

        private EMFPageHeader header;

        private Metafile pageImage;

        private Image thumbnail;

        private List<EMFRecord> records;

        private Boolean pageProcessed;

        private MemoryStream pageStream;


        /// <summary>
        /// Obtem o "header record" da página
        /// </summary>
        public EMFPageHeader Header
        {
            // Para obter o header não é necessário processar a página, o header é lido no construtor
            get { return header; }
        }

        /// <summary>
        /// Obtem uma imagem da página em tamanho real (escala 1:1)
        /// </summary>
        public Metafile PageImage
        {
            // Utiliza "lazy instantiation" para evitar consumo de recursos enquanto eles não são necessários
            get
            {
                if (!pageProcessed) ProcessPage();
                return pageImage;
            }
        }

        /// <summary>
        /// Obtem uma imagem reduzida da página. O tamanho do thumbnail é definido por sua escala
        /// </summary>
        public Image Thumbnail
        {
            // Utiliza "lazy instantiation" para evitar consumo de recursos enquanto eles não são necessários
            get
            {
                if (!pageProcessed) ProcessPage();
                return thumbnail;
            }
        }

        /// <summary>
        /// Obtem os registros EMF contidos na página
        /// </summary>
        public List<EMFRecord> Records
        {
            // Utiliza "lazy instantiation" para evitar consumo de recursos enquanto eles não são necessários
            get
            {
                if (!pageProcessed) ProcessPage();
                return records;
            }
        }


        // Monta a página a partir de seu stream, o stream deve estar posicionado para a leitura
        private void ProcessPage()
        {
            NotifyListener("Iniciando processamento da página.");

            // Obtem o metafile da página e cria o thumbnail
            pageImage = new Metafile(pageStream);
            NotifyListener("Criando thumbnail da página.");
            Int32 scale = 20;
            thumbnail = pageImage.GetThumbnailImage((int)(header.Frame.Width / scale), (int)(header.Frame.Height / scale), null, IntPtr.Zero);
            pageStream.Seek(0, SeekOrigin.Begin);

            records = new List<EMFRecord>();
            for (int record = 1; record <= header.RecordCount; record++) // O primeiro registro é o header
            {
                EMFRecord emfRecord = new EMFRecord(pageStream);
                records.Add(emfRecord);
                pageStream.Seek(emfRecord.RecSeek + emfRecord.RecSize, SeekOrigin.Begin);
            }

            NotifyListener("Processamento da página concluído.");
            pageProcessed = true;
        }

        /// <summary>
        /// Construtor da classe. Armazena o conteúdo do registro EMF (gravado em disco) na
        /// memória para processamento posterior
        /// </summary>
        public EMFPage(BinaryReader fileReader, IListener listener)
        {
            this.listener = listener;

            // Marca a posição inicial de leitura do fileStream
            Int64 startPos = fileReader.BaseStream.Position;

            // Faz a leitura do header e retorna para a posição inicial
            NotifyListener("Lendo o header da página EMF.");
            header = new EMFPageHeader(fileReader);
            fileReader.BaseStream.Seek(startPos, SeekOrigin.Begin);

            Byte[] buffer = fileReader.ReadBytes(header.FileSize);
            this.pageProcessed = false;
            this.pageStream = new MemoryStream();
            this.pageStream.Write(buffer, 0, buffer.Length);
            this.pageStream.Seek(0, SeekOrigin.Begin);
        }

        private void NotifyListener(Object obj)
        {
            if (listener != null)
                listener.NotifyObject(obj);
        }
    }

}
