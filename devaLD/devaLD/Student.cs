using System;
using System.Collections.Generic;
using System.Linq;

namespace devaLD
{
    public class Student
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<double> HomeworkGrades { get; set; }
        
        public double ExamGrade { get; set; }

        public double Average => HomeworkGrades.Any()
            ? (HomeworkGrades.Sum() + ExamGrade) / (HomeworkGrades.Count + 1)
            : ExamGrade;
        
        public Student()
        {
            HomeworkGrades = new List<double>();
        }

        public static Student New(int id)
        {
            return new Student() { ID = id };
        }

        public static Student PrepareFromText(List<string> columns, List<string> line)
        {
            var stud = new Student {FirstName = line[0], LastName = line[1]};

            foreach (var column in columns.Where(x => x.Contains("ND")))
            {
                stud.HomeworkGrades.Add(double.Parse(line[columns.IndexOf(column)]));
            }

            var examIndex = columns.IndexOf("Egzaminas");
            stud.ExamGrade = double.Parse(line[examIndex]);
            
            return stud;
        }
    }
}