using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MovieMetaData
{
    public class BuildDB
    {
        public static string Builder()
        {
            //Find movie folder path
            string movieDir = BuildDB.MovieDirPath();

            //Create MovieTable
            Console.WriteLine("\n  Creating MovieMetaDataBase . . . .");
            SQLiteConnection mDBconn = DBCommands.CreateConnection();
            object mDBcmd = DBCommands.CreateNewMovieTable(mDBconn);
            Console.WriteLine("\n\n  Database created.  You should now populate it.");
            return movieDir;
        }

        //Prompt user about .exe and location of movie folder to cycle though
        public static string MovieDirPath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string movieDir = null;
            Console.WriteLine("\n  We need to find your movie folder.\n" +
                                "\n  Use the Mock Movie Folder?   Y or N or <ENTER> to quit.\n");
            string YN = Console.ReadKey().Key.ToString();
            switch (YN.ToUpper())
            {
                case "Y":
                    //movieDir = Directory.GetCurrentDirectory();
                    movieDir = AppDomain.CurrentDomain.BaseDirectory;
                    movieDir = movieDir.Replace("bin\\Debug", "MockFolderSet");
                    break;
                case "N":
                    Console.WriteLine("\n\nOK.\n  Please type the directory path to the folder holding the movie folders.\n" +
                        "\t(typically movies are placed in \n\t\t" + @"C:\Users\'your user name here'\Videos\n");
                    //movieDir = @"c:\Users\napie\Videos";
                    movieDir = Console.ReadLine();
                    break;
                case "X":
                    movieDir = @"c:\Users\napie\Videos";
                    break;
                default:
                    movieDir = userPath + @"\Videos";
                    break;
            }
            return movieDir;
        }

        //public static void addMetaData()  // Add the movie to the database?
        //{
        //    bool confirmed = false;
        //    while (!confirmed)
        //    {
        //        Console.WriteLine("\n  Add Movie metadata to MovieTable? Y or N ");
        //        string YorN = Console.ReadKey().Key.ToString().ToUpper();
        //        while (YorN != "Y") { CycleThuFolders(MovieDirPath());}
        //        Console.Write("\n\n  Hit any key for Main Menu"); Console.ReadKey();
        //        Program.AppMainMenu();
        //    }
        //}

        // One by one, use folder names to derive movie name then prompt user if correct movie 
        public static object CycleThuFolders(string movieDir)
        {
            List<string> folderNames = GetFolders.GetFolderNames(movieDir);

            //cycle through folderNames by hitting API then checking of good reponse
            foreach (string name in folderNames)
            {
                string newname = name;
                Console.WriteLine("\n  Looking up movie: {0}", name);
                ResponseStrings OMDBResponse = OMDBWebRequest.GetOMDBWebRequest(name);
                while (OMDBResponse.Response == "False")
                {
                    //OMDBResponse.newname = name;
                    OMDBResponse = EditTitleLoop.TitleNotFound(name);
                };

            string mTitle = OMDBResponse.Title;
            string mYear = OMDBResponse.Year;
            string mActors = OMDBResponse.Actors;
            ResponseStrings OMDBResponse2 = EditTitleLoop.CorrectMovie(mTitle, mYear, mActors, name, OMDBResponse);
            AddMovie2Database(OMDBResponse2, movieDir);   
            }
            return null;
        }

        private static bool AddMovie2Database(ResponseStrings OMDBResponse2, string movieDir)
        {
            //bool confirmed = false;
            //while (!confirmed)
            {
                Console.WriteLine("\n  Add Movie metadata to MovieTable? Y or N ");
                string YorN = Console.ReadKey().Key.ToString().ToUpper();
                if (YorN == "Y") { DBCommands.InsertOBDMData( DBCommands.CreateConnection(),OMDBResponse2); return true; } // dynamically update table with omdResponse values
                else { Console.WriteLine("\n|n  OK, on to next movie . . .\n"); return false; }
            }
        }

        //TEST Method to make List from API object properties
        static List<String>  GetOMDBPropertyName(ResponseStrings  OMDBReponse)
        {
            List<string> OMDResponseNames = new List<string>();
            string label;
            Type t = OMDBReponse.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                label = prop.Name;
                if (prop.GetIndexParameters().Length == 0)
                    OMDResponseNames.Add(label);
            }
            return OMDResponseNames;
        }

        //TEST Method to make list from API object values
        static List<String> GetOMDBPropertyValue(Object OMDBReponse)
        {
            List<string> mDBdata = new List<string>();
            string data;
            foreach (var prop in OMDBReponse.GetType().GetProperties())
            {
                data = prop.GetValue(OMDBReponse) as string;
                if (prop.GetIndexParameters().Length == 0)
                    mDBdata.Add(data);
            }
            return mDBdata;
        }

        // Hit the OMDB API with the movie title
        //public static ResponseStrings OMDBGetResponse(string name)
        //{
        //    return OMDBWebRequest.GetOMDBWebRequest(name);
        //}

        //Create dictionary from object properties returned from API - will be used to 100% correctly populate MovieTable
        public static Dictionary<string, object> ObjectToDictionary(object obj)  
        {
            Dictionary<string, object> OMDBdict = new Dictionary<string, object>();

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                string propName = prop.Name;
                var val = obj.GetType().GetProperty(propName).GetValue(obj, null);
                if (val != null)
                {
                    OMDBdict.Add(propName, val.ToString());
                }
                else
                {
                    OMDBdict.Add(propName, null);
                }
            }

            return OMDBdict;
        }


    }
}
