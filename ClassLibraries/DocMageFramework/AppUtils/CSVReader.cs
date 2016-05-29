using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using DocMageFramework.FileUtils;


namespace DocMageFramework.AppUtils
{
    public class CSVReader
    {
        private String filename;

        private IListener listener;


        /// <summary>
        /// Lê um arquivo .CSV e gera uma tabela em memória (DataTable) com o seu conteúdo
        /// </summary>
        public CSVReader(String filename, IListener listener)
        {
            this.filename = filename;
            this.listener = listener;
        }

        /// <summary>
        /// Remove as aspas duplas de uma string em uma das colunas do .CSV
        /// </summary>
        private String RemoveQuotes(String text)
        {
            // Texto vazio ou de 1 caracter(não possui aspas), retorna o texto original
            if (String.IsNullOrEmpty(text) || (text.Length == 1))
                return text;

            // Texto sem abre aspas/fecha aspas, retorna o texto original
            if ((text[0] != '"') || (text[text.Length - 1] != '"'))
                return text;

            // Remove as aspas
            String processedText = text.Substring(1, text.Length - 2);
            return processedText;
        }
        
        /// <summary>
        /// Verifica se a linha de valores do .CSV está no formato correto e caso não esteja
        /// corrige as falhas. Retorna a linha corrigida
        /// </summary>
        private String[] FixRow(String row, String[] values, DataTable table)
        {
            if (values.Length == table.Columns.Count)
                return values; // Caso a quantidade de valores esteja correta retorna

            // Faz o ajuste pois a quantidade de campos separados por virgula difere da quantidade de colunas
            List<String> fixedValues = new List<String>();

            String currentToken = "";
            Boolean quotedToken = false;
            foreach (Char ch in row)
            {
                if ((ch != '"') && (ch != ',')) currentToken += ch;
                if (ch == '"') quotedToken = !quotedToken;
                if (ch == ',')
                {
                    if (quotedToken)
                    {
                        currentToken += ch;
                        continue;
                    }
                    fixedValues.Add(currentToken);
                    currentToken = "";
                }
            }
            fixedValues.Add(currentToken);

            return fixedValues.ToArray();
        }

        /// <summary>
        /// Move o cursor para a primeira linha (Pula os comentários no inicio do arquivo)
        /// </summary>
        private void MoveCursor(TextReader reader, int startLine)
        {
            int lineIndex = 0;
            while (lineIndex < startLine)
            {
                reader.ReadLine();
                lineIndex++;
            }
        }

        /// <summary>
        /// Obtem o cabeçalho do .CSV e cria as colunas correspondentes no DataTable
        /// </summary>
        private void GetColumns(TextReader reader, DataTable table)
        {
            String line = reader.ReadLine();
            String[] fields = line.Split(new Char[] { ',' });
            foreach (String field in fields)
            {
                table.Columns.Add(new DataColumn(field.Trim(), Type.GetType("System.String")));
            }
        }

        /// <summary>
        /// Obtem o conteúdo do .CSV e cria as linhas correspondentes no DataTable
        /// </summary>
        private void GetRows(TextReader reader, DataTable table)
        {
            DataRow newRow = null;
            String line = "";

            while (line != null)
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    newRow = table.NewRow();
                    // Remove a virgula no final da linha caso exista
                    if (line[line.Length - 1] == ',') line = line.Remove(line.Length-1, 1);
                    String[] values = line.Split(new Char[] { ',' });
                    values = FixRow(line, values, table);
                    for (int ndx = 0; ndx < table.Columns.Count; ndx++)
                    {
                        newRow[ndx] = RemoveQuotes(values[ndx]);
                    }
                    table.Rows.Add(newRow);
                }
            }
        }

        /// <summary>
        /// Faz o parse do arquivo e gera o DataTable, evita a concorrencia ao arquivo sendo lido
        /// </summary>
        public DataTable Read(int startLine)
        {
            DataTable returnTable = new DataTable();

            try
            {
                // Abre uma copia do arquivo para evitar erros caso ele esteja aberto em
                // outro processo para gravação/append de novos registros
                String targetDir = Path.GetDirectoryName(filename);
                String csvTimeStamp = DateTime.Now.Ticks.ToString();
                String csvClone = Path.Combine(targetDir, "Clone" + csvTimeStamp + ".CSV");
                File.Copy(filename, csvClone, true);
                TextReader reader = new StreamReader(csvClone);

                // Pula os comentários no inicio do arquivo
                MoveCursor(reader, startLine);

                // Carrega os nomes dos campos
                GetColumns(reader, returnTable);
                
                // Carrega os valores dos campos para cada linha
                GetRows(reader, returnTable);

                reader.Close();
                File.Delete(csvClone);
            }
            catch (Exception exc)
            {
                NotifyListener(exc);
            }

            return returnTable;
        }

        /// <summary>
        /// Faz o parse do arquivo e gera o DataTable. Por default considera arquivos .CSV com
        /// uma linha de comentário (linha 0) e cabeçalho na linha seguinte (linha 1)
        /// </summary>
        public DataTable Read()
        {
            return Read(1); // pula os comentários e começa na linha 1
        }

        private void NotifyListener(Object obj)
        {
            if (listener != null)
                listener.NotifyObject(obj);
        }
    }

}
