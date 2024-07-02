using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SHSchool_class_semester_history.DAO;

namespace SHSchool_class_semester_history.UIForm
{
    public partial class frmCreateClassSemsHistory : FISCA.Presentation.Controls.BaseForm
    {
        BackgroundWorker bgWorker;
        EventHandler eh;
        string EventCode = "SHSchool_class_semester_history";

        List<string> SelectClassIDs;

        public frmCreateClassSemsHistory()
        {
            InitializeComponent();
            SelectClassIDs = new List<string>();
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += BgWorker_DoWork;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            //啟動更新事件
            eh = FISCA.InteractionService.PublishEvent(EventCode);

        }

        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("班級歷程產生中...", e.ProgressPercentage);
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
            btnRun.Enabled = true;
            if (e.Error == null)
            {
                eh(this, EventArgs.Empty);
                MsgBox.Show("產生完成");
                this.Close();
            }
            else
            {
                MsgBox.Show("產生失敗：", e.Error.Message);
            }
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bgWorker.ReportProgress(1);
            // 取得目前最新班級歷程資料
            List<udtClassSemesterHistory> currentClassHistoryList = UDTTransfer.GetClassSemesterHistoryNowByClassIDs(SelectClassIDs, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);

            bgWorker.ReportProgress(30);
            // 讀取目前所有班級歷程(舊) 需要刪除
            List<udtClassSemesterHistory> ClassHistoryOldList = UDTTransfer.GetClassSemesterHistoryByClassIDs(SelectClassIDs, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);

            bgWorker.ReportProgress(50);
            // 清除舊資料
            foreach (udtClassSemesterHistory data in ClassHistoryOldList)
            {
                data.Deleted = true;
            }
            ClassHistoryOldList.SaveAll();

            bgWorker.ReportProgress(80);
            // 新增資料
            currentClassHistoryList.SaveAll();

            bgWorker.ReportProgress(100);
        }

        private void frmCreateClassSemsHistory_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
            try
            {
                lblMsg.Text = string.Format("將產生{0}學年度第{1}學期班級歷程資料", K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            bgWorker.RunWorkerAsync();

        }

        // 設定傳入選取的班級ID
        public void SetClassIDs(List<string> ClassIDs)
        {
            SelectClassIDs = ClassIDs;
        }
    }
}
