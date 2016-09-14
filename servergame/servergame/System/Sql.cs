using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//using System.Web.Script.Serialization;

namespace servergame
{
    public class Sql
    {
        public MySqlConnection conn;
        private MySqlTransaction transaction;
        private MySqlDataAdapter dataAdapter;
        public void Connect() //connection to DB
        {
            string serverName = "localhost"; // Адрес сервера (для локальной базы пишите "localhost")
            string userName = "server_game"; // Имя пользователя
            string password = "admin"; // Пароль для подключения
            string dbName = "online"; //Имя базы данных
            string port = "3306"; // Порт для подключения

            string connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + dbName +
                ";port=" + port +
                ";password=" + password + ";";
            conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] Unable to connect to SQL" + e.ToString());
            }
            

        }
        public void Close()
        {
            conn.Close();
        }
        private void Execute(ref MySqlCommand cmd)
        {
            Object lockThis = new Object();
            lock (lockThis)
            {
                cmd.ExecuteNonQuery();
            }
        }
        public string GetIdByAuth_id(string auth_key)
        {
            string result = null;
            string sqlrequest = "SELECT ID, default_char FROM _users WHERE auth_key='" + auth_key + "'";    // Строка запроса
            try
            {
                MySqlCommand sqlCom = new MySqlCommand(sqlrequest, conn);
                Execute(ref sqlCom);  
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                var myData = dt.Select();
                if (myData.Length > 0)
                {
                    result = myData[0].ItemArray[0].ToString();         //user id
                    
                    Console.WriteLine("[SQL] Connected ID: " + result[0]);
                }
                else
                {
                    Console.WriteLine("[SQL] Invalid auth_key");
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine("[SQL] Error [GetIdByAuth_id] : \n'" + e.ToString());
            }
            return result;
        }
        public string GetCharacters(int id)             //choose hero screen
        {
            string data = null;
          //  string[] fields = {"ID", "race","class","name","exp","equip","deleted" };
            int count = 0;
            string sqlrequest = "SELECT race,class,name,sex,face,exp,equip FROM _characters WHERE owner_id='" + id.ToString() + "' AND deleted = 0";
            try
            {
                MySqlCommand sqlCom = new MySqlCommand(sqlrequest, conn);
                Execute(ref sqlCom);
                //sqlCom.ExecuteNonQuery();                                                    //may be error more users in one time
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                var myData = dt.Select();
                //string[] data = { null, null };
                count = myData.Length;
                if (count > 0)  //if have min one char
                {
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            data += myData[i].ItemArray[j].ToString();
                            if (j < 7-1)
                                data += "$";
                        }
                        if (count > 1 && i < count-1)
                            data += "#";
                    }
                    Console.WriteLine("[SQL] Characters count: " + count);
                //    Console.WriteLine("[SQL] Characters info: " + data);
                }
                else  //if no chars
                {
                    Console.WriteLine("[Server] No chars");
                    data = null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] SQL Error [GetCharactersCount]: \n'" + e.ToString());
            }
            return data;
        }
        public string LoadCharacter(ref Character ch)   //choose hero screen
        {
            string data = null;
            //  string[] fields = {"ID", "race","class","name","exp","equip","deleted" };
            int count = 0;
            string sqlrequest = "SELECT ID,race,class,name,exp,equip,deleted FROM _characters WHERE owner_id='" + "" + "'";
            try
            {
             /*   MySqlCommand sqlCom = new MySqlCommand(sqlrequest, conn);
                sqlCom.ExecuteNonQuery();                                                    //may be error more users in one time
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                var myData = dt.Select();
                //string[] data = { null, null };
                count = myData.Length;
                if (count > 0)  //if have min one char
                {
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            data += myData[i].ItemArray[j].ToString();
                            if (j < 7 - 1)
                                data += "@";
                        }
                        if (count > 1 && i < count - 1)
                            data += "#";
                    }
                    Console.WriteLine("[SQL] Characters count: " + count);
                    Console.WriteLine("[SQL] Characters info: " + data);
                }
                else  //if no chars
                {
                    Console.WriteLine("[Server] No chars");
                    data = null;
                }*/

            }
            catch (Exception e)
            {
                Console.WriteLine("[Server] SQL Error [GetCharactersCount]: \n'" + e.ToString());
            }
            return data;
        }
        public bool SaveCharacter(Character character)
        {
            if (character != null)
            {
                if (character.id != 0)
                {
                    //string sqlrequest = "UPDATE bla bla bla";
                    try
                    {
                        /*      MySqlCommand sqlCom = new MySqlCommand(sqlrequest, conn);
                              sqlCom.ExecuteNonQuery();                                                    //may be error more users in one time
                              MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
                              DataTable dt = new DataTable();*/
                        Console.WriteLine("[SQL] Character ID: " + character.id + ", char owner_id: " + character.owner + " has been saved");
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[Server] SQL Error [SaveCharacter]: \n'" + e.ToString());
                        return false;
                    }                    
                }
                else
                    return true;
            }
            else
                return true;

        }
        public int CreateCharacter(string info, int userid)
        {
            
            int creationStatus = -1; // 1- success created, 0-already exists, -1-not created;
            string[] charInfo = info.Split('$');
            string charId="";
            MySqlCommand sqlCom = conn.CreateCommand();
            //check for exists name
            transaction = conn.BeginTransaction();
            sqlCom.Connection = conn;
            sqlCom.Transaction = transaction;
            try
            {
                sqlCom.CommandText = "SELECT name FROM `_characters` WHERE `name`='"+ charInfo[1] + "'";
                sqlCom.ExecuteNonQuery();
                transaction.Commit();
                transaction = null;
                Console.WriteLine("[SQL] exsists: '" + sqlCom.ToString() );
                dataAdapter = new MySqlDataAdapter(sqlCom);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (dt.Rows.Count > 0) //if name already exits
                    creationStatus = 0;
                Console.WriteLine("[SQL] exsists: '" + dt.Rows.Count.ToString());
                var myData = dt.Select();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                transaction = null;
                Console.WriteLine("[SQL] error: [CreateCharacter.Checkexists]'" + e.ToString() );
            }
            if (creationStatus != 0)
            {
                //creater character
                transaction = conn.BeginTransaction();
                sqlCom.Connection = conn;
                sqlCom.Transaction = transaction;
                try
                {              
                    sqlCom.CommandText = "INSERT INTO `_characters`(`owner_id`, `race`, `class`, `name`, `sex`, `face`, `equip`, `exp`, `pos`, `hp`, `mp`, `deleted`) VALUES (" + userid + "," + charInfo[0] + ",0,'" + charInfo[1] + "','" + charInfo[2] + "','" + charInfo[3] + "','{l1010,l2010,0}'," + 0 + ",'{0,0,0,0}',0,0,0)";
                    sqlCom.ExecuteNonQuery();
                    sqlCom.CommandText = "SELECT LAST_INSERT_ID()";
                    sqlCom.ExecuteNonQuery();
                    // getting id of new char
                    dataAdapter = new MySqlDataAdapter(sqlCom);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);
                    var myData = dt.Select();
                    charId = myData[0].ItemArray[0].ToString();
                    //---
                    sqlCom.CommandText = "CREATE TABLE `Inventory_" + charId + "` (itemname VARCHAR(10), count BIGINT(20), equipped INT(1))";
                    sqlCom.ExecuteNonQuery();
                    sqlCom.CommandText = "CREATE TABLE `Skills_" + charId + "` (skillname VARCHAR(10), lvl INT(11), enchant INT(11))";
                    sqlCom.ExecuteNonQuery();
                    sqlCom.CommandText = "INSERT INTO `Inventory_" + charId + "` (`itemname`, `count`, `equipped`) VALUES ('l1010', 1, 1),('l2010', 1, 1),('helpb', 1, 0)";
                    sqlCom.ExecuteNonQuery();

                    transaction.Commit();
                    transaction = null;
                    creationStatus = 1;
                    
                    Console.WriteLine("[SQL] user id: '" + userid.ToString() + "', Created new character id = " + charId + " , time: ");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    transaction = null;
                    Console.WriteLine("[Server] SQL Error [CreateCharacter]: \n'" + e.ToString());
                    creationStatus = -1;
                }
            }
            return creationStatus;
        }
    }
}
