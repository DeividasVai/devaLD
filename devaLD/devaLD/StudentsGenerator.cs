using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace devaLD
{
    public class StudentsGenerator
    {
        public static readonly string AllStudentsFileName = "GeneratedStudents{#count}.txt";
        
        public static readonly string BadStudentsFileName = "Vargsiukai{#count}.txt";
        
        public static readonly string GoodStudentsFileName = "Saunuoliai{#count}.txt";

        private static Random _grades = new Random();

        #region ColumnNames

        private const string NameCol = "Vardas";
        
        private const string SurnameCol = "Pavardė";
        
        private const string Nd1Col = "ND1";
        
        private const string Nd2Col = "ND2";
        
        private const string Nd3Col = "ND3";
        
        private const string Nd4Col = "ND4";
        
        private const string Nd5Col = "ND5";
        
        private const string ExamCol = "Egzaminas";

        #endregion
        
        public string MainFileDirectory { get; set; }

        public static StudentsGenerator New()
        {
            return new StudentsGenerator();
        }
        
        public static StudentsGenerator New(string destinationDirectory)
        {
            return new StudentsGenerator() { MainFileDirectory = destinationDirectory };
        }

        public void AddFileHeaders(int tabulation, string fileName)
        {
            var fileFullPath = Path.Combine(MainFileDirectory, fileName); 
            
            var header = $"{TextFormatter.PrepareFileColumn(NameCol, tabulation)}" +
                         $"{TextFormatter.PrepareFileColumn(SurnameCol, tabulation)}" +
                         $"{TextFormatter.PrepareFileColumn(Nd1Col, 2)}" +
                         $"{TextFormatter.PrepareFileColumn(Nd2Col, 2)}" +
                         $"{TextFormatter.PrepareFileColumn(Nd3Col, 2)}" +
                         $"{TextFormatter.PrepareFileColumn(Nd4Col, 2)}" +
                         $"{TextFormatter.PrepareFileColumn(Nd5Col, 2)}" +
                         $"{TextFormatter.PrepareFileColumn(ExamCol, 2)}";
            
            if (!Directory.Exists(MainFileDirectory))
                Directory.CreateDirectory(MainFileDirectory);
            
            if (File.Exists(fileFullPath))
                File.Delete(fileFullPath);
            
            using var sw = new StreamWriter(fileFullPath);
            sw.WriteLine(header);
            sw.Close();
        }
        
        public void GenerateStudents(int count)
        {
            var longestName = $"{NameCol}{count}".Length;
            var longestSurname = $"{SurnameCol}{count}".Length;

            try
            {
                var minimumTabulation = longestName > longestSurname
                    ? Math.Ceiling(new decimal(longestName) / 4)
                        .ToString(CultureInfo.InvariantCulture)
                    : Math.Ceiling(new decimal(longestSurname) / 4)
                        .ToString(CultureInfo.InvariantCulture);
                var fileFullPath = Path.Combine(MainFileDirectory, AllStudentsFileName.Replace("{#count}", count.ToString()));
                
                AddFileHeaders(int.Parse(minimumTabulation) + 1, fileFullPath);

                using (var sw = new StreamWriter(fileFullPath, true, Encoding.UTF8))
                {
                    for (var row = 1; row <= count; row++)
                    {
                        var rowText = TextFormatter.PrepareFileColumn($"{NameCol}{row}", int.Parse(minimumTabulation) + 1);
                        rowText += TextFormatter.PrepareFileColumn($"{SurnameCol}{row}", int.Parse(minimumTabulation) + 1);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);
                        rowText += TextFormatter.PrepareFileColumn($"{_grades.Next(1, 11)}", 2);

                        sw.WriteLine(rowText);
                    }
                }
                SplitStudentsInGroups(count, int.Parse(minimumTabulation));

            }
            catch (Exception e)
            {
                e.Data.Add("StudentsGenerator", $"params: count = {count}, mainDirectory = {MainFileDirectory}");
                throw;
            }
        }

        public void SplitStudentsInGroups(int count, int tabulation)
        {
            var fileFullPath = Path.Combine(MainFileDirectory, AllStudentsFileName.Replace("{#count}", count.ToString()));
            var goodStudFullPath = Path.Combine(MainFileDirectory, GoodStudentsFileName.Replace("{#count}", count.ToString()));
            var badStudFullPath = Path.Combine(MainFileDirectory, BadStudentsFileName.Replace("{#count}", count.ToString()));
            
            if (File.Exists(goodStudFullPath))
                File.Delete(goodStudFullPath);
            
            if (File.Exists(badStudFullPath))
                File.Delete(badStudFullPath);
            
            AddFileHeaders(tabulation + 1, goodStudFullPath);
            AddFileHeaders(tabulation + 1, badStudFullPath);

            using (var sr = new StreamReader(fileFullPath))
            {
                using var swBad = new StreamWriter(badStudFullPath, true, Encoding.UTF8);
                using var swGood = new StreamWriter(goodStudFullPath, true, Encoding.UTF8);
                
                var line = string.Empty;
                var firstLine = true;
                var columns = new List<string>();
                while ((line = sr.ReadLine()) != null)
                {
                    if (firstLine)
                    {
                        columns = TextFormatter.FormatLineFromFileWithoutTabulation(line);
                        firstLine = false;
                        continue;
                    }

                    var student = Student.PrepareFromText(columns, TextFormatter.FormatLineFromFileWithoutTabulation(line));
                    if (student.Average > 5)
                        swGood.WriteLine(line);
                    else
                        swBad.WriteLine(line);

                    // FilesManager.AddToFile(student.Average > 5 ? goodStudFullPath : badStudFullPath, line);
                }
                
                swBad.Close();
                swGood.Close();
            }
        }
    }
}