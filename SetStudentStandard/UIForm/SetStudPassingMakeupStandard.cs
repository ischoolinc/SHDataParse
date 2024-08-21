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
using K12.Data;
using SetStudentStandard.DAO;

namespace SetStudentStandard.UIForm
{
    public partial class SetStudPassingMakeupStandard : BaseForm
    {
        // 檢查是否已有資料
        BackgroundWorker bgWorkerCheckHasData;

        // 產生資料
        BackgroundWorker bgWorkerCreateData;

        // 課程修課資料
        Dictionary<string, List<SCAttendInfo>> SCAttendDict;

        // 有及格補考標準資料
        List<SCAttendInfo> hasSCAttendList;

        public SetStudPassingMakeupStandard()
        {
            InitializeComponent();
            bgWorkerCheckHasData = new BackgroundWorker();
            bgWorkerCheckHasData.DoWork += BgWorkerCheckHasData_DoWork;
            bgWorkerCheckHasData.ProgressChanged += BgWorkerCheckHasData_ProgressChanged;
            bgWorkerCheckHasData.RunWorkerCompleted += BgWorkerCheckHasData_RunWorkerCompleted;
            bgWorkerCheckHasData.WorkerReportsProgress = true;

            bgWorkerCreateData = new BackgroundWorker();
            bgWorkerCreateData.DoWork += BgWorkerCreateData_DoWork;
            bgWorkerCreateData.ProgressChanged += BgWorkerCreateData_ProgressChanged;
            bgWorkerCreateData.RunWorkerCompleted += BgWorkerCreateData_RunWorkerCompleted;
            bgWorkerCreateData.WorkerReportsProgress = true;

            SCAttendDict = new Dictionary<string, List<SCAttendInfo>>();
            hasSCAttendList = new List<SCAttendInfo>();

        }

        private void BgWorkerCreateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCreate.Enabled = true;
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
            FISCA.Features.Invoke("CourseSyncAllBackground");
            MsgBox.Show("產生完成");
            this.Close();
        }

