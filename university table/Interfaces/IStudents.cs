using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace university_table.Interfaces
{
    public interface IStudents
    {
        void CreateTableStudents();
        void AddStudent(string name, string surname, string department, string dateOfBirth);
     
        void GetStudents();
        void ChangeStudent(int id, string name, string surname, string department, string dateOfBirth);
        void DeleteStudent(int id);
        void GetStudentsByDepartment(string department);
        void GetStudentsByCourse(int courseId);
    }

}
