using System;
using System.IO;
using System.Collections.Generic;
using DocMageFramework.AppUtils;


namespace AccountingLib.Spool.EMF
{
    public class EMFSpoolFile: JobSpoolFile
    {
        private IListener listener;

        private Boolean malformedFile;

        private String jobDescription;

        private DevMode devModeRecord;


        public Boolean MalformedFile
        {
            get { return malformedFile; }
        }

        public String JobDescription
        {
            get { return jobDescription; }
        }

        public DevMode DevModeRecord
        {
            get { return devModeRecord; }
        }

        public EMFSpoolFile(BinaryReader fileReader, IListener listener): base(fileReader)
        {
            this.listener = listener;

            // Aborta caso não consiga obter informações do job (arquivo mal formado)
            if (!GetJobInfo(fileReader))
            {
                malformedFile = true;
                return;
            }

            SPLRecord nextRecord = new SPLRecord(fileReader);
            while (nextRecord.RecType != SPLRecordTypeEnum.SRT_EOF)
            {
                ProcessSPLRecord(nextRecord, fileReader);
                nextRecord = new SPLRecord(fileReader);
            }
        }

        private Boolean GetJobInfo(BinaryReader fileReader)
        {
            SPLRecord record = new SPLRecord(fileReader);
            Int64 recSeek = record.RecSeek;
            if (record.RecType != SPLRecordTypeEnum.SRT_JOB_INFO)
                return false;

            fileReader.ReadBytes(8);
            Char[] jobDescriptionArray = StringResource.Get(fileReader);
            jobDescription = new String(jobDescriptionArray);
            fileReader.BaseStream.Seek(recSeek, SeekOrigin.Begin);
            if (String.IsNullOrEmpty(jobDescription))
                return false;

            return true;
        }

        private void ProcessSPLRecord(SPLRecord record, BinaryReader fileReader)
        {
            Int64 recSeek = record.RecSeek;
            Int32 recSize = record.RecSize;
            if (recSize <= 0) recSize = 8;

            switch (record.RecType)
            {
                case SPLRecordTypeEnum.SRT_JOB_INFO:
                    fileReader.BaseStream.Seek(recSeek + recSize, SeekOrigin.Begin);
                    break;
                case SPLRecordTypeEnum.SRT_EOF:
                    // Final de arquivo, não faz nada
                    break;
                case SPLRecordTypeEnum.SRT_DEVMODE:
                    devModeRecord = new DevMode(fileReader);
                    fileReader.BaseStream.Seek(recSeek + 8 + recSize, SeekOrigin.Begin);
                    break;
                case SPLRecordTypeEnum.SRT_PAGE:
                case SPLRecordTypeEnum.SRT_EXT_PAGE:
                    ProcessEMFPage(record, fileReader);
                    break;
                case SPLRecordTypeEnum.SRT_EOPAGE1:
                case SPLRecordTypeEnum.SRT_EOPAGE2:
                    Byte[] bytes = fileReader.ReadBytes(recSize);
                    if (recSize == 0x08)
                        fileReader.BaseStream.Seek(recSeek + recSize + 8, SeekOrigin.Begin);
                    break;
                case SPLRecordTypeEnum.SRT_EXT_FONT2:
                    fileReader.BaseStream.Seek(recSeek + 4, SeekOrigin.Begin);
                    break;
                default:
                    fileReader.BaseStream.Seek(recSeek + recSize, SeekOrigin.Begin);
                    break;
            }
        }

        private void ProcessEMFPage(SPLRecord record, BinaryReader fileReader)
        {
            Int64 nextRecordStart = record.RecSeek + 8;
            fileReader.BaseStream.Seek(nextRecordStart, SeekOrigin.Begin);
            
            EMFPage emfPage = new EMFPage(fileReader, listener);
            Pages.Add(emfPage);
            if (listener != null) listener.NotifyObject("Adicionada página EMF número " + Pages.Count);

            nextRecordStart = nextRecordStart + emfPage.Header.FileSize;
            fileReader.BaseStream.Seek(nextRecordStart, SeekOrigin.Begin);
        }
    }

}
