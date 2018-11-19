using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// чтение данных с БД и запись в массив
namespace LabDB4
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "SELECT * FROM TEST_lab_tab";
            string str1, str2, str3;
            string name_str0 = " ", name_str1, name_str2, name_str3;
            string type_db0, type_db1, type_db2, type_db3;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sqlExpression, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
        static void Replace(string connectionStr, string n_str, string z_str, string new_str)
        {

            using (SqlConnection conn_upate = new SqlConnection(connectionStr))
            {
                conn_upate.Open();
                using (SqlCommand cmd2 = new SqlCommand("UPDATE TEST_lab_tab SET text_data_1 = @New_str WHERE text_data_1 = @z_str", conn_upate))
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

        
    }
}
