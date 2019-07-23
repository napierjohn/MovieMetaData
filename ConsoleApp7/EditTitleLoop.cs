﻿using System;
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
            Console.Write("\'{0}\' was not found.", name);
            ResponseStrings OMDBResponse2 = EditTitle();
            return OMDBResponse2;
        }

        //if user needs to correct the movie title
        public static ResponseStrings EditTitle()
        {
            Console.WriteLine("\nPlease enter the correct movie title:  ");
            Console.WriteLine();
            string newname = Console.ReadLine();
            ResponseStrings OMDBResponse2 = OMDBGetResponse2(newname);

            return OMDBResponse2;
        }
        //for web request to API if original title is incorrect
        public static ResponseStrings OMDBGetResponse2(string newname)
        {
            string name = newname;
            ResponseStrings OMDBResponse2 = OMDBWebRequest.GetOMDBWebRequest(name);
            return OMDBResponse2;
        }

        //Show sample of returned API data for user verification
        public static ResponseStrings CorrectMovie(string mTitle, string mYear, string mActors, string name, ResponseStrings OMDBResponse)
        {
            Console.WriteLine("\nDoes this look like the correct movie?  Y or N ");
            Console.WriteLine("\n\tTitle: {0}\n\tYear: {1}\n\tActors: {2}\n", mTitle, mYear, mActors);
            string correctMovie;
            correctMovie = Console.ReadKey().Key.ToString().ToUpper();
            if (correctMovie == "N")
            {
                OMDBResponse = EditTitle();
                return OMDBResponse;
            };
            return OMDBResponse;
        }

    }
}
