using System;


namespace MovieMetaData
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("\n\n  This app builds a database to hold movie metadata." +
                "\n  It looks up the movie titles in your movie folder and retrieves movie metadata from the OMDB API." + 
                "\n  On your first use:  \n\tCreate the Database (Opotions A)\n\tthen Populate it (Options B)"+
            "\n  You can then search the database and add user commemts.");
            Console.WriteLine("\n\n  For Demonstration Purposes:" +
                "\n\tA mock Main Movie folder has been included in this solution." +
                "\n\tWhen prompted, please use it.\n");

            while (true)
            {
                AppMainMenu();
            }

        }

        static public void AppMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("\n\n     ++++ Main Menu ++++");
            Console.WriteLine();
            Console.WriteLine("A: Create new MovieMetaDatabase");
            Console.WriteLine("B: Populate MovieTable from movie folders");
            Console.WriteLine("C: View movie list");
            Console.WriteLine("D: Search MovieMetDatabase (MovieTable)");
            Console.WriteLine("E: Edit your movie comments");
            Console.WriteLine("X: Exit");
            var menuChoice = Console.ReadKey().Key.ToString().ToUpper();
            MenuOptions(menuChoice);
        }

    public static void MenuOptions(string menuChoice)
        {
            switch (menuChoice)
            {
                case "A":
                    Console.WriteLine("\n\tYou are about to create a new empty MovieMetaData database and erase any existing database." +
                        "\n\tContinue?    Y or N \n");
                    ConsoleKeyInfo readKey;
                    bool check = false;
                    do
                    {
                        readKey = Console.ReadKey(true);
                        check = !((readKey.Key == ConsoleKey.Y) || (readKey.Key == ConsoleKey.N));
                    } while (check);
                    switch (readKey.Key)
                    {
                        case ConsoleKey.Y: BuildDB.Builder();  break;
                        case ConsoleKey.N: AppMainMenu(); break;
                    }
                    break;
                case "B":
                    string movieDir = BuildDB.MovieDirPath();
                    BuildDB.CycleThuFolders(movieDir);
                    break;
                case "C":
                    if (DBCommands.CheckIfDBExists() == true) { DBCommands.ViewMovieList(); }
                    else { Console.WriteLine("\n  Need to populate database."); }
                    break;
                case "D":
                    DBCommands.SearchMovieTable();
                    break;
                case "E":
                    DBCommands.UpdateUserComment();

                    break;
                case "X":
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine("\n ");
                    Console.Beep();
                    DateTime Tthen = DateTime.Now;
                    do {} while (Tthen.AddSeconds(1) > DateTime.Now);
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\n Invalid response");
                    //Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
            }
            //Console.WriteLine("Key to End");
            //Console.ReadKey();
        }
    }
}


