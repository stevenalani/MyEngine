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
            if (obj is List<Location>)
                ser = new DataContractJsonSerializer(typeof(List<Location>));
            else if (obj is LocationResult)
                ser = new DataContractJsonSerializer(typeof(LocationResult));
            else
                return "";
            ser.WriteObject(objectstream, obj);
            var json = objectstream.ToArray();
            objectstream.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static List<LocationResult> GetOpenElevationData(Vector2 start, Vector2 end, Vector2 pointsPerAxis = default(Vector2))
        {
            pointsPerAxis = pointsPerAxis.X != 0 && pointsPerAxis.Y != 0 ? pointsPerAxis :new Vector2(10f);
            var steps = new Vector2((end - start).X / pointsPerAxis.X,(end - start).Y / pointsPerAxis.Y);
            double stepsX = steps.X;
            double stepsY = steps.Y;
            string data = "";
            List<Location> dataList = new List<Location>();
            for (int i = 1; i <= pointsPerAxis.X; i++)
            for (int j = 1; j <= pointsPerAxis.Y; j++)
            {             
                Location temp = new Location();
                temp.latitude = (float) (i * stepsX) + start.X;
                temp.longitude = (float)(j * stepsY) + start.Y;
                dataList.Add(temp);
                data += temp.ToJSON();
            }
            /*
            for (double latitude = start.X; latitude < end.X; latitude += stepsX)
            for (double longitude = start.Y; longitude < end.Y; longitude += stepsY)
            {
                Location temp = new Location();
                temp.latitude = (float)latitude;
                temp.longitude = (float)longitude;
                dataList.Add(temp);
                data += temp.ToJSON();
            }*/

            data = dataList.ToJSON();

            string url = "https://api.open-elevation.com/api/v1/lookup";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.Accept = "application/json";
            request.ContentType="Content-Type: application/json";
            var requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream);
            streamWriter.Write(data);
            streamWriter.Close();
            request.Timeout = 10000;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();
                var locationResults = JsonConvert.DeserializeObject<List<LocationResult>>(result);

                
                sr.Close();
                
                return locationResults;
            }
            catch (Exception e)
            {
                List<LocationResult> testdata = new List<LocationResult>();
                Random random = new Random(DateTime.Now.Millisecond % 15);
                for (int i = 1; i <= pointsPerAxis.X; i++)
                for (int j = 1; j <= pointsPerAxis.Y; j++)
                {
                    LocationResult result = new LocationResult();
                    result.latitude = (float)(i * stepsX) + start.X;
                    result.longitude = (float)(j * stepsY) + start.Y;
                    result.elevation = random.Next(0, 10);
                    testdata.Add(result);
                    
                }

                return testdata;
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