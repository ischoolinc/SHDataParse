using FISCA.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SetStudentStandard.DAO
{
    public class DataAccess
    {
        // 傳入多筆課程系統編號，取得課程修課學生資料，需要檢查如果沒有學生及格、補考標準填null
        public static Dictionary<string, List<SCAttendInfo>> GetSCAttendInfoByCourseIDs(List<string> courseIDs, bool isDefaultSchoolYearSemester)
        {
            Dictionary<string, List<SCAttendInfo>> value = new Dictionary<string, List<SCAttendInfo>>();

            try
            {
                string query = "";

                // 與預設相同，學生使用目前班級年級，如果不是使用學生學期對照上班級年級
                if (isDefaultSchoolYearSemester)
                {
                    query = string.Format(@"
                    SELECT
                        sc_attend.id AS sc_attend_id,
                        student.id AS student_id,
                        student.name AS student_name,
                        course.id AS course_id,
                        course.school_year AS school_year,
                        course.semester AS semester,
                        class.grade_year AS grade_year,
                        course.course_name AS course_name,
                        course.subject AS subject_name,
                        course.subj_level AS subj_level,
                        class.class_name AS class_name,
                        student.seat_no AS seat_no,
                        sc_attend.passing_standard,
                        sc_attend.makeup_standard
                    FROM
                        student
                        INNER JOIN sc_attend ON student.id = sc_attend.ref_student_id
                        INNER JOIN course ON sc_attend.ref_course_id = course.id
                        LEFT JOIN class ON student.ref_class_id = class.id
                    WHERE
                        course.id IN({0})                         
                    ORDER BY
                        class_name,
                        seat_no;
                    ", string.Join(",", courseIDs.ToArray()));
                }
                else
                {
                    query = string.Format(@"
                    WITH course_student AS(
                        SELECT
                            sc_attend.id AS sc_attend_id,
                            student.id AS student_id,
                            student.name AS student_name,
                            course.id AS course_id,
                            course.school_year AS school_year,
                            course.semester AS semester,
                            course.course_name AS course_name,
                            course.subject AS subject_name,
                            course.subj_level AS subj_level,
                            class.class_name AS class_name,
                            sc_attend.passing_standard,
                            sc_attend.makeup_standard
                        FROM
                            student
                            INNER JOIN sc_attend ON student.id = sc_attend.ref_student_id
                            INNER JOIN course ON sc_attend.ref_course_id = course.id
                            LEFT JOIN class ON student.ref_class_id = class.id
                        WHERE
                            course.id IN({0})                            
                        ORDER BY
                            class_name,
                            seat_no
                    ),
                    sems_history AS (
                        SELECT
                            id,
                            MAX(school_year) OVER(PARTITION BY id, grade_year, semester) AS school_year,
                            grade_year,
                            semester,
                            seat_no
                        FROM
                            (
                                SELECT
                                    student_sems_history.id,
                                    (
                                        '0' || array_to_string(xpath('//History/@SchoolYear', history_xml), '') :: TEXT
                                    ) :: INTEGER AS school_year,
                                    (
                                        '0' || array_to_string(xpath('//History/@GradeYear', history_xml), '') :: TEXT
                                    ) :: INTEGER AS grade_year,
                                    (
                                        '0' || array_to_string(xpath('//History/@Semester', history_xml), '') :: TEXT
                                    ) :: INTEGER AS semester,
                                    (
                                        '0' || array_to_string(xpath('//History/@SeatNo', history_xml), '') :: TEXT
                                    ) :: INTEGER AS seat_no
                                FROM
                                    (
                                        SELECT
                                            id,
                                            unnest(
                                                xpath(
                                                    '//root/History',
                                                    xmlparse(content '<root>' || sems_history || '</root>')
                                                )
                                            ) AS history_xml
                                        FROM
                                            student
                                        WHERE
                                            student.id IN(
                                                SELECT
                                                    student_id
                                                FROM
                                                    course_student
                                            )
                                    ) AS student_sems_history
                            ) student_sems_history_expand
                    )
                    SELECT
                        course_student.*,
                        grade_year,
                        seat_no
                    FROM
                        course_student
                        INNER JOIN sems_history ON course_student.student_id = sems_history.id
                        AND course_student.school_year = sems_history.school_year
                        AND course_student.semester = sems_history.semester
                    WHERE
                        course_id IN({0})
                    ORDER BY
                        class_name,
                        seat_no;
                    ", string.Join(",", courseIDs.ToArray()));
                }


                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(query);

                foreach (DataRow dr in dt.Rows)
                {
                    SCAttendInfo sc = new SCAttendInfo();

                    sc.SCAttendID = dr["sc_attend_id"] + "";

                    // student.id AS student_id,
                    sc.StudentID = dr["student_id"] + "";

                    // student.name AS student_name,
                    sc.StudentName = dr["student_name"] + "";

                    // course.id AS course_id,
                    sc.CourseID = dr["course_id"] + "";

                    // course.school_year AS school_year,
                    sc.SchoolYear = dr["school_year"] + "";

                    // course.semester AS semester,
                    sc.Semester = dr["semester"] + "";

                    // class.grade_year AS grade_year,
                    sc.GradeYear = dr["grade_year"] + "";

                    // course.course_name AS course_name,
                    sc.CourseName = dr["course_name"] + "";

                    // course.subject AS subject_name,
                    sc.SubjectName = dr["subject_name"] + "";

                    // course.subj_level AS subj_level,
                    sc.SubjLevel = dr["subj_level"] + "";

                    // class.class_name AS class_name,
                    sc.ClassName = dr["class_name"] + "";

                    // student.seat_no AS seat_no,
                    sc.SeatNo = dr["seat_no"] + "";

                    decimal passing_standard;
                    // sc_attend.passing_standard,
                    if (decimal.TryParse(dr["passing_standard"] + "", out passing_standard))
                    {
                        sc.PassStandard = passing_standard;
                    }

                    decimal makeup_standard;
                    // sc_attend.makeup_standard
                    if (decimal.TryParse(dr["makeup_standard"] + "", out makeup_standard))
                    {
                        sc.MakeupStandard = makeup_standard;
                    }

                    if (!value.ContainsKey(sc.CourseID))
                        value.Add(sc.CourseID, new List<SCAttendInfo>());
                    value[sc.CourseID].Add(sc);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSCAttendInfoByCourseIDs," + ex.Message);
            }


            return value;
        }


        // 透過課程編號取得目前課程學年度、學期
        public static Dictionary<bool, List<string>> GetCourseIsDefault(List<string> CourseIDs)
        {
            Dictionary<bool, List<string>> value = new Dictionary<bool, List<string>>();
            try
            {
                QueryHelper qh = new QueryHelper();
                string query = string.Format(@"
                SELECT
                    school_year,
                    semester,
                    id
                FROM
                    course
                WHERE
                    id IN({0});
                ", string.Join(",", CourseIDs.ToArray()));

                DataTable dt = qh.Select(query);
                foreach (DataRow dr in dt.Rows)
                {
                    bool isDefault = false;
                    string cid = dr["id"] + "";
                    if (dr["school_year"] + "" == K12.Data.School.DefaultSchoolYear && dr["semester"] + "" == K12.Data.School.DefaultSemester)
                    {
                        isDefault = true;
                    }
                    if (!value.ContainsKey(isDefault))
                        value.Add(isDefault, new List<string>());
                    value[isDefault].Add(cid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSCAttendInfoByCourseIDs" + ex.Message);
            }
            return value;
        }

        // 傳入學生ID，取得學生(班級)成績計算規則各年級及格與補考標準
        public static Dictionary<string, StudentScoreRuleInfo> GetStudentScoreRuleInfoDictByIDs(List<string> StudentIDs)
        {
            Dictionary<string, StudentScoreRuleInfo> value = new Dictionary<string, StudentScoreRuleInfo>();

            try
            {
                // 取得學生類別
                Dictionary<string, List<string>> studentTypeDict = GetStudentTagsByIDs(StudentIDs);


                // 1. 學生(自己或班級)成績計算規則資料並解析
                string query = string.Format(@"
                WITH student_rule_id AS(
                    SELECT
                        student.id AS student_id,
                        COALESCE(
                            student.ref_score_calc_rule_id,
                            class.ref_score_calc_rule_id
                        ) AS score_calc_rule_id
                    FROM
                        student
                        LEFT JOIN class ON student.ref_class_id = class.id
                    WHERE
                        student.id IN({0})
                )
                SELECT
                    student_rule_id.*,
                    score_calc_rule.name,
                    score_calc_rule.content
                FROM
                    student_rule_id
                    INNER JOIN score_calc_rule ON student_rule_id.score_calc_rule_id = score_calc_rule.id
                ", string.Join(",", StudentIDs.ToArray()));

                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(query);
                // 讀取並解析成績計算規則
                foreach (DataRow dr in dt.Rows)
                {
                    string sid = dr["student_id"] + "";
                    if (!value.ContainsKey(sid))
                        value.Add(sid, new StudentScoreRuleInfo());

                    // 放入學生類別
                    if (studentTypeDict.ContainsKey(sid))
                    {
                        value[sid].StudentTags = studentTypeDict[sid];
                    }

                    // 成績計算規則
                    try
                    {
                        XElement elmRoot = XElement.Parse(dr["content"] + "");
                        if (elmRoot != null)
                            value[sid].ScoreCalcRuleXml = elmRoot;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("xml:" + ex.Message);
                    }

                    // 解析成績
                    value[sid].ParsePassingMakeupStandard();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetStudentScoreRuleInfoDictByIDs," + ex.Message);
            }

            return value;
        }


        // 透過學生系統編號取得學生類別List
        public static Dictionary<string, List<string>> GetStudentTagsByIDs(List<string> StudentIDs)
        {
            Dictionary<string, List<string>> value = new Dictionary<string, List<string>>();
            try
            {
                string query = string.Format(@"
                SELECT
                    tag_student.ref_student_id AS student_id,
                    tag.prefix || ':' || tag.name AS TagName
                FROM
                    tag_student
                    INNER JOIN tag ON tag_student.ref_tag_id = tag.id
                WHERE
                    tag_student.ref_student_id IN({0});
                ", string.Join(",", StudentIDs.ToArray()));
                QueryHelper qh = new QueryHelper();
                DataTable dt = qh.Select(query);
                foreach (DataRow dr in dt.Rows)
                {
                    string sid = dr["student_id"] + "";
                    string tagName = dr["TagName"] + "";
                    if (!value.ContainsKey(sid))
                        value.Add(sid, new List<string>());
                    value[sid].Add(tagName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetStudentTagsByIDs," + ex.Message);
            }
            return value;
        }

        //  透過修課紀錄更新及格補考標準
        public static void UpdateSCAttendData(List<SCAttendInfo> SCAttendInfoList)
        {
            try
            {
                List<string> updateSQL = new List<string>();
                foreach (SCAttendInfo sc in SCAttendInfoList)
                {
                    string sql = string.Format(@"
                    UPDATE
                        sc_attend
                    SET
                        passing_standard = {0},
                        makeup_standard = {1}
                    WHERE
                        WHERE id = {2} ;
                    ", sc.PassStandard.Value, sc.MakeupStandard.Value, sc.SCAttendID);
                    updateSQL.Add(sql);
                }
                K12.Data.UpdateHelper uh = new K12.Data.UpdateHelper();
                uh.Execute(updateSQL);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateSCAttendData," + ex.Message);
            }

        }
    }
}
