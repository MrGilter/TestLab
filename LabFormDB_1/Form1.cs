using System.Configuration;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabFormDB_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            defSetings();
        }

        private void fillTheList()
        {
            DataTable dt = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow dr in dt.Rows)
            {
                string instanceName = "";
                //Вывод найденной информации о серверах
                //в элемент управления СomboBox.
                if (Convert.ToString(dr["InstanceName"]) == "")
                {
                    instanceName = "SQLEXPRESS";
                }
                else
                {
                    instanceName = dr["InstanceName"].ToString();
                }
                comboBox1.Items.Add(String.Format("{0}{1}{2}", Convert.ToString(dr["ServerName"]), "\\", instanceName));
            }
        }
        private void defSetings()
        {
            fillTheList();
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = true;
            radioButton4.Checked = false;
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            comboBox1.Enabled = radioButton1.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox1.Enabled = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton4.Checked;
            textBox3.Enabled = radioButton4.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ConfigurationManager.AppSettings.Get("Info_en"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flagConStr = false, flagLogin = false;
            if (radioButton1.Checked)
            {
                if (String.IsNullOrEmpty(comboBox1.Text))
                {
                    MessageBox.Show("ERROR - select an available connection from the list");
                }
                else
                {
                    ConnectionClass.severStr = comboBox1.Text;
                    flagConStr = true;
                }
                            
            }
            if (radioButton2.Checked)
            {
                if (String.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("ERROR - Enter connection point");
                }
                else
                {
                    if (ConnectionClass.Scan(textBox1.Text))
                    {
                        ConnectionClass.severStr = textBox1.Text;
                        flagConStr = true;
                    }
                    else
                    {
                        MessageBox.Show("ERROR - Invalid character found in connection string");
                        
                    }
                    
                }
            }
            if (radioButton3.Checked)
            {
                ConnectionClass.winAuthentication = true;
                flagLogin = true;
            }
            else
            {
                ConnectionClass.winAuthentication = false;
            }
            if (radioButton4.Checked)
            {
                if (String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("ERROR - the string 'Name' and / or 'Password' is empty");
                }
                else
                {
                    if (ConnectionClass.Scan(textBox2.Text) && ConnectionClass.Scan(textBox3.Text))
                    {
                        ConnectionClass.loginUser = textBox2.Text;
                        ConnectionClass.password = textBox3.Text;
                        flagLogin = true;
                    }
                    else
                    {
                        MessageBox.Show("ERROR LOGIN - Invalid character found in connection string");

                    }
                    

                }
            }

            if (flagConStr && flagLogin)
            {
                this.Hide();
                Form2 form2 = new Form2();

                form2.ShowDialog();

                
            }
        }
    }
    static class ConnectionClass
    {
        public static string severStr;
        public static string loginUser;
        public static string password;
        public static bool winAuthentication = false;

        public static bool Scan(string str)
        {
            string[] mas = new string[] { "@", "%", "*", " ", "--", "DROP", "#", "$", "&", "=", ";" };
            bool flag = true;
            int i = 0, d = mas.Length;
            for (i = 0; i < d; i++)
            {
                if (str.Length > mas[i].Length)
                {
                    if (str.Contains(mas[i]))
                    {
                        flag = false;
                    }
                }
            }

            return flag;
        }

        public static string ConnectionStr(string nameDB)
        {
            var conStrBild = new SqlConnectionStringBuilder();
            if (winAuthentication == true)
            {
                conStrBild.DataSource = severStr;
                conStrBild.InitialCatalog = nameDB;
                conStrBild.IntegratedSecurity = true;
            }
            else
            {

                conStrBild.DataSource = severStr;
                conStrBild.InitialCatalog = nameDB;
                conStrBild.UserID = loginUser;
                conStrBild.Password = password;
            }
            return conStrBild.ToString();
        }

        public static DataTable FillDataTable(string nameDB, string nameTable)
        {
            using (SqlConnection connectionOTP = new SqlConnection(ConnectionStr(nameDB)))
            {
                connectionOTP.Open();
                string sqlExpression = String.Format("SELECT * FROM {0}", nameTable);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connectionOTP);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }
        public static DataTable ReturnTable(string nameDB, string sqlExpression)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionStr(nameDB)))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
        }
        public static void UpdateDataTable(string nameDB,string nameTable, DataTable ObjectTable)
        {
            using (SqlConnection connectionOTP = new SqlConnection(ConnectionStr(nameDB)))
            {
                connectionOTP.Open();
                string sqlExpression = String.Format("SELECT * FROM {0}", nameTable);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connectionOTP);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(ObjectTable);
            }
        }
        public static void Execute(string nameDB,  string sqlExpression)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionStr(nameDB)))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlExpression, connection);
                cmd.ExecuteNonQuery();
            }
        }

        /*public static void Print(string a)
        {
            MessageBox.Show(a);
        }*/

       
    }
}
