using System;


namespace MovieMetaData
{
    class Program
    {
        static void Main()
        {

            string menuOptions = AppMainMenu();
            switch (menuOptions)
            {
                case "A":
                    BuildDB.Builder();
                    break;
                case "B":

                    break;
                case "C":

                    break;
                case "D":
             
                    break;
                case "E":

                    break;
                default:
                    Console.WriteLine("Invalid response");
                    break;
            }
            Console.WriteLine("Key to End");
            Console.ReadKey();
        }

    static public string AppMainMenu()
        {
            Console.WriteLine("     ++++ Main Menu ++++");
            Console.WriteLine();
            Console.WriteLine("A: Create new MovieMetaDatabase");
            Console.WriteLine("B: View movie list");
            Console.WriteLine("C: Search MovieMetDatabase");
            Console.WriteLine("D: Edit your movie comments");
            Console.WriteLine("E: Exit");
            var menuChoice = Console.ReadKey().Key.ToString().ToUpper();
            return menuChoice;
        }
    }
}


