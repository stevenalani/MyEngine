using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using MyEngine.XMLMaps;
using OpenTK;

namespace MyEngine
{
    public class HeightmapImporter
    {
        static string folderName = @"XMLMaps";
        public static string getOpenStreetXMLPath(Vector2 leftBottom, Vector2 rightTop, string mapName, int cluster=1)
        {
            /*left minlon.            bottom minlat.            right maxlon.            top maxlat.*/
            Vector2 steps = (rightTop - leftBottom) / cluster;
            var clusterEnd = leftBottom + steps;
            string pathString = System.IO.Path.Combine(folderName, mapName);
            
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
                if (!Directory.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString);
                }
            };
            for (int y = 0; y < cluster; y++)
            {
                for (int x = 0; x < cluster; x++)
                {
                    var fileName = String.Concat(mapName, x.ToString() + x.ToString());
                    var pathString2 = System.IO.Path.Combine(pathString, fileName + ".xml");
                    string url = "https://api.openstreetmap.org/api/0.6/map?bbox=" + leftBottom.X.ToString(CultureInfo.InvariantCulture) + "," + leftBottom.Y.ToString(CultureInfo.InvariantCulture) + "," + clusterEnd.X.ToString(CultureInfo.InvariantCulture) + "," + clusterEnd.Y.ToString(CultureInfo.InvariantCulture) + "";
                    clusterEnd.X += steps.X;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    request.Method = WebRequestMethods.Http.Get;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    var stream = response.GetResponseStream();

                    StreamReader sr = new StreamReader(stream);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    StreamWriter sw = new StreamWriter(pathString2);


                    sw.Write(result);
                    sw.Close();
                }
                clusterEnd.Y += steps.Y;
            }
            return "success";
        }
    }
}