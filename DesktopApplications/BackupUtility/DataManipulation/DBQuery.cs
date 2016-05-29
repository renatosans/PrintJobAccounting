using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;


namespace DataManipulation
{
    public class DBQuery
    {
        private String query;

        private SqlConnection sqlConnection;

        private SqlCommand sqlCommand;

        private SqlDataReader sqlDataReader;

        public String Query
        {
            get { return query; }
            set { query = value; }
        }


        public DBQuery(SqlConnection sqlConnection)
        {
            this.query = "";
            this.sqlConnection = sqlConnection;
        }

        public DBQuery(String query, SqlConnection sqlConnection)
        {
            this.query = query;
            this.sqlConnection = sqlConnection;
        }
        
        public void Execute(Boolean retrieveResultset)
        {
            if (String.IsNullOrEmpty(query))
                throw new Exception("A query não pode ser uma string vazia.");

            // Convenções: 
            // A query não deve retornar mais de um resultset
            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.Clear();

            // Armazena o retorno caso solicitado
            if (retrieveResultset)
            {
                sqlDataReader = sqlCommand.ExecuteReader();
                return;
            }
            
            // Caso não tenha sido solicitado retorno apenas executa
            sqlCommand.ExecuteNonQuery();
        }

        public List<Object> ExtractFromResultset(Type objectType)
        {
            // Convenções:
            // As colunas do resultset devem ser equivalentes aos campos publicos de objectType
            // Somente os campos publicos não estáticos de objectType serão considerados

            List<Object> returnList = new List<Object>();
            FieldInfo[] info = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Object listItem;

            while (sqlDataReader.Read())
            {
                listItem = Activator.CreateInstance(objectType);
                foreach (FieldInfo fieldInfo in info)
                {
                    if (sqlDataReader[fieldInfo.Name] is DBNull)
                        fieldInfo.SetValue(listItem, null);
                    else
                        fieldInfo.SetValue(listItem, sqlDataReader[fieldInfo.Name]);
                }
                returnList.Add(listItem);
            }
            sqlDataReader.Close();

            return returnList;
        }

        public DataTable ExtractFromResultset(Type objectType, String tableName)
        {
            // Convenções:
            // As colunas do resultset devem ser equivalentes aos campos publicos de objectType
            // Somente os campos publicos não estáticos de objectType serão considerados

            DataTable returnTable;
            DataRow newRow;
            FieldInfo[] info;

            returnTable = new DataTable(tableName);
            info = objectType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in info)
            {
                returnTable.Columns.Add(new DataColumn(fieldInfo.Name, fieldInfo.FieldType));
            }

            while (sqlDataReader.Read())
            {
                newRow = returnTable.NewRow();
                foreach (FieldInfo fieldInfo in info)
                {
                    newRow[fieldInfo.Name] = sqlDataReader[fieldInfo.Name];
                }
                returnTable.Rows.Add(newRow);
            }
            sqlDataReader.Close();

            return returnTable;
        }

        public List<Object> ExtractFromResultset(String[] fieldNames)
        {
            List<Object> returnList = new List<Object>();
            while (sqlDataReader.Read())
            {
                Object[] fieldValues = new Object[fieldNames.Length];
                for (int index = 0; index < fieldNames.Length; index++)
                {
                    String fieldName = fieldNames[index];

                    if (sqlDataReader[fieldName] is DBNull)
                        fieldValues[index] = null;
                    else
                        fieldValues[index] = sqlDataReader[fieldName];
                }
                returnList.Add(fieldValues);
            }
            sqlDataReader.Close();

            return returnList;
        }

        public int? ExtractFromResultset()
        {
            if (!sqlDataReader.Read())
                return null;

            if (sqlDataReader.FieldCount != 1)
                return null;

            if (sqlDataReader[0] is DBNull)
            {
                sqlDataReader.Close();
                return null;
            }

            int returnValue = int.Parse(sqlDataReader[0].ToString());
            sqlDataReader.Close();

            return returnValue;
        }
    }

}
