using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        RemoveStudent,
        StudentImport,
        StudentsGenerator
    }
    
    public class StudentsManager
    {
        
        public List<Student> Students { get; set; }
        
        public bool Working { get; set; }
        
        public VisibleContent VisibleContent { get; set; }

        public StudentsManager()
        {
            Working = true;
            VisibleContent = VisibleContent.MainMenu;
            Students = new List<Student>();
        }

        public void Run()
        {
            while (Working)
            {
                try
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
                        case VisibleContent.StudentImport:
                            ImportStudentsView();
                            break;
                        case VisibleContent.StudentsGenerator:
                            GenerateStudentsView();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("--- ERROR ---\n\n");
                    foreach (DictionaryEntry entry in e.Data)
                    {
                        Console.Out.WriteLine($"{entry.Key} -- {entry.Value}");
                    }
                    
                    Console.WriteLine($"\nGenerated message:\n{e.Message}");
                    Console.WriteLine($"\nStack trace:\n{e.StackTrace}");

                    Console.WriteLine("\n\nPress enter to continue");
                    Console.ReadLine();
                }
            }
        }

        private void PrepareDummyStudents()
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

        private void MainMenuView()
        {
            Console.Title = $"Main Menu";
            Console.Clear();
            Console.WriteLine($"\n---MAIN MENU---");
            // Console.WriteLine($"Program automatically adds 3 students by default. Comment out line 33 in StudentsManager.cs to prevent it from doing it.\n");
            
            Console.WriteLine($"Write a number and press enter to change the view\n");
            Console.WriteLine($"1. Add student(-s)");
            Console.WriteLine($"2. Add homework grades for a student");
            Console.WriteLine($"3. Add an exam grade for a student");
            Console.WriteLine($"4. List students with their averages");
            Console.WriteLine($"5. Remove student(-s)");
            Console.WriteLine($"6. Import students from file");
            Console.WriteLine($"7. Generate students");
            Console.WriteLine($"0. Exit");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }
            
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
                case 6:
                    VisibleContent = VisibleContent.StudentImport;
                    break;
                case 7:
                    VisibleContent = VisibleContent.StudentsGenerator;
                    break;
                case 0:
                    Working = false;
                    break;
                default:
                    VisibleContent = VisibleContent.MainMenu;
                    break;
            }
        }

        private void AddStudentView()
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
            
            if (string.IsNullOrEmpty(input?.Split(" ")?[0]) || 
                string.IsNullOrEmpty(input?.Split(" ")?[1]))
                return;

            AddStudent(input?.Split(" ")?[0], input?.Split(" ")?[1]);
        }

        private void AddHomeworkGradesView()
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
            Console.WriteLine($"You have chosen: {Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} " +
                              $"{Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
            Console.WriteLine("Grade student's homework");
            var result = "";
            while ((result = Console.ReadLine()?.ToLower()) != "stop")
                if (double.TryParse(result, out var grade))
                    if (grade >= 1 && grade <= 10)
                        Students.FirstOrDefault(x => x.ID == studentId)?.HomeworkGrades.Add(grade);
            VisibleContent = VisibleContent.MainMenu;
        }

        private void AddExamGradesView()
        {
            Console.Clear();
            Console.Title = "---ADD EXAM GRADES---";
            Console.WriteLine("Choose a student's ID");
            ListOfStudentsView();
            int.TryParse(Console.ReadLine(), out var studentId);
            if (Students.All(x => x.ID != studentId))
                return;
            Console.WriteLine($"You have chosen: {Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} " +
                              $"{Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
            Console.WriteLine("Grade student's exam");

            while (!AddGrade(studentId, true))
            { /*Simply go till u have a grade*/ }
            VisibleContent = VisibleContent.MainMenu;
        }

        private void ListOfStudentsView(bool showAverages = false)
        {
            if (showAverages)
            {
                Console.Clear();
                Console.WriteLine("---LIST OF STUDENTS---");
                Console.WriteLine($"{TextFormatter.TabulatedText("Vardas")}" +
                                  $"{TextFormatter.TabulatedText("Pavarde")}" +
                                  $"{TextFormatter.TabulatedText("Galutinis (vid.)")}" +
                                  $"{TextFormatter.TabulatedText("Galutinis (med.)")}");
                foreach (var student in Students.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                {
                    var stAverage = Math.Round(student.Average, 2);
                    var studentGrades = new List<double>(student.HomeworkGrades) {student.ExamGrade};
                    studentGrades = studentGrades.OrderBy(x => x).ToList();
                    var gradeCount = studentGrades.Count;

                    var stMedian = gradeCount % 2 == 0
                        ? (studentGrades[gradeCount / 2] + studentGrades[(gradeCount / 2) + 1]) / 2
                        : studentGrades[gradeCount / 2];
                    
                    Console.Write($"{TextFormatter.TabulatedText(student.FirstName)}" +
                                  $"{TextFormatter.TabulatedText(student.LastName)}" +
                                  $"{TextFormatter.TabulatedText(stAverage.ToString(CultureInfo.InvariantCulture))}" +
                                  $"{TextFormatter.TabulatedText(stMedian.ToString(CultureInfo.InvariantCulture))}\n");
                }
                Console.WriteLine("Press enter to go back");
                Console.ReadLine();
                VisibleContent = VisibleContent.MainMenu;
            }
            else
            {
                foreach (var student in Students)
                {
                    Console.Write($"{TextFormatter.TabulatedText(student.ID.ToString())}{TextFormatter.TabulatedText(student.FirstName)}{TextFormatter.TabulatedText(student.LastName)}\n");
                }
            }
        }

        private void RemoveStudentView()
        {
            Console.Clear();
            Console.Title = "---REMOVE STUDENT---";
            Console.WriteLine("Choose a student's ID");
            Console.WriteLine("Type 'STOP' to return to main menu");
            ListOfStudentsView();
            var result = Console.ReadLine();
            if (result?.ToUpper() == "STOP")
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }
            int.TryParse(result, out var studentId);
            if (Students.All(x => x.ID != studentId))
                return;
            Console.WriteLine($"Are you sure you want to remove: " +
                              $"{Students.FirstOrDefault(x => x.ID == studentId)?.FirstName} " +
                              $"{Students.FirstOrDefault(x => x.ID == studentId)?.LastName}");
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

        private void ImportStudentsView()
        {
            try
            {
                var directoryInfo = Directory
                    .GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent;
                if (directoryInfo == null) return;
                
                var workingDirectory = directoryInfo.FullName;
                var dataFolder = Path.Combine(workingDirectory, "data");
                var studentsFile = Path.Combine(dataFolder, "students.txt");


                var studentsFileLines = File.ReadLines(studentsFile).ToList();
                var firstRow = true;
                var columns = new List<string>();
                foreach (var line in studentsFileLines)
                {
                    if (firstRow)
                    {
                        columns = TextFormatter.FormatLineFromFile(line);
                        firstRow = false;
                        continue;
                    }

                    var stColumns = TextFormatter.FormatLineFromFile(line);
                    
                    var studId = AddStudent(stColumns[0], stColumns[1]);
                    if (studId == -1) continue;
                    foreach (var column in columns.Where(x => x.Contains("ND")))
                    {
                        AddGrade(studId, false, double.Parse(stColumns[columns.IndexOf(column)]));
                    }

                    var examIndex = columns.IndexOf("Egzaminas");
                    AddGrade(studId, true, double.Parse(stColumns[examIndex]));
                }
            }
            catch (Exception e)
            {
                e.Data.Add("ImportStudents",
                    "Error possibly due to the directory not having the correct document, or it being incorrect itself");
                var directoryInfo = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent;
                if (directoryInfo != null)
                    e.Data.Add("Requirements",
                        $"a folder called 'data' withing working directory (currently {directoryInfo.FullName}) with a file" +
                        $" called 'students.txt'");
                else
                    e.Data.Add("Requirements",
                        $"a folder called 'data' withing working directory  with a file called 'students.txt'");
                throw;
            }
            finally
            {
                VisibleContent = VisibleContent.MainMenu;                
            }

        }

        private void GenerateStudentsView()
        {
            Console.Clear();
            Console.Title = "---GENERATE STUDENTS---";
            Console.WriteLine("Choose a number of student's to generate\nType 'STOP' to exit");
            
            Console.WriteLine("1. 1 000");
            Console.WriteLine("2. 10 000");
            Console.WriteLine("3. 100 000");
            Console.WriteLine("4. 1 000 000");
            Console.WriteLine("5. 10 000 000");

            var select = Console.ReadLine();
            if (select?.ToLower() == "stop")
            {
                VisibleContent = VisibleContent.MainMenu;
                return;
            }
            
            if (!int.TryParse(select, out var selectedNo))
                GenerateStudentsView();

            var directoryInfo = Directory
                .GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent;
            if (directoryInfo == null) return;
            var studGenerator = StudentsGenerator.New(Path.Combine(directoryInfo.FullName, "data"));
            
            switch (selectedNo)
            {
                case 1:
                    studGenerator.GenerateStudents(1000);
                    break;
                case 2:
                    studGenerator.GenerateStudents(10000);
                    break;
                case 3:
                    studGenerator.GenerateStudents(100000);
                    break;
                case 4:
                    studGenerator.GenerateStudents(1000000);
                    break;
                case 5:
                    studGenerator.GenerateStudents(10000000);
                    break;
            }

            VisibleContent = VisibleContent.MainMenu;
        }


        private bool IsAcceptOrDecline(string response, out bool answer)
        {
            answer = response.ToUpper() == "Y";
            return response.ToUpper() == "Y" || response.ToUpper() == "N";
        }
        
        private bool AddGrade(int id, bool isExamGrade = false, double grade = double.NaN)
        {
            if (double.IsNaN(grade))
            {
                double.TryParse(Console.ReadLine(), out var inputGrade);
                if (inputGrade < 1 || inputGrade > 10)
                    return false;
                grade = inputGrade;
            }

            if (!isExamGrade)
                Students.FirstOrDefault(x => x.ID == id)?.HomeworkGrades.Add(grade);
            else
            {
                var student = Students.FirstOrDefault(x => x.ID == id);
                if (student != null) student.ExamGrade = grade;
            }

            return true;
        }


        private int AddStudent(string firstName, string lastName)
        {
            if (Students.Any(x => x.FirstName == firstName && x.LastName == lastName))
                return -1;
            var student = Student.New(!Students.Any() ? 1 : Students.OrderBy(x => x.ID).Last().ID + 1);
            student.FirstName = firstName;
            student.LastName = lastName;
            Students.Add(student);
            return student.ID;
        }
    }
}