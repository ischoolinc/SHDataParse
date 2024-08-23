using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetStudentStandard.DAO
{
    // 課程修課學生資訊
    public class SCAttendInfo
    {
        // 修課系統編號
        public string SCAttendID { get; set; }

        // 學生系統編號
        public string StudentID { get; set; }

        // 課程系統編號
        public string CourseID { get; set; }

        // 學年度
        public string SchoolYear { get; set; }

        // 學期
        public string Semester { get; set; }

        // 年級
        public string GradeYear { get; set; }

        // 課程名稱
        public string CourseName { get; set; }

        // 科目名稱
        public string SubjectName { get; set; }

        // 級別   
        public string SubjLevel { get; set; }

        // 班級
        public string ClassName { get; set; }

        // 座號
        public string SeatNo { get; set; }

        // 姓名
        public string StudentName { get; set; }

        // 及格標準
        public decimal? PassStandard { get; set; }

        // 補考標準
        public decimal? MakeupStandard { get; set; }

        // 學生修課備註
        public string SCComment { get; set; }
    }
}
