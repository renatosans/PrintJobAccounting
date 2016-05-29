using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;


namespace DocMageFramework.DataManipulation
{
    public class ProcedureCall
    {
        public List<ProcedureParam> parameters;

        private String procedureName;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public ProcedureCall(String procedureName, SqlConnection sqlConnection)
        {
            parameters = new List<ProcedureParam>();

            this.procedureName = procedureName;
            this.sqlConnection = sqlConnection;
        }

        public void Execute(Boolean retrieveResultset)
        {
            // Convenções: 
            // Todos os parâmetros da procedure devem ser de entrada (param direction = input)
            // A procedure não deve retornar mais de um resultset

            sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = procedureName;

            sqlCommand.Parameters.Clear();
            foreach (ProcedureParam procedureParam in parameters)
            {
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = procedureParam.name;
                sqlParameter.SqlDbType = procedureParam.type;
                sqlParameter.Direction = ParameterDirection.Input;
                sqlParameter.Size = procedureParam.size;
                sqlParameter.Value = procedureParam.value;

                sqlCommand.Parameters.Add(sqlParameter);
            }

            if (retrieveResultset)
                sqlDataReader = sqlCommand.ExecuteReader();
            else
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

        public int? ExtractFromResultset()
        {
            if (!sqlDataReader.Read())
                return null;

            if (sqlDataReader.FieldCount != 1)
                return null;

            int returnValue = int.Parse(sqlDataReader[0].ToString());
            sqlDataReader.Close();

            return returnValue;
        }
    }

}
