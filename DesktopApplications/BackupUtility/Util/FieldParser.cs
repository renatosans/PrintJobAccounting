using System;


namespace Util
{
    /// <summary>
    /// Classe utilitária para parsing de campos de formulário
    /// Disponível também no sistema ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class FieldParser
    {
        /// <summary>
        /// Verifica se o campo possui apenas letras e números, quando o campo contém espaços e
        /// caracteres especiais ele não será considerado alfanumérico 
        /// </summary>
        public static Boolean IsAlphaNumeric(String fieldText)
        {
            if (String.IsNullOrEmpty(fieldText)) return false;
            Boolean isAlphaNumeric = true;

            foreach (Char character in fieldText)
            {
                if (!Char.IsLetterOrDigit(character))
                {
                    isAlphaNumeric = false;
                    break;
                }
            }

            return isAlphaNumeric;
        }

        /// <summary>
        /// Checa se a virgula é o separador decimal
        /// </summary>
        public static Boolean IsCommaDecimalSeparator()
        {
            Double checkValue = 110;
            String converted = "" + (checkValue/100);
            if (converted.Contains(",")) // checa se o valor 1.1 convertido em String tem virgula
                return true;

            return false;
        }
    }

}
