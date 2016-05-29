using System;
using System.Text;
using System.Security.Cryptography;


namespace DocMageFramework.AppUtils
{
    public static class Cipher
    {
        private const String salt = "EJ%S$@!";

        /// <summary>
        /// Gera um hash MD5 para proteção da senha ou token
        /// </summary>
        public static String GenerateHash(String inputString)
        {
            byte[] inputBuffer = Encoding.UTF8.GetBytes(inputString + salt);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] outputBuffer = md5.ComputeHash(inputBuffer);
            StringBuilder hash = new StringBuilder();
            foreach (Byte outputByte in outputBuffer)
            {
                hash.Append(outputByte.ToString("x2").ToUpper());
            }

            return hash.ToString();
        }
    }

}
