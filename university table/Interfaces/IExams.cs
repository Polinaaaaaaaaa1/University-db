using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace university_table.Interfaces
{
    public interface IExams
    {
        void CreateTableExams();
        void GetExams();
        void AddExam(string date, int courseid, int maxscore);
        void DeleteExam(int id);
    }
}
