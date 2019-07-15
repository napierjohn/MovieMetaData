using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ConsoleApp7
{
    class DBCommands
    {

        //Commands List:
        // SQLiteConnection sqlite_conn;
        //sqlite_conn = CreateConnection();
        //CreateTable(sqlite_conn);
        //InsertData(sqlite_conn);
        //ReadData(sqlite_conn);


    static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source= MovieMetaDB.db; Version = 3; New = True; Compress = True; ");
         // Open the connection:
         try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
            }
            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn)
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE SampleTable (Col1 VARCHAR(20), Col2 INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        static void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }
    }
}
