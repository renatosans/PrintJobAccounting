using System;
using System.Web;
using System.Collections.Specialized;


namespace DocMageFramework.WebUtils
{
    public static class URLHandler
    {
        public static String InsertQueryVariable(String url, String variable)
        {
            // Verifica se os parâmetros são nulos
            if (String.IsNullOrEmpty(url) || String.IsNullOrEmpty(variable))
                return url;

            // Verifica se a variável está no formato correto ( nome=valor )
            String[] variableParts = variable.Split(new Char[] { '=' });
            if (variableParts.Length != 2)
                return url;

            // Verifica se não possui uma query pré existente, caso não tenha cria uma
            if (!url.Contains("aspx?"))
                return url + "?" + variable;

            // Verifica se a url está no formato correto ( resource?queryString )
            String[] urlParts = url.Split(new Char[] { '?' });
            if (urlParts.Length != 2)
                return url;
            NameValueCollection queryVariables = HttpUtility.ParseQueryString(urlParts[1]);
            if (!queryVariables.HasKeys())
                return url;

            // Verifica se já possui a variável na query, caso possua substitui pela nova
            String variableName = variableParts[0];
            if (url.Contains(variableName))
            {
                String oldVariable = variableName + "=" + queryVariables[variableName];
                return url.Replace(oldVariable, variable);
            }

            // Por padrão (caso não se enquadre nas anteriores), apenas insere a variável na query
            return url + "&" + variable;
        }
    }

}
