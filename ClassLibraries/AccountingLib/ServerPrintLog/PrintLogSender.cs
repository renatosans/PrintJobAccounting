using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.AppUtils;


namespace AccountingLib.ServerPrintLog
{
    public class PrintLogSender
    {
        private List<PrintedDocument> printedDocumentList;

        private String serviceUrl;

        private IListener listener;

        private const int bufferSize = 16;

        private Boolean noErrors = true; // nenhum erro por enquanto


        public PrintLogSender(String serviceUrl, IListener listener)
        {
            ServicePointManager.Expect100Continue = false;

            this.printedDocumentList = new List<PrintedDocument>();
            this.serviceUrl = serviceUrl;
            this.listener = listener;
        }

        /// <summary>
        /// Adiciona um documento a ser enviado, quando a lista atinge o tamanho máximo do buffer
        /// realiza o envio
        /// </summary>
        public void AddPrintedDocument(PrintedDocument printedDocument)
        {
            printedDocumentList.Add(printedDocument);
            if (printedDocumentList.Count >= bufferSize)
            {
                SendPrintLogs();
            }
        }

        private void SendPrintLogs()
        {
            Boolean logsSent = false;
            int timeout = 16384;

            // Cria uma proteção contra loops infinitos (MAX_ATTEMPTS)
            const int MAX_ATTEMPTS = 3; // tenta no máximo 3 vezes

            int attempts = 0;
            while ((!logsSent) && (attempts < MAX_ATTEMPTS))
            {
                logsSent = TrySend(timeout);
                timeout = timeout * 2; // dobra o timeout a cada tentativa
                attempts++;
            }

            if (!logsSent)
            {
                // sinaliza problemas no envio e retorna
                noErrors = false;
                return;
            }

            // Se os logs foram enviados com sucesso limpa a lista para inserção dos próximos logs
            printedDocumentList.Clear();
        }

        private Boolean TrySend(int timeout)
        {
            Exception lastException = null;
            HttpStatusCode status = HttpStatusCode.BadRequest; // valor inicial antes do envio
            if (listener != null) listener.NotifyObject("Enviando pacote de logs de impressão");

            HttpWebRequest request = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            try
            {
                Byte[] serializedObject = ObjectSerializer.SerializeObjectToArray(printedDocumentList);
                String encodedData = HttpUtility.UrlEncode(Convert.ToBase64String(serializedObject));
                Byte[] postData = Encoding.UTF8.GetBytes("txtPostData=" + encodedData);

                request = (HttpWebRequest)WebRequest.Create(serviceUrl + "?action=LogPrintedDocuments");
                request.Method = "POST";
                request.ServicePoint.ConnectionLimit = timeout * 5;
                request.Timeout = timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                response = (HttpWebResponse)request.GetResponse();
                status = response.StatusCode;
            }
            catch (Exception exception)
            {
                lastException = exception;
            }
            finally
            {
                if (response != null) { ((IDisposable)response).Dispose(); response.Close(); }
                if (requestStream != null) requestStream.Close();
                request = null; // permite que o garbage collector elimine o objeto
            }

            if (lastException != null)
            {
                if (listener != null) listener.NotifyObject(lastException);
                return false;
            }

            if (status != HttpStatusCode.OK)
            {
                if (listener != null) listener.NotifyObject("Falha no envio. Status = " + status);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Envia os últimos documentos da lista, verifica se houve erro em algum envio de pacote
        /// </summary>
        public Boolean FinishSending()
        {
            SendPrintLogs();
            return noErrors;
        }
    }

}
