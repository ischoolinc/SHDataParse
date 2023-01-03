using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using Aspose.Cells;
using System.IO;
using System.Data;


namespace WeekCourseRecordsParse.DAO
{
    public class DataTransfer
    {

        /// <summary>
        /// 依學年度學期取得系統內課程紀錄
        /// </summary>
        /// <param name="SchoolYear"></param>
        /// <param name="Semester"></param>
        /// <returns></returns>
        public static List<CourseRecordInfo> GetCourseRecordListBySchoolYearSemester(string SchoolYear, string Semester)
        {
            List<CourseRecordInfo> value = new List<CourseRecordInfo>();
            try
            {
                QueryHelper qh = new QueryHelper();
                string query = @"
SELECT
	school_year
  , semester
  , course.id AS course_id
  , course_name
  , subject
  , teacher_name
  , class_name
FROM
	course
	INNER JOIN
		tc_instruct
		ON
			course.id = tc_instruct.ref_course_id
	INNER JOIN
		teacher
		ON
			tc_instruct.ref_teacher_id = teacher.id
	LEFT JOIN
		class
		ON
			course.ref_class_id = class.id
WHERE
	tc_instruct.sequence   = 1
	AND course.school_year = " + SchoolYear + @"
	AND course.semester    = " + Semester + @"
ORDER BY
	course_name;
";

                DataTable dt = qh.Select(query);
                foreach (DataRow dr in dt.Rows)
                {
                    CourseRecordInfo cr = new CourseRecordInfo();
                    cr.ClassName = dr["class_name"] + "";
                    cr.CourseID = dr["course_id"] + "";
                    cr.CourseName = dr["course_name"] + "";
                    cr.SchoolYear = dr["school_year"] + "";
                    cr.Semester = dr["semester"] + "";
                    cr.SubjectName = dr["subject"] + "";
                    cr.TeacherName1 = dr["teacher_name"] + "";

                    string key = cr.CourseName + "_" + cr.TeacherName1;
                    value.Add(cr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("讀取資料發生錯誤：" + ex.Message);
            }
            return value;
        }

        /// <summary>
        /// 處理全訊資料
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Workbook CourseDataParse01(Workbook wb, List<CourseRecordInfo> data)
        {
            Workbook value = new Workbook(new MemoryStream(Properties.Resources.匯入課程週課表樣板));
            Worksheet wst = wb.Worksheets[0];

            // 解析欄位
            Dictionary<string, int> ColIdxDic = Utility.ReadWorksheetColumnDict(wst);            

            // 檢查必要欄位:節次,課程全名,教師姓名，有資料再繼續
            if (ColIdxDic.ContainsKey("節次") && ColIdxDic.ContainsKey("課程全名") && ColIdxDic.ContainsKey("教師姓名"))
            {
                // 最後結果
                List<CourseRecordInfo> ResultData = new List<CourseRecordInfo>();

                List<ChkDataInfo> ChkDataList = new List<ChkDataInfo>();
                // 讀取需要資料
                for (int rowIdx = 1; rowIdx <= wst.Cells.MaxDataRow; rowIdx++)
                {
                    ChkDataInfo cd = new ChkDataInfo();
                    cd.CourseName = wst.Cells[rowIdx, ColIdxDic["課程全名"]].StringValue.Trim();

                    // 沒有課程名稱不處理
                    if (string.IsNullOrWhiteSpace(cd.CourseName))
                        continue;

                    List<string> teaName = wst.Cells[rowIdx, ColIdxDic["教師姓名"]].StringValue.Trim().Split(',').ToList();
                    if (teaName.Count > 0)
                        cd.TeacherName = teaName[0].Trim();

                    string wk = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();
                    if (wk.Length > 1)
                    {
                        cd.Week = wk.Substring(0, 1);
                        cd.Period = wk.Substring(1, 1);
                    }
                    ChkDataList.Add(cd);
                }

                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    // 比對 ischool 來課程資料，使用課程名稱 教師名
                    foreach (CourseRecordInfo cr in data)
                    {
                        if (cr.CourseName.Contains(cd.CourseName) && cr.TeacherName1.Contains(cd.TeacherName))
                        {                            
                            ResultData.Add(AddCourseRecordInfo(cr, cd));
                            break;
                        }
                    }
                }

                // 產生至 Excel 檔案
                value = ProcessWorkbook(value, ResultData);
            }

            return value;
        }

        /// <summary>
        /// 處理欣河資料
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Workbook CourseDataParse02(Workbook wb, List<CourseRecordInfo> data)
        {
            Workbook value = new Workbook(new MemoryStream(Properties.Resources.匯入課程週課表樣板));
            Worksheet wst = wb.Worksheets[0];

            // 解析欄位
            Dictionary<string, int> ColIdxDic = Utility.ReadWorksheetColumnDict(wst);
            
            // 檢查必要欄位:節次,課程全名,教師姓名，有資料再繼續
            if (ColIdxDic.ContainsKey("班級") && ColIdxDic.ContainsKey("科目名稱") && ColIdxDic.ContainsKey("教師名稱"))
            {
                // 最後結果
                List<CourseRecordInfo> ResultData = new List<CourseRecordInfo>();

                List<ChkDataInfo> ChkDataList = new List<ChkDataInfo>();
                // 讀取需要資料
                for (int rowIdx = 1; rowIdx <= wst.Cells.MaxDataRow; rowIdx++)
                {
                    ChkDataInfo cd = new ChkDataInfo();
                    cd.SubjectName = wst.Cells[rowIdx, ColIdxDic["科目名稱"]].StringValue.Trim();
                    cd.ClassName = wst.Cells[rowIdx, ColIdxDic["班級"]].StringValue.Trim();

                    // 沒有課程名稱不處理
                    if (string.IsNullOrWhiteSpace(cd.CourseName))
                        continue;

                    List<string> teaName = wst.Cells[rowIdx, ColIdxDic["教師名稱"]].StringValue.Trim().Split(',').ToList();
                    if (teaName.Count > 0)
                        cd.TeacherName = teaName[0].Trim();

                    cd.Week = wst.Cells[rowIdx, ColIdxDic["星期"]].StringValue.Trim();
                    cd.Period = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();
                    
                    ChkDataList.Add(cd);
                }

                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    // 比對 ischool 來比對班級+科目名稱+教師名稱
                    foreach (CourseRecordInfo cr in data)
                    {
                        if (cr.ClassName.Contains(cd.ClassName) && cr.TeacherName1.Contains(cd.TeacherName)&& cr.SubjectName.Contains(cd.SubjectName))
                        {
                            ResultData.Add(AddCourseRecordInfo(cr,cd));
                            break;
                        }
                    }
                }

                // 產生至 Excel 檔案
                value = ProcessWorkbook(value, ResultData);
            }

            return value;
        }

