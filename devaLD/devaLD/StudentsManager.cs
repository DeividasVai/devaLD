using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace devaLD
{
    public enum VisibleContent
    {
        MainMenu,
        AddStudent,
        AddGradesHomework,
        AddGradesExam,
        ListOfStudents,
        RemoveStudent
    }
    
    public class StudentsManager
    {
        private int _tabulationSize = 24;
        
        public List<Student> Students { get; set; }
        
        public bool Working { get; set; }
        
        public VisibleContent VisibleContent { get; set; }

        public StudentsManager()
        {
            Working = true;
            VisibleContent = VisibleContent.MainMenu;
            Students = new List<Student>();
            PrepareDummyStudents();
        }

        public void Run()
        {
            while (Working)
            {
                switch (VisibleContent)
                {
                    case VisibleContent.MainMenu:
                        MainMenuView();
                        break;
                    case VisibleContent.AddStudent:
                        AddStudentView();
                        break;
                    case VisibleContent.AddGradesHomework:
                        AddHomeworkGradesView();
                        break;
                    case VisibleContent.AddGradesExam:
                        AddExamGradesView();
                        break;
                    case VisibleContent.ListOfStudents:
                        ListOfStudentsView(true);
                        break;
                    case VisibleContent.RemoveStudent:
                        RemoveStudentView();
                        break;
                    default:
                        break;
                }
            }
        }

        public void PrepareDummyStudents()
        {
            Students = Students ?? new List<Student>();
            var student = Student.New(!Students.Any() ? 1 : Students.Last().ID + 1);
            student.FirstName = "Deividas";
            student.LastName = "Deividavicius";
            Students.Add(student);

            student = Student.New(!Students.Any() ? 1 : Students.Last().ID + 1);
            student.FirstName = "Tomas";
            student.LastName = "Tomavicius";
            Students.Add(student);
            
            student = Student.New(!Students.Any() ? 1 : Students.Last().ID + 1);
            student.FirstName = "Raminta";
            student.LastName = "Ramintiene";
            Students.Add(student);
        }
        
        public void MainMenuView()
        {
            Console.Title = $"Main Menu";
            Console.Clear();
            Console.WriteLine($"\n---MAIN MENU---");
            Console.WriteLine($"Program automatically adds 3 students by default. Comment out line 33 in StudentsManager.cs to prevent it from doing it.\n");
            Console.WriteLine($"Write a number and press enter to change the view\n");
            Console.WriteLine($"1. Add student(-s)");
            Console.WriteLine($"2. Add homework grades for a student");
            Console.WriteLine($"3. Add an exam grade for a student");
            Console.WriteLine($"4. List students with their averages");
            Console.WriteLine($"5. Remove student(-s)");
            Console.WriteLine($"0. Exit");

            var choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    VisibleContent = VisibleContent.AddStudent;
                    break;
                case 2:
                    VisibleContent = VisibleContent.AddGradesHomework;
                    break;
                case 3:
                    VisibleContent = VisibleContent.AddGradesExam;
                    break;
                case 4:
                    VisibleContent = VisibleContent.ListOfStudents;
                    break;
                case 5:
                    VisibleContent = VisibleContent.RemoveStudent;
                    break;
                case 0:
                    Working = false;
                    break;
                default:
                    VisibleContent = VisibleContent.MainMenu;
                    break;
            }
        }

        public void AddStudentView()
        {
            Console.Title = "Add student";
            Console.Clear();
            Console.WriteLine($"\n---ADD STUDENT---");
            Console.WriteLine($"Type in a student's name and surname to add it to the list\nTo stop - type 'STOP'\n");

            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return;

            if (input?.ToUpper() == "STOP")
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }

            var inputSplit = input?.Split(" ");
            if (inputSplit.Length <= 1)
                return;
            
            var firstName = input?.Split(" ")?[0];
            var lastName = input?.Split(" ")?[1];

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return;

            var student = Student.New(!Students.Any() ? 1 : Students.Last().ID + 1);
            student.FirstName = firstName;
            student.LastName = lastName;
            Students.Add(student);
        }

        public void AddHomeworkGradesView()
        {
            Console.Clear();
            Console.Title = "---ADD HOMEWORK GRADES---";
            Console.WriteLine("Choose a student's ID");
            Console.WriteLine("Type 'STOP' to return to main menu");
            ListOfStudentsView();
            var response = Console.ReadLine();
            if (response?.ToUpper() == "STOP")
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }
            int.TryParse(response, out var studentId);
            if (Students.All(x => x.ID != studentId))
                return;
            Console.WriteLine($"You have chosen: {Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} {Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
            Console.WriteLine("Grade student's homework");
            var result = "";
            while ((result = Console.ReadLine()?.ToLower()) != "stop")
                if (double.TryParse(result, out var grade))
                    if (grade >= 1 && grade <= 10)
                        Students.FirstOrDefault(x => x.ID == studentId)?.HomeworkGrades.Add(grade);
            VisibleContent = VisibleContent.MainMenu;
        }

        public void AddExamGradesView()
        {
            Console.Clear();
            Console.Title = "---ADD EXAM GRADES---";
            Console.WriteLine("Choose a student's ID");
            ListOfStudentsView();
            int.TryParse(Console.ReadLine(), out var studentId);
            if (Students.All(x => x.ID != studentId))
                return;
            Console.WriteLine($"You have chosen: {Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} {Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
            Console.WriteLine("Grade student's exam");

            double grade;
            while (double.IsNaN(grade = AddGrade()))
            { /*Simply go till u have a grade*/ }
            var first = Students.FirstOrDefault(x => x.ID == studentId);
            if (first != null) first.ExamGrade = grade;
            VisibleContent = VisibleContent.MainMenu;
        }
        
        public void ListOfStudentsView(bool showAverages = false)
        {
            if (showAverages)
            {
                Console.Clear();
                Console.WriteLine("---LIST OF STUDENTS---");
                Console.WriteLine($"{TabulatedText("Vardas")}{TabulatedText("Pavarde")}{TabulatedText("Galutinis (vid.)")}");
                foreach (var student in Students)
                {
                    Console.Write($"{TabulatedText(student.FirstName)}{TabulatedText(student.LastName)}{TabulatedText(student.Average.ToString(CultureInfo.InvariantCulture))}\n");
                }
                Console.WriteLine("Press enter to go back");
                Console.ReadLine();
                VisibleContent = VisibleContent.MainMenu;
            }
            else
            {
                foreach (var student in Students)
                {
                    Console.Write($"{TabulatedText(student.ID.ToString())}{TabulatedText(student.FirstName)}{TabulatedText(student.LastName)}\n");
                }
            }
        }

        public void RemoveStudentView()
        {
            Console.Clear();
            Console.Title = "---REMOVE STUDENT---";
            Console.WriteLine("Choose a student's ID");
            Console.WriteLine("Type 'STOP' to return to main menu");
            ListOfStudentsView();
            var result = Console.ReadLine();
            if (result.ToUpper() == "STOP")
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }
            int.TryParse(result, out var studentId);
            if (Students.All(x => x.ID != studentId))
                return;
            Console.WriteLine($"Are you sure you want to remove: {Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} {Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
            Console.WriteLine("Y - yes\nN - no");
            var delete = false;
            while (true)
            {
                if (!IsAcceptOrDecline(Console.ReadLine(), out var answer)) continue;
                delete = answer;
                break;
            }

            if (delete)
                Students.Remove(Students.FirstOrDefault(x => x.ID == studentId));
            VisibleContent = VisibleContent.MainMenu;
        }

        private bool IsAcceptOrDecline(string response, out bool answer)
        {
            answer = response.ToUpper() == "Y";
            return response.ToUpper() == "Y" || response.ToUpper() == "N";
        }
        
        private double AddGrade()
        {
            double.TryParse(Console.ReadLine(), out var grade);
            return grade < 1 || grade > 10 ? double.NaN : grade;
        }

        private string TabulatedText(string text)
        {
            while (text.Length != _tabulationSize)
                text += " ";
            return text;
        }
    }
}