using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using university_table.Interfaces;

namespace university_table
{
    internal class Exams: IExams
    {

        private readonly SQLiteConnection _connection;
        public Exams(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTableExams()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Exams (
                ""ID""	INTEGER NOT NULL UNIQUE,
	""Date""	TEXT NOT NULL,
	""CourseID""	INTEGER NOT NULL,
	""MaxScore""	INTEGER NOT NULL,
	PRIMARY KEY(""ID"" AUTOINCREMENT),
	FOREIGN KEY(""CourseID"") REFERENCES ""Courses""(""ID"")
            );";
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица 'Exams' успешно создана.");
            }
        }

        public void GetExams()
        {
            using (var command = new SQLiteCommand("SELECT * FROM Exams", _connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("Данные в таблице 'Exams':");
                while (reader.Read())
                {
                    var id = reader["ID"];
                    var date = reader["Date"];
                    var courseid = reader["CourseID"];
                    var maxscore = reader["MaxScore"];
                    Console.WriteLine($"{id}, {date}, {courseid}, {maxscore}");
                }
            }
        }

        public void AddExam(string date,int courseid, int maxscore)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = @"INSERT INTO Exams (Date, CourseID, MaxScore) 
                                VALUES (@date, @courseid, @maxscore)";

                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@courseid", courseid);
                command.Parameters.AddWithValue("@maxscore", maxscore);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Курс  добавлен.");
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Ошибка при добавлении курса: {ex.Message}");
                }
            }
        }

        public void DeleteExam(int id)
        {
            using (var command = new SQLiteCommand(_connection))
            {
                command.CommandText = "DELETE FROM Exams WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();


                Console.WriteLine($"{id} успешно удален.");
            }
        }
    }
}
