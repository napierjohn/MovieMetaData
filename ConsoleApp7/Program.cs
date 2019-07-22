using System;


namespace MovieMetaData
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                AppMainMenu();
            }

        }

        static public void AppMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("     ++++ Main Menu ++++");
            Console.WriteLine();
            Console.WriteLine("A: Create new MovieMetaDatabase");
            Console.WriteLine("B: Create MovieTable from movie folders");
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
                    BuildDB.Builder();
                    break;
                case "B":
                    string movieDir = BuildDB.MovieDirPath();
                    BuildDB.CycleThuFolders(movieDir);
                    break;
                case "C":
                    DBCommands.ViewMovieList();
                    break;
                case "D":
                    DBCommands.SearchMovieTable();
                    break;
                case "E":
                    DBCommands.AddComment();

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
                    Console.WriteLine("\nInvalid response");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
            }
            //Console.WriteLine("Key to End");
            //Console.ReadKey();
        }
    }
}


