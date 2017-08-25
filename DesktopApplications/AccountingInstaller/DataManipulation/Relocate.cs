using System;
using System.Data.SqlClient;


namespace AccountingInstaller.DataManipulation
{
    public class Relocate
    {
        public String databaseName;

        public SqlConnection sqlConnection;

        public Relocate(String databaseName)
        {
            this.databaseName = databaseName;
        }
    }

}
