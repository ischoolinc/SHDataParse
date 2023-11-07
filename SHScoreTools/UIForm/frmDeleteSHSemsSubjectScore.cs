using SHScoreTools.DAO;
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

namespace SHScoreTools.UIForm
{
    public partial class frmDeleteSHSemsSubjectScore : FISCA.Presentation.Controls.BaseForm
    {

        // 選擇學生ID集合
        List<string> _StudentIDList;

        // 學期科目成績集合
        List<SemsScoreInfo> _SemsScoreList;

        // 所有科目名稱
        Dictionary<string, SubjInfo> AllSubjDict;

        // 需要刪除科目名稱
        List<SubjInfo> DelSubjList;

        // 選擇學年度
        string _SchoolYear = "";

        // 選擇學期
        string _Semester = "";

        // 查詢資料
        BackgroundWorker _bgWorkerLoadData;

        // 刪除資料用
        BackgroundWorker _bgWorkerDelData;

        public frmDeleteSHSemsSubjectScore()
        {
            InitializeComponent();
            _StudentIDList = new List<string>();
            _SemsScoreList = new List<SemsScoreInfo>();
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
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學期科目成績刪除中...", e.ProgressPercentage);
        }

        private void _bgWorkerDelData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");

            // 重新載入
            ControlEnable(false);
            _bgWorkerLoadData.RunWorkerAsync();
        }

        private void _bgWorkerDelData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgWorkerDelData.ReportProgress(10);

            // 需要刪除科目
            List<SemsScoreInfo> DelSemsScoreList = new List<SemsScoreInfo>();

            // 比對刪除資料
            foreach (SemsScoreInfo ss in _SemsScoreList)
            {
                bool isDel = false;
                foreach (SubjInfo si in DelSubjList)
                {
                    foreach (XElement elm in ss.ScoreInfoXML.Elements("Subject"))
                    {
                        if (elm.Attribute("科目").Value == si.SubjectName && elm.Attribute("科目級別").Value == si.SubjectLevel)
                        {
                            elm.Remove();
                            isDel = true;
                        }
                    }
                }
                if (isDel)
                    DelSemsScoreList.Add(ss);
            }

            _bgWorkerDelData.ReportProgress(100);
        }

        private void _bgWorkerLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("學期科目成績讀取中...", e.ProgressPercentage);

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
                dgData.Rows[rowIdx].Cells["科目級別"].Value = AllSubjDict[key].SubjectLevel;
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
                chkCol1.Width = 30;
                chkCol1.TrueValue = "是";
                chkCol1.FalseValue = "否";
                chkCol1.IndeterminateValue = "否";

                dgData.Columns.Add(chkCol1);

                string textColumnStrig = @"
                    [
                        {
                            ""HeaderText"": ""科目名稱"",
                            ""Name"": ""科目名稱"",
                            ""Width"": 300,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""科目級別"",
                            ""Name"": ""科目級別"",
                            ""Width"": 40,
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

            // 讀取學期科目成績
            _SemsScoreList = DataAccess.GetStudentSemsScoreInfoBySchoolYearSemester(_SchoolYear, _Semester, _StudentIDList);

            _bgWorkerLoadData.ReportProgress(80);

            // 整理科目名稱
            AllSubjDict.Clear();
            DelSubjList.Clear();
            foreach (SemsScoreInfo ss in _SemsScoreList)
            {
                foreach (XElement elm in ss.ScoreInfoXML.Elements("Subject"))
                {
                    string key = elm.Attribute("科目").Value + "_" + elm.Attribute("科目級別").Value;
                    if (!AllSubjDict.ContainsKey(key))
                    {
                        SubjInfo si = new SubjInfo();
                        si.SubjectName = elm.Attribute("科目").Value;
                        si.SubjectLevel = elm.Attribute("科目級別").Value;
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
            ControlEnable(false);

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

            if (MsgBox.Show("請問要刪除 " + DelSubjList.Count + " 科目成績? ", "刪除學期科目成績", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                _bgWorkerDelData.RunWorkerAsync();
            }

            ControlEnable(true);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            // 寫入畫面值
            _SchoolYear = comboSchoolYear.Text;
            _Semester = comboSemester.Text;
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

            // 取得預設學年度學期
            int sy = 0;
            int.TryParse(K12.Data.School.DefaultSchoolYear, out sy);

            for (int i = sy + 1; i >= sy - 4; i--)
            {
                comboSchoolYear.Items.Add(i + "");
            }
            comboSemester.Items.Add("1");
            comboSemester.Items.Add("2");

            comboSchoolYear.Text = K12.Data.School.DefaultSchoolYear;
            comboSemester.Text = K12.Data.School.DefaultSemester;
            comboSchoolYear.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSemester.DropDownStyle = ComboBoxStyle.DropDownList;

            ControlEnable(true);
        }

        private void ControlEnable(bool value)
        {
            comboSchoolYear.Enabled = comboSemester.Enabled = dgData.Enabled = btnQuery.Enabled = btnDel.Enabled = value;
        }
    }
}
