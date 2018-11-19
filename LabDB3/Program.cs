using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabDB3
{
    class Program
    {
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
        }
    }
}
