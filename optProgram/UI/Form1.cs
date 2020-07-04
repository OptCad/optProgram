using optProgram.coreTools;
using optProgram.elements;
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;

namespace optProgram.UI
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            // Initialize the dateGridView
            InitializeComponent();
            DataTable dtinit = new DataTable();
            //"物距l1", "物方孔径角u1", "物方折射率n1",
            String[] init = new String[] {
                "球面半径r", "折射率n'", "间距d"
                };
            for (int i = 0; i < init.Length; i++)
            {
                dtinit.Columns.Add(init[i]);
            }
            dGViewExcel.DataSource = dtinit;
        }
        private void getInputBtn_Click(object sender, EventArgs e)
        {
            //Read the excel file and display
            OpenFileDialog filedialog = new OpenFileDialog();
            string FileName = "";
            //Restrict file types
            filedialog.Filter = "Excel 97-2003 工作簿（*.xls）|*.xls|Excel 工作簿（*.xlsx）|*.xlsx";

            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                FileName = filedialog.FileName;

                ExcelHelper excel_helper = new ExcelHelper(FileName);
                DataTable dt = excel_helper.ExcelToDataTable("", true);
                dGViewExcel.DataSource = dt;

            }
        }

        private void outputBtn_Click(object sender, EventArgs e)
        {
            //Save the excel file.
            SaveFileDialog filedialog = new SaveFileDialog();
            string FileName = "";
            //Same as above
            filedialog.Filter = "Excel 97-2003 工作簿（*.xls）|*.xls|Excel 工作簿（*.xlsx）|*.xlsx";

            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                FileName = filedialog.FileName;

                ExcelHelper excel_helper = new ExcelHelper(FileName);
                DataTable dt = (dGViewExcel.DataSource as DataTable);
                excel_helper.DataTableToExcel(dt, "Sheet1", true);
                excel_helper.Dispose();
            }
        }

        private void infDistanceSelected(object sender, EventArgs e)
        {
            //To deal with the infinite objectDistance.
            if (infDistance.Checked == true)
            {
                objectDistance.Text = "";
                objectDistance.Enabled = false;
                apertureAngle.Text = "";
                apertureAngle.Enabled = false;
                objHeight.Text = "";
                objHeight.Enabled = false;
                fieldAngle.Enabled = true;
                pupilDiameter.Enabled = true;
            }
            else
            {
                apertureAngle.Enabled = true;
                objectDistance.Enabled = true;
                objHeight.Enabled = true;
                fieldAngle.Text = "";
                fieldAngle.Enabled = false;
                pupilDiameter.Enabled = false;
                pupilDiameter.Text = "";
            }
        }

        private void demoOutput_Click(object sender, EventArgs e)
        {
            //Template output.
            DataTable dt = (dGViewExcel.DataSource as DataTable);
            if (dt.Rows.Count != 0)
                MessageBox.Show("数据已修改，请使用输入数据导出！");
            else
            {
                SaveFileDialog filedialog = new SaveFileDialog();
                string FileName = "";
                filedialog.Filter = "Excel 97-2003 工作簿（*.xls）|*.xls|Excel 工作簿（*.xlsx）|*.xlsx";

                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = filedialog.FileName;
                    ExcelHelper excel_helper = new ExcelHelper(FileName);
                    excel_helper.DataTableToExcel(dt, "Sheet1", true);
                    excel_helper.Dispose();
                }
            }
        }

        private void cal_Click(object sender, EventArgs e)
        {
            //The entrance of the calculation system.
            try
            {
                //Read the object
                double test = double.Parse(envRefractive.Text);
                //To deal with the infinite objectDistance
                if (infDistance.Checked == false)
                {
                    test = double.Parse(objectDistance.Text);
                    test = double.Parse(apertureAngle.Text);
                    test = double.Parse(objHeight.Text);
                }
                else {
                    test = double.Parse(pupilDiameter.Text);
                    test = double.Parse(fieldAngle.Text);
                }

            }
            catch
            {
                MessageBox.Show("非法输入！");
                return;
            }
            //Construct the whole optical system.
            Queue<Sphere> inputs = new Queue<Sphere>();
            double envRefractiveIndex = double.Parse(envRefractive.Text);
            double pupilD = double.Parse(pupilDiameter.Text);
            double fieldAng = double.Parse(fieldAngle.Text);
            Obj obj;

            //TODO: 改一下
            if (infDistance.Checked == true)
                obj = new Obj(env_n:envRefractiveIndex,fieldAng:fieldAng,pupilD:pupilD);
            else
                obj = new Obj(distance: double.Parse(objectDistance.Text), angle:double.Parse(apertureAngle.Text), env_n:envRefractiveIndex);


            //Read the data from the table.
            for (int i = 0; i < dGViewExcel.Rows.Count - 1; i++)
            {
                double r_tmp = double.Parse(dGViewExcel.Rows[i].Cells[0].Value.ToString());
                double n_tmp = envRefractiveIndex;
                double d_tmp = 0;

                try
                {
                    d_tmp = double.Parse(dGViewExcel.Rows[i].Cells[2].Value.ToString());
                }
                catch { }
                try
                {
                    n_tmp = double.Parse(dGViewExcel.Rows[i].Cells[1].Value.ToString());
                }
                catch { }
                try
                {
                    pupilD = double.Parse(dGViewExcel.Rows[i].Cells[2].Value.ToString());
                }
                catch { }
                Sphere tmp = new Sphere(n_tmp, r_tmp, d_tmp);
                inputs.Enqueue(tmp);
            }
            //Calculate.
            OptSystem optSystem = new OptSystem(inputs, obj,infDistance.Checked);
            //Gaussian Optics
            //Beam output = optSystem.GaussianRefraction(new Beam(obj.objDistance, obj.apertureAngle), infDistance.Checked);
            optSystem.calAll();

        }
    }
}
