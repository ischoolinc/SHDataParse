using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.Permission;
using FISCA.Presentation.Controls;
using K12.Data;
using FISCA;
using FISCA.Presentation;
using WeekCourseRecordsParse.winForm;

namespace WeekCourseRecordsParse
{
    public class Program
    {
        [FISCA.MainMethod()]
        public static void main()
        {
            MotherForm.RibbonBarItems["課程", "其它"]["課程週課表解析"].Enable = UserAcl.Current["5B05FBB6-45D1-4FDD-BAE9-7A446986FCDE"].Executable;
            MotherForm.RibbonBarItems["課程", "其它"]["課程週課表解析"].Click += delegate
            {
                MainForm mf = new MainForm();
                mf.ShowDialog();
            };
            MotherForm.RibbonBarItems["課程", "其它"]["課程週課表解析"].Image = Properties.Resources.laptop;
            MotherForm.RibbonBarItems["課程", "其它"]["課程週課表解析"].Size = RibbonBarButton.MenuButtonSize.Medium;

            // 權限註冊
            Catalog catalog1 = RoleAclSource.Instance["課程"]["功能按鈕"];
            catalog1.Add(new RibbonFeature("5B05FBB6-45D1-4FDD-BAE9-7A446986FCDE", "課程週課表解析"));
        }
    }
}
