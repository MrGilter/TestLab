using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// лаба площадь треугольника, чтение-запись-редактирование данных в БД

namespace LabDB2
{
    class Program
    {
        static void Main(string[] args)
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


            
        }
    }
}
