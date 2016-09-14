using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace server
{
    public class Sql
    {
        public MySqlConnection conn;
        public void sqlconnect() //connection to DB
        {
            string serverName = "localhost"; // Адрес сервера (для локальной базы пишите "localhost")
            string userName = "server_auth"; // Имя пользователя
            string password = "admin"; // Пароль для подключения
            string dbName = "online"; //Имя базы данных
            string port = "3306"; // Порт для подключения

            string connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + dbName +
                ";port=" + port +
                ";password=" + password + ";";
            conn = new MySqlConnection(connStr);
            conn.Open();

        }
        public string sqlrequest(string[] user)
        {
            string ValidUser = null;
            string sqlrequest = "SELECT ID, login, pass FROM users"; // Строка запроса
            try
            {
                MySqlCommand sqlCom = new MySqlCommand(sqlrequest, conn);
                sqlCom.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                var myData = dt.Select();
                string[] data = { null, null };
                for (int i = 0; i < myData.Length; i++)
                {
                    data[0] = myData[i].ItemArray[1].ToString(); //login
                    data[1] = myData[i].ItemArray[2].ToString(); //pass
                    if ((data[0] == user[0]) && (data[1] == user[1]))
                    {
                        Random rnd = new Random();
                        int id = (int)myData[i].ItemArray[0];
                        ValidUser = (1000000 + id).ToString() + rnd.Next(1000000, 9999999).ToString();
                        sqlrequest = "UPDATE  `online`.`users` SET  `auth_key` =  '" + ValidUser + "' WHERE  `users`.`ID` =" + id + ";";
                        sqlCom = new MySqlCommand(sqlrequest, conn);
                        sqlCom.ExecuteNonQuery();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] Sql error: \n'" + e.ToString());
            }
            return ValidUser;
        }
    }
}
