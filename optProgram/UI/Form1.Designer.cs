namespace optProgram.UI
{
    partial class Form1
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
            this.getInputBtn = new System.Windows.Forms.Button();
            this.dGViewExcel = new System.Windows.Forms.DataGridView();
            this.outputBtn = new System.Windows.Forms.Button();
            this.infDistance = new System.Windows.Forms.CheckBox();
            this.demoOutput = new System.Windows.Forms.Button();
            this.cal = new System.Windows.Forms.Button();
            this.objectDistance = new System.Windows.Forms.TextBox();
            this.envRefractive = new System.Windows.Forms.TextBox();
            this.apertureAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pupilDiameter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fieldAngle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.objHeight = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dGViewExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // getInputBtn
            // 
            this.getInputBtn.Location = new System.Drawing.Point(22, 141);
            this.getInputBtn.Margin = new System.Windows.Forms.Padding(4);
            this.getInputBtn.Name = "getInputBtn";
            this.getInputBtn.Size = new System.Drawing.Size(212, 76);
            this.getInputBtn.TabIndex = 0;
            this.getInputBtn.Text = "输入读取";
            this.getInputBtn.UseVisualStyleBackColor = true;
            this.getInputBtn.Click += new System.EventHandler(this.getInputBtn_Click);
            // 
            // dGViewExcel
            // 
            this.dGViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGViewExcel.Location = new System.Drawing.Point(22, 261);
            this.dGViewExcel.Margin = new System.Windows.Forms.Padding(6);
            this.dGViewExcel.Name = "dGViewExcel";
            this.dGViewExcel.RowHeadersWidth = 82;
            this.dGViewExcel.RowTemplate.Height = 23;
            this.dGViewExcel.Size = new System.Drawing.Size(1143, 416);
            this.dGViewExcel.TabIndex = 1;
            // 
            // outputBtn
            // 
            this.outputBtn.Location = new System.Drawing.Point(276, 26);
            this.outputBtn.Margin = new System.Windows.Forms.Padding(4);
            this.outputBtn.Name = "outputBtn";
            this.outputBtn.Size = new System.Drawing.Size(212, 76);
            this.outputBtn.TabIndex = 2;
            this.outputBtn.Text = "输入数据导出";
            this.outputBtn.UseVisualStyleBackColor = true;
            this.outputBtn.Click += new System.EventHandler(this.outputBtn_Click);
            // 
            // infDistance
            // 
            this.infDistance.AutoSize = true;
            this.infDistance.Location = new System.Drawing.Point(542, 26);
            this.infDistance.Margin = new System.Windows.Forms.Padding(6);
            this.infDistance.Name = "infDistance";
            this.infDistance.Size = new System.Drawing.Size(162, 28);
            this.infDistance.TabIndex = 4;
            this.infDistance.Text = "物距无限远";
            this.infDistance.UseVisualStyleBackColor = true;
            this.infDistance.CheckedChanged += new System.EventHandler(this.infDistanceSelected);
            // 
            // demoOutput
            // 
            this.demoOutput.Location = new System.Drawing.Point(22, 26);
            this.demoOutput.Margin = new System.Windows.Forms.Padding(4);
            this.demoOutput.Name = "demoOutput";
            this.demoOutput.Size = new System.Drawing.Size(212, 76);
            this.demoOutput.TabIndex = 5;
            this.demoOutput.Text = "模板输出";
            this.demoOutput.UseVisualStyleBackColor = true;
            this.demoOutput.Click += new System.EventHandler(this.demoOutput_Click);
            // 
            // cal
            // 
            this.cal.Location = new System.Drawing.Point(276, 141);
            this.cal.Margin = new System.Windows.Forms.Padding(4);
            this.cal.Name = "cal";
            this.cal.Size = new System.Drawing.Size(212, 76);
            this.cal.TabIndex = 2;
            this.cal.Text = "计算";
            this.cal.UseVisualStyleBackColor = true;
            this.cal.Click += new System.EventHandler(this.cal_Click);
            // 
            // objectDistance
            // 
            this.objectDistance.Location = new System.Drawing.Point(680, 63);
            this.objectDistance.Name = "objectDistance";
            this.objectDistance.Size = new System.Drawing.Size(133, 35);
            this.objectDistance.TabIndex = 6;
            this.objectDistance.Tag = "";
            // 
            // envRefractive
            // 
            this.envRefractive.Location = new System.Drawing.Point(680, 164);
            this.envRefractive.Name = "envRefractive";
            this.envRefractive.Size = new System.Drawing.Size(133, 35);
            this.envRefractive.TabIndex = 6;
            // 
            // apertureAngle
            // 
            this.apertureAngle.Location = new System.Drawing.Point(1001, 63);
            this.apertureAngle.Name = "apertureAngle";
            this.apertureAngle.Size = new System.Drawing.Size(133, 35);
            this.apertureAngle.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(541, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "物距";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(862, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "物方孔径角";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(541, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "物方折射率";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(862, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "入瞳直径";
            // 
            // pupilDiameter
            // 
            this.pupilDiameter.Enabled = false;
            this.pupilDiameter.Location = new System.Drawing.Point(1001, 117);
            this.pupilDiameter.Name = "pupilDiameter";
            this.pupilDiameter.Size = new System.Drawing.Size(133, 35);
            this.pupilDiameter.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(541, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "半视场角";
            // 
            // fieldAngle
            // 
            this.fieldAngle.Enabled = false;
            this.fieldAngle.Location = new System.Drawing.Point(680, 117);
            this.fieldAngle.Name = "fieldAngle";
            this.fieldAngle.Size = new System.Drawing.Size(133, 35);
            this.fieldAngle.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(862, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 24);
            this.label6.TabIndex = 7;
            this.label6.Text = "物高";
            // 
            // objHeight
            // 
            this.objHeight.Location = new System.Drawing.Point(1001, 164);
            this.objHeight.Name = "objHeight";
            this.objHeight.Size = new System.Drawing.Size(133, 35);
            this.objHeight.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 759);
            this.Controls.Add(this.objectDistance);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.apertureAngle);
            this.Controls.Add(this.objHeight);
            this.Controls.Add(this.pupilDiameter);
            this.Controls.Add(this.fieldAngle);
            this.Controls.Add(this.envRefractive);
            this.Controls.Add(this.demoOutput);
            this.Controls.Add(this.infDistance);
            this.Controls.Add(this.cal);
            this.Controls.Add(this.outputBtn);
            this.Controls.Add(this.dGViewExcel);
            this.Controls.Add(this.getInputBtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dGViewExcel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getInputBtn;
        private System.Windows.Forms.DataGridView dGViewExcel;
        private System.Windows.Forms.Button outputBtn;
        private System.Windows.Forms.CheckBox infDistance;
        private System.Windows.Forms.Button demoOutput;
        private System.Windows.Forms.Button cal;
        private System.Windows.Forms.TextBox objectDistance;
        private System.Windows.Forms.TextBox envRefractive;
        private System.Windows.Forms.TextBox apertureAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pupilDiameter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox fieldAngle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox objHeight;
    }
}

