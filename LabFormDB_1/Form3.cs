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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Enter sql query");
            }
            else { ConnectionClass.Execute(textBox2.Text, textBox1.Text); }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Enter sql query");
            }
            else if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Enter name database");
            }
            else
            {
                DataTable dataTable = ConnectionClass.ReturnTable(textBox2.Text, textBox1.Text);
                Form4 form4 = new Form4(dataTable);
                form4.Show();

                
            }
        }
    }
}
