using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Data;
using FISCA.Presentation.Controls;

namespace SHDeleteStudentLearningHistory.UIForm
{
    public partial class frmDelStudentLearningHistory : FISCA.Presentation.Controls.BaseForm
    {
        private List<string> _StudentIDs;

        public frmDelStudentLearningHistory(List<string> studentIDs)
        {
            InitializeComponent();

            _StudentIDs = studentIDs == null
                ? new List<string>()
                : new List<string>(studentIDs);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDelStudentLearningHistory_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.MaximumSize = this.Size;

            InitializeFormData();
        }

        private void InitializeFormData()
        {
            InitSchoolYear();
            InitSemester();
            InitScoreType();
        }

        private void InitSchoolYear()
        {
            int defaultSchoolYear;

            if (int.TryParse(K12.Data.School.DefaultSchoolYear, out defaultSchoolYear))
            {
                cbxSchoolYear.Items.Clear();

                for (int i = -3; i <= 3; i++)
                {
                    cbxSchoolYear.Items.Add((defaultSchoolYear + i).ToString());
                }

                cbxSchoolYear.Text = defaultSchoolYear.ToString();
            }
        }

        private void InitSemester()
        {
            cbxSemester.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxSemester.Items.Clear();
            cbxSemester.Items.Add("1");
            cbxSemester.Items.Add("2");

            cbxSemester.Text = K12.Data.School.DefaultSemester;
        }

        private void InitScoreType()
        {
            cbxScoreType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxScoreType.Items.Clear();

            cbxScoreType.Items.Add("學期成績(4.2)");
            cbxScoreType.Items.Add("補修成績(4.3)");
            cbxScoreType.Items.Add("轉學轉科成績(4.4)");
            cbxScoreType.Items.Add("重修成績(5.2)");
            cbxScoreType.Items.Add("重讀成績(5.3)");
            cbxScoreType.Items.Add("學期成績(6.2)");
            cbxScoreType.Items.Add("補考成績(6.3)");
            cbxScoreType.Items.Add("轉學轉科成績(6.4)");
            cbxScoreType.Items.Add("學期成績(預檢)(41.2)");
            cbxScoreType.Items.Add("學期成績(預檢)(42.2)");

            if (cbxScoreType.Items.Count > 0)
                cbxScoreType.SelectedIndex = 0;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                btnDel.Enabled = false;

                int schoolYear;
                if (!int.TryParse(cbxSchoolYear.Text, out schoolYear))
                {
                    MsgBox.Show("請選擇學年度。");
                    return;
                }

                int semester;
                if (!int.TryParse(cbxSemester.Text, out semester) || (semester != 1 && semester != 2))
                {
                    MsgBox.Show("請選擇合法學期。");
                    return;
                }

                string scoreTypeDisplay = cbxScoreType.Text;
                if (string.IsNullOrWhiteSpace(scoreTypeDisplay))
                {
                    MsgBox.Show("請選擇成績類別。");
                    return;
                }

                string scoreName;
                string serialNo;
                if (!TryParseScoreType(scoreTypeDisplay, out scoreName, out serialNo))
                {
                    MsgBox.Show("成績類別格式無法解析。");
                    return;
                }

                List<long> studentIDs = GetValidStudentIDs();
                if (studentIDs.Count == 0)
                {
                    MsgBox.Show("沒有合法的學生系統編號，無法執行刪除。");
                    return;
                }

                DataTable data = GetLearningHistoryData(
                    schoolYear,
                    semester,
                    scoreName,
                    serialNo,
                    studentIDs);

                if (data == null || data.Rows.Count == 0)
                {
                    MsgBox.Show(string.Format(
                        "查無符合條件的學習歷程資料。\n\n學年度：{0}\n學期：{1}\n成績類別：{2}",
                        schoolYear,
                        semester,
                        scoreTypeDisplay));
                    return;
                }

                List<long> historyIDs = new List<long>();
                HashSet<long> matchedStudentIDs = new HashSet<long>();

                foreach (DataRow row in data.Rows)
                {
                    long historyID;
                    if (long.TryParse(Convert.ToString(row["id"]), out historyID))
                        historyIDs.Add(historyID);

                    long studentID;
                    if (long.TryParse(Convert.ToString(row["ref_student_id"]), out studentID))
                        matchedStudentIDs.Add(studentID);
                }

                historyIDs = historyIDs.Distinct().ToList();
                int totalStudents = matchedStudentIDs.Count;
                int totalRows = data.Rows.Count;

                string confirmMessage = string.Format(
                    "請確認要刪除以下學習歷程資料：\n\n學年度：{0}\n學期：{1}\n成績類別：{2}\n學生數：{3}\n總筆數：{4}\n\n確定要刪除嗎？",
                    schoolYear,
                    semester,
                    scoreTypeDisplay,
                    totalStudents,
                    totalRows);

                DialogResult confirmResult = MsgBox.Show(
                    confirmMessage,
                    "確認刪除",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmResult != DialogResult.OK)
                    return;

                List<long> deletedIDs = DeleteLearningHistory(historyIDs);
                int deletedCount = deletedIDs.Count;

                WriteDeleteSuccessLog(
                    schoolYear,
                    semester,
                    scoreTypeDisplay,
                    scoreName,
                    serialNo,
                    totalStudents,
                    totalRows,
                    deletedCount,
                    matchedStudentIDs.OrderBy(x => x).ToList(),
                    deletedIDs);

                try
                {
                    FISCA.Features.Invoke("StudentLearningHistoryDetailContent");
                }
                catch (Exception ex)
                {
                    // 重新整理失敗不影響已完成的刪除。
                    Console.WriteLine(
       "StudentLearningHistoryDetailContent 無法呼叫：" + ex.Message);
                }

                MsgBox.Show(string.Format(
                    "刪除完成。\n\n學年度：{0}\n學期：{1}\n成績類別：{2}\n學生數：{3}\n刪除筆數：{4}",
                    schoolYear,
                    semester,
                    scoreTypeDisplay,
                    totalStudents,
                    deletedCount));
            }
            catch (Exception ex)
            {
                try
                {
                    FISCA.LogAgent.ApplicationLog.Log(
                        "刪除學習歷程資料",
                        "刪除失敗",
                        ex.ToString());
                }
                catch
                {
                }

                MsgBox.Show("刪除學習歷程資料發生錯誤：\n" + ex.Message);
            }
            finally
            {
                btnDel.Enabled = true;
            }
        }

