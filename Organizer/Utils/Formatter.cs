using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Organizer.Utils
{
    internal static class Formatter
    {
        public static TSettings GetSettings<TSettings>(string fileName) where TSettings : class
        {
            XmlSerializer formatter = new XmlSerializer(typeof(TSettings));

            using (FileStream fs = File.Open(fileName, FileMode.Open))
            {
                return formatter.Deserialize(fs) as TSettings;
            }
        }
        public static void SaveSettings<TSettings>(TSettings settings, string fileName) where TSettings : class
        {
            XmlSerializer formatter = new XmlSerializer(typeof(TSettings));

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, settings);
            }
        }
        public static void SaveTextFile(string path, string text)
        {
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }
        }
        public static string GetFileContent(string path)
        {
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                return Encoding.Default.GetString(array);
            }
        }
    }
}
