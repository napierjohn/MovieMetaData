using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using System.Data;

namespace MovieMetaData
{
    public class DBCommands
    {

        //Commands List:
        // SQLiteConnection sqlite_conn;
        //sqlite_conn = CreateConnection();
        //CreateTable(sqlite_conn);
        //InsertData(sqlite_conn);
        //ReadData(sqlite_conn);




        public static SQLiteConnection CreateConnection()
        {   // Create a new database connection:
            SQLiteConnection mDBconn = new SQLiteConnection("Data Source = MovieMetaDB.sqlite; Version = 3; New = True; Compress = True; ");
            return mDBconn;
        }

        public static object CreateNewMovieTable(SQLiteConnection mDBconn)
        {
            mDBconn.Open();
            SQLiteCommand mDBcmd = mDBconn.CreateCommand();
            mDBcmd.CommandText = @"DROP TABLE IF EXISTS MovieTable";
            mDBcmd.ExecuteNonQuery();
            mDBcmd.CommandText = @"CREATE TABLE IF NOT EXISTS MovieTable  (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, Title TEXT, Year TEXT, Rated TEXT, Released TEXT, Runtime TEXT, Genre TEXT, Director TEXT, Writer TEXT, Actors TEXT, Plot TEXT, Language TEXT, Country TEXT, Awards TEXT, Poster TEXT, Ratings TEXT, Metascore TEXT, imdbRating TEXT, imdbVotes TEXT, imdbID TEXT, Type TEXT, DVD TEXT, BoxOffice TEXT, Production TEXT, Website TEXT, Response TEXT, newname TEXT , UserComment TEXT)";
            mDBcmd.ExecuteNonQuery();
            mDBconn.Close();
            Console.WriteLine("\n MovieTable made: " + mDBcmd.CommandText + "\n");
            return mDBcmd;
        }

        public static void InsertOBDMData(SQLiteConnection mDBconn, ResponseStrings OMDBResponse)
        {   
            SQLiteCommand mDBcmd = mDBconn.CreateCommand();
            string cmdText = @"INSERT INTO MovieTable  (";
            string keyText = "";
            string valText = "";
            string InsertedValues = "";

            var OMDBDict = BuildDB.ObjectToDictionary(OMDBResponse);
            mDBcmd.CommandType = CommandType.Text;
            for (int index = 0; index < OMDBDict.Count-1; index++)
            {
                var item = OMDBDict.ElementAt(index);
                var itemKey = item.Key;
                string itemVal = item.Value.ToString();
                if (index < OMDBDict.Count-2)
                {
                    keyText += itemKey + ", ";
                    valText += "@" + itemKey +  ", "; //"=" + itemKey +
                    mDBcmd.Parameters.Add("@" + itemKey, DbType.AnsiString).Value = itemVal;
                    InsertedValues += itemVal + ", ";
                    Console.WriteLine("Added to Database: " + itemKey + " = " + itemVal);
                }
                else
                {
                    keyText += itemKey + ")";
                    valText += "@" + itemKey + ", )";  //"=" + itemKey + 
                    mDBcmd.Parameters.Add("@" + itemKey, DbType.AnsiString).Value = itemVal;
                    Console.WriteLine("Added to Database: " + itemKey + " = " + itemVal); 
                }
            }
            cmdText += keyText + " VALUES (" + valText + ";";
            try
            {
                mDBcmd.CommandText = cmdText;
                mDBconn.Open();
                mDBcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException sx)
            {   
                Console.Write(sx); //TODO:  improve SQL error handling
            }
            finally
            {
                mDBconn.Close();
                Console.WriteLine("\n Movie data insertion complete: " + OMDBResponse.Title + "\n");
            }
            
        }




        public static void ViewMovieList()
        {
            Console.WriteLine("\n");
            using (SQLiteConnection mDBconn = CreateConnection())
            {
                mDBconn.Open();

                string stm = "SELECT Id, Title, imdbID from MovieTable";

                using (SQLiteCommand cmd = new SQLiteCommand(stm, mDBconn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("\t{0}. {1} \t Verify at: https://www.imdb.com/title/{2}", reader["Id"], reader["Title"], reader["imdbID"]);

                        }
                    }
                }

                mDBconn.Close();
            }
        }

        public static void SearchMovieTable()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Which movie from Database?  (enter \"L\" to see List)");
            string searchTitle = Console.ReadLine().Trim();
            char firstLetter = searchTitle.ToUpper()[0];
            if (firstLetter.Equals('L')) { ViewMovieList(); SearchMovieTable(); }

            using (SQLiteConnection mDBconn = CreateConnection()) 
            {
                mDBconn.Open();
                string sql = "SELECT * from MovieTable Where Title = @Title";
                using (SQLiteCommand mDBcmd = new SQLiteCommand(sql, mDBconn))
                {
                    mDBcmd.Parameters.Add("@Title", DbType.AnsiString).Value = searchTitle;
                    using (SQLiteDataReader reader = mDBcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                    Console.WriteLine("{0,-10} = {1,-10}", reader.GetName(i), reader.GetValue(i));
                            }
                            //Console.WriteLine("{0,-10} {1,-10} {2,5}",  reader.["Title"], reader["imdbID"], reader["Year"]);

                        }
                    }
                }
                mDBconn.Close();
            }
            
        }


        public static void ReadData(SQLiteConnection conn)
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  