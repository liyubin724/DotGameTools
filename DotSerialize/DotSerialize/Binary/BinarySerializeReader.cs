using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dot.Serialize.Binary
{
    public static class BinarySerializeReader
    {
        public static T ReadFromBinary<T>(string filePath) where T:class
        {
            T data = null;
            if(File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fileStream = File.OpenRead(filePath);
                try
                {
                    data = (T)bf.Deserialize(fileStream);
                }
                catch
                {
                }
                fileStream.Close();
            }

            return data;
        }
    }
}
