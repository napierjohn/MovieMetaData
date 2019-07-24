using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMetaData
{
    class EditTitleLoop
    {
        //if title not found in API, direct user to edit title
        public static ResponseStrings TitleNotFound(string name)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  \'{0}\'", name);
            Console.ResetColor();
            Console.Write(" was not found.\n");

            ResponseStrings OMDBResponse = EditTitle();
         
            return OMDBResponse;
        }

        //if user needs to correct the movie title
        public static ResponseStrings EditTitle()
        {
            Console.WriteLine("\n  Please enter the correct movie title:  ");
            Console.WriteLine();
            string name = Console.ReadLine();
            ResponseStrings OMDBResponse = OMDBWebRequest.GetOMDBWebRequest(name);
            string mTitle = OMDBResponse.Title;
            string mYear = OMDBResponse.Year;
            string mActors = OMDBResponse.Actors;
            CorrectMovie(mTitle, mYear, mActors, name, OMDBResponse);
            Console.WriteLine(OMDBResponse.Title, "EditTitleMethod");

            return OMDBResponse;
        }

        //Show sample of returned API data for user verification
        public static ResponseStrings CorrectMovie(string mTitle, string mYear, string mActors, string name, ResponseStrings OMDBResponse)
        {
            Console.WriteLine("\n\tMovie found:\n\t  Title: {0}\n\t  Year: {1}\n\t  Actors: {2}\n", mTitle, mYear, mActors);
            Console.WriteLine("\n\tDoes this look like the correct movie?  Y or N ");

            ConsoleKeyInfo readKey;
            bool check = false;
            do
            {
                readKey = Console.ReadKey(true);
                check = !((readKey.Key == ConsoleKey.Y) || (readKey.Key == ConsoleKey.N));
            } while (check);
            switch (readKey.Key)
            {
                case ConsoleKey.Y: break;
                case ConsoleKey.N:
                    OMDBResponse = EditTitle();
                    //mTitle = OMDBResponse.Title;
                    //mYear = OMDBResponse.Year;
                    //mActors = OMDBResponse.Actors;
                    //Console.WriteLine("CorrectMovieMethod");
                    //CorrectMovie(mTitle, mYear, mActors, name, OMDBResponse);
                    break;
            }return OMDBResponse; 
        }
    }
}
