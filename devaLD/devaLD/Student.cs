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
    }
}