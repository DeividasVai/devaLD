using System.IO;
using System.Text;

namespace devaLD
{
    public class FilesManager
    {
        public static void AddToFile(string path, string text)
        {
            // File.AppendAllText(path, text);
            var sw = new StreamWriter(path, true, Encoding.UTF8);
            sw.WriteLine(text);
            sw.Close();
        }
    }
}