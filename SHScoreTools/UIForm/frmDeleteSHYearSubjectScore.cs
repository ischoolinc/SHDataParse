﻿using SHScoreTools.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Web.Script.Serialization;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA.LogAgent;

namespace SHScoreTools.UIForm
{
    public partial class frmDeleteSHYearSubjectScore : FISCA.Presentation.Controls.BaseForm
    {

        // 選擇學生ID集合
        List<string> _StudentIDList;

        // 學年科目成績集合
        List<YearScoreInfo> _YearScoreList;

        // 所有科目名稱
        Dictionary<string, SubjInfo> AllSubjDict;

        // 需要刪除科目名稱
        List<SubjInfo> DelSubjList;

        // 選擇學年度
        string _SchoolYear = "";

        // 查詢資料
        BackgroundWorker _bgWorkerLoadData;

        // 刪除資料用
        BackgroundWorker _bgWorkerDelData;

        public frmDeleteSHYearSubjectScore()
        {
            InitializeComponent();
            _StudentIDList = new List<string>();
            _YearScoreList = new List<YearScoreInfo>();
            AllSubjDict = new Dictionary<string, SubjInfo>();
            DelSubjList = new List<SubjInfo>();
            _bgWorkerLoadData = new BackgroundWorker();
            _bgWorkerLoadData.DoWork += _bgWorkerLoadData_DoWork;
            _bgWorkerLoadData.RunWorkerCompleted += _bgWorkerLoadData_RunWorkerCompleted;
            _bgWorkerLoadData.ProgressChanged += _bgWorkerLoadData_ProgressChanged;
            _bgWorkerLoadData.WorkerReportsProgress = true;

            _bgWorkerDelData = new BackgroundWorker();
            _bgWorkerDelData.DoWork += _bgWorkerDelData_DoWork;
            _bgWorkerDelData.RunWorkerCompleted += _bgWorkerDelData_RunWorkerCompleted;
            _bgWorkerDelData.ProgressChanged += _bgWorkerDelData_ProgressChanged;
            _bgWorkerDelData.WorkerReportsProgress = true;

        }

        private void _bgWorkerDelData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學年科目成績刪除中...", e.ProgressPercentage);
        }

        private void _bgWorkerDelData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");

