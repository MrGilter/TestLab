using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Свойства
namespace LabDB10
{
    class TVBox
    {
        private static string sqlExpression2 = "SELECT Comment_text_1 FROM TEST_lab_tab WHERE text_data_1 LIKE '%Channel%'";
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static DataTable channelList = ChannelList();

        private bool condition = false;
        private string current_TV_Channel = "Dark screen";
        private int volume_value = 0;


        public bool TV_Status
        {
            get
            {
                return condition;
            }
            set
            {
                if (value == condition && condition == false)
                {
                    Console.WriteLine("Stupid user, the TV is off");
                }
                else if (value != condition && condition == false)
                {
                    condition = value;
                    Console.WriteLine("TV On");
                }
                else if (value == condition && condition == true)
                {
                    Console.WriteLine("Stupid user, the TV is on");
                }
                else if (value != condition && condition == true)
                {
                    condition = value;
                    Console.WriteLine("TV Off");
                }
            }
        }
        public int TV_Channel
        {
            set
            {
                if (condition == true)
                {
                    if (value > 0 && value <= channelList.Rows.Count)
                    {
                        current_TV_Channel = String.Format("{0}        ID:{1}", channelList.Rows[value - 1]["TV List"], channelList.Rows[value - 1]["ID"]);
                    }
                    else { Console.WriteLine("Not correct!"); }
                }
                else { Console.WriteLine("Turn on the TV, dork!"); }
            }
        }
        public string InfoChannel
        {
            get
            {
                if (condition == true)
                {
                    Console.WriteLine("Для выбора канаlа введите его номер от 1 до {0}", channelList.Rows.Count);
                    return current_TV_Channel;
                }
                else { return "Turn on the tv, dork!"; }

            }
        }
        public int Volume
        {
            get
            {
                return volume_value;
            }
            set
            {
                if (condition == true)
                {
                    if (value >= 0 && value <= 100)
                    {
                        volume_value = value;
                    }
                    else { Console.WriteLine("Error...Press value 0-100"); }
                }
                else { Console.WriteLine("Turn off the tv, dork!"); }
            }
        }

        private static DataTable ChannelList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable tableTV = new DataTable();
                tableTV.Columns.Add("ID", typeof(Int32));
                tableTV.Columns.Add("TV List", typeof(String));
                tableTV.Columns["ID"].Unique = true;
                tableTV.Columns["ID"].AutoIncrement = true;
                SqlCommand cmd = new SqlCommand(sqlExpression2, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        tableTV.Rows.Add(i, reader.GetString(0));
                        i++;
                    }
                }
                return tableTV;
            }
        }
    }
    class Program
    {
        //static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //static string sqlExpression = "SELECT * FROM TEST_lab_tab";
        //static string sqlExpression2 = "SELECT * FROM TEST_lab_tab WHERE text_data_1 LIKE '%Channel%'";
        //static string sqlExp = "SELECT * FROM TEST_lab_tab WHERE text_data_1='Stydent_5'";
        //connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Test_lab; User Id = StupidUserTest; Password = 2046"
        static void Main(string[] args)
        {
            Console.WriteLine("TV Box");
            TVBox tv = new TVBox();
            bool flag = true; int t = 0;
            while (flag == true)
            {
                if (tv.TV_Status == true)
                {
                    Console.WriteLine("TV STATUS - TV ON");
                }
                else
                {
                    Console.WriteLine("TV STATUS - TV OFF");
                }
                Console.WriteLine("Выберете действие:");
                Console.WriteLine("1 - Включить телевизор");
                Console.WriteLine("2 - Выбрать уровень громкости");
                Console.WriteLine("3 - Выбрать канал");
                Console.WriteLine("4 - Выключиь телевизор");
                Console.WriteLine("5 - Завершение программы");
                t = Convert.ToInt32(Console.ReadLine());

                switch (t)
                {
                    case 1:
                        tv.TV_Status = true;
                        Console.WriteLine(tv.InfoChannel);
                        break;
                    case 2:
                        Console.WriteLine("Введите уровень громкости от 0 до 100:");
                        tv.Volume = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 3:
                        Console.WriteLine("______________________________________________________________");
                        Console.WriteLine("Введите номер канала:");
                        tv.TV_Channel = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(tv.InfoChannel);
                        break;
                    case 4:
                        tv.TV_Status = false;
                        Console.WriteLine(tv.InfoChannel);
                        break;
                    case 5:
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }

            Console.ReadKey();

        }

    }
}
