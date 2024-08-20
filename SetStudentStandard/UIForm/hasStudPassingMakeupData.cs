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
using SetStudentStandard.DAO;
using Aspose.Cells;

namespace SetStudentStandard.UIForm
{
    public partial class hasStudPassingMakeupData : BaseForm
    {
        // 已有及格補考標準資料
        List<SCAttendInfo> hasSCAttendList;

        public hasStudPassingMakeupData()
        {
            InitializeComponent();
            hasSCAttendList = new List<SCAttendInfo>();
        }

        private void hasStudPassingMakeupData_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;

            // 顯示已有筆數
            lblMsg.Text = "有 " + hasSCAttendList.Count + " 筆修課記錄已有及格、補考標準、請確認是否覆蓋 ?";

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // 回傳取消，不寫入
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = btnWrite.Enabled = false;
            FISCA.Presentation.MotherForm.SetStatusBarMessage("匯出已有修課記錄中...");

            // 將已有資料匯出至Excel檔案            
            // 讀取樣板
            Workbook wb = new Workbook(new System.IO.MemoryStream(Properties.Resources.已有修課及格補考標準樣板));

            Worksheet wst = wb.Worksheets["已有修課及格補考標準"];

            // 讀取樣板與相關位置
            Dictionary<string, int> colIdx = new Dictionary<string, int>();
            for (int cidx = 0; cidx <= wst.Cells.MaxDataColumn; cidx++)
            {
                string colName = wst.Cells[0, cidx].StringValue;
                if (!colIdx.ContainsKey(colName))
                    colIdx.Add(colName, cidx);
            }

            // 填入資料
            int rowIdx = 1;
            foreach (SCAttendInfo rec in hasSCAttendList)
            {
                WriteValue(wst, rowIdx, colIdx, "學生系統編號", rec.StudentID);
                WriteValue(wst, rowIdx, colIdx, "課程系統編號", rec.CourseID);
                WriteValue(wst, rowIdx, colIdx, "學年度", rec.SchoolYear);
                WriteValue(wst, rowIdx, colIdx, "學期", rec.Semester);
                WriteValue(wst, rowIdx, colIdx, "年級", rec.GradeYear);
                WriteValue(wst, rowIdx, colIdx, "課程名稱", rec.CourseName);
                WriteValue(wst, rowIdx, colIdx, "科目名稱", rec.SubjectName);
                WriteValue(wst, rowIdx, colIdx, "級別   ", rec.SubjLevel);
                WriteValue(wst, rowIdx, colIdx, "班級", rec.ClassName);
                WriteValue(wst, rowIdx, colIdx, "座號", rec.SeatNo);
                WriteValue(wst, rowIdx, colIdx, "姓名", rec.StudentName);
                if (rec.PassStandard.HasValue)
                    WriteValue(wst, rowIdx, colIdx, "及格標準", rec.PassStandard.Value + "");

                if (rec.MakeupStandard.HasValue)
                    WriteValue(wst, rowIdx, colIdx, "補考標準", rec.MakeupStandard.Value + "");

                rowIdx++;
            }

            wst.AutoFitColumns();
            // 匯出 Excel 檔案
            Utility.ExportXls("已有修課及格補考標準", wb);

            btnExport.Enabled = btnWrite.Enabled = true;
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
        }

        // 寫入 Excel 工作表
        private void WriteValue(Worksheet wst, int rowIdx, Dictionary<string, int> colIdx, string colName, string value)
        {
            if (colIdx.ContainsKey(colName))
                wst.Cells[rowIdx, colIdx[colName]].PutValue(value);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            // 回傳要寫入Yes
            this.DialogResult = DialogResult.Yes;
        }

        // 傳入已有資料
        public void SetData(List<SCAttendInfo> scAttendList)
        {
            hasSCAttendList = scAttendList;
        }

    }
}