        private void BgWorkerCreateData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("寫入修課及格補考標準中...", e.ProgressPercentage);
        }

        private void BgWorkerCreateData_DoWork(object sender, DoWorkEventArgs e)
        {
            bgWorkerCreateData.ReportProgress(10);
            // 取得所選修課的學生系統編號
            List<string> StudentIDs = new List<string>();
            // 需要更新修課及格補考標準的學生
            List<SCAttendInfo> updateSCAttendList = new List<SCAttendInfo>();
            foreach (string cid in SCAttendDict.Keys)
            {
                foreach (SCAttendInfo info in SCAttendDict[cid])
                {
                    if (!StudentIDs.Contains(info.StudentID))
                    {
                        StudentIDs.Add(info.StudentID);
                    }
                }
            }

            // 取得學生成績計算規則，年級與及格標準
            Dictionary<string, StudentScoreRuleInfo> StudentScoreRuleDict = DataAccess.GetStudentScoreRuleInfoDictByIDs(StudentIDs);

            bgWorkerCreateData.ReportProgress(30);

            List<SCAttendInfo> updateInfoList = new List<SCAttendInfo>();

            // 覆蓋資料
            foreach (string cid in SCAttendDict.Keys)
            {
                foreach (SCAttendInfo si in SCAttendDict[cid])
                {
                    // 讀取學生成績計算規則及格補考標準
                    if (StudentScoreRuleDict.ContainsKey(si.StudentID))
                    {
                        if (si.GradeYear == "1")
                        {
                            if (StudentScoreRuleDict[si.StudentID].Grade1PassingStandard.HasValue)
                                si.PassStandard = StudentScoreRuleDict[si.StudentID].Grade1PassingStandard.Value;

                            if (StudentScoreRuleDict[si.StudentID].Grade1MakeupStandard.HasValue)
                                si.MakeupStandard = StudentScoreRuleDict[si.StudentID].Grade1MakeupStandard.Value;

                        }

                        if (si.GradeYear == "2")
                        {
                            if (StudentScoreRuleDict[si.StudentID].Grade2PassingStandard.HasValue)
                                si.PassStandard = StudentScoreRuleDict[si.StudentID].Grade2PassingStandard.Value;

                            if (StudentScoreRuleDict[si.StudentID].Grade2MakeupStandard.HasValue)
                                si.MakeupStandard = StudentScoreRuleDict[si.StudentID].Grade2MakeupStandard.Value;
                        }

                        if (si.GradeYear == "3")
                        {
                            if (StudentScoreRuleDict[si.StudentID].Grade3PassingStandard.HasValue)
                                si.PassStandard = StudentScoreRuleDict[si.StudentID].Grade3PassingStandard.Value;

                            if (StudentScoreRuleDict[si.StudentID].Grade3MakeupStandard.HasValue)
                                si.MakeupStandard = StudentScoreRuleDict[si.StudentID].Grade3MakeupStandard.Value;
                        }

                        if (si.GradeYear == "4")
                        {
                            if (StudentScoreRuleDict[si.StudentID].Grade4PassingStandard.HasValue)
                                si.PassStandard = StudentScoreRuleDict[si.StudentID].Grade4PassingStandard.Value;

                            if (StudentScoreRuleDict[si.StudentID].Grade4MakeupStandard.HasValue)
                                si.MakeupStandard = StudentScoreRuleDict[si.StudentID].Grade4MakeupStandard.Value;
                        }
                        updateInfoList.Add(si);
                    }
                }
            }
            bgWorkerCreateData.ReportProgress(60);
            // 更新及格與補考標準
            DataAccess.UpdateSCAttendData(updateInfoList);
            bgWorkerCreateData.ReportProgress(100);
        }

        private void BgWorkerCheckHasData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 檢查已有及格補考標準，另存List，並將資料傳給已有料畫面，讓畫面可以顯示筆數與匯出資料。
            if (hasSCAttendList.Count > 0)
            {
                hasStudPassingMakeupData spm = new hasStudPassingMakeupData();
                spm.SetData(hasSCAttendList);
                if (spm.ShowDialog() == DialogResult.Yes)
                {
                    // 覆蓋資料
                    bgWorkerCreateData.RunWorkerAsync();
                }
                else
                {
                    btnCreate.Enabled = true;
                }
            }
            // 沒有資料，直接寫入
            else
            {
                bgWorkerCreateData.RunWorkerAsync();
            }
        }

        private void BgWorkerCheckHasData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("課程修課資料檢查中...", e.ProgressPercentage);
        }

        private void BgWorkerCheckHasData_DoWork(object sender, DoWorkEventArgs e)
        {
            // 取得是否目前學年度學期課程
            Dictionary<bool, List<string>> CourseIDDict = DataAccess.GetCourseIsDefault(K12.Presentation.NLDPanels.Course.SelectedSource);

            SCAttendDict.Clear();

            // 讀取課程並加入
            foreach (bool key in CourseIDDict.Keys)
            {
                Dictionary<string, List<SCAttendInfo>> da = DataAccess.GetSCAttendInfoByCourseIDs(CourseIDDict[key], key);
                foreach (string cid in da.Keys)
                {
                    if (!SCAttendDict.ContainsKey(cid))
                        SCAttendDict.Add(cid, new List<SCAttendInfo>());

                    SCAttendDict[cid].AddRange(da[cid]);
                }
            }

            // 檢查課程修課中真正沒有及格補考標準
            hasSCAttendList.Clear();
            foreach (string cid in SCAttendDict.Keys)
            {
                foreach (SCAttendInfo info in SCAttendDict[cid])
                {
                    if (info.PassStandard.HasValue || info.MakeupStandard.HasValue)
                        hasSCAttendList.Add(info);
                }
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetStudPassingMakeupStandard_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            btnCreate.Enabled = false;

            // 檢查是否已有資料，如果已有資料提示並讓使用者選擇後決定覆蓋或離開
            bgWorkerCheckHasData.RunWorkerAsync();

        }
    }
}
