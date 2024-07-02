using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.DSAUtil;
using System.Data;
using FISCA.Data;
using System.Diagnostics;

namespace SHSchool_class_semester_history.DAO
{
    public class UDTTransfer
    {
        /// <summary>
        /// 建立使用到的 UDT Table
        /// </summary>
        public static void CreateUDTTable()
        {
            try
            {
                FISCA.UDT.SchemaManager Manager = new SchemaManager(new DSConnection(FISCA.Authentication.DSAServices.DefaultDataSource));
                Manager.SyncSchema(new udtClassSemesterHistory());
            }
            catch (Exception ex)
            {
                Console.WriteLine("SchemaManager Error:" + ex.Message);
            }
        }

        // 透過班級 ID 取得班級歷程
        public static List<udtClassSemesterHistory> GetClassSemesterHistoryByClassID(string ClassID)
        {
            List<udtClassSemesterHistory> value = new List<udtClassSemesterHistory>();
            try
            {
                AccessHelper access = new AccessHelper();
                value = access.Select<udtClassSemesterHistory>(string.Format("ref_class_id = {0}", ClassID)).OrderBy(x => x.SchoolYear).ThenBy(x => x.Semester).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetClassSemesterHistoryByClassID Error:" + ex.Message);
            }

            return value;
        }

        // 傳入多筆班級系統編號，取得班級歷程需要資料
        public static List<udtClassSemesterHistory> GetClassSemesterHistoryByClassIDs(List<string> ClassIDs, string SchoolYear, string Semester)
        {
            List<udtClassSemesterHistory> value = new List<udtClassSemesterHistory>();
            try
            {
                if (ClassIDs.Count > 0)
                {
                    AccessHelper access = new AccessHelper();
                    string query = string.Format("ref_class_id in ({0}) AND school_year = {1} AND semester = {2}", string.Join(",", ClassIDs), SchoolYear, Semester);

                    List<udtClassSemesterHistory> list = access.Select<udtClassSemesterHistory>(query);
                    foreach (udtClassSemesterHistory data in list)
                    {
                        value.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetClassSemesterHistoryByClassIDs" + ex.Message);
            }

            return value;
        }

        // 傳入多筆班級系統編號，取得目前班級歷程(班級、科別、教師)需要資料
        public static List<udtClassSemesterHistory> GetClassSemesterHistoryNowByClassIDs(List<string> ClassIDs, string SchoolYear, string Semseter)
        {
            List<udtClassSemesterHistory> value = new List<udtClassSemesterHistory>();

            try
            {
                if (ClassIDs.Count > 0)
                {
                    // 班級歷程資料
                    string query = string.Format(@"
                SELECT
                    class.id AS class_id,
                    class_name,
                    grade_year,
                    teacher.id AS teacher_id,
                    dept.id AS dept_id,
                    class_number,
                    dept.name AS dept_name,
                    teacher.teacher_name AS teacher_name,
                    teacher.nickname AS teacher_nickname,
                    teacher.teacher_number AS teacher_number
                FROM
                    class
                    LEFT JOIN dept ON class.ref_dept_id = dept.id
                    LEFT JOIN teacher ON class.ref_teacher_id = teacher.id
                WHERE
                    class.id IN({0})
                ", string.Join(",", ClassIDs.ToArray()));

                    QueryHelper qh = new QueryHelper();
                    DataTable dt = qh.Select(query);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            udtClassSemesterHistory data = new udtClassSemesterHistory();
                            int ci, di, ti, sy, ss, gr;
                            string ClassID = dr["class_id"] + "";
                            if (string.IsNullOrEmpty(ClassID))
                                continue;
                            if (int.TryParse(ClassID, out ci))
                                data.RefClassID = ci;

                            if (int.TryParse(dr["teacher_id"] + "", out ti))
                                data.RefTeacherID = ti;

                            if (int.TryParse(dr["dept_id"] + "", out di))
                                data.RefDeptID = di;

                            if (int.TryParse(SchoolYear, out sy))
                                data.SchoolYear = sy;

                            if (int.TryParse(Semseter, out ss))
                                data.Semester = ss;

                            if (int.TryParse(dr["grade_year"] + "", out gr))
                                data.GradeYear = gr;

                            data.ClassName = dr["class_name"] + "";
                            data.RefClassNumber = dr["class_number"] + "";
                            data.DeptName = dr["dept_name"] + "";
                            data.RefTeacherNumber = dr["teacher_number"] + "";
                            if (dr["teacher_nickname"] != null && dr["teacher_nickname"].ToString() != "")
                            {
                                // 有暱稱
                                data.ClassTeacher = string.Format("{0}({1})", dr["teacher_name"] + "", dr["teacher_nickname"] + "");
                            }
                            else
                            {
                                data.ClassTeacher = dr["teacher_name"] + "";
                            }

                            value.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetClassSemesterHistoryNowByClassIDs Error:" + ex.Message);
            }
            return value;
        }
    }
}
