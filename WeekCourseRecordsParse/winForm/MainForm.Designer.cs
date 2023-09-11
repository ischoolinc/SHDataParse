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
            this.buttonParse03 = new DevComponents.DotNetBar.ButtonX();
            this.grpCustomer = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtSeparator = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkCourseTeacher = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtClassNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkClassNumber = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtTeacherName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkTeacherName = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtClassName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkClassName = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtSubjectName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkSubjectName = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnAnalysis = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.grpCustomer.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonParse01
            // 
            this.buttonParse01.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonParse01.BackColor = System.Drawing.Color.Transparent;
            this.buttonParse01.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonParse01.Location = new System.Drawing.Point(22, 47);
            this.buttonParse01.Name = "buttonParse01";
            this.buttonParse01.Size = new System.Drawing.Size(326, 23);
            this.buttonParse01.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonParse01.TabIndex = 1;
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
            this.labelSchoolYear.TabIndex = 0;
            this.labelSchoolYear.Text = "111學年度第1學期";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(234, 467);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(116, 24);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // buttonParse02
            // 
            this.buttonParse02.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonParse02.BackColor = System.Drawing.Color.Transparent;
            this.buttonParse02.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonParse02.Location = new System.Drawing.Point(22, 84);
            this.buttonParse02.Name = "buttonParse02";
            this.buttonParse02.Size = new System.Drawing.Size(326, 23);
            this.buttonParse02.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonParse02.TabIndex = 2;
            this.buttonParse02.Text = "讀取欣河資料並轉換";
            this.buttonParse02.Click += new System.EventHandler(this.buttonParse02_Click);
            // 
            // buttonParse03
            // 
            this.buttonParse03.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonParse03.BackColor = System.Drawing.Color.Transparent;
            this.buttonParse03.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonParse03.Location = new System.Drawing.Point(22, 121);
            this.buttonParse03.Name = "buttonParse03";
            this.buttonParse03.Size = new System.Drawing.Size(326, 23);
            this.buttonParse03.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonParse03.TabIndex = 3;
            this.buttonParse03.Text = "讀取(科目名稱+所屬班級+授課教師一)並轉換";
            this.buttonParse03.Click += new System.EventHandler(this.buttonParse03_Click);
            // 
            // grpCustomer
            // 
            this.grpCustomer.BackColor = System.Drawing.Color.Transparent;
            this.grpCustomer.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpCustomer.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpCustomer.Controls.Add(this.txtSeparator);
            this.grpCustomer.Controls.Add(this.chkCourseTeacher);
            this.grpCustomer.Controls.Add(this.txtClassNumber);
            this.grpCustomer.Controls.Add(this.chkClassNumber);
            this.grpCustomer.Controls.Add(this.txtTeacherName);
            this.grpCustomer.Controls.Add(this.chkTeacherName);
            this.grpCustomer.Controls.Add(this.txtClassName);
            this.grpCustomer.Controls.Add(this.chkClassName);
            this.grpCustomer.Controls.Add(this.txtSubjectName);
            this.grpCustomer.Controls.Add(this.chkSubjectName);
            this.grpCustomer.Controls.Add(this.btnAnalysis);
            this.grpCustomer.Controls.Add(this.labelX1);
            this.grpCustomer.Location = new System.Drawing.Point(22, 151);
            this.grpCustomer.Name = "grpCustomer";
            this.grpCustomer.Size = new System.Drawing.Size(328, 242);
            // 
            // 
            // 
            this.grpCustomer.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpCustomer.Style.BackColorGradientAngle = 90;
            this.grpCustomer.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpCustomer.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCustomer.Style.BorderBottomWidth = 1;
            this.grpCustomer.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpCustomer.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCustomer.Style.BorderLeftWidth = 1;
            this.grpCustomer.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCustomer.Style.BorderRightWidth = 1;
            this.grpCustomer.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpCustomer.Style.BorderTopWidth = 1;
            this.grpCustomer.Style.Class = "";
            this.grpCustomer.Style.CornerDiameter = 4;
            this.grpCustomer.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpCustomer.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpCustomer.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpCustomer.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.grpCustomer.StyleMouseDown.Class = "";
            this.grpCustomer.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.grpCustomer.StyleMouseOver.Class = "";
            this.grpCustomer.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.grpCustomer.TabIndex = 5;
            this.grpCustomer.Text = "自訂欄位解析";
            // 
            // txtSeparator
            // 
            // 
            // 
            // 
            this.txtSeparator.Border.Class = "TextBoxBorder";
            this.txtSeparator.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSeparator.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSeparator.Location = new System.Drawing.Point(280, 135);
            this.txtSeparator.Name = "txtSeparator";
            this.txtSeparator.Size = new System.Drawing.Size(39, 29);
            this.txtSeparator.TabIndex = 10;
            this.txtSeparator.Text = ",";
            this.txtSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkCourseTeacher
            // 
            // 
            // 
            // 
            this.chkCourseTeacher.BackgroundStyle.Class = "";
            this.chkCourseTeacher.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkCourseTeacher.Location = new System.Drawing.Point(4, 185);
            this.chkCourseTeacher.Name = "chkCourseTeacher";
            this.chkCourseTeacher.Size = new System.Drawing.Size(121, 23);
            this.chkCourseTeacher.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkCourseTeacher.TabIndex = 9;
            this.chkCourseTeacher.Text = "同步產生配課表";
            // 
            // txtClassNumber
            // 
            // 
            // 
            // 
            this.txtClassNumber.Border.Class = "TextBoxBorder";
            this.txtClassNumber.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtClassNumber.Location = new System.Drawing.Point(118, 96);
            this.txtClassNumber.Name = "txtClassNumber";
            this.txtClassNumber.Size = new System.Drawing.Size(201, 25);
            this.txtClassNumber.TabIndex = 8;
            // 
            // chkClassNumber
            // 
            // 
            // 
            // 
            this.chkClassNumber.BackgroundStyle.Class = "";
            this.chkClassNumber.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkClassNumber.Location = new System.Drawing.Point(4, 96);
            this.chkClassNumber.Name = "chkClassNumber";
            this.chkClassNumber.Size = new System.Drawing.Size(108, 23);
            this.chkClassNumber.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkClassNumber.TabIndex = 7;
            this.chkClassNumber.Text = "班碼名稱欄位";
            // 
            // txtTeacherName
            // 
            // 
            // 
            // 
            this.txtTeacherName.Border.Class = "TextBoxBorder";
            this.txtTeacherName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTeacherName.Location = new System.Drawing.Point(118, 139);
            this.txtTeacherName.Name = "txtTeacherName";
            this.txtTeacherName.Size = new System.Drawing.Size(97, 25);
            this.txtTeacherName.TabIndex = 6;
            // 
            // chkTeacherName
            // 
            // 
            // 
            // 
            this.chkTeacherName.BackgroundStyle.Class = "";
            this.chkTeacherName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkTeacherName.Location = new System.Drawing.Point(4, 139);
            this.chkTeacherName.Name = "chkTeacherName";
            this.chkTeacherName.Size = new System.Drawing.Size(108, 23);
            this.chkTeacherName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkTeacherName.TabIndex = 5;
            this.chkTeacherName.Text = "教師名稱欄位";
            // 
            // txtClassName
            // 
            // 
            // 
            // 
            this.txtClassName.Border.Class = "TextBoxBorder";
            this.txtClassName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtClassName.Location = new System.Drawing.Point(118, 56);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(201, 25);
            this.txtClassName.TabIndex = 4;
            // 
            // chkClassName
            // 
            // 
            // 
            // 
            this.chkClassName.BackgroundStyle.Class = "";
            this.chkClassName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkClassName.Location = new System.Drawing.Point(4, 56);
            this.chkClassName.Name = "chkClassName";
            this.chkClassName.Size = new System.Drawing.Size(108, 23);
            this.chkClassName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkClassName.TabIndex = 3;
            this.chkClassName.Text = "班級名稱欄位";
            // 
            // txtSubjectName
            // 
            // 
            // 
            // 
            this.txtSubjectName.Border.Class = "TextBoxBorder";
            this.txtSubjectName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSubjectName.Location = new System.Drawing.Point(118, 14);
            this.txtSubjectName.Name = "txtSubjectName";
            this.txtSubjectName.Size = new System.Drawing.Size(201, 25);
            this.txtSubjectName.TabIndex = 2;
            // 
            // chkSubjectName
            // 
            // 
            // 
            // 
            this.chkSubjectName.BackgroundStyle.Class = "";
            this.chkSubjectName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSubjectName.Location = new System.Drawing.Point(4, 14);
            this.chkSubjectName.Name = "chkSubjectName";
            this.chkSubjectName.Size = new System.Drawing.Size(108, 23);
            this.chkSubjectName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSubjectName.TabIndex = 1;
            this.chkSubjectName.Text = "科目名稱欄位";
            // 
            // btnAnalysis
            // 
            this.btnAnalysis.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAnalysis.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAnalysis.Location = new System.Drawing.Point(208, 185);
            this.btnAnalysis.Name = "btnAnalysis";
            this.btnAnalysis.Size = new System.Drawing.Size(111, 23);
            this.btnAnalysis.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAnalysis.TabIndex = 0;
            this.btnAnalysis.Text = "解析";
            this.btnAnalysis.Click += new System.EventHandler(this.btnAnalysis_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(221, 139);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 23);
            this.labelX1.TabIndex = 11;
            this.labelX1.Text = "分隔符號";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(22, 399);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(328, 62);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "請勾選比對欄位，並輸入欄位中對應科目、班級名稱、班碼及教師名稱等欄位之實際名稱，如果匯入檔案中沒有上述欄位則保留空白";
            this.labelX2.WordWrap = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 497);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.grpCustomer);
            this.Controls.Add(this.buttonParse03);
            this.Controls.Add(this.buttonParse02);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelSchoolYear);
            this.Controls.Add(this.buttonParse01);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "課程週課表解析";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpCustomer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonParse01;
        private DevComponents.DotNetBar.LabelX labelSchoolYear;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX buttonParse02;
        private DevComponents.DotNetBar.ButtonX buttonParse03;
        private DevComponents.DotNetBar.Controls.GroupPanel grpCustomer;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSubjectName;
        private DevComponents.DotNetBar.ButtonX btnAnalysis;
        private DevComponents.DotNetBar.Controls.TextBoxX txtClassNumber;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkClassNumber;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTeacherName;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTeacherName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtClassName;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkClassName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSubjectName;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkCourseTeacher;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSeparator;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}