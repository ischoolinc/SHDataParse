using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SHSchool_class_semester_history.UIForm
{
    public partial class frmCreateClassSemsHistory : FISCA.Presentation.Controls.BaseForm
    {
        public frmCreateClassSemsHistory()
        {
            InitializeComponent();
        }

        private void frmCreateClassSemsHistory_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
        }
    }
}
