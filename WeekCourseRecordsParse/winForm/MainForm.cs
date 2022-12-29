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

            // 畫面值
            labelSchoolYear.Text = $"{SchoolYear}學年度第{Semester}學期";
        }

        private void buttonParse01_Click(object sender, EventArgs e)
        {
            buttonEnable(false);
            // 讀取解析檔
            Workbook wb = Utility.ReadXlsFile();
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
                MsgBox.Show("讀取 Excel 檔案發生錯誤");
            }

            buttonEnable(true);
        }

        private void buttonEnable(bool value)
        {
            buttonParse01.Enabled = buttonParse02.Enabled = value;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }
    }
}
