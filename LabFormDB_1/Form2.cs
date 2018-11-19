using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabFormDB_1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            FillTreeView();
           
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var q = from i in TableList.list
                    where i.NameTable == treeView1.SelectedNode.Text && i.NameDB == treeView1.SelectedNode.Parent.Text
                    select i;
            if (q.Count<ObjectTabPage>() == 0)
            {
                
                if (treeView1.SelectedNode.Parent != null)
                {
                    TableList.Add(treeView1.SelectedNode.Parent.Text, treeView1.SelectedNode.Text);
                    foreach (ObjectTabPage n in TableList.list)
                    {
                        if (n.NameDB == treeView1.SelectedNode.Parent.Text && n.NameTable == treeView1.SelectedNode.Text)
                        {
                            tabControl1.TabPages.Add(n.NameTable, n.NameTable);
                            DataGridView view = new DataGridView();
                            view.Name = n.NameTable;
                            view.Dock = DockStyle.Fill;
                            view.DataSource = n.ObjectTable;
                            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            tabControl1.TabPages[n.NameTable].Controls.Add(view);

                        }
                    }
                }
            }
            //q = null;
        }
        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            // проверяем что нажата была правая кнопка
            if (e.Button == MouseButtons.Right)
            {
                // проходим циклом по всем табам для поиска на котором был клик
                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    // получаем область таба и проверяем входит ли курсор в него или нет
                    Rectangle r = tabControl1.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        // показываем контекстое меню и сохраняем номер таба
                        //System.Diagnostics.Debug.WriteLine("TabPressed: " + i);
                        //contextMenuStrip1.Tag = i; // сохраняем номер таба
                        //contextMenuStrip1.Show((Control)sender, (e.Location));
                        
                        DialogResult result = MessageBox.Show(String.Format(" Save changes in {0}? ",tabControl1.TabPages[i].Text),"Info", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);

                        if(result == DialogResult.No)
                        {
                            TableList.Delete(tabControl1.TabPages[i].Text);
                            if (tabControl1.SelectedIndex == i && i>0)
                            {
                                tabControl1.SelectedIndex = i - 1;
                            }
                            else if (i==0 && tabControl1.SelectedIndex == i && tabControl1.TabPages.Count >= 1)
                            {
                                tabControl1.SelectedIndex++;
                            }
                            tabControl1.TabPages.Remove(tabControl1.TabPages[i]); 
                        }
                        else if(result == DialogResult.Yes)
                        {
                            TableList.SaveAndDelete(tabControl1.TabPages[i].Text);
                            if (tabControl1.SelectedIndex == i && i > 0)
                            {
                                tabControl1.SelectedIndex = i - 1;
                            }
                            else if (i == 0 && tabControl1.SelectedIndex == i && tabControl1.TabPages.Count >= 1)
                            {
                                tabControl1.SelectedIndex++;
                            }
                            tabControl1.TabPages.Remove(tabControl1.TabPages[i]);
                        }
                      
                    }
                }
            }
        }
        private void FillTreeView()
        {

            using (SqlConnection connection = new SqlConnection(ConnectionClass.ConnectionStr("master")))
            {
                List<string> nameDB = new List<string>();
               
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT name, database_id FROM sys.databases WHERE database_id>4", connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nameDB.Add(reader[0].ToString());
                            treeView1.Nodes.Add(reader[0].ToString(), reader[0].ToString());

                        }
                    }
                    foreach (string s in nameDB)
                    {
                        FillNameTable(s);
                    }
                }
                catch(SqlException e)
                {
                    MessageBox.Show("Connection problems, check the correctness of the specified data in the connection window.Or contact your SQL Server administrator.\n Error: "+e.Message);
                    Application.Exit();
                }
                finally
                {
                    
                }
                              
                

            }
            
        }
        private void FillNameTable(string nameDB)
        {
            
            using(SqlConnection con = new SqlConnection(ConnectionClass.ConnectionStr(nameDB)))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM sys.tables", con);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        treeView1.Nodes[nameDB].Nodes.Add(reader[0].ToString(), reader[0].ToString());
                    }
                }
            }


        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            TableList.Save(tabControl1.SelectedTab.Text);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                TableList.Save(tabControl1.TabPages[i].Text);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            FillTreeView();
            treeView1.SelectedNode = treeView1.Nodes[0];
            //treeView1.Focus();

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            TableList.Update(tabControl1.SelectedTab.Text);
            for(int i = 0; i < TableList.list.Count; i++)
            {
                if (tabControl1.SelectedTab.Text == TableList.list[i].NameTable)
                {
                    (tabControl1.TabPages[tabControl1.SelectedIndex].Controls[tabControl1.SelectedTab.Text] as DataGridView).DataSource = TableList.list[i].ObjectTable;
                }
            }
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show(String.Format(" Save changes in {0}? ", tabControl1.SelectedTab.Text), "Info", MessageBoxButtons.YesNo,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.No)
            {
                int select = tabControl1.SelectedIndex;
                TableList.Delete(tabControl1.SelectedTab.Text);
                if (select > 0)
                {
                    tabControl1.SelectedIndex--;
                }
                else if (select == 0 && tabControl1.TabPages.Count >= 1)
                {
                    tabControl1.SelectedIndex++;
                }
                tabControl1.TabPages.Remove(tabControl1.TabPages[select]);
            }
            else if (result == DialogResult.Yes)
            {
                int select = tabControl1.SelectedIndex;
                TableList.SaveAndDelete(tabControl1.SelectedTab.Text);
                if (select > 0)
                {
                    tabControl1.SelectedIndex--;
                }
                else if (select == 0 && tabControl1.TabPages.Count >= 1)
                {
                    tabControl1.SelectedIndex++;
                }
                tabControl1.TabPages.Remove(tabControl1.TabPages[select]);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // CREATE SQL EXPRESSION
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ConfigurationManager.AppSettings.Get("Info_help"));
        }

        private void aboutSoftwareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ConfigurationManager.AppSettings.Get("Info_about"));
        }

        private void createAShopDBAndFillItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // SHOP DB
            ShopDB.Run();
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Form frl = Application.OpenForms[0];
            frl.Show();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }

    class TableList
    {
        public static List<ObjectTabPage> list = new List<ObjectTabPage>();
        public static void Add(string nameDB, string nameTable)
        {
            list.Add(new ObjectTabPage(nameDB, nameTable));

        }
        public static void Delete(string nameTable)
        {
            
            for(int i = 0; i < list.Count; i++)
            { 
                if (list[i].NameTable == nameTable)
                {
                    list.Remove(list[i]);
            
                }
            }
        }
        public static void Save(string nameTable)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameTable == nameTable)
                {
                    list[i].SaveToServer();
                }
            }
        }
        public static void SaveAndDelete(string nameTable)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameTable == nameTable)
                {
                    list[i].SaveToServer();
                    Delete(nameTable);
                }
            }
        }
        public static void Update(string nameTable)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].NameTable == nameTable)
                {
                    list[i].UpdateDT();
                }
            }
        }
    }
    class ObjectTabPage
    {
        public string NameDB { get; private set; }
        public string NameTable { get; private set; }
        public DataTable ObjectTable { get; set; }
        public ObjectTabPage(string nameDB, string nameTable)
        {
            NameDB = nameDB;
            NameTable = nameTable;

            ObjectTable = ConnectionClass.FillDataTable(nameDB, nameTable);
            
        }
      
        public void SaveToServer()
        {
            ConnectionClass.UpdateDataTable(NameDB, NameTable, ObjectTable);
        }
        public void UpdateDT()
        {
            ObjectTable.Clear();
            ObjectTable = ConnectionClass.FillDataTable(NameDB, NameTable);
            
        }
    }
}
