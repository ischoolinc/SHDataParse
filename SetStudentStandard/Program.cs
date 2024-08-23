using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Presentation;
using FISCA.Permission;
using FISCA.Presentation.Controls;

namespace SetStudentStandard
{
    public class Program
    {
        [FISCA.MainMethod]
        public static void Main()
        {
            string regCode = "ab9bad6e-cff4-4b8b-b51a-fe57230df68d";
            RibbonBarItem rptItem = MotherForm.RibbonBarItems["課程", "教務"];
            rptItem["產生及格補考標準"].Image = Properties.Resources.calc_save_64;
            rptItem["產生及格補考標準"].Size = RibbonBarButton.MenuButtonSize.Medium;
            rptItem["產生及格補考標準"].Enable = UserAcl.Current[regCode].Executable;
            rptItem["產生及格補考標準"].Click += delegate
            {
                if (K12.Presentation.NLDPanels.Course.SelectedSource.Count > 0)
                {
                    UIForm.SetStudPassingMakeupStandard form = new UIForm.SetStudPassingMakeupStandard();
                    form.ShowDialog();
                }
                else
                {
                    MsgBox.Show("請選擇課程");
                }
            };

            Catalog catalog1a = RoleAclSource.Instance["課程"]["功能按鈕"];
            catalog1a.Add(new RibbonFeature(regCode, "產生及格補考標準"));
        }
    }
}
