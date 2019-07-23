using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using System.Data;

namespace MovieMetaData
{   // Methods to manage database
    public class DBCommands
    {
        public static SQLiteConnection CreateConnection()
        {   // Create a new database connection:
            SQLiteConnection mDBconn = new SQLiteConnection("Data Source = MovieMetaDB.sqlite; Version = 3; New = True; Compress = True; ");
            return mDBconn;
        }

        // Creates table to match OMDB response
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

        // Insert Proprties of object holing OMDB API resonse 
        // This dyanmially builds the SQL and uses Reflection and parameterization of object to avoid SQL Injection
        public static void InsertOBDMData(SQLiteConnection mDBconn, ResponseStrings OMDBResponse)
        {   
            SQLiteCommand mDBcmd = mDBconn.CreateCommand();
            string cmdText = @"INSERT INTO MovieTable  (";
            string keyText = "";
            string valText = "";
            //create dictionary
            var OMDBDict = BuildDB.ObjectToDictionary(OMDBResponse);
            //set up for creating sql command text
            mDBcmd.CommandType = CommandType.Text;
            for (int index = 0; index < OMDBDict.Count-2; index++)
            {   
                var item = OMDBDict.ElementAt(index);
                var itemKey = item.Key;
                string itemVal = item.Value.ToString();

                if (index < OMDBDict.Count-3)
                {
                    keyText += itemKey + ", ";
                    valText += "@" + itemKey +  ", "; //"=" + itemKey +
                    mDBcmd.Parameters.Add("@" + itemKey, DbType.AnsiString).Value = itemVal;
                     Console.WriteLine("Added to Database: " + itemKey + " = " + itemVal);
                }
                else
                {
                    keyText += itemKey + " )";
                    valText += "@" + itemKey + " )";  //"=" + itemKey + 
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

        //Shows a formatted table of movie titles w/ their IMDB http address to check
        public static void ViewMovieList()
        {
            Console.WriteLine("\n");
            using (SQLiteConnection mDBconn = CreateConnection())
            {
                mDBconn.Open();

                string sql = "SELECT Id, Title, imdbID from MovieTable";

                using (SQLiteCommand mDBcmd = new SQLiteCommand(sql, mDBconn))
                {
                    using (SQLiteDataReader reader = mDBcmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(string.Format("{0,2}.{1,-25}{2}{3}", reader["Id"], reader["Title"], "Verify at: https://www.imdb.com/title/",reader["imdbID"]));

                        }
                    }
                }

                mDBconn.Close();
            }
        }
        // Search MovieTable by Title
        // This dyanmially builds the SQL and uses parameterization of object to avoid SQL Injection
        public static void SearchMovieTable() 
        {
            Console.WriteLine("\n\tWhich movie from Database?  (enter \"L\" to see List)\n\t");
            string searchTitle = Console.ReadLine().Trim();
            char firstLetter = searchTitle.ToUpper()[0];
            if (firstLetter.Equals('L')) { ViewMovieList(); SearchMovieTable(); }  // Option to show Movie List

            using (SQLiteConnection mDBconn = CreateConnection()) 
            {
                mDBconn.Open();
                string sql = "SELECT * from MovieTable Where Title = @Title";
                using (SQLiteCommand mDBcmd = new SQLiteCommand(sql, mDBconn))
                {
                    mDBcmd.Parameters.Add("@Title", DbType.AnsiString).Value = searchTitle;
                    using (SQLiteDataReader reader = mDBcmd.ExecuteReader())
                    {   //print movie list table
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine("{0,-10} = {1,-10}", reader.GetName(i), reader.GetValue(i));
                            }
                        }
                    }
                }
                mDBconn.Close();
            }
            
        }

        //Paramaterized UPDATE UserComment column
        // This dyanmially builds the SQL and uses parameterization of object to avoid SQL Injection
        public static void UpdateUserComment()  
        {
            Console.WriteLine("\n\tWhich movie from Database?  (enter \"L\" to see List)\n\t");
            string searchTitle = Console.ReadLine().Trim();

            char firstLetter = searchTitle.ToUpper()[0];
            if (firstLetter.Equals('L')) { ViewMovieList(); SearchMovieTable(); }  // option to show Movie List

            Console.WriteLine("\n\tType your comments without hitting <Enter> till you are done.\n\t");
            string userComm = Console.ReadLine().Trim();

            using (SQLiteConnection mDBconn = CreateConnection())
            {
                mDBconn.Open();
                string sql = "UPDATE MovieTable SET UserComment = @UserComment WHERE Title = @Title";
                using (SQLiteCommand mDBcmd = new SQLiteCommand(sql, mDBconn))
                {
                    mDBcmd.Parameters.Add("@Title", DbType.AnsiString).Value = searchTitle;
                    mDBcmd.Parameters.Add("@UserComment", DbType.AnsiString).Value = userComm;
                    mDBcmd.ExecuteNonQuery();
                }
                Console.WriteLine("\n\tUser Comment for {0} updated to read: {1}\n\t", searchTitle, userComm);
                mDBconn.Close();
            }
        }

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  