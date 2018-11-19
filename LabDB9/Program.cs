using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// конструкторы классов
namespace LabDB9
{
    class Stydent
    {
        private string name;
        private int kurs;
        private double grade_point_average;
        private int Id;
        private string login;

        public Stydent()
        {
            name = "Unknown";
            kurs = 0;
            grade_point_average = 0;
            Id = 0;
        }
        public Stydent(string name, int kurs, double grade_point_average)
        {
            this.name = name;
            this.kurs = kurs;
            this.grade_point_average = grade_point_average;
        }
        public Stydent(string name, int kurs, double grade_point_average, int id, string login)
        {
            this.name = name;
            this.kurs = kurs;
            this.grade_point_average = grade_point_average;
            this.Id = id;
            this.login = login;

        }

        public void GetInfo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Name :   ");
            Console.ResetColor();
            Console.Write(name + "\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("ID :   ");
            Console.ResetColor();
            Console.Write(Id + "\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Kurs :   ");
            Console.ResetColor();
            Console.Write(kurs + "\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Grade point average :   ");
            Console.ResetColor();
            Console.Write(grade_point_average + "\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Login :   ");
            Console.ResetColor();
            Console.Write(login + "\n\n\n");
        }

    }

    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";
        static string sqlExpression2 = "SELECT * FROM TEST_lab_tab WHERE text_data_1 LIKE '%Stydent%'";
        //connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Test_lab; User Id = StupidUserTest; Password = 2046"
        static void Main(string[] args)
        {
            Regex re = new Regex(@"\d+");
            string a, n;
            int t = 0, k, u;
            double g;
            bool flag = true;
            List<Stydent> stydents = new List<Stydent>();
            DataTable table = ReadDT();
            while (flag == true)
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    stydents.Add(new Stydent(Convert.ToString(table.Rows[i]["Comment_text_1"]), Convert.ToInt32(table.Rows[i]["int_data_1"]), Convert.ToDouble(table.Rows[i]["float_data_1"]), Convert.ToInt32(table.Rows[i]["Id"]), Convert.ToString(table.Rows[i]["text_data_1"])));
                    Match m = re.Match((string)table.Rows[i]["text_data_1"]);

                    if (m.Success)
                    {
                        if (t < Convert.ToInt32(m.Value.ToString()))
                        {
                            t = Convert.ToInt32(m.Value.ToString());
                        }
                    }
                }
                foreach (var s in stydents)
                {
                    s.GetInfo();
                }
                Console.WriteLine("Add Stydent press to 'Y'\nExit - 'N'");
                a = Console.ReadLine();
                switch (a)
                {
                    case "Y":
                        Console.WriteLine("Name");
                        n = Console.ReadLine();
                        Console.WriteLine("Kurs");
                        k = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Grade point average");
                        g = Convert.ToDouble(Console.ReadLine());
                        u = UpdateDT(n, k, g, t);
                        stydents.Add(new Stydent(n, k, g, u, String.Format("Stydent_{0}", t + 1)));
                        break;

                    case "N":
                        flag = false;
                        break;


                }

            }

            Console.ReadKey();

        }
        static DataTable ReadDT()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression2, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                DataTable dt = dataSet.Tables[0];

                return dt;
            }
        }
        static int UpdateDT(string name, int kurs, double srB, int t_login)
        {

            using (SqlConnection connectionUpd = new SqlConnection(connectionString))
            {
                connectionUpd.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlExpression, connectionUpd);
                DataSet dataS = new DataSet();
                dataAdapter.Fill(dataS);
                DataRow newRow = dataS.Tables[0].NewRow();
                newRow["text_data_1"] = String.Format("Stydent_{0}", t_login + 1);
                newRow["int_data_1"] = kurs;
                newRow["float_data_1"] = srB;
                newRow["Comment_text_1"] = name;
                dataS.Tables[0].Rows.Add(newRow);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dataS);
                dataS.Clear();

                dataAdapter.Fill(dataS);
                int q = Convert.ToInt32(dataS.Tables[0].Rows[0]["Id"]);
                for (int w = 0; w < dataS.Tables[0].Rows.Count; w++)
                {
                    Console.WriteLine(dataS.Tables[0].Rows[w]["Id"]);
                    if ((string)dataS.Tables[0].Rows[w]["text_data_1"] == String.Format("Stydent_{0}", t_login + 1))
                    {

                        q = Convert.ToInt32(dataS.Tables[0].Rows[w]["Id"]);
                    }

                }
                return q;


            }

        }


    }
}
