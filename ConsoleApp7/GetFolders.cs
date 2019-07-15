using System;
using System.Collections.Generic;
using System.IO;


namespace MovieMetaData
{
    public class GetFolders
    {
        public object dirArr { get; private set; }


        public static List<string> GetFolderNames(string movDir)
        {
            // Make a reference to a directory.
            DirectoryInfo di = new DirectoryInfo(movDir);
            DirectoryInfo[] dirArr = di.GetDirectories();
            List<string> foldNames = new List<string>();
            // Display the names of the directories.
            foreach (var dir in dirArr)
            {
                foldNames.Add(dir.Name);
            }
            return CleanFolderNames(foldNames);

        }

        public static List<string> CleanFolderNames(List<string> foldNames)
        {
            List<string> cleanFolderList = new List<string>();
            char[] stopChar = { '.', '(', '-' };

            foreach (string fname in foldNames)
            {
                if (fname.IndexOfAny(stopChar) == -1)
                {
                    cleanFolderList.Add(fname);
                    continue;
                }

                int par = fname.IndexOf('(');
                int period = fname.IndexOf('.');
                int dash = fname.IndexOf("-"); //"\u002D"

                int index = par > -1 ? par - 1 : period > -1 ? period : dash - 1;

                cleanFolderList.Add(fname.Substring(0, index));
            }

            return cleanFolderList;
        }

    }
}
