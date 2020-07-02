using System;
using System.Data;
using System.Windows.Forms;
using optProgram.coreTools;
using optProgram.elements;

namespace optProgram.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void getInputBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            string FileName = "";
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
           SaveFileDialog filedialog = new SaveFileDialog();
            string FileName = "";
            filedialog.Filter = "Excel 97-2003 工作簿（*.xls）|*.xls|Excel 工作簿（*.xlsx）|*.xlsx";

            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                FileName = filedialog.FileName;

                ExcelHelper excel_helper = new ExcelHelper(FileName);
                DataTable dt = (dGViewExcel.DataSource as DataTable);
                excel_helper.DataTableToExcel(dt, "Sheet1", true);
            }
        }

        private void infDistanceSelected(object sender, EventArgs e)
        {
            Lens Lens1 = new Lens(1, 2);
            MessageBox.Show(Lens1.ToString());
        }
    }
}
