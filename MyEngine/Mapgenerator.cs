using MyEngine.Models.Voxel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine
{
    public class Mapgenerator
    {
        private const float Water = 0.1f;
        private const float Sand = 0.2f;
        private const float Dirt = 0.3f;
        private const float Gras = 0.6f;
        private const float Rock = 0.8f;
        private const float Snow = 0.9f;

        private Material waterl = new Material(){ Color = new Vector4(0, 151, 255, 127)};
        private Material water = new Material() { Color = new Vector4(177, 159, 144, 255)};
        private Material sand = new Material() { Color = new Vector4(134, 100, 71, 255)};
        private Material dirt = new Material() { Color = new Vector4(0, 136, 0, 255)};
        private Material gras = new Material() { Color = new Vector4(170, 170, 170, 255)};
        private Material rock = new Material() { Color = new Vector4(190, 190, 190, 255)};
        private Material snow = new Material() { Color = new Vector4(200, 200, 200, 255)};

        private Random rand = RandomProvider.GetThreadRandom();
        private int Y(int x, double slope, int y0)
        {
            return (int)Math.Round(x * slope + y0);
        }

        private Material Material(float y, int deltaheight)
        {
            var heightpitch = 1f;//;rand.NextDouble() * 2 - 1.0;
            var heightcolorscale = 1.0;
            while (heightcolorscale * deltaheight * Water < 1) heightcolorscale *= 1.1f;
            Material color;
            if (y - heightpitch <= deltaheight * Water * heightcolorscale)
                color = waterl;
            else if (y - heightpitch > deltaheight * Water * heightcolorscale && y - heightpitch <= deltaheight * Sand * heightcolorscale)
                color = water;
            else if (y - heightpitch > deltaheight * Sand * heightcolorscale && y - heightpitch <= deltaheight * Dirt * heightcolorscale)
                color = sand;
            else if (y - heightpitch > deltaheight * Dirt * heightcolorscale && y - heightpitch <= deltaheight * Gras * heightcolorscale)
                color = dirt;
            else if (y - heightpitch > deltaheight * Gras * heightcolorscale && y - heightpitch <= deltaheight * Rock * heightcolorscale)
                color = dirt;
            else if (y - heightpitch > deltaheight * Rock * heightcolorscale && y - heightpitch <= deltaheight * Snow * heightcolorscale)
                color = rock;
            else
                color = snow;
            return color;
        }
        public BigColorVolume GenerateMapFromHeightData(short[,] heights)
        {
            var colNRowCnt = (int)(Math.Sqrt(heights.Length) / 10);
            int maxVal = heights[0,0];
            int minVal = heights[0,0];
            for (int y = 1; y < colNRowCnt; y++)
            {
                for (int x = 1; x < colNRowCnt; x++)
                {
                    if (maxVal < heights[x, y]) maxVal = heights[x, y];
                    if (minVal > heights[x, y]) minVal = heights[x, y];
                }
            }
            var deltaheight = maxVal - minVal;
            var volume = new BigColorVolume(colNRowCnt, deltaheight, colNRowCnt,1,32);

            for (int y = 0; y < colNRowCnt; y++)
            {
                for (int x = 0; x < colNRowCnt; x++)
                {
                    int[] neighbours = null;
                    if(x == 0 && y == 0)
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x + 1, y + 1],
                            heights[x + 1, y],
                        };
                    }else if (x == 0 && y < colNRowCnt-1)
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x + 1, y + 1],
                            heights[x + 1, y],
                            heights[x, y - 1],
                            heights[x + 1, y - 1],
                        };
                    }
                    else if (x == 0 && y == colNRowCnt - 1)
                    {
                        neighbours = new int[]
                        {
                            heights[x + 1, y],
                            heights[x, y - 1],
                            heights[x + 1, y - 1],
                        };
                    }
                    else if (x < colNRowCnt - 1 && y == 0)
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x - 1, y + 1],
                            heights[x + 1, y + 1],
                            heights[x - 1, y],
                            heights[x + 1, y],
                        };
                    }
                    else if (x < colNRowCnt - 1 && y == colNRowCnt - 1)
                    {
                        neighbours = new int[]
                        {
                            heights[x - 1, y],
                            heights[x + 1, y],
                            heights[x, y - 1],
                            heights[x - 1, y - 1],
                            heights[x + 1, y - 1],
                        };
                    }
                    else if(x == colNRowCnt -1 && y == 0)
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x - 1, y + 1],
                            heights[x - 1, y],
                        };
                    }
                    else if( x == colNRowCnt - 1 && y < colNRowCnt - 1)
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x - 1, y + 1],
                            heights[x - 1, y],
                            heights[x, y - 1],
                            heights[x - 1, y - 1],

                        };
                    }
                    else if(x == colNRowCnt -1 && y == colNRowCnt -1)
                    {
                        neighbours = new int[]
                        {
                            heights[x - 1, y],
                            heights[x, y - 1],
                            heights[x - 1, y - 1],
                        };
                    }
                    else
                    {
                        neighbours = new int[]
                        {
                            heights[x, y + 1],
                            heights[x - 1, y + 1],
                            heights[x + 1, y + 1],
                            heights[x - 1, y],
                            heights[x + 1, y],
                            heights[x, y - 1],
                            heights[x - 1, y - 1],
                            heights[x + 1, y - 1],
                        };
                    }

                    var minNeighbour = neighbours.Min();
                    for (int i = minNeighbour - minVal; i <= heights[x,y] - minVal; i++)
                        volume.SetVoxel(x, i, y, Material(i, deltaheight));
                }
            }

            volume.Position.Y = -minVal;
            return volume;
        }
        private int[] calculateLine(int y0, int y1, int pointCount)
        {
            var result = new List<int>();
            var deltaHeight = y1 - y0;
            var slope = deltaHeight / (double)pointCount;
            for (var i = 0; i < pointCount; i++)
            {
                var height = Y(i, slope, y0);
                result.Add(height);
            }

            return result.ToArray();
        }
    }
}