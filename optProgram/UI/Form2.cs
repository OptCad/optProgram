using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using optProgram.coreTools;

namespace optProgram.UI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void resultBtn_Click(object sender, EventArgs e)
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
                DataTable dt = (resultDGV.DataSource as DataTable);
                excel_helper.DataTableToExcel(dt, "Sheet1", true);
                excel_helper.Dispose();
            }
        }
    }
}
