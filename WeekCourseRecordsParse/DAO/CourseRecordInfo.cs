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
        public string SubjectLevel { get; set; } // 科目級別
        public string ClassName { get; set; } // 班級
        public string ClassNumber { get; set; } // 班級代碼
        //public string TeacherName1 { get; set; } // 授課教師1

         // 存授課教師 1,2,3
        public List<string> TeacherNameList { get; set; }

    }
    public class CourseFiledsMap
    {
        public string fieldName { get; set; }
        public string ref_filedName { get; set; }
        public bool isForCompare { get; set; }
        public CourseFiledsMap(string fieldName, string ref_filedName, bool isForCompare)
        {
            this.fieldName = fieldName;
            this.ref_filedName = ref_filedName;
            this.isForCompare = isForCompare;
        }
        public CourseFiledsMap() 
        {
            this.fieldName = "";
            this.ref_filedName = "";
            this.isForCompare = false;
        }
    }
}
