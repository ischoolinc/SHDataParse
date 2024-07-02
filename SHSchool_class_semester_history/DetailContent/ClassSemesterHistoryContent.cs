using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Campus.Windows;
using SHSchool_class_semester_history.DAO;

namespace SHSchool_class_semester_history.DetailContent
{
    [FISCA.Permission.FeatureCode("5F410BE9-9634-470B-ACD5-ADB8AC8CCB87", "班級歷程資料")]
    public partial class ClassSemesterHistoryContent : FISCA.Presentation.DetailContent
    {
        private ChangeListener _ChangeListener;

        EventHandler eh;
        string EventCode = "SHSchool_class_semester_history";

        // 班級歷程資料
        List<udtClassSemesterHistory> ClassSemesterHistory;
        bool _isBusy = false;
        BackgroundWorker _bgWorker;


        public ClassSemesterHistoryContent()
        {
            InitializeComponent();
            _ChangeListener = new ChangeListener();
            ClassSemesterHistory = new List<udtClassSemesterHistory>();
            this.Group = "班級歷程資料";
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            // 加入控制項變動檢查            
            _ChangeListener.Add(new DataGridViewSource(dgData));

            _ChangeListener.StatusChanged += _ChangeListener_StatusChanged;

            //啟動更新事件
            eh = FISCA.InteractionService.PublishEvent(EventCode);

            FISCA.InteractionService.SubscribeEvent("SHSchool_class_semester_history", (sender, args) =>
            {
                ReloadData();
            });

        }

        private void _ChangeListener_StatusChanged(object sender, ChangeEventArgs e)
        {
            this.CancelButtonVisible = (e.Status == ValueStatus.Dirty);
            this.SaveButtonVisible = (e.Status == ValueStatus.Dirty);
        }

        private void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isBusy)
            {
                _isBusy = false;
                _bgWorker.RunWorkerAsync();
                return;
            }
            LoadData();
        }

        private void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 依班級 ID 取得班級歷程資料
            ClassSemesterHistory = UDTTransfer.GetClassSemesterHistoryByClassID(PrimaryKey);
        }

        private void LoadData()
        {
            _ChangeListener.SuspendListen();
            dgData.Rows.Clear();
            foreach (udtClassSemesterHistory data in ClassSemesterHistory)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = data;
                dgData.Rows[rowIdx].Cells[colSchoolYear.Index].Value = data.SchoolYear;
                dgData.Rows[rowIdx].Cells[colSemester.Index].Value = data.Semester;
                dgData.Rows[rowIdx].Cells[colGradeYear.Index].Value = data.GradeYear;
                dgData.Rows[rowIdx].Cells[colDeptName.Index].Value = data.DeptName;
                dgData.Rows[rowIdx].Cells[colClassName.Index].Value = data.ClassName;
                dgData.Rows[rowIdx].Cells[colClassTeacher.Index].Value = data.ClassTeacher;
                dgData.Rows[rowIdx].Cells[colClassID.Index].Value = data.RefClassID;
                dgData.Rows[rowIdx].Cells[colClassNumber.Index].Value = data.RefClassNumber;
            }


            _ChangeListener.Reset();
            _ChangeListener.ResumeListen();
            this.Loading = false;
        }

        private void _BGRun()
        {
            if (_bgWorker.IsBusy)
                _isBusy = true;
            else
                _bgWorker.RunWorkerAsync();
        }

        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            ReloadData();
        }

        private void ReloadData()
        {
            this.Loading = true;
            this.CancelButtonVisible = false;
            this.SaveButtonVisible = false;
            // 清空 DataGridView
            dgData.Rows.Clear();

            _BGRun();
        }

        protected override void OnSaveButtonClick(EventArgs e)
        {
            //Save();
            _BGRun();
        }

        protected override void OnCancelButtonClick(EventArgs e)
        {
            this.CancelButtonVisible = false;
            this.SaveButtonVisible = false;
            LoadData();
        }

    }
}
