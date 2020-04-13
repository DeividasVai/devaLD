using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        
        public static void OrderStudentsWithList(string directory, string fileName, int count)
        {
            var path = Path.Combine(directory, fileName.Replace("{#count}", count.ToString()));
            using var sr = new StreamReader(path);
            var header = string.Empty;
            var firstLine = true;
            
            var line = string.Empty;
            var students = new List<Student>();
            var columns = new List<string>();
            
            while ((line = sr.ReadLine()) != null)
            {
                if (firstLine)
                {
                    columns = TextFormatter.FormatLineFromFileWithoutTabulation(line);
                    header = line;
                    firstLine = false;
                    continue;
                }
                students.Add(Student.PrepareFromText(columns, TextFormatter.FormatLineFromFileWithoutTabulation(line)));
            }
            sr.Close();

            var sortPath = Path.Combine(directory, fileName.Replace(".txt", "_orderedWithQueue.txt")
                .Replace("{#count}", count.ToString()));
            
            if (File.Exists(sortPath))
                File.Delete(sortPath);
            
            using var sw = new StreamWriter(sortPath, true, Encoding.UTF8);

            
            var minimumTabulation = students.Last().FirstName.Length > students.Last().LastName.Length
                ? Math.Ceiling(new decimal(students.Last().FirstName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture)
                : Math.Ceiling(new decimal(students.Last().LastName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture);
            sw.WriteLine(header);
            foreach (var student in students.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                var rowText = TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[0]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[1]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[2]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[3]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[4]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.ExamGrade}", 2);
                sw.WriteLine(rowText);
            }
            sw.Close();
        }

        public static void OrderStudentsWithLinkedList(string directory, string fileName, int count)
        {
            var path = Path.Combine(directory, fileName.Replace("{#count}", count.ToString()));
            using var sr = new StreamReader(path);
            var header = string.Empty;
            var firstLine = true;
            
            var line = string.Empty;
            var students = new LinkedList<Student>();
            var columns = new List<string>();
            
            while ((line = sr.ReadLine()) != null)
            {
                if (firstLine)
                {
                    columns = TextFormatter.FormatLineFromFileWithoutTabulation(line);
                    header = line;
                    firstLine = false;
                    continue;
                }
                students.AddFirst(Student.PrepareFromText(columns, TextFormatter.FormatLineFromFileWithoutTabulation(line)));
            }
            sr.Close();
            

            var sortPath = Path.Combine(directory, fileName.Replace(".txt", "_orderedWithQueue.txt")
                .Replace("{#count}", count.ToString()));
            
            if (File.Exists(sortPath))
                File.Delete(sortPath);
            
            using var sw = new StreamWriter(sortPath, true, Encoding.UTF8);

            
            var minimumTabulation = students.Last().FirstName.Length > students.Last().LastName.Length
                ? Math.Ceiling(new decimal(students.Last().FirstName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture)
                : Math.Ceiling(new decimal(students.Last().LastName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture);
            sw.WriteLine(header);
            foreach (var student in students.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                var rowText = TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[0]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[1]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[2]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[3]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[4]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.ExamGrade}", 2);
                sw.WriteLine(rowText);
            }
            sw.Close();
        }

        public static void OrderStudentsWithQueue(string directory, string fileName, int count)
        {
            var path = Path.Combine(directory, fileName.Replace("{#count}", count.ToString()));
            var header = string.Empty;
            var firstLine = true;
            
            var line = string.Empty;
            var students = new Queue<Student>();
            var columns = new List<string>();
            
            // read and store
            using var sr = new StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                if (firstLine)
                {
                    columns = TextFormatter.FormatLineFromFileWithoutTabulation(line);
                    header = line;
                    firstLine = false;
                    continue;
                }
                students.Enqueue(Student.PrepareFromText(columns, TextFormatter.FormatLineFromFileWithoutTabulation(line)));
            }
            sr.Close();

            var sortPath = Path.Combine(directory, fileName.Replace(".txt", "_orderedWithQueue.txt")
                .Replace("{#count}", count.ToString()));
            
            if (File.Exists(sortPath))
                File.Delete(sortPath);
            
            using var sw = new StreamWriter(sortPath, true, Encoding.UTF8);

            
            // sort and write
            var minimumTabulation = students.Last().FirstName.Length > students.Last().LastName.Length
                ? Math.Ceiling(new decimal(students.Last().FirstName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture)
                : Math.Ceiling(new decimal(students.Last().LastName.Length) / 4)
                    .ToString(CultureInfo.InvariantCulture);
            sw.WriteLine(header);
            students = new Queue<Student>(students.OrderBy(x => x.FirstName).ThenBy(x => x.LastName));
            while (students.TryDequeue(out var student))
            {
                var rowText = TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.FirstName}", int.Parse(minimumTabulation) + 1);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[0]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[1]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[2]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[3]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.HomeworkGrades[4]}", 2);
                rowText += TextFormatter.PrepareFileColumn($"{student.ExamGrade}", 2);
                sw.WriteLine(rowText);
            }
            
            sw.Close();
        }
    }
}