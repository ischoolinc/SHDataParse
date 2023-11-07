using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using FISCA.Data;

namespace SHScoreTools.DAO
{
    public class DataAccess
    {
        // 取得學生學期科目成績資料
        public static List<SemsScoreInfo> GetStudentSemsScoreInfoBySchoolYearSemester(string SchoolYear, string Semester, List<string> StudentIDs)
        {
            List<SemsScoreInfo> value = new List<SemsScoreInfo>();

            try
            {
                QueryHelper qh = new QueryHelper();
                string strSQL = string.Format(@"
                SELECT
                    id,
                    ref_student_id AS student_id,
                    school_year,
                    semester,
                    grade_year,
                    score_info
                FROM
                    sems_subj_score
                WHERE
                    ref_student_id IN({0})
                    AND school_year = {1} 
                    AND semester = {2} 
                ", string.Join(",", StudentIDs.ToArray()), SchoolYear, Semester);

                DataTable dt = qh.Select(strSQL);
                foreach(DataRow dr in dt.Rows)
                {
                    SemsScoreInfo ss = new SemsScoreInfo();
                    ss.ID = dr["id"] + "";
                    ss.StudentID = dr["student_id"] + "";
                    ss.SchoolYear = dr["school_year"]+"";
                    ss.Semester = dr["semester"] + "";
                    ss.GradeYear = dr["grade_year"] + "";
                    ss.ScoreInfo = dr["score_info"] + "";
                    ss.ParseScoreInfoToXML();
                    value.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return value;
        }
    }
}
