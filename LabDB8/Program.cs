using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// работа с класами и методами класов
namespace LabDB8
{
    class TVSet
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";
        //connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Test_lab; User Id = StupidUserTest; Password = 2046"

        private static string[] channelName = new string[5];
        private static string[] infoChannel = new string[5];
        private static int currentChannel;
        private static bool flag = true;

        public void TV_On()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("TV On");
            Console.ResetColor();

            if (channelName[0] == null)
            {
                FillTVSet();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Текущий канал:  {0}", channelName[currentChannel]);
            Console.WriteLine("Описание канала:  {0}", infoChannel[currentChannel]);
            Console.ResetColor();
            flag = true;

        }
        public void TV_Off()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("TV Off");
            Console.ResetColor();
            flag = false;
        }
        public void TV_NextChannel()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("TV Next Channel");
            Console.ResetColor();
            if (flag == true)
            {
                currentChannel = currentChannel + 1;


                if (currentChannel == 5)
                {
                    currentChannel = 0;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Текущий канал:  {0}", channelName[currentChannel]);
                Console.WriteLine("Описание канала:  {0}", infoChannel[currentChannel]);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("WARING - TV OFF!!!");
            }
        }
        public void TV_BackChannel()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("TV Back Channel");
            Console.ResetColor();
            if (flag == true)
            {

                currentChannel = currentChannel - 1;
                if (currentChannel == -1)
                {
                    currentChannel = 4;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Текущий канал:  {0}", channelName[currentChannel]);
                Console.WriteLine("Описание канала:  {0}", infoChannel[currentChannel]);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("WARING - TV OFF!!!");
            }
        }
        private static void FillTVSet()
        {
            string s; int t = 0;
            DataTable tableTV = Read_DB();
            for (int i = 0; i < tableTV.Rows.Count; i++)
            {
                s = Convert.ToString(tableTV.Rows[i]["text_data_1"]);
                if (s.Contains("Channel"))
                {
                    channelName[t] = Convert.ToString(tableTV.Rows[i]["text_data_1"]);
                    infoChannel[t] = Convert.ToString(tableTV.Rows[i]["Comment_text_1"]);
                    t++;
                }

            }
            currentChannel = 0;
        }
        private static DataTable Read_DB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet set = new DataSet();
                adapter.Fill(set);
                DataTable dt = set.Tables[0];
                return dt;
            }
        }

    }
    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";
        //connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Test_lab; User Id = StupidUserTest; Password = 2046"
        static void Main(string[] args)
        {

            int d = 1;
            TVSet tVSet = new TVSet();
            tVSet.TV_On();
            while (d != 0)
            {
                Console.WriteLine("TV_On - 1");
                Console.WriteLine("TV_Off - 2");
                Console.WriteLine("TV_NextChannel - 3");
                Console.WriteLine("TV_BackChannel - 4");
                Console.WriteLine("PowerOff - 0");
                d = Convert.ToInt32(Console.ReadLine());
                switch (d)
                {
                    case 1:
                        tVSet.TV_On();
                        break;
                    case 2:
                        tVSet.TV_Off();
                        break;
                    case 3:
                        tVSet.TV_NextChannel();
                        break;
                    case 4:
                        tVSet.TV_BackChannel();
                        break;
                    case 0:
                        Console.WriteLine("POWER OFF");
                        break;
                    default:
                        Console.WriteLine("ERROR!");
                        break;

                }

            }
            Console.ReadKey();
        }



    }
}
