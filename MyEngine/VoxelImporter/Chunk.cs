using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyEngine.VoxelImporter
{
    public class Chunk
    {
        protected int ChunkChildrenSize;
        protected int ChunkSize;
        protected string Id;
    }

    public class MainChunk : Chunk
    {
        public MainChunk(byte[] dataBytes)
        {
            ChildChunks = new List<Chunk>();
            Id = Encoding.UTF8.GetString(dataBytes.Take(4).ToArray());
            ChunkSize = BitConverter.ToInt32(dataBytes.Skip(4).Take(4).ToArray(), 0);
            ChunkChildrenSize = BitConverter.ToInt32(dataBytes.Skip(8).Take(4).ToArray(), 0);
            var bytesRead = 12;
            while (bytesRead < dataBytes.Length)
            {
                var chunkId = Encoding.UTF8.GetString(dataBytes.Skip(bytesRead).Take(4).ToArray());
                bytesRead += 4;
                var size = BitConverter.ToInt32(dataBytes.Skip(bytesRead).Take(4).ToArray(), 0);
                bytesRead += 4;
                var sizeChildren = BitConverter.ToInt32(dataBytes.Skip(bytesRead).Take(4).ToArray(), 0);
                bytesRead += 4;
                var chunkData = dataBytes.Skip(bytesRead).Take(size).ToArray();
                bytesRead += size;
                switch (chunkId)
                {
                    case "PACK":
                        ChildChunks.Add(new PackChunk(chunkData, size, sizeChildren));
                        break;
                    case "SIZE":
                        ChildChunks.Add(new SizeChunk(chunkData, size, sizeChildren));
                        break;
                    case "XYZI":
                        ChildChunks.Add(new XyziChunk(chunkData, size, sizeChildren));
                        break;
                    case "RGBA":
                        ChildChunks.Add(new RgbaChunk(chunkData, size, sizeChildren));
                        break;
                    case "MATT":
                        ChildChunks.Add(new MattChunk(chunkData, size, sizeChildren));
                        break;
                }
            }
        }

        public List<Chunk> ChildChunks { get; set; }
    }

    public class PackChunk : Chunk
    {
        public int NumberOfChildModels { get; set; }
        public PackChunk(byte[] chunkData, int size, int sizeChildren)
        {
            Id = "PACK";
            ChunkSize = size;
            ChunkChildrenSize = sizeChildren;
            NumberOfChildModels = BitConverter.ToInt32(chunkData, 0);
        }
    }

    public class SizeChunk : Chunk
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public SizeChunk(byte[] chunkData, int size, int sizeChildren)
        {
            Id = "SIZE";
            ChunkSize = size;
            ChunkChildrenSize = sizeChildren;

            var bytesProcessed = 0;
            X = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            Y = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            Z = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
        }
    }

    public class XyziChunk : Chunk
    {
        public int numberOfVoxels { get; set; }
        public List<Tuple<int, int, int, int>> Voxels { get; set; }
        public XyziChunk(byte[] chunkData, int size, int sizeChildren)
        {
            Id = "XYZI";
            ChunkSize = size;
            ChunkChildrenSize = sizeChildren;
            var bytesProcessed = 0;
            numberOfVoxels = BitConverter.ToInt32(chunkData.Take(4).ToArray(), 0);
            bytesProcessed += 4;
            Voxels = new List<Tuple<int, int, int, int>>();
            for (int i = 0; i < numberOfVoxels; i++)
            {
                int x = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1;
                int y = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1;
                int z = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1;
                int colorindex = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1;
                Voxels.Add(new Tuple<int, int, int, int>(x,y,z, colorindex));
            }
        }
    }

    public class RgbaChunk : Chunk
    {
        public List<Tuple<int, int, int, int>> RGBA { get; set; } = new List<Tuple<int, int, int, int>>();
        public RgbaChunk(byte[] chunkData, int size, int sizeChildren)
        {
            Id = "RGBA";
            ChunkSize = size;
            ChunkChildrenSize = sizeChildren;
            var bytesProcessed = 0;
            RGBA.Add(new Tuple<int, int, int, int>(0, 0, 0, 0));
            for (int i = 1; i < 256; i++)
            {
                int R = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1; ;
                int G = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1; ; ;
                int B = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1; ;
                int A = chunkData.Skip(bytesProcessed).First();
                bytesProcessed += 1; ;
                RGBA.Add(new Tuple<int, int, int, int>(R,G,B,A));
            }
            
        }
    }

    public class MattChunk : Chunk
    {
        public int matId { get; set; }
        public int type { get; set; }
        public float weight { get; set; }
        public int prop { get; set; }
        public List<float> normalizedProp{ get; set; }
        public MattChunk(byte[] chunkData, int size, int sizeChildren)
        {
            Id = "MATT";
            ChunkSize = size;
            ChunkChildrenSize = sizeChildren;
            var bytesProcessed = 0;
            matId = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            type = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            weight = BitConverter.ToSingle(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            prop = BitConverter.ToInt32(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
            bytesProcessed += 4;
            var leftBytes = chunkData.Length - bytesProcessed;
            normalizedProp = new List<float>();
            for (int i = 0; i < leftBytes; i++)
            {
                var npSingle = BitConverter.ToSingle(chunkData.Skip(bytesProcessed).Take(4).ToArray(), 0);
                bytesProcessed += 4;
                normalizedProp.Add(npSingle);
            }
        }
    }
}