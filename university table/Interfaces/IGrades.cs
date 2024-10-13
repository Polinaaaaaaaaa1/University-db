using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace university_table.Interfaces
{
    public interface IGrades
    {
        void Average(int id);
        void CreateTableGrades();
        void AddGrade( int studentid, int examid, int score);
        void GetGrades();
        void DeleteGrade(int id);
        void AverageByDepartment(string department);
        void AverageByCourse(int studentid, int courseid);
        void GetStudentsByCourse(int courseid);
    }
}
