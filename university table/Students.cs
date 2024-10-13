using System;
using System.Data.SQLite;
using System.Xml.Linq;
using university_table.Interfaces;

public class Students : IStudents
{
    private readonly SQLiteConnection _connection;

    public Students(SQLiteConnection connection)
    {
        _connection = connection;
    }

    public void CreateTableStudents()
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Students (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Surname TEXT NOT NULL,
                Department TEXT NOT NULL,
                DateOfBirth TEXT NOT NULL
            );";
            command.ExecuteNonQuery();
            Console.WriteLine("Таблица 'Students' успешно создана.");
        }
    }

    public void GetStudentsByCourse(int courseId)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = @"
            SELECT s.ID, s.Name, s.Surname
            FROM Students s
            JOIN Grades g ON g.StudentID = s.ID
            JOIN Exams e ON e.ID = g.ExamID
            JOIN Courses c ON c.ID = e.CourseID
            WHERE c.ID = @courseId
            GROUP BY s.ID, s.Name, s.Surname;";

            command.Parameters.AddWithValue("@courseId", courseId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine($"Студенты, зачисленные на курс с ID {courseId}:");

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
                    Console.WriteLine($"Нет данных для курса с ID {courseId}.");
                }
            }
        }
    }

    public void AddStudent(string name, string surname, string department, string dateOfBirth)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = "INSERT INTO Students (Name, Surname, Department, DateOfBirth) VALUES (@name, @surname, @department, @dateOfBirth)";

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@department", department); 
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

            command.ExecuteNonQuery();
            Console.WriteLine($"Пользователь {name} добавлен.");
        }
    }


    public void GetStudents()
    {
        using (var command = new SQLiteCommand("SELECT * FROM Students", _connection))
        {
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("Данные в таблице 'Students':");
                while (reader.Read())
                {
                    
                    var id = reader["ID"];
                    var name = reader["Name"];
                    var surname = reader["Surname"];
                    var department = reader["Department"];
                    var dateOfBirth = reader["DateOfBirth"];

                    Console.WriteLine($"{id}, {name}, {surname}, {department}, {dateOfBirth}");
                }
            }
        }
    }
    public void ChangeStudent(int id, string name, string surname, string department, string dateOfBirth)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = @"
            UPDATE Students
            SET Name = @name, Surname = @surname, Department = @department, DateOfBirth = @dateOfBirth
            WHERE ID = @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@department", department);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

            try
            {
                int rowsAffected = command.ExecuteNonQuery();  
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Студент с ID {id} успешно изменен.");
                }
                else
                {
                    Console.WriteLine($"Студент с ID {id} не найден.");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка при изменении студента: {ex.Message}");
            }
        }
    }
    public void DeleteStudent(int id)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = "DELETE FROM Students WHERE ID = @id";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();


            Console.WriteLine($"Ученик {id} успешно удален.");
        }
    }

    public void GetStudentsByDepartment(string department)
    {
        using (var command = new SQLiteCommand(_connection))
        {
            command.CommandText = @"SELECT ID, Name, Surname FROM Students WHERE Department = @department";
            command.Parameters.AddWithValue("@department", department);

            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine($"Студенты на факультете {department}:");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader["ID"];
                        var name = reader["Name"];
                        var surname = reader["Surname"];

                        Console.WriteLine($"ID: {id}, Имя: {name}, Фамилия: {surname}");
                    }
                }
                else
                {
                    Console.WriteLine("На данном факультете нет студентов.");
                }
            }
        }
    }

    


}