        private static CourseRecordInfo AddCourseRecordInfo(CourseRecordInfo cr, ChkDataInfo cd)
        {
            CourseRecordInfo nCr = new CourseRecordInfo();
            nCr.ClassName = cr.ClassName;
            nCr.CourseID = cr.CourseID;
            nCr.CourseName = cr.CourseName;
            nCr.SchoolYear = cr.SchoolYear;
            nCr.Semester = cr.Semester;
            nCr.TeacherName1 = cr.TeacherName1;
            nCr.SubjectName = cr.SubjectName;
            nCr.Period = cd.Period;
            nCr.Week = cd.Week;
            return nCr;
        }


        private static Workbook ProcessWorkbook(Workbook wb, List<CourseRecordInfo> Data)
        {
            int coIdx = 0;
            // 寫入結果
            Dictionary<string, int> colIdxDict = new Dictionary<string, int>();
            for (int col = 0; col <= wb.Worksheets[0].Cells.MaxColumn; col++)
            {
                string colName = wb.Worksheets[0].Cells[0, col].StringValue;
                if (!colIdxDict.ContainsKey(colName))
                {
                    colIdxDict.Add(colName, coIdx);
                    coIdx++;
                }
            }
            int roIdx = 1;
            foreach (CourseRecordInfo cr in Data)
            {
                wb.Worksheets[0].Cells[roIdx, colIdxDict["課程系統編號"]].PutValue(cr.CourseID);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["星期"]].PutValue(cr.Week);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["節次"]].PutValue(cr.Period);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["學年度"]].PutValue(cr.SchoolYear);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["學期"]].PutValue(cr.Semester);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["科目"]].PutValue(cr.SubjectName);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["班級"]].PutValue(cr.ClassName);
                wb.Worksheets[0].Cells[roIdx, colIdxDict["授課教師1"]].PutValue(cr.TeacherName1);
                roIdx++;
            }
            wb.Worksheets[0].AutoFitColumns();
            return wb;
        }

    }
}
