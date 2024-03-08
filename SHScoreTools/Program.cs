using FISCA.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA;
using FISCA.Presentation;
using DevComponents.DotNetBar;
using SHScoreTools.UIForm;   

namespace SHScoreTools
{
    public class Program
    {
        [FISCA.MainMethod()]
        public static void main()
        {
            Catalog ribbons1 = RoleAclSource.Instance["學生"]["功能按鈕"];
            ribbons1.Add(new RibbonFeature("59545F0A-D451-4272-B0EA-BB1BCCFAD468", "刪除「學期」科目成績"));
            ribbons1.Add(new RibbonFeature("F100AEA9-E87A-48B9-AEB3-0B9B56B349D6", "刪除「學年」科目成績"));

            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學期」科目成績"].Image = Properties.Resources.exam_close_64;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學期」科目成績"].Enable = FISCA.Permission.UserAcl.Current["59545F0A-D451-4272-B0EA-BB1BCCFAD468"].Executable;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學期」科目成績"].Click += delegate {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    frmDeleteSHSemsSubjectScore ss = new frmDeleteSHSemsSubjectScore();
                    ss.SetStudentIDs(K12.Presentation.NLDPanels.Student.SelectedSource);
                    ss.ShowDialog();
                }
            };

            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學年」科目成績"].Image = Properties.Resources.subject_close_64;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學年」科目成績"].Enable = FISCA.Permission.UserAcl.Current["F100AEA9-E87A-48B9-AEB3-0B9B56B349D6"].Executable;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除「學年」科目成績"].Click += delegate {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    frmDeleteSHYearSubjectScore year = new frmDeleteSHYearSubjectScore();
                    year.SetStudentIDs(K12.Presentation.NLDPanels.Student.SelectedSource);
                    year.ShowDialog();
                }
            };
        }
    }
}
