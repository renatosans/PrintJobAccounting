using System;
using System.Collections.Generic;


namespace Util
{
    /// <summary>
    /// Classe utilitária para parsing de argumentos
    /// Disponível também no sistema ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class ArgumentParser
    {
        /// <summary>
        /// Faz o parse da linha de comando e gera um array de argumentos 
        /// </summary>
        public static String[] ParseCommandLine(String commandLine)
        {
            List<String> argumentList = new List<String>();

            String currentToken = "";
            Boolean quotedToken = false;
            foreach (Char ch in commandLine)
            {
                if ((ch != '"') && (ch != ' ')) currentToken += ch;
                if (ch == '"') quotedToken = !quotedToken;
                if (ch == ' ')
                {
                    if (quotedToken)
                    {
                        currentToken += ch;
                        continue;
                    }
                    argumentList.Add(currentToken);
                    currentToken = "";
                }
            }
            argumentList.Add(currentToken);

            return argumentList.ToArray();
        }

        /// <summary>
        /// Obtem o valor atribuido ao argumento ( exemplo:  em  /U:sa o argumento é /U e o valor é 'sa')
        /// </summary>
        public static String GetValue(String argument)
        {
            String[] argumentTokens = argument.Split(new char[] { ':' });

            Boolean firstToken = true;
            String argumentValue = "";
            // Não utiliza StringBuilder pois há uma quantidade pequena de concatenações
            foreach (String token in argumentTokens)
            {
                // reconstroi ocorrências de dois pontos no meio do valor
                if (!String.IsNullOrEmpty(argumentValue)) argumentValue += ":";
                // concatena ao valor os pedaços
                if (!firstToken) argumentValue += token;
                firstToken = false;
            }

            return argumentValue;
        }

        /// <summary>
        /// Obtem o valor atribuido ao argumento ( exemplo:  em  /F:10 o argumento é /F e o valor é 10)
        /// </summary>
        public static int GetValue(String argument, int defaultValue)
        {
            int value;

            String stringValue = GetValue(argument);
            Boolean isNumeric = int.TryParse(stringValue, out value);
            if (!isNumeric) value = defaultValue;
            
            return value;
        }
    }

}
