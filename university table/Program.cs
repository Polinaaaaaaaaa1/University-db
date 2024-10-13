using System;
using System.Data.SQLite;
using university_table;
using university_table.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=university.db;Version=3;";
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var students=new Students(connection);
            var teachers=new Teachers(connection);
            var courses=new Courses(connection);
            var grades=new Grades(connection);
            var exams=new Exams(connection);

            Start();


            void Start()
            {
                if (!DatabaseExists())
                {
                    Console.WriteLine("База данных не найдена. Создание таблиц...");
                    CreateTables();
                }

                while (true)
                {
                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1. Добавить");
                    Console.WriteLine("2. Изменить");
                    Console.WriteLine("3. Удалить");
                    Console.WriteLine("4. Получить список студентов по факультету");
                    Console.WriteLine("5. Получить список курсов преподавателя");
                    Console.WriteLine("6. Получить список студентов на курсе");
                    Console.WriteLine("7. Получить оценки студентов по курсу");
                    Console.WriteLine("8. Средний балл студента по курсу");
                    Console.WriteLine("9. Средний балл студента в целом");
                    Console.WriteLine("10. Средний балл по факультету");
                    Console.WriteLine("0. Выйти");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.WriteLine("Добавить:");
                        Console.WriteLine("1.1 - Студента");
                        Console.WriteLine("1.2 - Преподавателя");
                        Console.WriteLine("1.3 - Курс");
                        Console.WriteLine("1.4 - Экзамен");
                        Console.WriteLine("1.5 - Оценку");
                        string addChoice = Console.ReadLine();

                        if (addChoice == "1.1")
                        {
                            Console.WriteLine("Введите имя, фамилию, факультет и дату рождения студента:");
                            students.AddStudent(Console.ReadLine(), Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                        }
                        else if (addChoice == "1.2")
                        {
                            Console.WriteLine("Введите имя, фамилию и факультет преподавателя:");
                            teachers.AddTeacher(Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                        }
                        else if (addChoice == "1.3")
                        {
                            Console.WriteLine("Введите название, описание курса и id преподавателя:");
                            courses.AddCourses(Console.ReadLine(), Console.ReadLine(), int.Parse(Console.ReadLine()));
                        }
                        else if (addChoice == "1.4")
                        {
                            Console.WriteLine("Введите дату, ID курса, максимальный балл:");
                            exams.AddExam(Console.ReadLine(), int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
                        }
                        else if (addChoice == "1.5")
                        {
                            Console.WriteLine("Введите ID студента, ID экзамена и оценку:");
                            grades.AddGrade(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.WriteLine("Изменить:");
                        Console.WriteLine("2.1 - Студента");
                        Console.WriteLine("2.2 - Преподавателя");
                        Console.WriteLine("2.3 - Курс");
                        string changeChoice = Console.ReadLine();

                        if (changeChoice == "2.1")
                        {
                            Console.WriteLine("Введите ID студента и новые данные (имя, фамилию, факультет, дату рождения):");
                            students.ChangeStudent(int.Parse(Console.ReadLine()), Console.ReadLine(), Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                        }
                        else if (changeChoice == "2.2")
                        {
                            Console.WriteLine("Введите ID преподавателя и новые данные (имя, фамилию, факультет):");
                            teachers.ChangeTeacher(int.Parse(Console.ReadLine()), Console.ReadLine(), Console.ReadLine(), Console.ReadLine());
                        }
                        else if (changeChoice == "2.3")
                        {
                            Console.WriteLine("Введите ID курса и новые данные (название, описание, id учителя):");
                            courses.ChangeCourse(int.Parse(Console.ReadLine()), Console.ReadLine(), Console.ReadLine(), int.Parse(Console.ReadLine()));
                        }
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("Удалить:");
                        Console.WriteLine("3.1 - Студента");
                        Console.WriteLine("3.2 - Преподавателя");
                        Console.WriteLine("3.3 - Курс");
                        Console.WriteLine("3.4 - Экзамен");
                        string deleteChoice = Console.ReadLine();

                        if (deleteChoice == "3.1")
                        {
                            Console.WriteLine("Введите ID студента для удаления:");
                            students.DeleteStudent(int.Parse(Console.ReadLine()));
                        }
                        else if (deleteChoice == "3.2")
                        {
                            Console.WriteLine("Введите ID преподавателя для удаления:");
                            teachers.DeleteTeacher(int.Parse(Console.ReadLine()));
                        }
                        else if (deleteChoice == "3.3")
                        {
                            Console.WriteLine("Введите ID курса для удаления:");
                            courses.DeleteCourse(int.Parse(Console.ReadLine()));
                        }
                        else if (deleteChoice == "3.4")
                        {
                            Console.WriteLine("Введите ID экзамена для удаления:");
                            exams.DeleteExam(int.Parse(Console.ReadLine()));
                        }
                    }
                    else if (choice == "4")
                    {
                        Console.WriteLine("Введите факультет для получения списка студентов:");
                        students.GetStudentsByDepartment(Console.ReadLine());
                    }
                    else if (choice == "5")
                    {
                        Console.WriteLine("Введите ID преподавателя для получения списка курсов:");
                        courses.GetCoursesByTeacher(int.Parse(Console.ReadLine()));
                    }
                    else if (choice == "6")
                    {
                        Console.WriteLine("Введите ID курса для получения списка студентов:");
                        students.GetStudentsByCourse(int.Parse(Console.ReadLine()));
                    }
                    else if (choice == "7")
                    {
                        Console.WriteLine("Введите ID курса для получения оценок студентов:");
                        courses.GetGradesByCourse(int.Parse(Console.ReadLine()));
                    }
                    else if (choice == "8")
                    {
                        Console.WriteLine("Введите ID студента и ID курса для получения среднего балла:");
                        grades.AverageByCourse(int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
                    }
                    else if (choice == "9")
                    {
                        Console.WriteLine("Введите ID студента для получения среднего балла:");
                        grades.Average(int.Parse(Console.ReadLine()));
                    }
                    else if (choice == "10")
                    {
                        Console.WriteLine("Введите факультет для получения среднего балла:");
                        grades.AverageByDepartment(Console.ReadLine());
                    }
                    else if (choice == "0")
                    {
                        Console.WriteLine("Завершение программы.");
                        break; 
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    }
                }
            }

            bool DatabaseExists()
            {
                return System.IO.File.Exists("university.db");
            }

            void CreateTables()
            {
                students.CreateTableStudents();
                teachers.CreateTableTeachers();
                courses.CreateTableCourses();
                exams.CreateTableExams();
                grades.CreateTableGrades();
            }




            //students.AddStudent("Chang", "Leonard", "Engineering", "12-09-2005");
            //students.ChangeStudent(19, "John", "Johnson", "Physics", "12-12-2000");
            //students.DeleteStudent(30);
            //students.GetStudentsByDepartment("IT");
            // students.GetStudents();



            //teachers.AddTeacher(1, "Ella", "Johnson", "IT");
            //teachers.DeleteTeacher(1);
            //teachers.GetTeachers();


            // courses.AddCourses("Engineering","bbb",2);

            // courses.ChangeCourse(2, "mATH", "asds", 3);
            //courses.GetCourses();
            //courses.GetCoursesByTeacher(3);

            //grades.GetGrades();
            //grades.AddGrade(31, 4, 4);
            //grades.GetGrades();

            //teachers.ChangeTeacher(2, "Luisa", "Dumb", "IT");
            //exams.AddExam("19 - 10 - 2025", 4, 100);
            //exams.GetExams();

            //courses.GetGradesByCourse(4);

            //grades.Average(31);
            //grades.AverageByDepartment("ASDQ");
            //grades.AverageByCourse(3, 1);
            // grades.GetStudentsByCourse(1);

            connection.Close();
            Console.WriteLine("Соединение с базой данных закрыто.");
        }

    }
}

    