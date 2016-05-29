using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;


namespace AccountingClientInstaller.Util
{
    // Classe responsavel pelas requisições feitas ao servidor durante a instalação
    public class RequestHandler
    {
        private String serviceUrl;

        private int timeout;

        private String requestAction;

        private Object requestData;

        private String responseData;

        private IListener listener;


        public RequestHandler(String serviceUrl, int timeout, IListener listener)
        {
            this.serviceUrl = serviceUrl;
            this.timeout = timeout;
            this.listener = listener;
        }

        private Boolean TrySend()
        {
            Exception lastException = null;
            HttpStatusCode status = HttpStatusCode.BadRequest; // valor inicial antes do envio
            if (listener != null) listener.NotifyObject("Enviando requisição...");

            HttpWebRequest request = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            try
            {
                Byte[] serializedObject = ObjectSerializer.SerializeObjectToArray(requestData);
                String encodedData = HttpUtility.UrlEncode(Convert.ToBase64String(serializedObject));
                Byte[] postData = Encoding.UTF8.GetBytes("txtPostData=" + encodedData);

                request = (HttpWebRequest)WebRequest.Create(serviceUrl + "?action=" + requestAction);
                request.Method = "POST";
                request.ServicePoint.ConnectionLimit = 10;  // define limite para 10 conexões
                request.Timeout = this.timeout;             // define o timeout
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                response = (HttpWebResponse)request.GetResponse();
                status = response.StatusCode;
                responseStream = response.GetResponseStream();
                Byte[] buffer = new Byte[response.ContentLength];
                responseStream.Read(buffer, 0, (int)response.ContentLength);
                responseData = Encoding.UTF8.GetString(buffer);
            }
            catch (Exception exception)
            {
                lastException = exception;
            }
            finally
            {
                if (responseStream != null) responseStream.Close();
                if (response != null) { ((IDisposable)response).Dispose(); response.Close(); }
                if (requestStream != null) requestStream.Close();
                request = null; // permite que o garbage collector elimine o objeto
            }

            if (lastException != null)
            {
                if (listener != null) listener.NotifyObject(lastException.Message);
                return false;
            }

            if (status != HttpStatusCode.OK)
            {
                if (listener != null) listener.NotifyObject("Falha no envio. Status = " + status);
                return false;
            }

            return true;
        }

        private Boolean SendRequest()
        {
            Boolean requestSent = false;

            // Cria uma proteção contra loops infinitos (MAX_ATTEMPTS)
            const int MAX_ATTEMPTS = 3; // tenta no máximo 3 vezes

            int attempts = 0;
            while ((!requestSent) && (attempts < MAX_ATTEMPTS))
            {
                requestSent = TrySend();
                timeout = timeout * 2; // dobra o timeout a cada tentativa
                attempts++;
            }

            return requestSent;
        }

        // Inicia a requisição ao servidor
        public Boolean StartRequest(String requestAction, Object requestData)
        {
            this.requestAction = requestAction;
            this.requestData = requestData;

            if (!SendRequest())
            {
                if (listener != null) listener.NotifyObject("Falha ao enviar requisição. Action = " + requestAction);
                return false;
            }

            return true;
        }

        // Trata cada tipo de resposta de acordo com o tipo do objeto
        public Object ParseResponse(Type objectType)
        {
            Object parsedValue = null;

            String responseDelimiter = "</response>";
            int responseLenght = responseData.IndexOf(responseDelimiter) + responseDelimiter.Length;
            String serializedObject = responseData.Substring(0, responseLenght);
            // Remove as tags da resposta
            serializedObject = serializedObject.Replace("<response>", "");
            serializedObject = serializedObject.Replace("</response>", "");

            // Se o tipo do objeto for int faz o parse correspondente
            if (objectType == typeof(int))
                parsedValue = int.Parse(serializedObject);

            // Se o tipo do objeto for DateTime faz o parse correspondente
            if (objectType == typeof(DateTime))
                parsedValue = DateTime.Parse(serializedObject);

            // Se o tipo do objeto for Boolean faz o parse correspondente
            if (objectType == typeof(Boolean))
                parsedValue = Boolean.Parse(serializedObject);

            // Se o tipo do objeto for String retorna seu valor sem o parse
            if (objectType == typeof(String))
                parsedValue = serializedObject;

            // Caso não se enquadre em nenhum dos tipos primitivos, trata o tipo como "objeto serializado"
            if (parsedValue == null)
                parsedValue = ObjectSerializer.DeserializeObject(serializedObject, objectType);

            return parsedValue;
        }

        public String GetRawResponse()
        {
            return responseData;
        }
    }

}
