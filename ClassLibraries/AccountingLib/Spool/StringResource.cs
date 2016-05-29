using System;
using System.IO;


namespace AccountingLib.Spool
{
    public static class StringResource
    {
        /// <summary>
        /// Lê uma string (null terminated) a partir do stream. Não retorna a posição inicial de leitura
        /// </summary>
        public static Char[] Get(BinaryReader reader)
        {
            // Marca a posição inicial de leitura da stream
            Int64 startPos = reader.BaseStream.Position;

            // Verifica o tamanho da string
            int size = 0;
            Char nextChar = reader.ReadChar();
            while ((nextChar != 0) && (reader.BaseStream.Position <= reader.BaseStream.Length))
            {
                size++;
                nextChar = reader.ReadChar();
            }

            // Faz a leitura
            reader.BaseStream.Seek(startPos, SeekOrigin.Begin);
            Char[] stringResource = reader.ReadChars(size);

            return stringResource;
        }
    }

}
