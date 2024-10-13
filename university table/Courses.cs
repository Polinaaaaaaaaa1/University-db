using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using university_table.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace university_table
{
    public class Courses:ICourses
    {
        private readonly SQLiteConnection _connection;
        public Courses(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTableCourses()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Courses (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,  -- ID курса, автоинкремент
                Title TEXT NOT NULL,                   -- Название курса
                Description TEXT NOT NULL,             -- Описание курса
                ""Teacher ID"" INTEGER NOT NULL,       -- ID учителя с пробелом в имени
                FOREIGN KEY (""Teacher ID"") REFERENCES Teachers(ID)  -- Внешний ключ на таблицу Teachers
            );";
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица 'Courses' успешно создана.");
            }
        }


        public void AddCourses(string title, string description, int teacherid)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"INSERT INTO Courses (Title, Description, ""Teacher ID"") 
                                VALUES (@title, @description, @teacherid)";

                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@teacherid", teacherid);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Курс {title} добавлен.");
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Ошибка при добавлении курса: {ex.Message}");
                }
            }
        }





        public void GetCourses()
        {

            using (var command = new SQLiteCommand("SELECT * FROM Courses", _connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("Данные в таблице 'Courses':");
                while (reader.Read())
                {
                    var id = reader["ID"];
                    var title = reader["Title"];
                    var description = reader["Description"];
                    var teacherid = reader["Teacher ID"];
                    Console.WriteLine($"{id}, {title}, {description}, {teacherid}");
                }
            }
        }
        public void ChangeCourse(int id, string title, string description, int teacherid)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            UPDATE Courses
            SET Title = @title, Description = @description, ""Teacher ID"" = @teacherid
            WHERE ID = @id";  

                command.Parameters.AddWithValue("@id", id); 
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@teacherid", teacherid);  

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();  
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Курс {id} успешно изменен.");
                    }
                    else
                    {
                        Console.WriteLine($"Курс с ID {id} не найден.");
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Ошибка при изменении курса: {ex.Message}");
                }
            }
        }
        public void DeleteCourse(int id)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = "DELETE FROM Courses WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();


                Console.WriteLine($"Курс {id} успешно удален.");
            }
        }

        public void GetCoursesByTeacher(int teacherId)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"SELECT Title FROM Courses WHERE ""Teacher ID"" = @teacherId";
                command.Parameters.AddWithValue("@teacherId", teacherId);

                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine($"Курсы, которые ведет учитель с ID {teacherId}:");

                    if (reader.HasRows) // Проверка на наличие строк
                    {
                        while (reader.Read())
                        {
                            var title = reader["Title"]; 
                            Console.WriteLine(title); 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Этот учитель не ведет ни одного курса.");
                    }
                }
            }
        }

        public void GetGradesByCourse(int courseId)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT g.Score, g.StudentID
            FROM Courses c 
            JOIN Exams e ON e.CourseID = c.ID 
            JOIN Grades g ON g.ExamID = e.ID 
            WHERE c.ID = @courseId";

                command.Parameters.AddWithValue("@courseId", courseId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"Баллы по курсу с ID {courseId}:");

                        while (reader.Read())
                        {
                            var score = reader["Score"];
                            var studentId = reader["StudentID"];
                            Console.WriteLine($"Студент ID {studentId}: Балл {score}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет данных об оценках для курса с ID {courseId}.");
                    }
                }
            }
        }

    }




}


