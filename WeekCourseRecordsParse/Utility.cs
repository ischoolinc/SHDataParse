using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using Aspose.Cells;

namespace WeekCourseRecordsParse
{
    public class Utility
    {
        /// <summary>
        /// 匯出 Excel
        /// </summary>
        /// <param name="inputReportName"></param>
        /// <param name="inputXls"></param>
        public static void ExportXls(string ReportName, Workbook wbXls)
        {
            string reportName = ReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xlsx");

            Workbook wb = wbXls;

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, SaveFormat.Xlsx);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xlsx";
                sd.Filter = "Excel檔案 (*.xlsx)|*.xlsx|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, SaveFormat.Xlsx);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        public static Workbook ReadXlsFile()
        {
            Workbook wb = null;
            try
            {    // 開始對話框，讓使用者選擇 Excel檔案來源
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "請選需要解析檔";
                ofd.Title = "請選需要解析檔";
                ofd.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 讀取 Excel 檔案，Excel 97 以上
                    wb = new Workbook(ofd.FileName);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return wb;
        }

        public static Dictionary<string, int> ReadWorksheetColumnDict(Worksheet wst)
        {
            Dictionary<string, int> ColIdxDic = new Dictionary<string, int>();
            int colIdx = 0;
            for (int col = 0; col <= wst.Cells.MaxDataColumn; col++)
            {
                string colName = wst.Cells[0, col].StringValue;
                if (!ColIdxDic.ContainsKey(colName))
                {
                    ColIdxDic.Add(colName, colIdx);
                    colIdx++;
                }
            }
            return ColIdxDic;
        }
    }
}
