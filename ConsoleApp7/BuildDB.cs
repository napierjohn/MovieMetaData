﻿using System;
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

            //TODO: Check if building for 1st time - Is MovieMetaDB file at stated directory?
            //string FirstBuildCheck = Console.WriteLine("\n You are about to make a new copy of the MovieTable.  Continue?  ");

            //Create MovieTable
            Console.WriteLine("\n Creating MovieMetaDataBase . . . .");
            SQLiteConnection mDBconn = DBCommands.CreateConnection();
            object mDBcmd = DBCommands.CreateNewMovieTable(mDBconn);




            return movieDir;
        }
    
        //Ask to add Movies to table
        public static void addMetaData()
        {
            bool confirmed = false;
            while (!confirmed)
            {
                Console.WriteLine("\n Add Movie metadata to MovieTable? Y or N ");
                string YorN = Console.ReadKey().Key.ToString().ToUpper();
                if (YorN == "Y") { CycleThuFolders(MovieDirPath()); }
                else { Program.AppMainMenu();}
            }
        }


        public static object CycleThuFolders(string movieDir)
        {
            List<string> folderNames = GetFolders.GetFolderNames(movieDir);

            //cycle through folderNames by hitting API then checking of good reponse
            foreach (string name in folderNames)
            {
                string newname = name;
                Console.WriteLine("\nLooking up movie: {0}", name);
                ResponseStrings OMDBResponse = OMDBGetResponse(name);
                while (OMDBResponse.Response == "False")
                {
                    //OMDBResponse.newname = name;
                    OMDBResponse = EditTitleLoop.TitleNotFound(name);
                };

                string mTitle = OMDBResponse.Title;
                string mYear = OMDBResponse.Year;
                string mActors = OMDBResponse.Actors;
                EditTitleLoop.CorrectMovie(mTitle, mYear, mActors, name, OMDBResponse);
                AddMovie2Database(OMDBResponse, movieDir);
                
            }
            return null;
        }

        private static void AddMovie2Database(ResponseStrings OMDBResponse, string movieDir)
        {
            //bool confirmed = false;
            //while (!confirmed)
            {
                Console.WriteLine("\n Add Movie metadata to MovieTable? Y or N ");
                string YorN = Console.ReadKey().Key.ToString().ToUpper();
                if (YorN == "Y") { DBCommands.InsertOBDMData( DBCommands.CreateConnection(),OMDBResponse); } // dynamically update table with omdResponse values
                else { Program.AppMainMenu(); }
            }
        }


        //TEST MEthod
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




        public static ResponseStrings OMDBGetResponse(string name)
        {
            return OMDBWebRequest.GetOMDBWebRequest(name);
        }

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

        public static string MovieDirPath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string movieDir =null;
            Console.WriteLine("\nFirst we need to find the main movie directory holding the movie folders.\n" +
                                "\nAre the movie folder in the same folder as you placed this program? Y or N or <ENTER> to quit.\n");
            string YN = Console.ReadKey().Key.ToString();
            switch (YN.ToUpper())
            {
                case "Y":
                    movieDir = Directory.GetCurrentDirectory();
                    break;
                case "N":
                    Console.WriteLine("\n\nOK.\n Please type the directory path to the folder holding the movie folders.\n" + 
                        "\t(typically movies are placed in \n\t\t" +@"C:\Users\'your user name here'\Videos )" + 
                        "\n If you are not sure, an easy method to find it is to use 'File Explorer' to locate the folder then\n" + " copy the address from 'address bar'.");
                    movieDir = @"c:\Users\napie\Videos";
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
    }
}
