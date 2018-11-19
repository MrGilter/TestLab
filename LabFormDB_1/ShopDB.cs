using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabFormDB_1
{
    static class ShopDB
    {
        private static bool flagDB = false;
        
        private static string sqlDB_Customers = "CREATE TABLE Customers ( " +
            "Id_Customer INT IDENTITY(1,1) PRIMARY KEY," +
            " Name NVARCHAR(50) NOT NULL," +
            " Surname NVARCHAR(50) NOT NULL," +
            " NumberPhone NVARCHAR(50) NOT NULL," +
            " Login NVARCHAR(50) NOT NULL," +
            " Password NVARCHAR(50) NOT NULL );";
        private static string sqlDB_Orders = "CREATE TABLE Orders (" +
            " Id_Order INT IDENTITY(1,1) PRIMARY KEY," +
            " Date DATETIME2 NOT NULL," +
            " Customer INT REFERENCES Customers(Id_Customer)," +
            " Delivery_City NVARCHAR(50) NOT NULL," +
            " Delivery_Address NVARCHAR(50) NOT NULL," +
            " Cost FLOAT," +
            " Status NVARCHAR(50) NOT NULL);";
        private static string sqlDB_OrdersProducts = "CREATE TABLE OrdersProducts (" +
            " ID_Order INT REFERENCES Orders(Id_Order)," +
            " ID_Product INT REFERENCES Products(Id_Product));";
        private static string sqlDB_Products = "CREATE TABLE Products (" +
            " Id_Product INT IDENTITY(1,1) PRIMARY KEY," +
            " NameProduct NVARCHAR(50) NOT NULL," +
            " Description NVARCHAR(MAX)," +
            " Price FLOAT)";
        
        private static void CheckDB()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionClass.ConnectionStr("master")))
            {
                List<string> nameDB = new List<string>();

                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT name, database_id FROM sys.databases WHERE database_id>4", connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nameDB.Add(reader[0].ToString());
                        }
                    }
                    foreach (string s in nameDB)
                    {
                        if (s == "ShopDB_test_task")
                        {
                            flagDB = true;
                           
                        }
                    }


                }
                catch (SqlException e)
                {
                    Console.WriteLine("Connection problems, check the correctness of the specified data in the connection window.Or contact your SQL Server administrator.\n Error: " + e.Message);
                }
            }
        }
        private static void Create()
        {
            ConnectionClass.Execute("master", "CREATE DATABASE ShopDB_test_task");
            ConnectionClass.Execute("ShopDB_test_task", sqlDB_Customers);
            ConnectionClass.Execute("ShopDB_test_task", sqlDB_Products);
            ConnectionClass.Execute("ShopDB_test_task", sqlDB_Orders);
            ConnectionClass.Execute("ShopDB_test_task", sqlDB_OrdersProducts);
        }
        private static void CreateShopDB()
        {
            CheckDB();
            if (flagDB == false)
            {
                Create();
            }
            else 
            {
                ConnectionClass.Execute("ShopDB_test_task", "USE master; ALTER DATABASE ShopDB_test_task SET SINGLE_USER WITH ROLLBACK IMMEDIATE;DROP DATABASE ShopDB_test_task;");
                Create();
            }
        }
        private static void FillShopDB()
        {
            string[] name_mas = new string[] { "Лысый", "Валера", "Огурчик Рик", "Бильба", "Ариец", "Дзенька", "Баклан", "Роман", "Пельмень", "Плет" };
            string[] surname_mas = new string[] { "Гомик", "Звездный час", "Сычуанський", "Бегинса", "Эталонный", "Тупой", "Аркадиевич", "ЛуДший", "Пузякович", "Бойс" };
            string[] products = new string[] {"Устройство для щекотания очка","ЭлектроДрын","Надувной Жигуль","Квантовый памятник кедру","Летающая тарeлка","Бомж Петрович",
                "Охладитель траханья","Бластер Петровича","Пикачу","Болт"};
            Random random = new Random((int)DateTime.Now.Ticks);

            for(int i = 0; i < 10; i++)
            {
                string numberPhone = String.Format("+380{0}", random.Next(950000000, 959999999));
                string login = String.Format("{0}_{1}", surname_mas[i], random.Next(0, 1000));
                string passvord = String.Format("{0}{1}{2}", random.Next(0, 100), name_mas[i], random.Next(100, 1000));
                string sqlExpression = String.Format("INSERT INTO Customers(Name,Surname,NumberPhone,Login,Password) VALUES ('{0}','{1}','{2}','{3}','{4}')",
                    name_mas[i], surname_mas[i], numberPhone, login, passvord);
                ConnectionClass.Execute("ShopDB_test_task", sqlExpression);

                sqlExpression = String.Format("INSERT INTO Products(NameProduct,Description,Price) VALUES ('{0}','lorem ipsum...',{1})", products[i],random.Next(50, 1000));
                ConnectionClass.Execute("ShopDB_test_task", sqlExpression);

            }
            DataTable user = new DataTable();
            DataTable shop = new DataTable();
            DataTable order = new DataTable();
            user = ConnectionClass.ReturnTable("ShopDB_test_task", "SELECT Id_Customer FROM Customers");
            shop = ConnectionClass.ReturnTable("ShopDB_test_task", "SELECT * FROM Products");

            for(int t = 0; t < 10; t++)
            {
                for (int i = 0; i < 10; i++)
                {
                    List<int> idProd = new List<int>();
                    int Q = random.Next(1, 4);
                    for (int j = 0; j < Q; j++)
                    {
                        int W = random.Next(0, shop.Rows.Count - 1);
                        idProd.Add(Convert.ToInt32(shop.Rows[W]["Id_Product"]));
                    }
                    int sum = 0;
                    foreach (int id in idProd)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            if (Convert.ToInt32(shop.Rows[y]["Id_Product"]) == id)
                            {
                                sum += Convert.ToInt32(shop.Rows[y]["Price"]);
                            }
                        }
                    }
                    ConnectionClass.Execute("ShopDB_test_task", String.Format("INSERT INTO Orders(Date,Customer,Delivery_City,Delivery_Address,Cost,Status)" +
                        " VALUES('{0}',{1},'random city','random address',{2},'in processing')", DateTime.Now, Convert.ToInt32(user.Rows[i]["Id_Customer"]), sum));
                    order = ConnectionClass.ReturnTable("ShopDB_test_task", "SELECT Id_Order FROM Orders");

                    int idOrder = Convert.ToInt32(order.Rows[order.Rows.Count - 1]["Id_Order"]);
                    foreach (int id in idProd)
                    {
                        ConnectionClass.Execute("ShopDB_test_task", String.Format("INSERT INTO OrdersProducts(ID_Order,ID_Product) VALUES({0},{1})", idOrder, id));

                    }
                    //idProd.Clear();
                    //sum = 0;
                }
            }
            

        }
        public static void Run()
        {
            CreateShopDB();
            FillShopDB();
        }
    }
}
