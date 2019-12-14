using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dot.Serialize.Binary
{
    public static class BinaryWriter
    {
        public static void WriteToBinary<T>(string filePath,T data) where T:class
        {
            if(data == null)
            {
                return;
            }

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }else
            {
                var dirPath = Path.GetDirectoryName(filePath).Replace("\\", "/");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            bf.Serialize(file, data);
            file.Close();
        }
    }
}
