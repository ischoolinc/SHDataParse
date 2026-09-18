namespace SHDeleteStudentLearningHistory.UIForm
{
    partial class frmDeltudentLearningHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbxSchoolYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbxSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cbxScoreType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // cbxSchoolYear
            // 
            this.cbxSchoolYear.DisplayMember = "Text";
            this.cbxSchoolYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxSchoolYear.FormattingEnabled = true;
            this.cbxSchoolYear.ItemHeight = 19;
            this.cbxSchoolYear.Location = new System.Drawing.Point(88, 19);
            this.cbxSchoolYear.Name = "cbxSchoolYear";
            this.cbxSchoolYear.Size = new System.Drawing.Size(86, 25);
            this.cbxSchoolYear.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxSchoolYear.TabIndex = 0;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(37, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(45, 32);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "學年度";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(200, 15);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(36, 32);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "學期";
            // 
            // cbxSemester
            // 
            this.cbxSemester.DisplayMember = "Text";
            this.cbxSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxSemester.FormattingEnabled = true;
            this.cbxSemester.ItemHeight = 19;
            this.cbxSemester.Location = new System.Drawing.Point(242, 19);
            this.cbxSemester.Name = "cbxSemester";
            this.cbxSemester.Size = new System.Drawing.Size(60, 25);
            this.cbxSemester.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxSemester.TabIndex = 2;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(22, 56);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(60, 32);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "成績類別";
            // 
            // cbxScoreType
            // 
            this.cbxScoreType.DisplayMember = "Text";
            this.cbxScoreType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbxScoreType.FormattingEnabled = true;
            this.cbxScoreType.ItemHeight = 19;
            this.cbxScoreType.Location = new System.Drawing.Point(88, 60);
            this.cbxScoreType.Name = "cbxScoreType";
            this.cbxScoreType.Size = new System.Drawing.Size(214, 25);
            this.cbxScoreType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbxScoreType.TabIndex = 4;
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(109, 108);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(82, 26);
            this.btnDel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "刪除";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(220, 108);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 26);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmDeltudentLearningHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 150);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cbxScoreType);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.cbxSemester);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cbxSchoolYear);
            this.DoubleBuffered = true;
            this.Name = "frmDeltudentLearningHistory";
            this.Text = "刪除學習歷程成績相關資料";
            this.Load += new System.EventHandler(this.frmDeltudentLearningHistory_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxSchoolYear;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxSemester;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbxScoreType;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnExit;
    }
}