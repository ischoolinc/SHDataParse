using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SHScoreTools.DAO
{
    /// <summary>
    /// 學生學期科目成績資訊
    /// </summary>
    public class SemsScoreInfo
    {
        // 系統編號
        public string ID { get; set; }

        // 學年度
        public string SchoolYear { get; set; }

        // 學期
        public string Semester { get; set; }

        // 年級
        public string GradeYear { get; set; }

        // 學生系統編號
        public string StudentID { get; set; }

        // 學期科目成績資料
        public string ScoreInfo { get; set; }

        // 學期科目成績資料 XML
        public XElement ScoreInfoXML { get; set; }

        // 解析成績文字成XML
        public void ParseScoreInfoToXML()
        {
            try
            {
                ScoreInfoXML = XElement.Parse(ScoreInfo);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
