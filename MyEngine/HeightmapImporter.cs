using MyEngine.XMLMaps;
using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using MyEngine.Logging;


namespace MyEngine
{

    public static class HeightmapImporter
    {
        private static int[] testdata = new[]
        {
            19, 7, 18, 17,20, 7, 3, 1,6, 3, /**/20, 14, 2, 20, 4, 2, 16, 16, 1, 13, /*2.*/6, 9, 20, 7, 7, 11, 19, 6, 20,
            15, /**/6, 16, 15, 5, 2, 15, 1, 9, 3, 17, /**/15, 9, 8, 15, 3, 20, 3, 18, 3, 19, 19, 18, 3, 11, 3, 9, 11,15,
            18, 10, 3, 14, 6, 10, 4, 17, 4, 3, 5, 20, 19, 11, 13, 6, 5, 4, 18, 4, 17, 18, 9, 8, 20, 8, 20, 13, 8, 13,12,
            9, 6, 17, 19, 12, 8, 19, 2, 13, 10, 5, 7, 11, 4, 3, 12, 16, 12, 2, 12, 6, 1, 1, 10, 13, 14, 10, 6, 14, 2, 18,
            2, 1, 13, 7, 6, 7, 19, 10, 2, 3, 17, 19, 3, 8, 13, 10, 7, 20, 12, 16, 7, 10, 5, 8, 15, 19, 20, 11, 8, 11, 5,
            17, 4, 7, 2, 8, 5, 19, 16, 9, 9, 16, 6, 11, 19, 7, 13, 11, 5, 12, 5, 7, 20, 17, 19, 15, 20, 19, 17, 2, 4, 18,
            15, 10, 3, 4, 3, 16, 4, 16, 8, 14, 7, 7, 15, 19, 8, 8, 17, 12, 1, 15, 12, 16, 20, 20, 13, 12, 15, 5, 16, 4,
            14, 4, 15, 10, 2, 4, 6, 15, 15, 8, 1, 2, 6, 2, 18, 3, 8, 15, 5, 1, 2, 15, 2, 6, 6, 12, 18, 4, 12, 9, 17, 12,
            4, 2, 14, 11, 3, 17, 19, 18, 18, 10, 13, 13, 8, 7, 12, 3, 11, 19, 3, 1, 1, 12, 17, 8, 10, 19, 11, 1, 5, 1,
            18, 5, 14, 1, 5, 20, 13, 8, 15, 9, 20, 17, 3, 13, 17, 14, 6, 18, 1, 12, 20, 2, 1, 20, 2, 7, 3, 5, 5, 20, 5,
            9, 17, 8, 19, 5, 8, 1, 19, 3, 1, 4, 13, 13, 13, 15, 15, 7, 18, 11, 17, 5, 4, 9, 14, 8, 9, 15, 7, 12, 16, 20,
            11, 8, 15, 4, 18, 7, 16, 5, 3, 20, 20, 2, 11, 16, 12, 5, 2, 4, 9, 6, 19, 18, 7, 13, 3, 11, 6, 10, 17, 18,
            10, 9, 20, 13, 13, 12, 3, 14, 1, 12, 2, 18, 9, 17, 18, 12, 6, 5, 18, 7, 4, 14, 10, 20, 7, 18, 14, 15, 2, 3,
            6, 12, 18, 2, 10, 12, 19, 2, 9, 9, 19, 6, 16, 3, 6, 13, 2, 13, 7, 13, 18, 13, 11, 14, 4, 1, 18, 7, 8, 15,
            12, 1, 3, 9, 16, 16, 12, 15, 9, 11, 17, 9, 7, 13, 5, 2, 7, 20, 3, 14, 14, 14, 15, 5, 18, 7, 14, 3, 14, 10,
            17, 14, 12, 17, 8, 6, 15, 9, 4, 2, 15, 17, 19, 1, 13, 13, 13, 5, 2, 10, 11, 9, 2, 16, 4, 2, 2, 10, 18, 14,
            18, 8, 9, 6, 4, 12, 4, 5, 5, 11, 10, 2, 1, 18, 11, 14, 12, 10, 14, 9, 5, 16, 18, 2, 16, 19, 2, 10, 8, 2, 5,
            17, 9, 3, 3, 16, 19, 17, 8, 16, 9, 8, 15, 5, 11, 11, 19, 10, 16, 12, 10, 17, 1, 8, 8, 1, 16, 15, 7, 6, 4, 1,
            10, 20, 4, 9, 4, 2, 14, 3, 14, 5, 10, 5, 2, 12, 5, 7, 10, 19, 5, 8, 8, 3, 20, 5, 12, 11, 20, 8, 16, 2, 6,
            20, 15, 11, 18, 3, 18, 10, 17, 14, 8, 20, 16, 9, 11, 10, 13, 19, 16, 1, 16, 16, 17, 5, 1, 15, 14, 11, 10,
            11, 12, 3, 4, 12, 5, 11, 12, 2, 7, 12, 15, 3, 8, 1, 10, 17, 11, 2, 6, 4, 10, 7, 16, 19, 1, 4, 19, 8, 10, 16,
            2, 6, 12, 5, 17, 12, 13, 13, 13, 11, 16, 13, 14, 5, 12, 1, 10, 20, 5, 4, 3, 19, 14, 3, 20, 18, 13, 16, 1, 4,
            10, 4, 1, 5, 16, 20, 15, 17, 7, 8, 14, 2, 13, 7, 14, 14, 19, 8, 9, 20, 16, 16, 19, 14, 13, 16, 19, 3, 9, 12,
            18, 11, 12, 12, 14, 9, 15, 8, 16, 9, 4, 8, 16, 6, 17, 2, 20, 16, 16, 1, 9, 1, 7, 20, 16, 1, 19, 13, 6, 5, 8,
            5, 4, 16, 8, 19, 2, 6, 6, 3, 10, 18, 19, 3, 13, 17, 12, 2, 3, 4, 1, 9, 18, 10, 1, 16, 20, 10, 12, 18, 19, 2,
            12, 9, 18, 7, 12, 18, 13, 3, 15, 11, 2, 6, 14, 9, 18, 19, 18, 1, 15, 14, 19, 15, 18, 7, 6, 20, 18, 15, 11,
            5, 13, 20, 18, 10, 4, 17, 11, 18, 20, 20, 3, 10, 2, 20, 8, 13, 2, 1, 3, 12, 13, 10, 16, 19, 9, 18, 9, 7, 17,
            2, 17, 17, 3, 9, 13, 7, 6, 2, 20, 7, 8, 14, 8, 18, 20, 3, 14, 13, 19, 6, 18, 4, 5, 3, 18, 14, 15, 14, 10,
            19, 5, 1, 5, 20, 18, 19, 3, 16, 14, 9, 12, 11, 8, 8, 9, 2, 5, 11, 6, 8, 6, 9, 10, 18, 15, 7, 7, 12, 17, 7,
            19, 1, 10, 14, 3, 12, 18, 15, 16, 13, 6, 10, 2, 2, 13, 6, 8, 9, 12, 8, 9, 18, 13, 10, 1, 20, 1, 16, 2, 13,
            14, 18, 16, 1, 18, 10, 1, 5, 14, 9, 5, 11, 3, 20, 10, 13, 4, 10, 13, 5, 18, 4, 7, 19, 10, 16, 6, 17, 17, 3,
            6, 8, 11, 11, 12, 12, 9, 8, 2, 19, 3, 8, 7, 13, 12, 6, 9, 6, 7, 11, 6, 20, 4, 15, 4, 16, 5, 3, 8, 1, 7, 13,
            12, 19, 8, 16, 7, 13, 1, 16, 19, 17, 7, 15, 16, 12, 11, 2, 13, 20, 12, 1, 15, 11, 3, 15, 4, 14, 1, 20
        };
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
            if (obj is Location)
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
            pointsPerAxis = pointsPerAxis.X != 0 && pointsPerAxis.Y != 0 ? pointsPerAxis : new Vector2(10f);
            var steps = new Vector2((end - start).X / pointsPerAxis.X, (end - start).Y / pointsPerAxis.Y);
            double stepsX = steps.X;
            double stepsY = steps.Y;
            string data = "";
            List<Location> dataList = new List<Location>();
            for (int i = 1; i <= pointsPerAxis.X; i++)
                for (int j = 1; j <= pointsPerAxis.Y; j++)
                {
                    Location temp = new Location();
                    temp.latitude = (float)(i * stepsX) + start.X;
                    temp.longitude = (float)(j * stepsY) + start.Y;
                    dataList.Add(temp);
                    data += temp.ToJSON();
                }
            /*
            for (double latitude = start.Value; latitude < end.Value; latitude += stepsX)
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
            request.ContentType = "Content-Type: application/json";
            var requestStream = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream);
            streamWriter.Write(data);
            streamWriter.Close();
            request.Timeout = 100;
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
                for (int i = 0; i <= pointsPerAxis.X * pointsPerAxis.Y; i++)
                {
                    Random random = RandomProvider.GetThreadRandom();
                    var result = new LocationResult();
                    result.latitude = (float)(i * stepsX) + start.X;
                    result.longitude = (float)(i * stepsY) + start.Y;
                    //result.elevation = HeightmapImporter.testdata[i];
                    result.elevation = random.Next(0, 50);
                    testdata.Add(result);
                    DebugHelpers.Log(e.ToString());
                }


                return testdata;
            }
        }


        public static string getOpenElevation(float lat, float lon)
        {
            string addCood = lat.ToString(CultureInfo.InvariantCulture) + "," + lon.ToString(CultureInfo.InvariantCulture);
            string url = "https://api.open-elevation.com/api/v1/lookup?locations=" + addCood;
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


        public static string getOpenStreetXMLBBox(Vector2 leftBottom, Vector2 rightTop, string mapName, int cluster = 1)
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
    public static class RandomProvider
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}