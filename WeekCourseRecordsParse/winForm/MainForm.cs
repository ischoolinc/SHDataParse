using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using Aspose.Cells;
using WeekCourseRecordsParse.DAO;

namespace WeekCourseRecordsParse.winForm
{
    public partial class MainForm : BaseForm
    {
        string SchoolYear = "";
        string Semester = "";
        public Dictionary<string, CourseFiledsMap> courseDic= new Dictionary<string, CourseFiledsMap>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;

            // 讀取系統預設學年度學期
            SchoolYear = K12.Data.School.DefaultSchoolYear;
            Semester = K12.Data.School.DefaultSemester;

            loadCourseFiledMap();

            // 畫面值
            labelSchoolYear.Text = $"{SchoolYear}學年度第{Semester}學期";

        }

        private void loadCourseFiledMap()
        {
            courseDic.Add("科目名稱", new CourseFiledsMap());
            courseDic.Add("班級名稱", new CourseFiledsMap());
            courseDic.Add("班碼名稱", new CourseFiledsMap());
            courseDic.Add("教師名稱", new CourseFiledsMap());
        }

        private void buttonParse01_Click(object sender, EventArgs e)
        {
            buttonEnable(false);
            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile(false);
            if (wb != null)
            {
                // 讀取課程資料
                List<CourseRecordInfo> CourseRecordList = DataTransfer.GetCourseRecordListBySchoolYearSemester(SchoolYear, Semester);

                //  解析資料
                Workbook resultWb = DataTransfer.CourseDataParse01(wb, CourseRecordList);

                // 輸出
                Utility.ExportXls(SchoolYear + "學年度第" + Semester + "學期_課程週課表解析", resultWb);

            }
            else
            {
                MsgBox.Show("讀取 Excel 檔案發生錯誤或版本過舊，要Excel 97以上版本。");
            }

            buttonEnable(true);
        }

        private void buttonEnable(bool value)
        {
            buttonParse01.Enabled = buttonParse02.Enabled = buttonParse03.Enabled = btnAnalysis.Enabled=value;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonParse02_Click(object sender, EventArgs e)
        {
            buttonEnable(false);
            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile(false);
            if (wb != null)
            {
                // 讀取課程資料
                List<CourseRecordInfo> CourseRecordList = DataTransfer.GetCourseRecordListBySchoolYearSemester(SchoolYear, Semester);

                //  解析資料
                Workbook resultWb = DataTransfer.CourseDataParse02(wb, CourseRecordList);

                // 輸出
                Utility.ExportXls(SchoolYear + "學年度第" + Semester + "學期_課程週課表解析", resultWb);

            }
            else
            {
                MsgBox.Show("讀取 Excel 檔案發生錯誤或版本過舊，要Excel 97以上版本。");
            }

            buttonEnable(true);
        }

        private void buttonParse03_Click(object sender, EventArgs e)
        {
            buttonEnable(false);
            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile(true);
            if (wb != null)
            {
                // 讀取課程資料
                List<CourseRecordInfo> CourseRecordList = DataTransfer.GetCourseRecordListBySchoolYearSemester(SchoolYear, Semester);

                //  解析資料
                Workbook resultWb = DataTransfer.CourseDataParse03(wb, CourseRecordList);

                // 輸出
                Utility.ExportXls(SchoolYear + "學年度第" + Semester + "學期_課程週課表解析", resultWb);

            }
            else
            {
                MsgBox.Show("讀取 Excel 檔案發生錯誤或版本過舊，要Excel 97以上版本。");
            }

            buttonEnable(true);
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            buttonEnable(false);

            bool hasChecked=false;

            //設定比對欄位與比對欄位名稱
            if(chkSubjectName.Checked)
            {
                hasChecked = true;
                courseDic["科目名稱"].isForCompare = true;
            }
            courseDic["科目名稱"].fieldName = "科目名稱";
            courseDic["科目名稱"].ref_filedName = txtSubjectName.Text;
            if (chkClassName.Checked)
            {
                hasChecked = true;
                courseDic["班級名稱"].isForCompare = true;
            }
            courseDic["班級名稱"].fieldName = "班級名稱";
            courseDic["班級名稱"].ref_filedName = txtClassName.Text;
            if (chkClassNumber.Checked)
            {
                hasChecked = true;
                courseDic["班碼名稱"].isForCompare = true;
            }
            courseDic["班碼名稱"].fieldName = "班碼名稱";
            courseDic["班碼名稱"].ref_filedName = txtClassNumber.Text;
            if (chkTeacherName.Checked)
            {
                hasChecked = true;
                courseDic["教師名稱"].isForCompare = true;
            }
            courseDic["教師名稱"].fieldName = "教師名稱";
            courseDic["教師名稱"].ref_filedName = txtTeacherName.Text;

            if(!hasChecked)
            {
                MsgBox.Show("請勾選自訂比對欄位");
                buttonEnable(true);
                return;
            }

            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile(true);
            if (wb != null)
            {
                // 讀取課程資料
                List<CourseRecordInfo> CourseRecordList = DataTransfer.GetCourseRecordListBySchoolYearSemester(SchoolYear, Semester);

                //  解析資料
                Workbook resultWb = DataTransfer.CourseDataParseX(wb, CourseRecordList,courseDic,txtSeparator.Text,chkCourseTeacher.Checked);

                // 輸出
                Utility.ExportXls(SchoolYear + "學年度第" + Semester + "學期_課程週課表解析", resultWb);

            }
            else
            {
                MsgBox.Show("讀取 Excel 檔案發生錯誤或版本過舊，要Excel 97以上版本。");
            }

            buttonEnable(true);
        }

        private void btnPrase021_Click(object sender, EventArgs e)
        {
            
            buttonEnable(false);
            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile(true);
            
            if (wb != null)
            {
                // 讀取課程資料
                List<CourseRecordInfo> CourseRecordList = DataTransfer.GetCourseRecordListBySchoolYearSemester(SchoolYear, Semester);

                //  解析資料
                Workbook resultWb = DataTransfer.CourseDataParse021(wb, CourseRecordList);

                // 輸出
                Utility.ExportXls(SchoolYear + "學年度第" + Semester + "學期_課程週課表解析", resultWb);               
            }
            else
            {
                MsgBox.Show("讀取 Excel 檔案發生錯誤或版本過舊，要Excel 97以上版本。");
            }

            buttonEnable(true);
        }
    }
}
