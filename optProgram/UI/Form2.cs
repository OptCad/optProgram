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
        public Form2(List<string> output)
        {
            InitializeComponent();
            initRGV(output);
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

        private void initRGV(List<string> output)
        {
            DataTable dtinit = new DataTable();
            string[] tmp = new string[]
            {
                "名称",
                "波长",
                "视场",
                "孔径",
                "数值"
            };
            foreach (var item in tmp)
                dtinit.Columns.Add(item);
            tmp = new string[]
            {
                "焦距",
                "理想像距",
                "",
                "",
                "实际像位置",
                "",
                "",
                "",
                "",
                "",
                "像方主面位置",
                "出瞳距",
                "理想像高",
                "",
                "球差",
                "",
                "位置色差",
                "",
                "",
                "子午场曲",
                "弧矢场曲",
                "像散",
                "实际像高",
                "",
                "",
                "",
                "",
                "",
                "相对畸变",
                "",
                "绝对畸变",
                "",
                "倍率色差",
                "",
                "子午彗差",
                "",
                "",
                "",
            };
            foreach (var item in tmp)
                dtinit.Rows.Add(item);
            resultDGV.DataSource = dtinit;
            tmp = new string[]
            {
                "d",
                "d",
                "C",
                "F",
                "d",
                "",
                "C",
                "",
                "F",
                "",
                "d",
                "d",
                "d",
                "",
                "d",
                "",
                "F-C",
                "",
                "",
                "d",
                "d",
                "d",
                "F",
                "",
                "d",
                "",
                "C",
                "",
                "d",
                "",
                "d",
                "",
                "F-C",
                "",
                "d",
                "",
                "",
                "",
            };
            for (int i = 0; i < dtinit.Rows.Count; i++)
            {
                resultDGV.Rows[i].Cells[1].Value = tmp[i];
            }
            tmp = new string[]
            {
                "",
                "",
                "",
                "",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "",
                "",
                "1",
                "0.7",
                "0",
                "0",
                "0",
                "0",
                "0",
                "1",
                "1",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "0.7",
                "1",
                "1",
            };

            for (int i = 0; i < dtinit.Rows.Count; i++)
            {
                resultDGV.Rows[i].Cells[2].Value = tmp[i];
            }
            tmp = new string[]
            {
                "",
                "",
                "",
                "",
                "1",
                "0.7",
                "1",
                "0.7",
                "1",
                "0.7",
                "",
                "",
                "0",
                "0",
                "0.7",
                "1",
                "0.7",
                "1",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "0",
                "",
                "",
                "",
                "",
                "0",
                "0",
                "0.7",
                "1",
                "0.7",
                "1",
            };
            for (int i = 0; i < dtinit.Rows.Count; i++)
            {
                resultDGV.Rows[i].Cells[3].Value = tmp[i];
            }
            tmp = output.ToArray();
            for (int i = 0; i < dtinit.Rows.Count; i++)
            {
                resultDGV.Rows[i].Cells[4].Value = tmp[i];
            }
        }
    }
}
