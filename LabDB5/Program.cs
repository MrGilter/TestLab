using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabDB5
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "SELECT * FROM TEST_lab_tab";
            using (SqlConnection connection = new SqlConnection(connectionString))
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
    }
}
