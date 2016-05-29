using System;
using System.Reflection;
using System.Collections.Generic;


namespace DocMageFramework.Reflection
{
    /// <summary>
    /// Classe que recebe uma sequência de parâmetros fora de ordem (ou faltando), e monta os
    /// parâmetros necessários para a criação de um objeto na ordem esperada
    /// </summary>
    public class ArgumentBuilder
    {
        private Dictionary<String, String> argumentList;

        private String lastError;


        public ArgumentBuilder()
        {
            argumentList = new Dictionary<String, String>();
        }

        /// <summary>
        /// Adiciona um argumento à lista
        /// </summary>
        public void Add(String argumentName, String argumentValue)
        {
            argumentList.Add(argumentName, argumentValue);
        }

        /// <summary>
        /// Retorna um dos argumentos contidos na lista, método privado utilizado em "GetArguments"
        /// </summary>
        private Object GetArgument(ParameterInfo parameterInfo)
        {
            // Caso não receba a informação necessária retorna null
            if (parameterInfo == null) return null;

            // Caso não exista na lista retorna null
            if (!argumentList.ContainsKey(parameterInfo.Name))
                return null;

            String argumentName = parameterInfo.Name;
            String argumentValue = argumentList[argumentName];
            Type argumentType = parameterInfo.ParameterType;

            // Caso o objeto seja uma string não é necessário fazer Parse
            if (argumentType == typeof(String)) return argumentValue;
            
            // Executa o método Parse para preencher o objeto com o seu valor
            MethodInfo parseMethod = argumentType.GetMethod("TryParse", new Type[] { typeof(String), argumentType.MakeByRefType() });
            if (parseMethod == null) return null;
            Object argument = Activator.CreateInstance(argumentType);
            Object[] parseMethodParams = new Object[] { argumentValue, argument };
            Boolean parseSucceded = (Boolean) argumentType.InvokeMember("TryParse", BindingFlags.InvokeMethod, null, null, parseMethodParams);
            if (!parseSucceded)
            {
                lastError = GetWarning();
                return null;
            }
            argument = parseMethodParams[1]; // parametro de output do método TryParse

            return argument;
        }

        /// <summary>
        /// Retorna os parâmetros necessários para a criação do objeto
        /// </summary>
        public Object[] GetArguments(Type objectType)
        {
            Object[] result = null;

            // Usa reflection para analisar os parâmetros passados na chamada do construtor
            ParameterInfo[] parameters = objectType.GetConstructors()[0].GetParameters();
            if ((parameters == null) || (parameters.Length < 1)) return null;

            result = new Object[parameters.Length];
            for (int index = 0; index < parameters.Length; index++)
            {
                result[index] = GetArgument(parameters[index]);
            }

            return result;
        }

        public static String GetWarning()
        {
            String warningMessage = "Os parâmetros passados para a página não estão em um formato válido.<br>" +
                                    "Retorne a página principal para continuar.<br>";
            return warningMessage;
        }
    }

}
