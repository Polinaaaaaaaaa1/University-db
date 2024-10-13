using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using university_table.Interfaces;

namespace university_table
{
    public class Grades :IGrades
    {
        private readonly SQLiteConnection _connection;

        public Grades(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTableGrades()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
        CREATE TABLE IF NOT EXISTS ""Grades"" (
    ""ID"" INTEGER PRIMARY KEY AUTOINCREMENT,
    ""StudentID"" INTEGER NOT NULL,
    ""ExamID"" INTEGER NOT NULL,
    ""Score"" INTEGER NOT NULL,
    UNIQUE(StudentID, ExamID),  -- Уникальная комбинация StudentID и ExamID
    FOREIGN KEY(StudentID) REFERENCES Students(ID),
    FOREIGN KEY(ExamID) REFERENCES Exams(ID)
);
";
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица 'Grades' успешно создана.");
            }
        }


        public void AddGrade(int studentId, int examId, int score)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            INSERT INTO Grades (StudentID, ExamID, Score)
            VALUES (@studentId, @examId, @score);";

                command.Parameters.AddWithValue("@studentId", studentId);
                command.Parameters.AddWithValue("@examId", examId);
                command.Parameters.AddWithValue("@score", score);

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine($"Оценка {score} успешно добавлена для студента с ID {studentId} на экзамене с ID {examId}.");
                }
                else
                {
                    Console.WriteLine("Ошибка при добавлении оценки.");
                }
            }
        }




        public void Average(int id)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT s.ID, s.Name, s.Surname, AVG(g.Score) AS AverageScore
            FROM Students s
            JOIN Grades g ON s.ID = g.StudentID
            WHERE s.ID = @id
            GROUP BY s.ID, s.Name, s.Surname;";

                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Средний балл студента:");

                        while (reader.Read())
                        {
                            var studentId = reader["ID"];
                            var name = reader["Name"];
                            var surname = reader["Surname"];
                            var avgScore = reader["AverageScore"];

                            Console.WriteLine($"Студент {name} {surname} (ID: {studentId}): Средний балл {avgScore}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет данных об оценках для студента с ID {id}.");
                    }
                }
            }
        }

        public void AverageByDepartment(string department)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT s.Department, AVG(g.Score) AS AverageScore
            FROM Students s
            JOIN Grades g ON s.ID = g.StudentID
            WHERE s.Department = @department
            GROUP BY s.Department;";

                // Добавляем параметр department в запрос
                command.Parameters.AddWithValue("@department", department);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var dept = reader["Department"];
                            var avgScore = reader["AverageScore"];

                            Console.WriteLine($"Факультет {dept}: Средний балл {avgScore}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет данных об оценках для факультета {department}.");
                    }
                }
            }
        }


        public void GetGrades()
        {

            using (var command = new SQLiteCommand("SELECT * FROM Grades", _connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("Данные в таблице 'Grades':");
                while (reader.Read())
                {
                    var id = reader["ID"];
                    var studentid= reader["StudentID"];
                    var examid = reader["ExamID"];
                    var score = reader["Score"];
                    Console.WriteLine($"{id}, {studentid}, {examid}, {score}");
                }
            }
        }

        public void DeleteGrade(int id)
        {
            try
            {
                using (var command = new SQLiteCommand(_connection))
                {
                    command.CommandText = "DELETE FROM Grades WHERE ID = @id";
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Оценка с ID {id} успешно удалена.");
                    }
                    else
                    {
                        Console.WriteLine($"Оценка с ID {id} не найдена.");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при удалении оценки: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        public void AverageByCourse(int studentid, int courseid)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT s.ID, s.Name, s.Surname, AVG(g.Score) AS AverageScore
            FROM Students s
            JOIN Grades g ON s.ID = g.StudentID
JOIN Exams e ON e.ID = g.ExamID
JOIN Courses c ON c.ID=e.CourseID
            WHERE s.ID = @studentid AND c.ID=@courseid
            GROUP BY s.ID,s.Name,s.Surname;";

                command.Parameters.AddWithValue("@studentid", studentid);
                command.Parameters.AddWithValue("@courseid", courseid);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var id = reader["ID"];
                            var name = reader["Name"];
                            var surname = reader["Surname"];
                            var avgScore = reader["AverageScore"];

                            Console.WriteLine($"Студент {name} {surname} (ID: {id}): Средний балл по курсу {avgScore}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет данных для студента с ID {studentid} по курсу с ID {courseid}.");
                    }
                }
            }
        }

        public void GetStudentsByCourse(int courseid)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT s.ID, s.Name, s.Surname
            FROM Students s
            JOIN Grades g ON g.StudentID = s.ID
            JOIN Exams e ON e.ID = g.ExamID
            JOIN Courses c ON c.ID = e.CourseID
            WHERE c.ID = @courseid
            GROUP BY s.ID, s.Name, s.Surname;";

                command.Parameters.AddWithValue("@courseid", courseid);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"Студенты на курсе с ID {courseid}:");
                        while (reader.Read())
                        {
                            var id = reader["ID"];
                            var name = reader["Name"];
                            var surname = reader["Surname"];

                            Console.WriteLine($"{id}. {name} {surname}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет данных для курса с ID {courseid}");
                    }
                }
            }
        }



    }
}
