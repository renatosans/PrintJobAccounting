using System;
using System.IO;
using System.Reflection;


namespace DocMageFramework.AppUtils
{
    public static class IOHandler
    {
        /// <summary>
        /// Copia o conteúdo de um Stream para outro
        /// </summary>
        public static void CopyStream(Stream source, Stream destination)
        {
            Byte[] buffer = new Byte[32768];
            int bytesRead;
            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
        }

        /// <summary>
        /// Busca um recurso embarcado na aplicação caso esteja disponível, alternativamente busca em disco
        /// Recebe o nome do recurso embarcado (ignora namespaces no nome)
        /// </summary>
        public static Stream GetEmbeddedResource(String name)
        {
            if (String.IsNullOrEmpty(name)) return null;

            String[] nameParts = name.Split(new Char[] { '.' });
            int length = nameParts.Length;
            if (length < 2) return null;
            String rawName = nameParts[length - 2] + "." + nameParts[length - 1];
            String qualifiedName = rawName;

            Assembly runningExe = Assembly.GetEntryAssembly();
            if (runningExe == null)
            {
                if (!File.Exists(rawName)) return null;
                return new FileStream(rawName, FileMode.Open);
            }

            Stream resourceStream = runningExe.GetManifestResourceStream(rawName);
            if (resourceStream == null)
            {
                qualifiedName = runningExe.GetName().Name + "." + rawName;
                resourceStream = runningExe.GetManifestResourceStream(qualifiedName);
            }
            if (resourceStream == null) return null;

            return resourceStream;
        }
    }

}
