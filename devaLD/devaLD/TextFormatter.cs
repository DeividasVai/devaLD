using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace devaLD
{
    public static class TextFormatter
    {
        private static readonly int TabulationSize = 24;
        
        public static string TabulatedText(string text)
        {
            while (text.Length != TabulationSize)
                text += " ";
            return text;
        }

        public static string PrepareFileColumn(string text, int tabulation)
        {
            while (text.Length < 4*tabulation)
                text += " ";

            return text;
        }
        
        public static List<string> FormatLineFromFile(string line)
        {
            var columns = line.Split("\t").ToList();
            columns.RemoveAll(s => s == "");
            return columns;
        }
        
        public static List<string> FormatLineFromFileWithoutTabulation(string line)
        {
            var columns = line.Split(" ").ToList();
            columns.RemoveAll(s => s == "");
            return columns;
        }
    }
}