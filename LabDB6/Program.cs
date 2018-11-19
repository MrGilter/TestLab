using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// работа из строками
namespace LabDB6
{
    class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string sqlExpression = "SELECT * FROM TEST_lab_tab";

        static void Main(string[] args)
        {
            string big_text;
            string zamena, text_s_bd;
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlExpression, connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);


                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[i];
                    if (dataRow.Field<string>(1) == text_data_1)
                    {
                        flag1 = true;
                    }
                }

                if (flag1 == true)
                {
                    for (int j = 0; j < dataSet.Tables[0].Rows.Count; j++)
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
            using (SqlConnection connectionRead = new SqlConnection(connectionString))
            {
                connectionRead.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlExpression, connectionRead);
                DataSet set = new DataSet();
                dataAdapter.Fill(set);

                for (int u = 0; u < set.Tables[0].Rows.Count; u++)
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
    }
}
