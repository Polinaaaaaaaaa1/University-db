using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace university_table.Interfaces
{
    public interface ITeachers
    {
        void CreateTableTeachers();
        void AddTeacher(string name, string surname, string department);

        void GetTeachers();
        void DeleteTeacher(int id);
        void ChangeTeacher(int id, string name, string surname, string department);
        void GetCoursesByTeacher(int teacherId);
    }

}
