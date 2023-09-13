using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Data;
using Aspose.Cells;
using System.IO;
using System.Data;
using FISCA.Presentation.Controls;

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
  , subj_level
  , teacher_name
  , class_name
  , class_number
  , sequence
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
	tc_instruct.sequence   IN (1,2,3) 
	AND course.school_year = " + SchoolYear + @"
	AND course.semester    = " + Semester + @"
ORDER BY
	course_name
    , tc_instruct.sequence ASC;
";
                // 整理課程多教師
                Dictionary<string, CourseRecordInfo> CourseRecordInfoDict = new Dictionary<string, CourseRecordInfo>();

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
                    cr.SubjectLevel = dr["subj_level"] + "";
                    cr.ClassNumber = dr["class_number"] + "";
                    string teacherName = dr["teacher_name"] + "";

                    if (!CourseRecordInfoDict.ContainsKey(cr.CourseID))
                    {
                        cr.TeacherNameList = new List<string>();
                        CourseRecordInfoDict.Add(cr.CourseID, cr);
                    }


                    if (!string.IsNullOrEmpty(teacherName))
                        CourseRecordInfoDict[cr.CourseID].TeacherNameList.Add(teacherName);
                }

                // 填入資料
                value = CourseRecordInfoDict.Values.ToList();
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

            // 檢查必要欄位:節次,課程全名,教師姓名，班級名稱，有資料再繼續
            if (ColIdxDic.ContainsKey("班級名稱") && ColIdxDic.ContainsKey("節次") && ColIdxDic.ContainsKey("課程全名") && ColIdxDic.ContainsKey("教師姓名"))
            {
                // 最後結果
                List<CourseRecordInfo> ResultData = new List<CourseRecordInfo>();

                List<ChkDataInfo> ChkDataList = new List<ChkDataInfo>();
                // 讀取需要資料
                for (int rowIdx = 1; rowIdx <= wst.Cells.MaxDataRow; rowIdx++)
                {
                    ChkDataInfo cd = new ChkDataInfo();
                    cd.CourseName = wst.Cells[rowIdx, ColIdxDic["課程全名"]].StringValue.Trim();

                    cd.ClassName = wst.Cells[rowIdx, ColIdxDic["班級名稱"]].StringValue.Trim();

                    // 沒有課程名稱不處理
                    if (string.IsNullOrWhiteSpace(cd.CourseName))
                        continue;

                    cd.TeacherNameList = ParseTeacherName(wst.Cells[rowIdx, ColIdxDic["教師姓名"]].StringValue);

                    string wk = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();
                    if (wk.Length > 1)
                    {
                        cd.Week = wk.Substring(0, 1);
                        cd.Period = wk.Substring(1, 1);
                    }

                    ChkDataList.Add(cd);
                }

                List<ChkDataInfo> ErrorData = new List<ChkDataInfo>();

                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    bool isPass = false;
                    // 比對 ischool 來課程資料，使用課程名稱 教師名，班級名稱
                    foreach (CourseRecordInfo cr in data)
                    {
                        if (cr.CourseName.Contains(cd.CourseName) && CheckTeacherNameContains(cr.TeacherNameList, cd.TeacherNameList) && cr.ClassName.Contains(cd.ClassName))
                        {
                            isPass = true;
                            ResultData.Add(AddCourseRecordInfo(cr, cd));
                            break;
                        }
                    }

                    if (isPass == false)
                        ErrorData.Add(cd);
                }

                // 產生至 Excel 檔案
                value = ProcessWorkbook(value, ResultData, ErrorData);
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

            // 檢查必要欄位:節次,班級,科目名稱,教師姓名，有資料再繼續
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

                    // 沒有科目名稱不處理
                    if (string.IsNullOrWhiteSpace(cd.SubjectName))
                        continue;

                    cd.TeacherNameList = ParseTeacherName(wst.Cells[rowIdx, ColIdxDic["教師名稱"]].StringValue);

                    cd.Week = wst.Cells[rowIdx, ColIdxDic["星期"]].StringValue.Trim();
                    cd.Period = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();

                    ChkDataList.Add(cd);
                }

                List<ChkDataInfo> ErrorData = new List<ChkDataInfo>();
                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    bool isPass = false;
                    // 比對 ischool 來比對班級+科目名稱+教師名稱
                    foreach (CourseRecordInfo cr in data)
                    {
                        if (cr.ClassName.Contains(cd.ClassName) && CheckTeacherNameContains(cr.TeacherNameList, cd.TeacherNameList) && cr.SubjectName.Contains(cd.SubjectName))
                        {
                            isPass = true;
                            ResultData.Add(AddCourseRecordInfo(cr, cd));
                            break;
                        }
                    }

                    if (isPass == false)
                        ErrorData.Add(cd);
                }


                // 產生至 Excel 檔案
                value = ProcessWorkbook(value, ResultData, ErrorData);
            }

            return value;
        }

        public static Workbook CourseDataParse03(Workbook wb, List<CourseRecordInfo> data)
        {
            Workbook value = new Workbook(new MemoryStream(Properties.Resources.匯入課程週課表樣板));
            Worksheet wst = wb.Worksheets[0];

            // 解析欄位
            Dictionary<string, int> ColIdxDic = Utility.ReadWorksheetColumnDict(wst);

            // 讀取(科目名稱+所屬班級+授課教師一)比對，再讀取星期,節次資料。

            if (ColIdxDic.ContainsKey("所屬班級") && ColIdxDic.ContainsKey("科目名稱") && ColIdxDic.ContainsKey("授課教師一"))
            {
                // 最後結果
                List<CourseRecordInfo> ResultData = new List<CourseRecordInfo>();

                List<ChkDataInfo> ChkDataList = new List<ChkDataInfo>();
                // 讀取需要資料
                for (int rowIdx = 1; rowIdx <= wst.Cells.MaxDataRow; rowIdx++)
                {
                    ChkDataInfo cd = new ChkDataInfo();
                    cd.SubjectName = wst.Cells[rowIdx, ColIdxDic["科目名稱"]].StringValue.Trim();
                    cd.ClassName = wst.Cells[rowIdx, ColIdxDic["所屬班級"]].StringValue.Trim();

                    // 沒有科目名稱不處理
                    if (string.IsNullOrWhiteSpace(cd.SubjectName))
                        continue;

                    cd.TeacherNameList = ParseTeacherName(wst.Cells[rowIdx, ColIdxDic["授課教師一"]].StringValue);

                    cd.Week = wst.Cells[rowIdx, ColIdxDic["星期"]].StringValue.Trim();
                    cd.Period = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();

                    ChkDataList.Add(cd);
                }

                List<ChkDataInfo> ErrorData = new List<ChkDataInfo>();
                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    bool isPass = false;
                    // 讀取(科目名稱+所屬班級+授課教師一)比對，再讀取星期,節次資料。
                    foreach (CourseRecordInfo cr in data)
                    {
                        if (cr.ClassName.Contains(cd.ClassName) && CheckTeacherNameContains(cr.TeacherNameList, cd.TeacherNameList))
                        {
                            isPass = true;
                            ResultData.Add(AddCourseRecordInfo(cr, cd));
                            break;
                        }
                    }
                    if (isPass == false)
                        ErrorData.Add(cd);
                }

                // 產生至 Excel 檔案
                value = ProcessWorkbook(value, ResultData, ErrorData);
            }
            return value;
        }

        public static Workbook CourseDataParseX(Workbook wb, List<CourseRecordInfo> data, Dictionary<string, CourseFiledsMap> cfDic,string separator,bool chkList)
        {
            Workbook value = new Workbook(new MemoryStream(Properties.Resources.匯入課程週課表樣板New));
            Worksheet wst = wb.Worksheets[0];

            // 解析欄位
            Dictionary<string, int> ColIdxDic = Utility.ReadWorksheetColumnDict(wst);

            // 確定比對欄位名稱及比對原則。
            string subjectName = cfDic["科目名稱"].ref_filedName;
            string className = cfDic["班級名稱"].ref_filedName;
            string classNumber = cfDic["班碼名稱"].ref_filedName ;
            string teacherName = cfDic["教師名稱"].ref_filedName;

            // 最後結果
            List<CourseRecordInfo> ResultData = new List<CourseRecordInfo>();

            List<CourseRecordInfo> ResultCourseList= new List<CourseRecordInfo>();

            List<ChkDataInfo> ChkDataList = new List<ChkDataInfo>();
            // 讀取需要資料
            for (int rowIdx = 1; rowIdx <= wst.Cells.MaxDataRow; rowIdx++)
            {
                ChkDataInfo cd = new ChkDataInfo();
                if((ColIdxDic.ContainsKey(subjectName)) && (subjectName!=""))
                    cd.SubjectName = wst.Cells[rowIdx, ColIdxDic[subjectName]].StringValue.Trim();
                if ((ColIdxDic.ContainsKey(className)) && (className!=""))
                    cd.ClassName = wst.Cells[rowIdx, ColIdxDic[className]].StringValue.Trim();
                if ((ColIdxDic.ContainsKey(classNumber)) && (classNumber!=""))
                    cd.ClassNumber = wst.Cells[rowIdx, ColIdxDic[classNumber]].StringValue.Trim();

                if ((ColIdxDic.ContainsKey(teacherName)) && (teacherName != "")) cd.TeacherNameList = ParseTeacherName(wst.Cells[rowIdx, ColIdxDic[teacherName]].StringValue,separator);

                cd.Week = wst.Cells[rowIdx, ColIdxDic["星期"]].StringValue.Trim();
                cd.Period = wst.Cells[rowIdx, ColIdxDic["節次"]].StringValue.Trim();

                ChkDataList.Add(cd);
            }

            List<ChkDataInfo> ErrorData = new List<ChkDataInfo>();
            try
            {
                // 比對資料
                foreach (ChkDataInfo cd in ChkDataList)
                {
                    // 讀取設定條件比對，再讀取星期,節次資料。
                    List<CourseRecordInfo> found= new List<CourseRecordInfo>();
                    if (cfDic["科目名稱"].isForCompare) found = data.FindAll(x => x.SubjectName.Contains(cd.SubjectName));

                    if ((found.Count>0) && (cfDic["班級名稱"].isForCompare))
                    {
                        found = found.FindAll(x => x.ClassName.Contains(cd.ClassName)); found = found.FindAll(x => x.ClassName.Contains(cd.ClassName));
                    }

                    if ((found.Count>0) && (cfDic["班碼名稱"].isForCompare))
                    {
                        found = found.FindAll(x => x.ClassNumber.Contains(cd.ClassNumber));
                    }

                    if ((found.Count>0) && (cfDic["教師名稱"].isForCompare) && (cd.TeacherNameList.Count>0))
                    {
                        found = found.FindAll(x => CheckTeacherNameContains(x.TeacherNameList, cd.TeacherNameList));
                    }

                    if (found.Count>0) 
                    {
                        ResultData.Add(AddCourseRecordInfo(found[0], cd));
                        if(chkList)
                        {
                            CourseRecordInfo findExist = ResultCourseList.Find(x => x.CourseID == found[0].CourseID);
                            if(findExist==null)
                            {
                                findExist = new CourseRecordInfo();
                                findExist.CourseID = found[0].CourseID;
                                findExist.CourseName = found[0].CourseName;
                                findExist.SchoolYear = found[0].SchoolYear;
                                findExist.Semester = found[0].Semester;
                                findExist.ClassName = found[0].ClassName;
                                findExist.ClassNumber = found[0].ClassNumber;
                                findExist.SubjectName = found[0].SubjectName;
                                findExist.SubjectLevel = found[0].SubjectLevel;
                                findExist.TeacherNameList = cd.TeacherNameList.ToList();
                                ResultCourseList.Add(findExist);
                            }
                        }
                    }
                    else
                    {
                        ErrorData.Add(cd);
                    }
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.ToString());
            }

            // 產生至 Excel 檔案
            value = ProcessWorkbook(value, ResultData, ErrorData,ResultCourseList,chkList);

            return value;
        }

        private static CourseRecordInfo AddCourseRecordInfo(CourseRecordInfo cr, ChkDataInfo cd)
        {
            CourseRecordInfo nCr = new CourseRecordInfo();
            nCr.ClassName = cr.ClassName;
            nCr.CourseID = cr.CourseID;
            nCr.CourseName = cr.CourseName;
            nCr.ClassNumber = cr.ClassNumber;
            nCr.SchoolYear = cr.SchoolYear;
            nCr.Semester = cr.Semester;
            if(cd.TeacherNameList.Count>0) nCr.TeacherNameList = cr.TeacherNameList.ToList();
            nCr.SubjectName = cr.SubjectName;
            nCr.Period = cd.Period;
            nCr.Week = cd.Week;
            return nCr;
        }

        /// <summary>
        /// 解析Excel檔案上教師名稱
        /// </summary>
        /// <param name="teacherName"></param>
        /// <returns></returns>
        private static List<string> ParseTeacherName(string teacherName)
        {
            List<string> value = new List<string>();

            List<string> nameList = teacherName.Trim().Split(',').ToList();
            foreach (string name in nameList)
            {
                string n1 = name.Trim();
                if (!string.IsNullOrWhiteSpace(n1))
                    value.Add(n1);
            }
            return value;
        }

        /// <summary>
        /// 解析Excel檔案上教師名稱(加分隔符號)
        /// </summary>
        /// <param name="teacherName"></param>
        /// <returns></returns>
        private static List<string> ParseTeacherName(string teacherName,string separator)
        {
            List<string> value = new List<string>();

            List<string> nameList = teacherName.Trim().Split(separator[0]).ToList();
            foreach (string name in nameList)
            {
                string n1 = name.Trim();
                if (!string.IsNullOrWhiteSpace(n1))
                    value.Add(n1);
            }
            return value;
        }



        private static bool CheckTeacherNameContains(List<string> t1, List<string> t2)
        {
            bool value = false;
            IEnumerable<string> both = t1.Intersect(t2);
            if (both.Count() > 0)
                value = true;
            return value;
        }


        private static Workbook ProcessWorkbook(Workbook wb, List<CourseRecordInfo> Data, List<ChkDataInfo> ErrorData, List<CourseRecordInfo> CourseList,bool isGenCourseList)
        {
            int coIdx = 0;
            // 寫入結果
            Dictionary<string, int> colIdxDict = new Dictionary<string, int>();
            for (int col = 0; col <= wb.Worksheets["週課表"].Cells.MaxColumn; col++)
            {
                string colName = wb.Worksheets["週課表"].Cells[0, col].StringValue;
                if (!colIdxDict.ContainsKey(colName))
                {
                    colIdxDict.Add(colName, coIdx);
                    coIdx++;
                }
            }
            int roIdx = 1;
            foreach (CourseRecordInfo cr in Data)
            {
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["課程系統編號"]].PutValue(cr.CourseID);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["星期"]].PutValue(cr.Week);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["節次"]].PutValue(cr.Period);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["學年度"]].PutValue(cr.SchoolYear);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["學期"]].PutValue(cr.Semester);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["科目"]].PutValue(cr.SubjectName);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["班級"]].PutValue(cr.ClassName);
                wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["班級代碼"]].PutValue(cr.ClassNumber);
                if(cr.TeacherNameList!=null)
                {
                    wb.Worksheets["週課表"].Cells[roIdx, colIdxDict["授課教師"]].PutValue(string.Join(",", cr.TeacherNameList.ToArray()));
                }    
                roIdx++;
            }
            wb.Worksheets["週課表"].AutoFitColumns();

            colIdxDict.Clear();
            coIdx = 0;
            for (int col = 0; col <= wb.Worksheets["無法解析"].Cells.MaxColumn; col++)
            {
                string colName = wb.Worksheets["無法解析"].Cells[0, col].StringValue;
                if (!colIdxDict.ContainsKey(colName))
                {
                    colIdxDict.Add(colName, coIdx);
                    coIdx++;
                }
            }

            roIdx = 1;
            // 錯誤資料
            foreach (ChkDataInfo cr in ErrorData)
            {
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["科目名稱"]].PutValue(cr.SubjectName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["班級"]].PutValue(cr.ClassName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["班級代碼"]].PutValue(cr.ClassNumber + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["星期"]].PutValue(cr.Week + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["節次"]].PutValue(cr.Period + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["授課教師"]].PutValue(string.Join(",", cr.TeacherNameList.ToArray()));
                
                roIdx++;
            }
            wb.Worksheets["無法解析"].AutoFitColumns();

            if(isGenCourseList) //產生配課表
            {
                colIdxDict.Clear();
                coIdx = 0;
                for (int col = 0; col <= wb.Worksheets["配課表"].Cells.MaxColumn; col++)
                {
                    string colName = wb.Worksheets["配課表"].Cells[0, col].StringValue;
                    if (!colIdxDict.ContainsKey(colName))
                    {
                        colIdxDict.Add(colName, coIdx);
                        coIdx++;
                    }
                }

                roIdx = 1;
                // 錯誤資料
                foreach (CourseRecordInfo cr in CourseList)
                {
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["課程系統編號"]].PutValue(cr.CourseID + "");
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["學年度"]].PutValue(cr.SchoolYear);
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["學期"]].PutValue(cr.Semester);
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName + "");
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["科目名稱"]].PutValue(cr.SubjectName + "");
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["科目級別"]].PutValue(cr.SubjectLevel + "");
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["所屬班級"]].PutValue(cr.ClassName + "");
                    wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["班級代碼"]].PutValue(cr.ClassNumber + "");
                    if(cr.TeacherNameList.Count > 0)
                    {
                        wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["授課教師一"]].PutValue(cr.TeacherNameList[0] + "");
                        if(cr.TeacherNameList.Count > 1)
                        {
                            wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["授課教師二"]].PutValue(cr.TeacherNameList[1] + "");
                            if(cr.TeacherNameList.Count > 2)
                            {
                                wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["授課教師三"]].PutValue(cr.TeacherNameList[2] + "");
                            }
                        }
                    }
                  //  wb.Worksheets["配課表"].Cells[roIdx, colIdxDict["授課教師一"]].PutValue(string.Join(",", cr.TeacherNameList.ToArray()));

                    roIdx++;
                }
                wb.Worksheets["配課表"].AutoFitColumns();
            }



            return wb;
        }

        private static Workbook ProcessWorkbook(Workbook wb, List<CourseRecordInfo> Data, List<ChkDataInfo> ErrorData)
        {
            int coIdx = 0;
            // 寫入結果
            Dictionary<string, int> colIdxDict = new Dictionary<string, int>();
            for (int col = 0; col <= wb.Worksheets["Data"].Cells.MaxColumn; col++)
            {
                string colName = wb.Worksheets["Data"].Cells[0, col].StringValue;
                if (!colIdxDict.ContainsKey(colName))
                {
                    colIdxDict.Add(colName, coIdx);
                    coIdx++;
                }
            }
            int roIdx = 1;
            foreach (CourseRecordInfo cr in Data)
            {
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["課程系統編號"]].PutValue(cr.CourseID);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["星期"]].PutValue(cr.Week);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["節次"]].PutValue(cr.Period);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["學年度"]].PutValue(cr.SchoolYear);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["學期"]].PutValue(cr.Semester);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["科目"]].PutValue(cr.SubjectName);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["班級"]].PutValue(cr.ClassName);
                wb.Worksheets["Data"].Cells[roIdx, colIdxDict["授課教師"]].PutValue(string.Join(",", cr.TeacherNameList.ToArray()));
                roIdx++;
            }
            wb.Worksheets["Data"].AutoFitColumns();

            colIdxDict.Clear();
            coIdx = 0;
            for (int col = 0; col <= wb.Worksheets["無法解析"].Cells.MaxColumn; col++)
            {
                string colName = wb.Worksheets["無法解析"].Cells[0, col].StringValue;
                if (!colIdxDict.ContainsKey(colName))
                {
                    colIdxDict.Add(colName, coIdx);
                    coIdx++;
                }
            }

            roIdx = 1;
            // 錯誤資料
            foreach (ChkDataInfo cr in ErrorData)
            {
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["課程名稱"]].PutValue(cr.CourseName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["科目名稱"]].PutValue(cr.SubjectName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["班級"]].PutValue(cr.ClassName + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["星期"]].PutValue(cr.Week + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["節次"]].PutValue(cr.Period + "");
                wb.Worksheets["無法解析"].Cells[roIdx, colIdxDict["授課教師"]].PutValue(string.Join(",", cr.TeacherNameList.ToArray()));

                roIdx++;
            }
            wb.Worksheets["無法解析"].AutoFitColumns();

            return wb;
        }

    }
}
