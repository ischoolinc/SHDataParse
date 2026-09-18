using FISCA.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA;
using FISCA.Presentation;
using DevComponents.DotNetBar;
using SHDeleteStudentLearningHistory.UIForm;

namespace SHDeleteStudentLearningHistory
{
    public class Program
    {
        [FISCA.MainMethod()]
        public static void main()
        {
            Catalog ribbons1 = RoleAclSource.Instance["學生"]["功能按鈕"];
            ribbons1.Add(new RibbonFeature("A55EAC32-CE29-4308-8F1E-D25AA623B3C5", "刪除學習歷程成績相關資料"));


            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學習歷程成績相關資料"].Enable = FISCA.Permission.UserAcl.Current["A55EAC32-CE29-4308-8F1E-D25AA623B3C5"].Executable;
            K12.Presentation.NLDPanels.Student.ListPaneContexMenu["刪除學習歷程成績相關資料"].Click += delegate {
                if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
                {
                    frmDeltudentLearningHistory fDLH = new frmDeltudentLearningHistory();
                    fDLH.ShowDialog();
                }
            };
        }
    }
}
