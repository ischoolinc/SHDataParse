using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekCourseRecordsParse.DAO
{
    public class CourseRecordInfo
    {
        public string CourseID { get; set; } // 課程系統編號
        public string Week { get; set; } // 星期
        public string Period { get; set; } // 節次
        public string SchoolYear { get; set; } // 學年度
        public string Semester { get; set; } // 學期
        public string CourseName { get; set; } // 課程名稱
        public string SubjectName { get; set; } // 科目
        public string ClassName { get; set; } // 班級
        public string TeacherName1 { get; set; } // 授課教師1

    }
}
