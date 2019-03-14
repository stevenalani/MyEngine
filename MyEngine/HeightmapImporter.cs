using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using MyEngine.XMLMaps;
using Newtonsoft.Json;
using OpenTK;


namespace MyEngine
{
    public static class HeightmapImporter
    {
        static string folderName = @"XMLMaps";


        [DataContract]
        internal class Location
        {
            [DataMember]
            internal float latitude;
            [DataMember]
            internal float longitude;
        }

        [DataContract]
        public class LocationResult
        {
            [DataMember]
            internal float latitude;
            [DataMember]
            internal float longitude;

            [DataMember] internal int elevation;
        }
        
        public static string ToJSON(this object obj) 
        {
            MemoryStream objectstream = new MemoryStream();
            DataContractJsonSerializer ser;
            if(obj is Location)
                ser = new DataContractJsonSerializer(typeof(Location));
            else if (obj is LocationResult)
                ser = new DataContractJsonSerializer(typeof(LocationResult));
            else
                return "";
            ser.WriteObject(objectstream, obj);
            var json = objectstream.ToArray();
            objectstream.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static LocationResult[] GetOpenElevationData(Vector2 start, Vector2 end)
        {
            string data = "";
            for (float latitude = start.X; latitude < end.X; latitude += 0.01f)
            for (float longitude = start.Y; longitude < end.Y; longitude += 0.01f)
            {
                Location temp = new Location();
                temp.latitude = latitude;
                temp.longitude = longitude;
                data += temp.ToJSON();
            }

            string url = "https://api.open-elevation.com/api/v1/lookup";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.Accept = "application/json";
            request.ContentType="Content-Type: application/json";
            var requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream);
            streamWriter.Write(data);
            streamWriter.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();
                OSM cood = JsonConvert.DeserializeObject<OSM>(result);

                Console.Write(cood.longitude.ToString());
                sr.Close();
                
                return new[] { new LocationResult() };
            }
            catch (Exception e)
            {
                List<LocationResult> testdata = new List<LocationResult>();
                Random random = new Random(DateTime.Now.Millisecond % 15);
                for (float latitude = start.X; latitude < end.X; latitude += 0.01f)
                for (float longitude = start.Y; longitude < end.Y; longitude += 0.01f)
                {
                    LocationResult result = new LocationResult();
                    result.latitude = latitude;
                    result.longitude = longitude;
                    result.elevation = random.Next(0, 150);
                    testdata.Add(result);
                    
                }

                return testdata.ToArray();
            }
        }


        public static string getOpenElevation( float lat, float lon)
        {
           string addCood = lat.ToString(CultureInfo.InvariantCulture) + "," + lon.ToString(CultureInfo.InvariantCulture);
           string url = "https://api.open-elevation.com/api/v1/lookup?locations="+ addCood;
           HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

           request.Method = WebRequestMethods.Http.Get;
           HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           var stream = response.GetResponseStream();

           StreamReader sr = new StreamReader(stream);
           string result = sr.ReadToEnd();
           OSM cood = JsonConvert.DeserializeObject<OSM>(result);

           Console.Write(cood.longitude.ToString());
           sr.Close();
           return result.ToString();
        }
        public static string getOpenStreetXMLBBox(Vector2 leftBottom, Vector2 rightTop, string mapName, int cluster=1)
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