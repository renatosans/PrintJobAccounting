using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Forms;


namespace AccountingInstaller.DataManipulation
{
    public class ContainerHandler
    {
        private void CreateContainer(String container)
        {
            // Cria o container vazio, sem nenhum arquivo
            String xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine +
                                "<files>" + Environment.NewLine +
                                "</files>" + Environment.NewLine;

            StreamWriter streamWriter = File.CreateText(container);
            streamWriter.Write(xmlContent);
            streamWriter.Close();
        }


        private void InjectIntoContainer(String container, String filePath, String rootDirectory)
        {
            // Falha na passagem dos parâmetros, sai do método
            if (!filePath.Contains(rootDirectory))
                return;

            int pos = filePath.IndexOf(rootDirectory);
            int len = rootDirectory.Length;
            String fileName = "." + filePath.Remove(pos, len);  // Path.GetFileName(filePath)

            TextReader textReader = new StreamReader(filePath);
            String fileContent = textReader.ReadToEnd();
            textReader.Close();

            List<ContainedFile> containedFiles = GetContainedFiles(container);
            String xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + Environment.NewLine +
                                "<files count=\"" + (containedFiles.Count+1) + "\">" + Environment.NewLine;
            foreach (ContainedFile file in containedFiles)
            {
                xmlContent = xmlContent + "    <file name=\"" + file.fileName + "\" >" +
                             file.fileContent + "</file>" + Environment.NewLine;
            }
            xmlContent = xmlContent + "    <file name=\"" + fileName + "\" >" + TextEncoder.Encode(fileContent) + "</file>" +
                             Environment.NewLine + "</files>" + Environment.NewLine;

            StreamWriter streamWriter = File.CreateText(container);
            streamWriter.Write(xmlContent);
            streamWriter.Close();
        }


        public void BuildContainer(String container, String rootDirectory)
        {
            // Cria o arquivo em disco
            CreateContainer(container);

            // Cria uma lista com os arquivos a serem inseridos no container
            // Faz isso buscando em todos os diretórios e subdiretórios de "rootDirectory"
            List<String> fileList = new List<String>();
            Stack<String> directoryStack = new Stack<String>();
            directoryStack.Push(rootDirectory);

            while (directoryStack.Count > 0)
            {
                String directory = directoryStack.Pop();

                fileList.AddRange(Directory.GetFiles(directory, "*.*"));
                foreach (String subDirectory in Directory.GetDirectories(directory))
                {
                    directoryStack.Push(subDirectory);
                }
            }

            // Injeta cada arquivo da lista no container
            foreach (String file in fileList)
            {
                InjectIntoContainer(container, file, rootDirectory);
            }
        }


        private List<ContainedFile> GetContainedFiles(String container)
        {
            // Os dados do arquivo são obtidos do xml sem nenhuma transformação (mantendo o encode)
            List<ContainedFile> files = new List<ContainedFile>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(container);
            XmlNode mainNode = xmlDoc.SelectSingleNode("//files");
            foreach (XmlNode childNode in mainNode.ChildNodes)
            {
                ContainedFile containedFile = new ContainedFile();
                containedFile.fileName = childNode.Attributes["name"].Value;
                containedFile.fileContent = childNode.InnerText;
                files.Add(containedFile);
            }

            return files;
        }


        public ContainedFile ExtractFromContainer(String container, String fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(container);
            XmlNode fileNode = xmlDoc.SelectSingleNode("//files/file[@name='" + fileName + "']");

            if (fileNode == null)
                return null;

            ContainedFile containedFile = new ContainedFile();
            containedFile.fileName = fileNode.Attributes["name"].Value;
            containedFile.fileContent = TextEncoder.Decode(fileNode.InnerText);

            return containedFile;
        }
    }

}
