using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.DSAUtil;

namespace SHSchool_class_semester_history.DAO
{
    public class UDTTransfer
    {
        /// <summary>
        /// 建立使用到的 UDT Table
        /// </summary>
        public static void CreateUDTTable()
        {
            FISCA.UDT.SchemaManager Manager = new SchemaManager(new DSConnection(FISCA.Authentication.DSAServices.DefaultDataSource));
            Manager.SyncSchema(new udtClassSemesterHistory()); 
        }

        // 透過班級 ID 取得班級歷程
        public static List<udtClassSemesterHistory> GetClassSemesterHistoryByClassID(string ClassID)
        {
            AccessHelper access = new AccessHelper();
            List<udtClassSemesterHistory> retVal = access.Select<udtClassSemesterHistory>(string.Format("ref_class_id = {0}", ClassID));
            return retVal;
        }

    }
}
