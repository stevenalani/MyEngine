using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MyEngine
{
    internal class HeightmapImporter
    {
        public int id { get; set; }

        public HeightmapImporter()
        {
            string url = "https://api.openstreetmap.org/api/0.6/map?bbox=11.54,48.14,11.543,48.145";
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
        }
    }
}