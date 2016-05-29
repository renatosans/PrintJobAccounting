using System;


namespace AccountingInstaller.DataManipulation
{
    public class DBObject
    {
        public int id;

        public String name;


        public DBObject()
        {
        }

        public DBObject(int id, String name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

}
