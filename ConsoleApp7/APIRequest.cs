using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;


namespace MovieMetaData
{
    //API request called from BuildDB.  'name' = variable from 'fore each' loop of FolderList
    class OMDBWebRequest
    {
        public static ResponseStrings GetOMDBWebRequest(string name)
        {
            var webClient = new WebClient();
            byte[] OMDBRequest = webClient.DownloadData(string.Format("http://www.omdbapi.com/?t={0}&apikey=3ab1833e&plot=full", name));
            var jsonString = System.Text.Encoding.UTF8.GetString(OMDBRequest);
            ResponseStrings OMDBResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseStrings>(jsonString);

            return OMDBResponse;
        }


    }

}



