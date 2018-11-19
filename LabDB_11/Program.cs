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

namespace LabDB_11
{
    

   

        class BaseClass
        {
            static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=LogDB; User Id = StupidUserTest; Password = 2046";
            private string name_t;
            private string name_class;
            public string nameT
            {
                get
                {
                    return name_t;
                }
                set
                {
                    name_t = value;
                    Log(Convert.ToString(GetType().Name), String.Format("Запись/изменение значения Свойства 'nameT', Класса {0}. Значение записано/изменено на {1}", GetType().Name, value));

                }
            }
            public string Name
            {
                get
                {
                    return name_class;
                }
                set
                {
                    if (string.IsNullOrEmpty(nameT))
                    {
                        Console.WriteLine("Не задана таблица подключения, для обьекта - " + value);
                    }
                    else
                    {
                        Log(Convert.ToString(GetType().Name), String.Format("Запись/изменение значения Свойства Name, Класса {0}. Значение записано/изменено на {1}", Convert.ToString(GetType().Name), value));
                        name_class = value;
                    }

                }
            }

            public BaseClass(string name, string nameTable)
            {
                nameT = nameTable;
                if (string.IsNullOrEmpty(nameT))
                {
                    Console.WriteLine("Не задана таблица подключения, для обьекта c именем- " + name);
                }
                else
                {
                    Log(Convert.ToString(GetType().Name), String.Format("Вызов конструктора BaseClass, Класса {0}. Значение записано/изменено на {1}", Convert.ToString(GetType().Name), name));
                    Name = name;
                }

            }

            public void Log(string data1, string data2)
            {
                string sqlExp = String.Format("INSERT INTO {0} (DateTime,Text_column_1,Text_column_2) VALUES (@dataT,@text1,@text2)", nameT);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sqlExp, connection);
                    DateTime dateTime = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dataT", dateTime);
                    cmd.Parameters.AddWithValue("@text1", data1);
                    cmd.Parameters.AddWithValue("@text2", data2);
                    cmd.ExecuteNonQuery();
                }
            }


        }

        class InheritanceClass : BaseClass
        {
            public InheritanceClass(string name, string nameTable) : base(name, nameTable)
            {
                if (string.IsNullOrEmpty(nameT))
                {
                    Console.WriteLine("Не задана таблица подключения, для обьекта c именем- " + name);
                }
                else
                {
                    Log(Convert.ToString(GetType().Name), String.Format("Вызов конструктора InheritanceClass, Класса {0}. Значение записано/изменено на {1}", Convert.ToString(GetType().Name), name));

                }
            }

            public void InheritanceClassMetod_1()
            {
                if (string.IsNullOrEmpty(nameT))
                {
                    Console.WriteLine("Не задана таблица подключения, для метода InheritanceClassMetod_1");
                }
                else
                {
                    Log(Convert.ToString(GetType().Name), String.Format("Вызов метода InheritanceClassMetod_1, Класса {0}.", Convert.ToString(GetType().Name)));

                }
            }
        }

        class InheritanceClass2 : BaseClass
        {
            public InheritanceClass2(string name, string nameTable) : base(name, nameTable)
            {
                if (string.IsNullOrEmpty(nameT))
                {
                    Console.WriteLine("Не задана таблица подключения, для обьекта c именем- " + name);
                }
                else
                {
                    Log(Convert.ToString(GetType().Name), String.Format("Вызов конструктора InheritanceClass2, Класса {0}. Значение записано/изменено на {1}", Convert.ToString(GetType().Name), name));

                }
            }

            public void InheritanceClass2Metod_1()
            {
                if (string.IsNullOrEmpty(nameT))
                {
                    Console.WriteLine("Не задана таблица подключения, для метода InheritanceClass2Metod_1");
                }
                else
                {
                    Log(Convert.ToString(GetType().Name), String.Format("Вызов метода InheritanceClass2Metod_1, Класса {0}.", Convert.ToString(GetType().Name)));

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
            static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=LogDB; User Id = StupidUserTest; Password = 2046";

            static void Main(string[] args)
            {
                CreateDB();
                string nTable = CreatingTables(ReturnIDTable());
                string s = "";
                bool flag = true;
                int t = 0, t1 = 0;
                List<BaseClass> classList = new List<BaseClass>();
                while (flag == true)
                {
                    Console.WriteLine("1 - Создание нового елемента списка");
                    Console.WriteLine("2 - Вызов методов елементов списка");
                    Console.WriteLine("3 - Удаление элементa");
                    Console.WriteLine("4 - Exit");
                    t = Convert.ToInt32(Console.ReadLine());
                    switch (t)
                    {
                        case 1:
                            Console.WriteLine("Введите 1 для создания первого класа наследника, или 2 для создания второго :");
                            t1 = Convert.ToInt32(Console.ReadLine());
                            if (t1 == 1)
                            {
                                Console.WriteLine("Введите имя элемента:");
                                classList.Add(new InheritanceClass(Convert.ToString(Console.ReadLine()), nTable));
                            }
                            else if (t1 == 2)
                            {
                                Console.WriteLine("Введите имя элемента:");
                                classList.Add(new InheritanceClass2(Convert.ToString(Console.ReadLine()), nTable));
                            }
                            else
                            {
                                Console.WriteLine("Error");
                            }
                            break;
                        case 2:
                            if (classList.Count != 0)
                            {
                                foreach (BaseClass Class in classList)
                                {
                                    if (Class is InheritanceClass)
                                    {
                                        (Class as InheritanceClass).InheritanceClassMetod_1();
                                    }
                                    else if (Class is InheritanceClass2)
                                    {
                                        (Class as InheritanceClass2).InheritanceClass2Metod_1();
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error, the list is empty");
                            }

                            break;
                        case 3:
                            if (classList.Count != 0)
                            {
                                Console.WriteLine("Введите имя элемента списка на удаление:");
                                s = Console.ReadLine();

                                for (int i = 0; i < classList.Count; i++)
                                {
                                    if (classList[i].Name == s)
                                    {
                                        classList[i].Log("Delete", String.Format("Удаление обьекта списка  с именем {0}, с типом {1}", classList[i].Name, classList[i].GetType().Name));
                                        classList.Remove(classList[i]);
                                        i--;
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Error, the list is empty");
                            }

                            break;
                        case 4:
                            flag = false;
                            break;
                    }
                    Console.WriteLine("");


                }

                Console.ReadKey();

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

    
}