        private bool TryParseScoreType(
            string displayText,
            out string scoreName,
            out string serialNo)
        {
            scoreName = null;
            serialNo = null;

            if (string.IsNullOrWhiteSpace(displayText))
                return false;

            int lastOpen = displayText.LastIndexOf('(');
            int lastClose = displayText.LastIndexOf(')');

            if (lastOpen < 0 || lastClose <= lastOpen)
                return false;

            scoreName = displayText.Substring(0, lastOpen).Trim();
            serialNo = displayText.Substring(lastOpen + 1, lastClose - lastOpen - 1).Trim();

            return !string.IsNullOrEmpty(scoreName) && !string.IsNullOrEmpty(serialNo);
        }

        private List<long> GetValidStudentIDs()
        {
            List<long> result = new List<long>();

            if (_StudentIDs == null)
                return result;

            foreach (string sid in _StudentIDs)
            {
                long id;
                if (long.TryParse(sid, out id))
                    result.Add(id);
            }

            return result.Distinct().ToList();
        }

        private DataTable GetLearningHistoryData(
            int schoolYear,
            int semester,
            string scoreName,
            string serialNo,
            List<long> studentIDs)
        {
            if (studentIDs == null || studentIDs.Count == 0)
                return new DataTable();

            string sql = string.Format(@"
SELECT
    id,
    ref_student_id,
    school_year,
    semester,
    name,
    serial_no
FROM
    student_learning_history
WHERE
    school_year = {0}
    AND semester = {1}
    AND name = '{2}'
    AND serial_no = '{3}'
    AND ref_student_id IN ({4})
ORDER BY
    ref_student_id,
    id;",
                schoolYear,
                semester,
                EscapeSql(scoreName),
                EscapeSql(serialNo),
                string.Join(",", studentIDs));

            QueryHelper qh = new QueryHelper();
            return qh.Select(sql);
        }

        private List<long> DeleteLearningHistory(List<long> historyIDs)
        {
            List<long> deletedIDs = new List<long>();

            if (historyIDs == null || historyIDs.Count == 0)
                return deletedIDs;

            List<long> distinctIDs = historyIDs.Distinct().ToList();

            string sql = string.Format(@"
DELETE FROM
    student_learning_history
WHERE
    id IN ({0})
RETURNING
    id;",
                string.Join(",", distinctIDs));

            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(sql);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    long id;
                    if (long.TryParse(Convert.ToString(row["id"]), out id))
                        deletedIDs.Add(id);
                }
            }

            return deletedIDs;
        }

        private void WriteDeleteSuccessLog(
            int schoolYear,
            int semester,
            string scoreTypeDisplay,
            string scoreName,
            string serialNo,
            int totalStudents,
            int queryCount,
            int deletedCount,
            List<long> studentIDs,
            List<long> deletedIDs)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("刪除 學習歷程成績相關 資料");
            sb.AppendLine();
            sb.AppendLine("學年度：" + schoolYear);
            sb.AppendLine("學期：" + semester);
            sb.AppendLine("成績類別：" + scoreTypeDisplay);
            sb.AppendLine("name：" + scoreName);
            sb.AppendLine("serial_no：" + serialNo);
            sb.AppendLine();
            sb.AppendLine("學生數：" + totalStudents);
            sb.AppendLine("查詢總筆數：" + queryCount);
            sb.AppendLine("實際刪除筆數：" + deletedCount);

            if (queryCount != deletedCount)
            {
                sb.AppendLine();
                sb.AppendLine("警告：查詢總筆數與實際刪除筆數不一致。");
            }

            sb.AppendLine();
            sb.AppendLine("學生系統編號：");
            sb.AppendLine(string.Join(",", studentIDs));
            sb.AppendLine();
            sb.AppendLine("刪除 student_learning_history id：");
            sb.AppendLine(string.Join(",", deletedIDs));

            FISCA.LogAgent.ApplicationLog.Log(
                "刪除學習歷程資料",
                "刪除",
                sb.ToString());
        }

        private static string EscapeSql(string value)
        {
            if (value == null)
                return string.Empty;

            return value.Replace("'", "''");
        }
    }
}
