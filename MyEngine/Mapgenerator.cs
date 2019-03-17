using System;
using System.Collections.Generic;
using System.Linq;
using MyEngine.Models.Voxel;
using OpenTK;

namespace MyEngine
{
    internal class Mapgenerator
    {
        private const float Water = 0.2f;
        private const float Sand = 0.4f;
        private const float Gras = 0.6f;
        private const float Rock = 0.8f;
        private const float Snow = 0.9f;

        private Volume vol;

        private int Y(int x, double slope, int y0)
        {
            return (int) Math.Round(x * slope + y0);
        }

        private Vector4 color(float y, int deltaheight)
        {
            var heightcolorscale = 1.0;
            while (heightcolorscale * deltaheight * 0.1f < 1) heightcolorscale *= 1.1f;
            Vector4 color;
            if (y <= deltaheight * Water * heightcolorscale)
                color = new Vector4(0, 51, 200, 127);
            else if (y > deltaheight * Water * heightcolorscale && y <= deltaheight * Sand * heightcolorscale)
                color = new Vector4(167, 126, 79, 255);
            else if (y > deltaheight * Sand * heightcolorscale && y <= deltaheight * Gras * heightcolorscale)
                color = new Vector4(0, 136, 0, 255);
            else if (y > deltaheight * Gras * heightcolorscale && y <= deltaheight * Rock * heightcolorscale)
                color = new Vector4(0, 136, 0, 255);
            else if (y > deltaheight * Rock * heightcolorscale && y <= deltaheight * Snow * heightcolorscale)
                color = new Vector4(190, 190, 190, 255);
            else
                color = new Vector4(200, 200, 200, 255);
            return color;
        }

        public VoxelMap GenerateMapFromHeightData(List<HeightmapImporter.LocationResult> heights,
            Vector2 itemsperaxis, int offset = -1)
        {
            offset = offset == -1 ? 3 : offset;
            var deltaheight = heights.Max(location => location.elevation) - heights.Min(location => location.elevation);
            var indexcount = itemsperaxis.X * itemsperaxis.Y;
            var vol = new VoxelMap((int) itemsperaxis.X * offset, deltaheight,
                (int) itemsperaxis.Y * offset);


            for (var z = 0; z < itemsperaxis.Y; z++)
            for (var x = 0; x < itemsperaxis.X; x++)
            {
                if (z == 1 && x == 1)
                {
                    var iks = 0;
                }
                var currentindex = (int) ( x+ z * itemsperaxis.X);
                var elevationCurrent = heights[currentindex].elevation;
                int elevationRight = -1, elevationFront = -1, elevationRightFront = -1;
                if (currentindex + 1 < indexcount && currentindex + (int) itemsperaxis.Y < indexcount &&
                    currentindex + (int) itemsperaxis.Y + 1 < indexcount)
                {
                    elevationRight = heights[currentindex + 1].elevation;
                    elevationFront = heights[currentindex + (int) itemsperaxis.Y].elevation;
                    elevationRightFront = heights[currentindex + (int) itemsperaxis.Y + 1].elevation;

                    var resultFrontFace = calculateLine(elevationCurrent, elevationRight, offset);
                    var resultBackFace = calculateLine(elevationFront, elevationRightFront, offset);

                    var resultLeftFace = calculateLine(elevationCurrent, elevationFront, offset);
                    var resultRightFace = calculateLine(elevationRight, elevationRightFront, offset);

                    for (var i = 0; i < offset; i++)
                    {
                        var heightFront = resultFrontFace[i];
                        var heightBack = resultBackFace[i];
                        var heightLeft = resultLeftFace[i];
                        var heightRight = resultRightFace[i];
                        var heightsZ = calculateLine(heightFront, heightBack, offset);
                        var heightsX = calculateLine(heightLeft, heightRight, offset);

                        for (var forward = 0; forward < offset; forward++)
                        {
                            for (var y = 0; y < heightsZ[forward]; y++)
                            {
                                var colorr = color(y, deltaheight);
                                vol.SetVoxel(x * offset + i, y, z * offset + forward, colorr);
                            }

                            for (var y = 0; y < heightsX[forward]; y++)
                            {
                                var colorr = color(y, deltaheight);
                                vol.SetVoxel(x * offset + forward, y, z * offset + i, colorr);
                            }
                        }
                    }

                    if (x == 1)
                    {
                        var iks = 0;
                    }

                    if (x == 2)
                    {
                        var iks = 0;
                    }
                }
            }
            vol.SetVoxel(0,0,0,new Vector4(255));
            vol.SetVoxel((int) (vol.size.X-1), (int)(vol.size.Y - 1), 0,new Vector4(255,0,0,1f));
            
            vol.SetVoxel(0, (int)(vol.size.Y - 1), (int)(vol.size.X - 1), new Vector4(0,0, 255, 1f));
            return vol;
        }

        public Volume generateLineFillLower(int y0, int y1, int lengthInVoxels, bool fill = false,
            Vector3 direction = default(Vector3))
        {
            Volume vol = null;
            var slope = (y1 - y0) / (double) lengthInVoxels;
            direction = direction == default(Vector3) ? Vector3.UnitX : Vector3.Normalize(direction);

            for (var i = 0; i < lengthInVoxels; i++)
            {
                var y = Y(i, slope, y0);
                if (fill)
                {
                    for (var yn = y; yn > 0; yn--)
                        if (direction == Vector3.UnitX)
                        {
                            if (vol == null)
                                vol = new Volume(lengthInVoxels, y1, 1);
                            vol.SetVoxel(i, yn, 0, new Vector4(0, 120, 255, 255));
                        }

                        else if (direction == Vector3.UnitY)
                        {
                            if (vol == null)
                                vol = new Volume(y1, lengthInVoxels, 1);
                            vol.SetVoxel(yn, i, 0, new Vector4(0, 120, 255, 255));
                        }
                        else
                        {
                            if (vol == null)
                                vol = new Volume(1, y1, lengthInVoxels);
                            vol.SetVoxel(0, yn, i, new Vector4(0, 120, 255, 255));
                        }
                }
                else
                {
                    if (direction == Vector3.UnitX)
                    {
                        if (vol == null)
                            vol = new Volume(lengthInVoxels, y1, 1);
                        vol.SetVoxel(i, y, 0, new Vector4(0, 120, 255, 255));
                    }

                    else if (direction == Vector3.UnitY)
                    {
                        if (vol == null)
                            vol = new Volume(y1, lengthInVoxels, 1);
                        vol.SetVoxel(y, i, 0, new Vector4(0, 120, 255, 255));
                    }
                    else
                    {
                        if (vol == null)
                            vol = new Volume(1, y1, lengthInVoxels);
                        vol.SetVoxel(0, y, i, new Vector4(0, 120, 255, 255));
                    }
                }
            }

            return vol;
        }

        private int[] calculateLine(int y0, int y1, int points)
        {
            var result = new List<int>();
            var deltaHeight = y1 - y0;
            var slope = deltaHeight / (double) points;
            for (var i = 0; i < points; i++)
            {
                var height = Y(i, slope, y0);
                result.Add(height);
            }

            return result.ToArray();
        }
    }
}