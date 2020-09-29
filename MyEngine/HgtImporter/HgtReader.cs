using System.IO;

namespace MyEngine.HgtImporter
{
    public enum Samplingrate { ONEARCSECOND = 3601, THREEARCSECONDS = 1201, THIRTYARCSECONDS = 121 }
    public class HgtReader
    {
        const uint SAMPLES = 3602;
        public short[,] GetElevationData(string path, Samplingrate samplingrate = Samplingrate.ONEARCSECOND)
        {

            short[,] result = new short[(uint)samplingrate, (uint)samplingrate];
            if (path == null || path == "")
                return result;
            FileStream fileStream = File.Open(path, FileMode.Open);
            BinaryReader reader = new BinaryReader(fileStream);
            byte[] buffer;
            for (int y = 0; y < (uint)samplingrate; y++)
            {
                for (int x = 0; x < (uint)samplingrate; x++)
                {
                    buffer = reader.ReadBytes(2);
                    var value = buffer[0] << 8 | buffer[1];
                    result[y, x] = (short)value;
                }
            }
            fileStream.Close();
            return result;
        }
    }
}
