using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekCourseRecordsParse.DAO
{
    public class ChkDataInfo
    {
        public string CourseName { get; set; } // 課程名稱
        //public string TeacherName { get; set; } // 授課教師       
        public string SubjectName { get; set; } // 科目
        public string ClassName { get; set; } // 班級
        public string ClassNumber { get; set; } // 班級代碼 Add by Jackie Wang 2023/09/10
        public string Week { get; set; } // 星期
        public string Period { get; set; } // 節次

        public List<string> TeacherNameList = new List<string>();   // 多教師
        
    }
}
