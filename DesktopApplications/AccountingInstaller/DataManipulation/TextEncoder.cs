using System;
using System.Text;
using System.Globalization;


namespace AccountingInstaller.DataManipulation
{
    public static class TextEncoder
    {
        public static String Encode(String textToEncode)
        {
            byte[] characters = Encoding.UTF8.GetBytes(textToEncode);
            StringBuilder encodedText = new StringBuilder();
            foreach (Byte character in characters)
            {
                // Poderia fazer a simples conversão para hexadecimal, porém ficaria fácil
                // um intruso transformar de volta para texto
                // Ofusca o character (*64) para dificultar a engenharia reversa
                ushort word = Convert.ToUInt16(64 * character);
                encodedText.Append(word.ToString("x4"));
            }
            return encodedText.ToString().ToUpper();
        }


        public static String Decode(String textToDecode)
        {
            // Verifica se o texto está no formato esperado (números hexadecimais de 4 digitos)
            if ((textToDecode.Length < 4) || (textToDecode.Length % 4 != 0))
                return null;

            int length = textToDecode.Length / 4;
            Byte[] characters = new Byte[length];
            for (int pos = 0; pos < length; pos++)
            {
                String hexValue = textToDecode.Substring(pos * 4, 4);
                int decValue = int.Parse(hexValue, NumberStyles.HexNumber) / 64;
                characters[pos] = (Byte)decValue;
            }

            return Encoding.UTF8.GetString(characters);
        }
    }

}
