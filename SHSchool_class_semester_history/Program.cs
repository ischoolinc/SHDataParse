using FISCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Permission;
using FISCA.Presentation;
using K12.Presentation;
using System.ComponentModel;
using SHSchool_class_semester_history.DAO;

namespace SHSchool_class_semester_history
{
    public class Program
    {
        static BackgroundWorker _bgLLoadUDT;

        static string UserPermissionCode = "5F410BE9-9634-470B-ACD5-ADB8AC8CCB87";

        [MainMethod()]
        public static void Main()
        {
            _bgLLoadUDT = new BackgroundWorker();
            _bgLLoadUDT.DoWork += _bgLLoadUDT_DoWork;
            _bgLLoadUDT.RunWorkerCompleted += _bgLLoadUDT_RunWorkerCompleted;
            _bgLLoadUDT.RunWorkerAsync();


            //FISCA.Permission.UserAcl.Current["791D9F02-F305-48BF-9FC5-B500363D74CE"].Executable;
            K12.Presentation.NLDPanels.Class.ListPaneContexMenu["產生班級歷程"].Click += delegate {
                if (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 0)
                {
              
                }
            };
        }

        private static void _bgLLoadUDT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // 資料項目-班級歷程
            Catalog catalog01 = RoleAclSource.Instance["班級"]["資料項目"];
            catalog01.Add(new DetailItemFeature(typeof(DetailContent.ClassSemesterHistoryContent)));

            FeatureAce UserPermission = FISCA.Permission.UserAcl.Current[UserPermissionCode];
            if (UserPermission.Editable)
                K12.Presentation.NLDPanels.Class.AddDetailBulider(new DetailBulider<DetailContent.ClassSemesterHistoryContent>());
        }

        private static void _bgLLoadUDT_DoWork(object sender, DoWorkEventArgs e)
        {
            UDTTransfer.CreateUDTTable();
        }      
    }
}
