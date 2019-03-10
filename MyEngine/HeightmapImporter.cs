using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using OpenTK;

namespace MyEngine
{
    public class HeightmapImporter
    {
        public HeightmapImporter()
        {
            string url = "https://api.openstreetmap.org/api/0.6/map?bbox=11.54,48.14,11.543,48.145";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);
            string result = sr.ReadToEnd();
            sr.Close();
            StreamWriter sw = new StreamWriter("openstreetresult.xml");
            sw.Write(result);
            sw.Close();
        }

        public static string getOpenStreetXMLPath(Vector2 leftBottom, Vector2 rightTop, string filePath)
        {
            /*left minlon.            bottom minlat.            right maxlon.            top maxlat.*/
            
            string filepath = "";
            string path = filePath == ""?"openStreet":filepath;
            string url = "https://api.openstreetmap.org/api/0.6/map?bbox=" + leftBottom.X + "," + leftBottom.Y + "," + rightTop.X +", " + rightTop.Y+"";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);
            string result = sr.ReadToEnd();
            sr.Close();
            StreamWriter sw = new StreamWriter("openstreetresult.xml");
            sw.Write(result);
            sw.Close();
            return "";
        }
    }
}