using System;
using System.Data;


namespace DocMageFramework.DataManipulation
{
    public class ProcedureParam
    {
        public String name;

        public SqlDbType type;

        public int size;

        public Object value;


        public ProcedureParam(String name, SqlDbType type, int size, Object value)
        {
            this.name = name;
            this.type = type;
            this.size = size;
            this.value = value;
        }
    }

}
