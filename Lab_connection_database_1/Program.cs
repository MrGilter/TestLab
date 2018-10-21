using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.IO;

namespace Lab_connection_database_1
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
        public  void TV_Off()
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
            string s;int t = 0;
            DataTable tableTV = Read_DB();
            for (int i = 0; i < tableTV.Rows.Count; i++)
            {
                s = Convert.ToString(tableTV.Rows[i]["text_data_1"]);
                if( s.Contains("Channel"))
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
            while (d!=0)
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

        void lab_db_7()
        {
            /*
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";
        //connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Test_lab; User Id = StupidUserTest; Password = 2046"
        static void Main(string[] args)
        {
            Console.WriteLine("Назови файл: ");
            string fileName = Console.ReadLine();
            CreateFile(fileName);
            Console.ReadKey();

        }
        static void CreateFile(string nameFile)
        {
            string route = String.Format("C:\\{0}.txt", nameFile);
            FileStream file1 = new FileStream(route, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(file1))
            {
                DataTable table = ReadDB();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        writer.Write(table.Rows[i][j] + "  ");
                    }
                    writer.WriteLine();
                }
                Console.WriteLine("gotovo blet");
            }
        }
        static DataTable ReadDB()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet set = new DataSet();
                adapter.Fill(set);
                DataTable dt = set.Tables[0];
                return dt;
            }
            
        }*/
        }// работа с фалами (перенос значений с бд в тхт-файл)
        void lab_db_6()
        {
            /*
              static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";

        static void Main(string[] args)
        {
            string big_text;
            string zamena,text_s_bd;
            string kill_slovo;
            int pos1, pos2;
            Console.WriteLine("Введите текст:");
            big_text = Console.ReadLine();
            Add_to_DB(big_text);
            text_s_bd = Read_DB();
            Console.WriteLine(text_s_bd);
            Console.WriteLine("Введите слово которое необходимо заменить:");
            kill_slovo = Console.ReadLine();
            Console.WriteLine("Введите слово на которое необходимо заменить:");
            zamena = Console.ReadLine();
            big_text = Zamena(text_s_bd, kill_slovo, zamena);
            Console.WriteLine(big_text);
            Console.WriteLine();
            Console.WriteLine("Введите позицию первого элемента строки после которого текст не будет обрезан:");
            pos1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите позицию символа которым будет заканчиваться текст:");
            pos2 = Convert.ToInt32(Console.ReadLine());
            Add_to_DB(Obrez(big_text, pos1, pos2));
            Console.WriteLine("Result:  {0}", Read_DB());
            Console.ReadKey();

        }
        public static void Add_to_DB(string text)
        {
            string text_data_1 = "lab6";
            bool flag1 = false;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);


                for(int i =0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[i];
                    if (dataRow.Field<string>(1) == text_data_1)
                    {
                        flag1 = true;
                    }
                }

                if(flag1 == true)
                {
                    for(int j = 0; j<dataSet.Tables[0].Rows.Count; j++)
                    {
                        DataRow dataRow1 = dataSet.Tables[0].Rows[j];
                        if (dataRow1.Field<string>(1) == text_data_1)
                        {
                            dataRow1["Comment_text_1"] = text;
                        }
                    }
                }
                else
                {
                    DataRow newRow = dataSet.Tables[0].NewRow();
                    newRow["text_data_1"] = text_data_1;
                    newRow["int_data_1"] = 0;
                    newRow["float_data_1"] = 0;
                    newRow["Comment_text_1"] = text;
                    dataSet.Tables[0].Rows.Add(newRow);

                }
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(dataSet);
                dataSet.Clear();
            }
        }//добавление текста в ечейку БД
        public static string Read_DB()
        {
            string result = null;
            string text_data_1_1 = "lab6";
            using(SqlConnection connectionRead = new SqlConnection(connectionString))
            {
                connectionRead.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlExpression, connectionRead);
                DataSet set = new DataSet();
                dataAdapter.Fill(set);

                for(int u = 0; u < set.Tables[0].Rows.Count; u++)
                {
                    if (set.Tables[0].Rows[u].Field<string>(1) == text_data_1_1)
                    {
                        result = (string)set.Tables[0].Rows[u]["Comment_text_1"];
                        break;
                    }
                    else { result = "Error. Not found data in DB"; }
                }
                return result;

            }            
        }//чтение нужного значения с БД
        public static string Zamena(string text_z, string slovo_kill, string slovo_zamena)
        {
            string s = text_z;
            return s.Replace(slovo_kill, slovo_zamena);

        }//замена слова в тексте
        public static string Obrez(string sl, int position1, int position2)
        {
                            
            return sl.Remove(position2).Substring(position1); 
        }//обрезание теста
             */
        }// работа из строками
        void lab_db_5()
        {
            /*
             static void Main(string[] args)
        {

            string sqlExpression = "SELECT * FROM TEST_lab_tab";
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                DataRow newRow = dt.NewRow();
                newRow["text_data_1"] = "Test_dataAdapter_builder";
                newRow["int_data_1"] = 1488;
                newRow["float_data_1"] = 3.14;
                dt.Rows.Add(newRow);

                // создаем объект SqlCommandBuilder
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                // альтернативный способ - обновление только одной таблицы
                //adapter.Update(dt);
                // заново получаем данные из бд
                // очищаем полностью DataSet
                ds.Clear();
                // перезагружаем данные
                adapter.Fill(ds);

                foreach (DataColumn column in dt.Columns)
                    Console.Write("\t{0}", column.ColumnName);
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                  
                    

                }
        
            }
            Console.ReadLine();
        }
             */
        }
        void lab_db_4()
        {
            /*
             static void Main(string[] args)
        {
=======
>>>>>>> Stashed changes
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "SELECT * FROM TEST_lab_tab";
            string str1, str2,str3;
            string name_str0=" ", name_str1, name_str2, name_str3;
            string type_db0, type_db1, type_db2, type_db3;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sqlExpression, connection))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            name_str0 = (string)reader.GetName(0);
                            name_str1 = Convert.ToString(reader.GetName(1));
                            name_str2 = Convert.ToString(reader.GetName(2));
                            name_str3 = Convert.ToString(reader.GetName(3));
                            type_db0 = reader.GetDataTypeName(0);
                            type_db1 = reader.GetDataTypeName(1);
                            type_db2 = reader.GetDataTypeName(2);
                            type_db3 = reader.GetDataTypeName(3);
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}", name_str0, reader.GetName(1), reader.GetName(2), reader.GetName(3));
                            Console.WriteLine("{0}\t{1}\t{2}\t\t{3}", reader.GetDataTypeName(0), reader.GetDataTypeName(1), reader.GetDataTypeName(2), reader.GetDataTypeName(3));

                            while (reader.Read())
                            {
                                Console.WriteLine("{0}\t{1}\t\t{2}\t\t{3}", reader.GetValue(0), reader.GetValue(1), reader.GetValue(2), reader.GetValue(3));
                                
                            }
                        }
                    }

                    
                }

            }
            Console.WriteLine("Введите название в столбца в котором нужно изменить значение (кроме поля Id): ");
            str1 = Console.ReadLine();
            Console.WriteLine("Введите значение которое необходимо заменить на новое: ");
            str2 = Console.ReadLine();
            Console.WriteLine("Введите новое значение: ");
            str3 = Console.ReadLine();
            //cmd
            Replace(connectionString, str1, str2, str3);

            Console.ReadLine();
        }
        static void Replace(string connectionStr,string n_str,string z_str,string new_str)
        {
            
            using(SqlConnection conn_upate = new SqlConnection(connectionStr))
            {
                conn_upate.Open();
                using(SqlCommand cmd2 = new SqlCommand("UPDATE TEST_lab_tab SET text_data_1 = @New_str WHERE text_data_1 = @z_str", conn_upate))
                {
                    Console.WriteLine(n_str + "  " + z_str + "  " + new_str);
                    cmd2.Parameters.AddWithValue("@N_str", n_str);
                    cmd2.Parameters.AddWithValue("@New_str", new_str);
                    cmd2.Parameters.AddWithValue("@z_str", z_str);
                    cmd2.CommandText = "UPDATE TEST_lab_tab SET text_data_1 = @New_str WHERE text_data_1 = @z_str";
                    int number = cmd2.ExecuteNonQuery();
                    Console.WriteLine("Добавлено объектов: {0}", number);
                }
            }

        }
        
        void lab_db_3()
        {/*
            static void Main(string[] args)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string sqlExpression = "SELECT COUNT(*) FROM TEST_lab_tab";
                int count = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlExpression, connection))
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                        string[,] str_mas = new string[count, 4];
                        int i = -1;

                        cmd.CommandText = "SELECT * FROM TEST_lab_tab";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    i++;
                                    str_mas[i, 0] = Convert.ToString(reader.GetValue(0));
                                    str_mas[i, 1] = Convert.ToString(reader.GetValue(1));
                                    str_mas[i, 2] = Convert.ToString(reader.GetValue(2));
                                    str_mas[i, 3] = Convert.ToString(reader.GetValue(3));

                                }
                            }
                        }
                        for (int u = 0; u < count; u++)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}", str_mas[u, 0], str_mas[u, 1], str_mas[u, 2], str_mas[u, 3]);
                        }

                    }




                }




                Console.WriteLine(count);
                Console.ReadLine();
            }*/
        } // чтение данных с БД и запись в массив
        void lab_db_2()
        {
           /* static void Main(string[] args)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string str; int _int = 0; double _double = 0; bool flag1 = false, flag2 = false, flag3 = false;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine(connection.ConnectionString);
                    Console.WriteLine(connection.Database);
                    Console.WriteLine(connection.DataSource);
                    Console.WriteLine(connection.ServerVersion);
                    Console.WriteLine(connection.State);
                    Console.WriteLine(connection.WorkstationId);
                    Console.WriteLine();
                    string sqlExpressionRead = "SELECT * FROM TEST_lab_tab";
                    Console.WriteLine("Введите 2а значения для вычесления площади треугольника...");
                    using (SqlCommand cmd = new SqlCommand(sqlExpressionRead, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // если есть данные
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    if (reader.GetString(1) == "Sa_triangle")
                                    {
                                        flag1 = true;
                                    }
                                    if (reader.GetString(1) == "Sb_triangle")
                                    {
                                        flag2 = true;
                                    }
                                    if (reader.GetString(1) == "S_triangle")
                                    {
                                        flag3 = true;
                                    }
                                }
                            }

                        }

                        if (flag1 == false)
                        {
                            cmd.CommandText = "INSERT INTO TEST_lab_tab (text_data_1,int_data_1,float_data_1) VALUES ('Sa_triangle',0,0)";
                            cmd.ExecuteNonQuery();
                        }
                        if (flag2 == false)
                        {
                            cmd.CommandText = "INSERT INTO TEST_lab_tab (text_data_1,int_data_1,float_data_1) VALUES ('Sb_triangle',0,0)";
                            cmd.ExecuteNonQuery();
                        }
                        if (flag2 == false)
                        {
                            cmd.CommandText = "INSERT INTO TEST_lab_tab (text_data_1,int_data_1,float_data_1) VALUES ('S_triangle',0,0)";
                            cmd.ExecuteNonQuery();
                        }

                        Console.WriteLine("Старые значения с БД:");
                        cmd.CommandText = "SELECT * FROM TEST_lab_tab";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // если есть данные
                            {
                                while (reader.Read()) // построчно считываем данные
                                {
                                    if (reader.GetString(1) == "Sa_triangle")
                                    {
                                        Console.WriteLine("Старые значениe Sa_triangle: {0}", reader.GetValue(3));
                                    }
                                    if (reader.GetString(1) == "Sb_triangle")
                                    {
                                        Console.WriteLine("Старые значениe Sb_triangle: {0}", reader.GetValue(3));
                                    }
                                    if (reader.GetString(1) == "S_triangle")
                                    {
                                        Console.WriteLine("Старые значениe S_triangle: {0}", reader.GetValue(3));
                                    }
                                }
                            }

                        }

                        double d1 = 0, d2 = 0, d3 = 0;
                        Console.WriteLine("Введите сторону треугольника, а :");
                        d1 = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Введите сторону треугольника, b :");
                        d2 = Convert.ToDouble(Console.ReadLine());
                        d3 = (d1 * d2) / 2;
                        Console.WriteLine("Пдощадь треугольника, S = {0}", d3);

                        cmd.CommandText = "UPDATE TEST_lab_tab SET float_data_1 = @dd1 WHERE text_data_1 = 'Sa_triangle'";
                        cmd.Parameters.AddWithValue("@dd1", d1);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE TEST_lab_tab SET float_data_1 = @dd2 WHERE text_data_1 = 'Sb_triangle'";
                        cmd.Parameters.AddWithValue("@dd2", d2);
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "UPDATE TEST_lab_tab SET float_data_1 = @dd3 WHERE text_data_1 = 'S_triangle'";
                        cmd.Parameters.AddWithValue("@dd3", d3);
                        cmd.ExecuteNonQuery();

                    }

                    Console.WriteLine("");
                    Console.ReadLine();
                }


            }*/
        } // лаба площадь треугольника, чтение-запись-редактирование данных в БД

        void lab_db_1()
        {
            /* using (SqlConnection connection = new SqlConnection(connectionString))
             { 
                 try
                 {
                     connection.Open();
                     Console.WriteLine(" Connetion Open... ");
                     Console.WriteLine(connection.State);
                     Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
                     Console.WriteLine("\tБаза данных: {0}", connection.Database);
                     Console.WriteLine("\tСервер: {0}", connection.DataSource);
                     Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
                     Console.WriteLine("\tСостояние: {0}", connection.State);
                     Console.WriteLine("\tWorkstationId: {0}", connection.WorkstationId);

                     /*Console.WriteLine("float:");
                     f = double.Parse(Console.ReadLine());
                     Console.WriteLine("_____"+f);
                     SqlCommand command = new SqlCommand("UPDATE TEST_lab_tab SET float_data_1 = @float WHERE text_data_1 = 'test5'",connection);
                     command.Parameters.AddWithValue("@float", f);
                     int number = command.ExecuteNonQuery();
                     Console.WriteLine("Добавлено объектов: {0}", number);
                     string text_data_1;
                     int int_data_1 = 0;
                     double float_data_1 = 0;

                     Console.WriteLine("Введите значение text_data_1 для добавления: ");
                     text_data_1 = Console.ReadLine();
                     Console.WriteLine("Введите значение int_data_1 для добавления: ");
                     int_data_1 = Convert.ToInt32(Console.ReadLine());
                     Console.WriteLine("Введите значение float_data_1 для добавления: ");
                     float_data_1 = double.Parse(Console.ReadLine());

                     string sqlExpression = String.Format("INSERT INTO TEST_lab_tab (text_data_1,int_data_1,float_data_1) VALUES ('{0}',{1},@float_2)", text_data_1, int_data_1);
                     SqlCommand cmd = new SqlCommand(sqlExpression, connection);
                     cmd.Parameters.AddWithValue("@float_2", float_data_1);
                     int number = cmd.ExecuteNonQuery();

                     Console.WriteLine("Добавлено объектов: {0}", number);

                     Console.WriteLine("Обновление|изменение значения...");
                     Console.WriteLine("Строковое значение-ключ для изменения:");
                     text_data_1 = Console.ReadLine();
                     cmd.Parameters.AddWithValue("@text_1", text_data_1);
                     Console.WriteLine("Введите данные на изменение ИНТ: ");
                     int_data_1 = Convert.ToInt32(Console.ReadLine());
                     cmd.Parameters.AddWithValue("@int_1", int_data_1);
                     Console.WriteLine("Введите данные на изменение ФЛОТ: ");
                     float_data_1 = double.Parse(Console.ReadLine());
                     cmd.Parameters.AddWithValue("@float_1", float_data_1);
                     cmd.CommandText = "UPDATE TEST_lab_tab SET int_data_1 = @int_1, float_data_1 = @float_1 WHERE text_data_1 = @text_1";
                     number = cmd.ExecuteNonQuery();
                     Console.WriteLine("update объектов: {0}", number);
                 }
                 catch (SqlException ex)
                 {
                     Console.WriteLine(ex);
                 }
                 finally
                 {
                     connection.Close();
                     Console.WriteLine("Connection Close... ");
                     Console.Beep();
                     Console.WriteLine("\tСостояние: {0}", connection.State);
                 }*/
        } 



    }
}
