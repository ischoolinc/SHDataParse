namespace WeekCourseRecordsParse.winForm
{
    partial class MainForm
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
            this.buttonParse01 = new DevComponents.DotNetBar.ButtonX();
            this.labelSchoolYear = new DevComponents.DotNetBar.LabelX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.buttonParse02 = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // buttonParse01
            // 
            this.buttonParse01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonParse01.BackColor = System.Drawing.Color.Transparent;
            this.buttonParse01.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonParse01.Location = new System.Drawing.Point(22, 47);
            this.buttonParse01.Name = "buttonParse01";
            this.buttonParse01.Size = new System.Drawing.Size(167, 23);
            this.buttonParse01.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonParse01.TabIndex = 0;
            this.buttonParse01.Text = "讀取全訊資料並轉換";
            this.buttonParse01.Click += new System.EventHandler(this.buttonParse01_Click);
            // 
            // labelSchoolYear
            // 
            this.labelSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelSchoolYear.BackgroundStyle.Class = "";
            this.labelSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelSchoolYear.Location = new System.Drawing.Point(22, 12);
            this.labelSchoolYear.Name = "labelSchoolYear";
            this.labelSchoolYear.Size = new System.Drawing.Size(167, 23);
            this.labelSchoolYear.TabIndex = 1;
            this.labelSchoolYear.Text = "111學年度第1學期";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(109, 132);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 23);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // buttonParse02
            // 
            this.buttonParse02.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonParse02.BackColor = System.Drawing.Color.Transparent;
            this.buttonParse02.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonParse02.Location = new System.Drawing.Point(22, 86);
            this.buttonParse02.Name = "buttonParse02";
            this.buttonParse02.Size = new System.Drawing.Size(167, 23);
            this.buttonParse02.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonParse02.TabIndex = 3;
            this.buttonParse02.Text = "讀取欣河資料並轉換";
            this.buttonParse02.Click += new System.EventHandler(this.buttonParse02_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 167);
            this.Controls.Add(this.buttonParse02);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelSchoolYear);
            this.Controls.Add(this.buttonParse01);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "課程週課表解析";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonParse01;
        private DevComponents.DotNetBar.LabelX labelSchoolYear;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX buttonParse02;
    }
}