            if (!e.Cancelled)
            {
                MsgBox.Show("已完成刪除。");
            }
            else
            {
                MsgBox.Show("未有需要刪除的學年成績!");
            }
            // 重新載入
            ControlEnable(false);
            _bgWorkerLoadData.RunWorkerAsync();
        }

        private void _bgWorkerDelData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgWorkerDelData.ReportProgress(10);

            // 需要刪除科目
            List<YearScoreInfo> DelYearScoreList = new List<YearScoreInfo>();

            // log data
            Dictionary<string, string> logStudNameDict = new Dictionary<string, string>();
            Dictionary<string, List<string>> LogSubjDataDict = new Dictionary<string, List<string>>();
            // 學生資料
            List<StudentRecord> StudRecList = Student.SelectByIDs(_StudentIDList);
            Dictionary<string, StudentRecord> StudRecDict = new Dictionary<string, StudentRecord>();
            foreach (StudentRecord stud in StudRecList)
            {
                if (!StudRecDict.ContainsKey(stud.ID))
                    StudRecDict.Add(stud.ID, stud);
            }

            // 比對刪除資料
            foreach (YearScoreInfo ss in _YearScoreList)
            {


                bool isDel = false;
                foreach (SubjInfo si in DelSubjList)
                {
                    foreach (XElement elm in ss.ScoreInfoXML.Elements("Subject"))
                    {
                        if (elm.Attribute("科目").Value == si.SubjectName)
                        {
                            // log 科目
                            if (!LogSubjDataDict.ContainsKey(ss.StudentID))
                                LogSubjDataDict.Add(ss.StudentID, new List<string>());

                            LogSubjDataDict[ss.StudentID].Add(string.Format("科目名稱：{0}", si.SubjectName));

                            elm.Remove();
                            isDel = true;
                        }
                    }
                }
                if (isDel)
                {
                    if (StudRecDict.ContainsKey(ss.StudentID))
                    {
                        StudentRecord stud = StudRecDict[ss.StudentID];
                        string className = "";
                        if (stud.Class != null)
                            className = stud.Class.Name;

                        string seta_no = "";
                        if (stud.SeatNo.HasValue)
                            seta_no = stud.SeatNo.Value.ToString();


                        string strName = string.Format(@"學生系統編號：{0} ,學號：{1} ,班級:{2} ,座號:{3} ,姓名:{4} ,學年度：{5} ,年級：{6}", stud.ID, stud.StudentNumber, className, seta_no, stud.Name, ss.SchoolYear, ss.GradeYear);

                        if (!logStudNameDict.ContainsKey(ss.StudentID))
                            logStudNameDict.Add(ss.StudentID, strName);
                    }
                    DelYearScoreList.Add(ss);
                }


            }

            _bgWorkerDelData.ReportProgress(40);

            // 刪除資料
            if (DelYearScoreList.Count > 0)
            {
                Dictionary<int, List<string>> UpdateSQLDict = new Dictionary<int, List<string>>();
                Dictionary<int, StringBuilder> UpdateLogDict = new Dictionary<int, StringBuilder>();

                int i = 1, idx = 1;
                foreach (YearScoreInfo ss in DelYearScoreList)
                {
                    string UpdateSQL = "";
                    // 判斷沒有成績就刪除資料
                    if (ss.ScoreInfoXML.Elements("Subject").Count() == 0)
                    {
                        // 沒有科目成績刪除
                        UpdateSQL = string.Format(@"
                    DELETE FROM 
                        year_subj_score                         
                        WHERE id = {0}", ss.ID);
                    }
                    else
                    {
                        // 更新資料
                        UpdateSQL = string.Format(@"
                    UPDATE 
                        year_subj_score 
                        SET score_info = '{0}' 
                        WHERE id = {1}", ss.ScoreInfoXML.ToString(), ss.ID);
                    }

                    // log 資料
                    StringBuilder sb = new StringBuilder();
                    if (logStudNameDict.ContainsKey(ss.StudentID))
                    {
                        sb.AppendLine(logStudNameDict[ss.StudentID]);
                        sb.AppendLine(" == 刪除學年科目 == ");
                    }
                    if (LogSubjDataDict.ContainsKey(ss.StudentID))
                    {
                        sb.AppendLine(string.Join(",", LogSubjDataDict[ss.StudentID].ToArray()));
                        sb.AppendLine("");
                    }

                    // 分批
                    if (i == 1)
                    {
                        UpdateSQLDict.Add(idx, new List<string>());
                        UpdateLogDict.Add(idx, new StringBuilder());
                    }
                    UpdateSQLDict[idx].Add(UpdateSQL);
                    UpdateLogDict[idx].AppendLine(sb.ToString());

                    if (i == 50)
                    {
                        i = 1;
                        idx++;
                    }
                    else
                        i++;

                }
                _bgWorkerDelData.ReportProgress(60);
                // 更新資料
                foreach (int key in UpdateSQLDict.Keys)
                {
                    try
                    {
                        UpdateHelper uh = new UpdateHelper();
                        uh.Execute(UpdateSQLDict[key]);

                        _bgWorkerDelData.ReportProgress(60 + (int)(40 * (key / UpdateSQLDict.Keys.Count)));

                        // 紀錄log
                        FISCA.LogAgent.ApplicationLog.Log("學年科目成績", "刪除學生學年科目成績", UpdateLogDict[key].ToString());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            else
            {
                e.Cancel = true;
            }


            _bgWorkerDelData.ReportProgress(100);
        }

        private void _bgWorkerLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學年科目成績讀取中...", e.ProgressPercentage);

        }

        private void _bgWorkerLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
            LoadDgDataColumns();
            LoadSubjectToDgData();
            ControlEnable(true);
        }

        // 載入科目資料
        private void LoadSubjectToDgData()
        {
            dgData.Rows.Clear();
            foreach (string key in AllSubjDict.Keys)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = AllSubjDict[key];
                dgData.Rows[rowIdx].Cells["科目名稱"].Value = AllSubjDict[key].SubjectName;
            }

        }

        // 載入欄位
        private void LoadDgDataColumns()
        {
            dgData.Columns.Clear();
            try
            {
                // 加入刪除勾選
                DataGridViewCheckBoxColumn chkCol1 = new DataGridViewCheckBoxColumn();
                chkCol1.Name = "勾選";
                chkCol1.HeaderText = "勾選";
                chkCol1.Width = 60;
                chkCol1.TrueValue = "是";
                chkCol1.FalseValue = "否";
                chkCol1.IndeterminateValue = "否";

                dgData.Columns.Add(chkCol1);

                string textColumnStrig = @"
                    [
                        {
                            ""HeaderText"": ""科目名稱"",
                            ""Name"": ""科目名稱"",
                            ""Width"": 400,
                            ""ReadOnly"": true
                        }
                    ]            
                
";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<DataGridViewTextBoxColumnInfo> jsonObjArray = serializer.Deserialize<List<DataGridViewTextBoxColumnInfo>>(textColumnStrig);
                foreach (DataGridViewTextBoxColumnInfo jObj in jsonObjArray)
                {
                    DataGridViewTextBoxColumn dgt = new DataGridViewTextBoxColumn();
                    dgt.Name = jObj.Name;
                    dgt.Width = jObj.Width;
                    dgt.HeaderText = jObj.HeaderText;
                    dgt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgt.ReadOnly = jObj.ReadOnly;
                    dgData.Columns.Add(dgt);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void _bgWorkerLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgWorkerLoadData.ReportProgress(20);

            // 讀取學年科目成績
            _YearScoreList = DataAccess.GetStudentYearScoreInfoBySchoolYear(_SchoolYear, _StudentIDList);

            _bgWorkerLoadData.ReportProgress(80);

            // 整理科目名稱
            AllSubjDict.Clear();
            DelSubjList.Clear();
            foreach (YearScoreInfo ss in _YearScoreList)
            {
                foreach (XElement elm in ss.ScoreInfoXML.Elements("Subject"))
                {
                    string key = elm.Attribute("科目").Value;
                    if (!AllSubjDict.ContainsKey(key))
                    {
                        SubjInfo si = new SubjInfo();
                        si.SubjectName = elm.Attribute("科目").Value;
                        AllSubjDict.Add(key, si);
                    }
                }
            }


            _bgWorkerLoadData.ReportProgress(100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {


            // 解析需要刪除科目
            DelSubjList.Clear();
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.Cells["勾選"].Value != null)
                    if (drv.Cells["勾選"].Value.ToString() == "是")
                    {
                        // MsgBox.Show("刪除");
                        SubjInfo si = drv.Tag as SubjInfo;
                        if (si != null)
                            DelSubjList.Add(si);
                    }
            }

            if (DelSubjList.Count == 0)
            {
                MsgBox.Show("請勾選要刪除的科目");
                return;
            }

            ControlEnable(false);

            if (MsgBox.Show("您確定要刪除 " + DelSubjList.Count + " 項學年科目成績嗎？這些資料將被永久刪除，並且無法恢復。 ", "刪除學期科目成績", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                PasswordForm pf = new PasswordForm();
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.GetPass())
                    {
                        _bgWorkerDelData.RunWorkerAsync();
                    }
                }

            }

            ControlEnable(true);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            // 寫入畫面值
            _SchoolYear = comboSchoolYear.Text;
            ControlEnable(false);
            _bgWorkerLoadData.RunWorkerAsync();
        }

        // 設定學生 ID
        public void SetStudentIDs(List<string> sids)
        {
            _StudentIDList = sids;
        }

        private void frmDeleteSHSemsSubjectScore_Load(object sender, EventArgs e)
        {
            ControlEnable(false);

            // 取得預設學年
            int sy = 0;
            int.TryParse(K12.Data.School.DefaultSchoolYear, out sy);

            for (int i = sy; i >= sy - 7; i--)
            {
                comboSchoolYear.Items.Add(i + "");
            }

            comboSchoolYear.Text = K12.Data.School.DefaultSchoolYear;
            comboSchoolYear.DropDownStyle = ComboBoxStyle.DropDownList;

            ControlEnable(true);
        }

        private void ControlEnable(bool value)
        {
            comboSchoolYear.Enabled = dgData.Enabled = btnQuery.Enabled = btnDel.Enabled = value;
        }

        private void ClearTempData()
        {
            dgData.Rows.Clear();
            _YearScoreList.Clear();
            AllSubjDict.Clear();
            DelSubjList.Clear();
        }

        private void comboSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearTempData();
        }

        private void comboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearTempData();
        }
    }
}
