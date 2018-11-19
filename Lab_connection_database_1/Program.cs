using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;


namespace Lab_connection_database_1
{
    class War
    {
        public string name { get; set; }
        public int life { get; set; }

        public War(string name, int life)
        {
            this.name = name;
            this.life = life;
            Log.AddRow(GetType().Name, "Вызов конструктора класса War");
        }

        public virtual void Damage(int damage)
        {
            Log.AddRow(GetType().Name, "Вызов виртуального метода Damage - War ");
        }
    }   
    class War_H:War
    {
        public War_H(string name,int life) : base(name, life)
        {
            Log.AddRow(GetType().Name, "Вызов конструктора класса War_H");
        }
        public override void Damage(int damage)
        {
            Log.AddRow(GetType().Name, "Вызов виртуального метода Damage - War_H ");
            if ((damage - 4) > 0)
            {
                life = life - (damage - 4);
                Log.AddRow(GetType().Name, String.Format("{0} с именем {1} нанесено урона {2}, осталось здоровья - {3} ",GetType().Name,name,damage-4,life));
            }
            
            
        }
    }
    class War_L : War
    {
        public War_L(string name, int life) : base(name, life)
        {
            Log.AddRow(GetType().Name, "Вызов конструктора класса War_L");
        }
        public override void Damage(int damage)
        {
            Log.AddRow(GetType().Name, "Вызов виртуального метода Damage - War_L ");
            if ((damage - 2) > 0)
            {
                Log.AddRow(GetType().Name, String.Format("{0} с именем {1} нанесено урона {2}, осталось здоровья - {3} ", GetType().Name, name, damage - 4, life));
                life = life - (damage - 4);
            }
        }
    }
    class Log
    {
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=LogDB; User Id = StupidUserTest; Password = 2046";

        static int index = ReturnIDTable();
       
        private static DataTable Table = CreateTableLog(new DataTable());
        private static DataTable CreateTableLog(DataTable dataTable)
        {
            dataTable.TableName = String.Format("LogDB_Table_{0}", index);
            dataTable.Columns.Add("Id", typeof(Int32));
            dataTable.Columns["Id"].Unique = true;
            dataTable.Columns["Id"].AllowDBNull = false;
            dataTable.Columns["Id"].AutoIncrement = true;
            dataTable.Columns["Id"].AutoIncrementSeed = 1;
            dataTable.Columns["Id"].AutoIncrementStep = 1;
            dataTable.Columns.Add("DateTime", typeof(DateTime));
            dataTable.Columns.Add("Text_column_1", typeof(String));
            dataTable.Columns.Add("Text_column_2", typeof(String));
            return dataTable;
        }
        public static void AddRow(string d1, string d2)
        {
            DataRow row = Table.NewRow();
            row["DateTime"] = DateTime.Now;
            row["Text_column_1"] = d1;
            row["Text_column_2"] = d2;
            Table.Rows.Add(row);
        }
        public static void Create()
        {

            CreateDB();
            string sqlExpression = String.Format("SELECT * FROM {0}", CreatingTables(index));
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(Table);


            }

        }
        static void CreateDB()
        {
            bool flagDB = false;
            string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=master; User Id=StupidUserTest;Password=2046";
            string sqlExpressionNameDB = "SELECT name FROM sys.databases";
            string nameDB = "LogDB";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlExpressionNameDB, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (nameDB == reader[0].ToString())
                        {
                            flagDB = true;
                        }
                    }
                }//проверяем существует ли БД с нужным именем

                if (flagDB == false)
                {
                    cmd.CommandText = String.Format("CREATE DATABASE {0}", nameDB);
                    cmd.ExecuteNonQuery();
                }

            }
        }//проверка существует ли БД, если нет то создаем
        static int ReturnIDTable()
        {
            Regex re = new Regex(@"\d+");
            int rez = 0;
            List<string> tableList = new List<string>();
            string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=master; User Id=StupidUserTest;Password=2046";
            string sqlExpInfoDB = "SELECT TABLE_NAME FROM LogDB.information_schema.tables";
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlExpInfoDB, connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Match m = re.Match(Convert.ToString(reader[0]));
                        tableList.Add(Convert.ToString(m));

                    }
                }
                foreach (string s in tableList)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        rez = 0;
                    }
                    else if (rez < Convert.ToInt32(s))
                    {
                        rez = Convert.ToInt32(s);
                    }
                }

            }
            return rez;
        }//поиск максимального числового значения в названии таблиц
        static string CreatingTables(int id)
        {
            id = id + 1;
            string cmdText = String.Format("CREATE TABLE LogDB_Table_{0} (Id int IDENTITY(1,1) PRIMARY KEY, DateTime datetime2, Text_column_1 text NULL, Text_column_2 text NULL) ", id);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(cmdText, connection);
                cmd.ExecuteNonQuery();
            }

            return String.Format("LogDB_Table_{0}", id);
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
            bool flag = true;
            List<War> wars = new List<War>();
            int t = 0, t1 = 0, buf_int=0,damage = 0;
            string buf_str = "";
            while(flag == true)
            {
                Console.WriteLine("1-добавить воина");
                Console.WriteLine("2-нанести всем урон");
                Console.WriteLine("3-выход");
                t = 0;
                t = Convert.ToInt32(Console.ReadLine());
                switch (t)
                {
                    case 1:
                        Console.WriteLine("Введите Имя:");
                        buf_str = Console.ReadLine();
                        Console.WriteLine("Введите к-во ХэПэ:");
                        buf_int = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Введите тип брони воина: \n1 - котелок на голове\n2 - друшляк на пузе ");
                        t1 = 0;
                        t1 = Convert.ToInt32(Console.ReadLine());
                        if (t1 == 1)
                        {
                            wars.Add(new War_H(buf_str, buf_int));
                        }
                        else if (t1 == 2)
                        {
                            wars.Add(new War_L(buf_str, buf_int));
                        }
                        else { Console.WriteLine("error"); }
                        break;
                    case 2:
                        damage = 0;
                        Console.WriteLine("Введите урон:");
                        damage = Convert.ToInt32(Console.ReadLine());
                        foreach(War war in wars)
                        {
                            if (war.life > 0)
                            {
                                war.Damage(damage);
                                if (war.life <= 0)
                                {
                                    Log.AddRow("Program", String.Format("{0} был убит", war.name));
                                }
                            }
                            else
                            {
                                Console.WriteLine(war.name + "мертв!");
                            }
                        }
                        break;
                    case 3:
                        flag = false;
                        Log.Create();
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                }
            }
            Console.ReadKey();

        }

        
    }
   
}
