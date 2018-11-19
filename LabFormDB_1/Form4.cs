using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabFormDB_1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        public Form4(DataTable dataTable)
        {

            DataGridView gridView = new DataGridView();
            gridView.DataSource = dataTable;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridView.Dock = DockStyle.Fill;
            this.Controls.Add(gridView);
           
        }
    }
}
