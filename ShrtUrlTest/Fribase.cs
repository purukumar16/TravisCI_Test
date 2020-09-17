using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace GoogleDynamicLinksTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = "AIzaSyANe_GmoAnwzouoJ1aRPAxobiVK3wRSdfA";
            string url = "https://google.com?id=123";

            Shorten(apiKey, url);
        }

        public static void Shorten(string key, string url)
        {
            string post = "{\"longDynamicLink\": \"https://testdl.page.link/?link=" + url + "\"}";
            string shortUrl = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=" + key);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");

                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();
                            shortUrl = Regex.Match(json, @"""shortLink"": ?""(?<shortLink>.+)""").Groups["shortLink"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.WriteLine("\nShortened URL: " + shortUrl);
            Console.ReadKey();
        }
    }
}