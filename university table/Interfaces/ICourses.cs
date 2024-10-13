using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace university_table.Interfaces
{
    public interface ICourses
    {
        void CreateTableCourses();
        void AddCourses(string title, string description, int teacherid);

        void GetCourses();
        void ChangeCourse(int ID,string title, string description, int teacherid);
        void DeleteCourse(int id);
        void GetCoursesByTeacher(int id);
        void GetGradesByCourse(int courseId);
    }
}
