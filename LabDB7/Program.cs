using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// работа с фалами (перенос значений с бд в тхт-файл)
namespace LabDB7
{
    class Program
    {
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
}
