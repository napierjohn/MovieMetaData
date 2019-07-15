using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MovieMetaData
{
    public class BuildDB
    {
        public static object Builder()
        {
            //Find movie folder path
            string movieDir = BuildDB.MovieDirPath();
            //TODO: Check if building for 1st time - Is MovieMetaDB file at stated directory?

            List<string> folderNames = GetFolders.GetFolderNames(movieDir);


            ResponseStrings OMDBResponse = OMDBGetResponse(folderNames[1]);
            Console.WriteLine();
            GetPropertyNameValues(OMDBResponse);
            return null;
        }



        //TEST MEthod
        private static void GetPropertyNameValues(Object obj)
        {
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
                if (prop.GetIndexParameters().Length == 0)
                    Console.WriteLine("   {0} : {1}", prop.Name,
                                      prop.GetValue(obj));
        }

        public static ResponseStrings OMDBGetResponse(string name)
        {
            return OMDBWebRequest.GetOMDBWebRequest(name);
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
                        "\n\tIf you are not sure, an easy method to find it is to use 'File Explorer' to locate the folder then\n" + "copy the address from 'address bar'.");
                    movieDir = @"c:\Users\napie\Videos";
                    //movieDir = Console.ReadLine();
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
