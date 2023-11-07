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
            ribbons1.Add(new RibbonFeature("59545F0A-D451-4272-B0EA-BB1BCCFAD468", "刪除學生學期科目成績"));

            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學生學期科目成績"].Enable = FISCA.Permission.UserAcl.Current["59545F0A-D451-4272-B0EA-BB1BCCFAD468"].Executable;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學生學期科目成績"].Click += delegate {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    frmDeleteSHSemsSubjectScore ss = new frmDeleteSHSemsSubjectScore();
                    ss.SetStudentIDs(K12.Presentation.NLDPanels.Student.SelectedSource);
                    ss.ShowDialog();
                }
            };
        }
    }
}
