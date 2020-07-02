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
            ((System.ComponentModel.ISupportInitialize)(this.dGViewExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // getInputBtn
            // 
            this.getInputBtn.Location = new System.Drawing.Point(11, 11);
            this.getInputBtn.Margin = new System.Windows.Forms.Padding(2);
            this.getInputBtn.Name = "getInputBtn";
            this.getInputBtn.Size = new System.Drawing.Size(106, 38);
            this.getInputBtn.TabIndex = 0;
            this.getInputBtn.Text = "读取";
            this.getInputBtn.UseVisualStyleBackColor = true;
            this.getInputBtn.Click += new System.EventHandler(this.getInputBtn_Click);
            // 
            // dGViewExcel
            // 
            this.dGViewExcel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGViewExcel.Location = new System.Drawing.Point(12, 54);
            this.dGViewExcel.Name = "dGViewExcel";
            this.dGViewExcel.RowTemplate.Height = 23;
            this.dGViewExcel.Size = new System.Drawing.Size(431, 208);
            this.dGViewExcel.TabIndex = 1;
            // 
            // outputBtn
            // 
            this.outputBtn.Location = new System.Drawing.Point(140, 11);
            this.outputBtn.Margin = new System.Windows.Forms.Padding(2);
            this.outputBtn.Name = "outputBtn";
            this.outputBtn.Size = new System.Drawing.Size(106, 38);
            this.outputBtn.TabIndex = 2;
            this.outputBtn.Text = "输出";
            this.outputBtn.UseVisualStyleBackColor = true;
            this.outputBtn.Click += new System.EventHandler(this.outputBtn_Click);
            // 
            // infDistance
            // 
            this.infDistance.AutoSize = true;
            this.infDistance.Location = new System.Drawing.Point(266, 13);
            this.infDistance.Name = "infDistance";
            this.infDistance.Size = new System.Drawing.Size(84, 16);
            this.infDistance.TabIndex = 4;
            this.infDistance.Text = "物距无限远";
            this.infDistance.UseVisualStyleBackColor = true;
            this.infDistance.CheckedChanged += new System.EventHandler(this.infDistanceSelected);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 290);
            this.Controls.Add(this.infDistance);
            this.Controls.Add(this.outputBtn);
            this.Controls.Add(this.dGViewExcel);
            this.Controls.Add(this.getInputBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
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
    }
}

