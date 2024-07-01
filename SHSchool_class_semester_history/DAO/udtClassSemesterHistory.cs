using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace SHSchool_class_semester_history.DAO
{
    /// <summary>
    /// 高中班級歷程
    /// </summary>
    [TableName("SHSchool.class_semester_history")]
    public class udtClassSemesterHistory : ActiveRecord
    {
        // 學年度
        [Field(Field = "school_year", Indexed = false)]
        public int SchoolYear { get; set; }

        // 學期
        [Field(Field = "semester", Indexed = false)]
        public int Semester { get; set; }

        // 年級
        [Field(Field = "grade_year", Indexed = false)]
        public int GradeYear { get; set; }

        // 科別
        [Field(Field = "dept_name", Indexed = false)]
        public string DeptName { get; set; }

        // 班級名稱
        [Field(Field = "class_name", Indexed = false)]
        public string ClassName { get; set; }

        // 班導師
        [Field(Field = "class_teacher", Indexed = false)]
        public string ClassTeacher { get; set; }

        // 科別系統編號
        [Field(Field = "ref_dept_id", Indexed = false)]
        public int RefDeptID { get; set; }

        // 班級系統編號
        [Field(Field = "ref_class_id", Indexed = false)]
        public int RefClassID { get; set; }

        // 班級編碼
        [Field(Field = "ref_class_number", Indexed = false)]
        public string RefClassNumber { get; set; }

        // 教師系統編號
        [Field(Field = "ref_teacher_id", Indexed = false)]
        public int RefTeacherID { get; set; }

        // 教師編碼
        [Field(Field = "ref_teacher_number", Indexed = false)]
        public string RefTeacherNumber { get; set; }

    }
}
