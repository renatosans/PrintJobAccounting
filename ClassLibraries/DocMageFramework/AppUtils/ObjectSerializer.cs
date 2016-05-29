using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;


namespace DocMageFramework.AppUtils
{
    /// <summary>
    /// Classe utilitária que possui métodos para serializar/deserializar objetos
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class ObjectSerializer
    {
        public static String SerializeObjectToString(Object obj)
        {
            if (obj == null) return null;

            Byte[] buffer = SerializeObjectToArray(obj);
            String serializedObject = Encoding.UTF8.GetString(buffer);

            return serializedObject;
        }

        public static Byte[] SerializeObjectToArray(Object obj)
        {
            if (obj == null) return null;

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(memoryStream, obj);
            Byte[] serializedObject = memoryStream.ToArray();

            return serializedObject;
        }

        public static Object DeserializeObject(String serializedObject, Type objectType)
        {
            if (String.IsNullOrEmpty(serializedObject)) return null;

            Object deserializedObject;
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject));
            XmlSerializer serializer = new XmlSerializer(objectType);
            try
            {
                // Tenta recuperar o objeto
                deserializedObject = serializer.Deserialize(memoryStream);
            }
            catch
            {
                // Caso falhe retorna um referência nula (o objeto não foi recuperado)
                return null;
            }

            return deserializedObject;
        }
    }

}
