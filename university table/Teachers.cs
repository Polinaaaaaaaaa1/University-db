using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using university_table.Interfaces;

namespace university_table
{
    public class Teachers:ITeachers
    {
        private readonly SQLiteConnection _connection;

        public Teachers(SQLiteConnection connection)
        {
            _connection=connection;
        }
        public void CreateTableTeachers()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Teachers (
                ID INTEGER PRIMARY KEY AUTOINCREMENT, 
                Name TEXT NOT NULL,                    -- Имя студента
                Surname TEXT NOT NULL,                 -- Фамилия студента
                Department TEXT NOT NULL              -- Отделение/факультет студента
            );";
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица 'Teachers' успешно создана.");
            }
        }

        public void AddTeacher(string name, string surname, string department)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = "INSERT INTO Teachers (Name, Surname, Department) VALUES (@name, @surname, @department)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@department", department);
                command.ExecuteNonQuery();
                Console.WriteLine($"Учитель {name} {surname} добавлен.");
            }
        }
        public void GetTeachers()
        {
            using (var command = new SQLiteCommand("SELECT * FROM Teachers", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Данные в таблице 'Teachers':");
                    while (reader.Read())
                    {

                        var id = reader["ID"];
                        var name = reader["Name"];
                        var surname = reader["Surname"];
                        var department = reader["Department"];

                        Console.WriteLine($"{id}, {name}, {surname}, {department}");
                    }
                }
            }
        }

        



        public  void DeleteTeacher(int id)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = "DELETE FROM Teachers WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();


                Console.WriteLine($"Учитель {id} успешно удален.");
            }
        }

        public void ChangeTeacher(int id, string name, string surname, string department)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            UPDATE Teachers
            SET Name = @name, Surname = @surname, Department = @department
            WHERE ID = @id";  

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@department", department);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Учитель с ID {id} успешно изменен.");
                    }
                    else
                    {
                        Console.WriteLine($"Учитель с ID {id} не найден.");
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Ошибка при изменении учителя: {ex.Message}");
                }
            }
        }
        public void GetCoursesByTeacher(int teacherId)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            SELECT c.ID, c.Title, c.Description
            FROM Courses c
            WHERE c.TeacherID = @teacherId";

                command.Parameters.AddWithValue("@teacherId", teacherId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine($"Курсы, читаемые преподавателем с ID {teacherId}:");

                        while (reader.Read())
                        {
                            var courseId = reader["ID"];
                            var title = reader["Title"];
                            var description = reader["Description"];

                            Console.WriteLine($"Курс ID {courseId}: {title} - {description}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Нет курсов для преподавателя с ID {teacherId}.");
                    }
                }
            }
        }

    }
}

