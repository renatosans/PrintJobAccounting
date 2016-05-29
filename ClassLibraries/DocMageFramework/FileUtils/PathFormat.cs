using System;


namespace DocMageFramework.FileUtils
{
    /// <summary>
    /// Classe utilitária que possui método para formatar um caminho
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class PathFormat
    {
        public static string Adjust(string unformattedPath)
        {
            // Sai do método caso a string seja vazia ( retorna a entrada )
            if (String.IsNullOrEmpty(unformattedPath))
                return unformattedPath;

            string formattedPath = unformattedPath;
            if (!unformattedPath[unformattedPath.Length - 1].Equals('\\'))
            {
                formattedPath = formattedPath + '\\';
            }

            return formattedPath;
        }
    }

}
