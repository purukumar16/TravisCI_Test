using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace ShrtUrlTest
{
    public class Program
    {
        public string GoogleAPIkey = "AIzaSyANe_GmoAnwzouoJ1aRPAxobiVK3wRSdfA";
        static void Main(string[] args)
        {
            string shorturl = ShortUrl1();

            Console.WriteLine(shorturl);
            Console.ReadLine();
        }

        public static string ShortUrl1()
        {
            string shorturl;
            string GoogleAPIkey = "AIzaSyANe_GmoAnwzouoJ1aRPAxobiVK3wRSdfA";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + GoogleAPIkey);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            string myFBDomain = "https://myappurl.page.link/?link=";

            string longUrl = myFBDomain + "https://www.youtube.com/watch?v=Sjm7cZmwK98";

            // string longurl = "https://YOURDOMAIN.page.link/?link=https://www.YOURLONGURL.com/account?g=sdasdhaskjdh4547576aksgskjg";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"longDynamicLink\":\"" + longUrl + "\"}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                dynamic data = JObject.Parse(responseText);
                shorturl = data.shortLink;
            }

            return shorturl;
        }
    }
}
