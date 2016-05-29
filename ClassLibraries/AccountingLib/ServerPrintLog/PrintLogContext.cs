using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using AccountingLib.Security;
using DocMageFramework.AppUtils;
using DocMageFramework.FileUtils;


namespace AccountingLib.ServerPrintLog
{
    /// <summary>
    /// Classe utilitária que possui métodos para gravar e recuperar os parâmetros de Job Routing
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class PrintLogContext
    {
        /// <summary>
        /// Obtem os parâmetros de execução a partir do XML
        /// </summary>
        public static NameValueCollection GetTaskParams()
        {
            NameValueCollection taskParams = new NameValueCollection();

            try
            {
                String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
                String xmlLocation = PathFormat.Adjust(baseDir) + "JobRouting.xml";
                XmlTextReader xmlReader = new XmlTextReader(xmlLocation);
                xmlReader.ReadStartElement("jobrouting");
                taskParams.Add("url", xmlReader.ReadElementString("url"));
                taskParams.Add("tenantId", xmlReader.ReadElementString("tenantid"));
                taskParams.Add("interval", xmlReader.ReadElementString("interval"));
                taskParams.Add("logDirectories", xmlReader.ReadElementString("logdirectories"));
                taskParams.Add("copyLogDir", xmlReader.ReadElementString("copylogdir"));
                taskParams.Add("installationKey", xmlReader.ReadElementString("installationkey"));
                taskParams.Add("xmlHash", xmlReader.ReadElementString("xmlhash"));
                xmlReader.ReadEndElement();
                xmlReader.Close();
            }
            catch
            {
                return null;
            }
            
            String installationKey = ResourceProtector.GetHardwareId();
            installationKey = Cipher.GenerateHash(installationKey);

            String xmlHash = taskParams["url"] + taskParams["tenantId"] + taskParams["interval"] + taskParams["logDirectories"];
            xmlHash = Cipher.GenerateHash(xmlHash);

            // Verifica se os parâmetros estão corretos (batem com os configurados durante a instalação)
            if (installationKey != taskParams["installationkey"]) return null;
            if (xmlHash != taskParams["xmlhash"]) return null;

            return taskParams;
        }

        /// <summary>
        /// Grava os parâmetros de execução no XML ( opcionalmente em outputStream caso fornecido )
        /// </summary>
        public static Boolean SetTaskParams(NameValueCollection taskParams, Stream outputStream)
        {
            try
            {
                XmlTextWriter xmlWriter = GetOutputWriter(outputStream);
                xmlWriter.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine);
                xmlWriter.WriteStartElement("jobrouting");
                xmlWriter.WriteRaw(Environment.NewLine);
                foreach (String param in taskParams)
                {
                    xmlWriter.WriteRaw("".PadLeft(4, ' '));
                    xmlWriter.WriteElementString(param.ToLower(), taskParams[param]);
                    xmlWriter.WriteRaw(Environment.NewLine);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteRaw(Environment.NewLine);
                xmlWriter.Close();
            }
            catch
            {
                return false; // retorna status de falha
            }

            return true; // retorna status de sucesso
        }

        private static XmlTextWriter GetOutputWriter(Stream outputStream)
        {
            XmlTextWriter xmlWriter = null;

            // Caso não exista um Stream para saída grava o XML em disco
            if (outputStream == null)
            {
                String baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.ToString());
                String xmlLocation = PathFormat.Adjust(baseDir) + "JobRouting.xml";
                xmlWriter = new XmlTextWriter(xmlLocation, Encoding.UTF8);
                return xmlWriter;
            }

            xmlWriter = new XmlTextWriter(outputStream, Encoding.UTF8);
            return xmlWriter;
        }
    }

}
