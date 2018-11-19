using System;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace LabDB1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
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

                    Console.WriteLine("float:");
                    double f = double.Parse(Console.ReadLine());
                    Console.WriteLine("_____" + f);
                    SqlCommand command = new SqlCommand("UPDATE TEST_lab_tab SET float_data_1 = @float WHERE text_data_1 = 'test5'", connection);
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
                    number = cmd.ExecuteNonQuery();

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
                }
            }
        }
    }
}
