using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SetStudentStandard.DAO
{
    public class StudentScoreRuleInfo
    {
        // 學生系統編號
        public string StudentID { get; set; }

        // 一年級及格標準
        public decimal? Grade1PassingStandard { get; set; }

        // 一年級補考標準
        public decimal? Grade1MakeupStandard { get; set; }

        // 二年級及格標準
        public decimal? Grade2PassingStandard { get; set; }

        // 二年級補考標準
        public decimal? Grade2MakeupStandard { get; set; }

        // 三年級及格標準
        public decimal? Grade3PassingStandard { get; set; }

        // 三年級補考標準
        public decimal? Grade3MakeupStandard { get; set; }

        // 四年級及格標準
        public decimal? Grade4PassingStandard { get; set; }

        // 四年級補考標準
        public decimal? Grade4MakeupStandard { get; set; }

        // 學生類別
        public List<string> StudentTags = new List<string>();

        // 學生成績計算規則 XML
        public XElement ScoreCalcRuleXml { get; set; }

        // 解析及格與補考表準
        public void ParsePassingMakeupStandard()
        {
            if (ScoreCalcRuleXml != null)
            {
                StudentTags.Add("預設");
                if (ScoreCalcRuleXml.Element("及格標準") != null)
                {
                    foreach (XElement elm in ScoreCalcRuleXml.Element("及格標準").Elements("學生類別"))
                    {
                        decimal gp1, gp2, gp3, gp4, gm1, gm2, gm3, gm4;

                        // 類別
                        string tagName = GetAttribute(elm, "類別");

                        // 有符合類別才判斷
                        if (!StudentTags.Contains(tagName))
                            continue;

                        if (decimal.TryParse(GetAttribute(elm, "一年級及格標準"), out gp1))
                        {
                            if (gp1 < Grade1PassingStandard)
                                Grade1PassingStandard = gp1;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "二年級及格標準"), out gp2))
                        {
                            if (gp2 < Grade2PassingStandard)
                                Grade2PassingStandard = gp2;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "三年級及格標準"), out gp3))
                        {
                            if (gp3 < Grade3PassingStandard)
                                Grade3PassingStandard = gp3;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "四年級及格標準"), out gp4))
                        {
                            if (gp4 < Grade4PassingStandard)
                                Grade4PassingStandard = gp4;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "一年級補考標準"), out gm1))
                        {
                            if (gm1 < Grade1MakeupStandard)
                                Grade1MakeupStandard = gm1;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "二年級補考標準"), out gm2))
                        {
                            if (gm2 < Grade2MakeupStandard)
                                Grade2MakeupStandard = gm2;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "三年級補考標準"), out gm3))
                        {
                            if (gm3 < Grade3MakeupStandard)
                                Grade3MakeupStandard = gm3;
                        }

                        if (decimal.TryParse(GetAttribute(elm, "四年級補考標準"), out gm4))
                        {
                            if (gm4 < Grade4MakeupStandard)
                                Grade4MakeupStandard = gm4;
                        }
                    }
                }
            }
        }

        private string GetAttribute(XElement elm, string name)
        {
            if (elm.Attribute(name) != null)
                return elm.Attribute(name).Value;
            else
                return "";
        }
    }
}
