﻿using MyEngine.HgtImporter;
using MyEngine.Models.Voxel;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEngine
{
    internal class Mapgenerator
    {
        private const float Water = 0.1f;
        private const float Sand = 0.2f;
        private const float Dirt = 0.3f;
        private const float Gras = 0.6f;
        private const float Rock = 0.8f;
        private const float Snow = 0.9f;

        private Vector4 waterl = new Vector4(0, 151, 255, 127);
        private Vector4 water = new Vector4(177, 159, 144, 255);
        private Vector4 sand = new Vector4(134, 100, 71, 255);
        private Vector4 dirt = new Vector4(0, 136, 0, 255);
        private Vector4 gras = new Vector4(170, 170, 170, 255);
        private Vector4 rock = new Vector4(190, 190, 190, 255);
        private Vector4 snow = new Vector4(200, 200, 200, 255);

        private VoxelMap vol;
        private Random rand = RandomProvider.GetThreadRandom();
        private int Y(int x, double slope, int y0)
        {
            return (int)Math.Round(x * slope + y0);
        }

        private Vector4 color(float y, int deltaheight)
        {
            var heightpitch = rand.NextDouble() * 2 - 1.0;
            var heightcolorscale = 1.0;
            while (heightcolorscale * deltaheight * Water < 1) heightcolorscale *= 1.1f;
            Vector4 color;
            if (y - heightpitch <= deltaheight * Water * heightcolorscale)
                color = waterl;
            else if (y - heightpitch > deltaheight * Water * heightcolorscale && y - heightpitch <= deltaheight * Sand * heightcolorscale)
                color = water;
            else if (y - heightpitch > deltaheight * Sand * heightcolorscale && y - heightpitch <= deltaheight * Dirt * heightcolorscale)
                color = sand;
            else if (y - heightpitch > deltaheight * Dirt * heightcolorscale && y - heightpitch <= deltaheight * Gras * heightcolorscale)
                color = dirt;
            else if (y - heightpitch > deltaheight * Gras * heightcolorscale && y - heightpitch <= deltaheight * Rock * heightcolorscale)
                color = gras;
            else if (y - heightpitch > deltaheight * Rock * heightcolorscale && y - heightpitch <= deltaheight * Snow * heightcolorscale)
                color = rock;
            else
                color = snow;
            return color;
        }
        public HgtMapVolume GenerateMapFromHeightData(short[,] heights)
        {
            var colNRowCnt = (int)(Math.Sqrt(heights.Length)/100);
            List<HeightmapImporter.LocationResult> locationResults = new List<HeightmapImporter.LocationResult>();
            for (int y = 0; y < colNRowCnt; y++)
                for (int x = 0; x < colNRowCnt; x++)
                    locationResults.Add(new HeightmapImporter.LocationResult() { latitude = x, longitude = y, elevation = heights[y, x] });
            int minVal = locationResults.Min(location => location.elevation);
            var deltaheight = locationResults.Max(location => location.elevation) - minVal;
            var volume = new HgtMapVolume(colNRowCnt, locationResults.Max(location => location.elevation) - minVal, colNRowCnt);
            var color = new Vector4(200, 200, 200, 255);
            foreach (var result in locationResults)
            {
                for (int i = 0; i < result.elevation - minVal; i++)
                    volume.SetVoxel((int)result.latitude, i, (int)result.longitude, color);
            }
            return volume;
        }

        public VoxelMap GenerateMapFromHeightData(List<HeightmapImporter.LocationResult> heights,
           Vector2 itemsperaxis, int offset = -1)
        {
            offset = offset == -1 ? 3 : offset;
            var deltaheight = heights.Max(location => location.elevation) - heights.Min(location => location.elevation);
            var indexcount = itemsperaxis.X * itemsperaxis.Y;
            vol = new VoxelMap(itemsperaxis, offset, heights.Select(x => x.elevation).ToArray());
            // //vol = new VoxelMap((int) itemsperaxis.X * offset, deltaheight,
            //    (int) itemsperaxis.Y * offset);


            for (var z = 0; z < itemsperaxis.Y; z++)
                for (var x = 0; x < itemsperaxis.X; x++)
                {
                    var currentindex = (int)(x + z * itemsperaxis.X);
                    var elevationCurrent = heights[currentindex].elevation;
                    int elevationRight = -1, elevationInfront = -1, elevationRightFront = -1;
                    int[] resultFrontFace = null;
                    int[] resultBackFace = null;
                    int[] resultLeftFace = null;
                    int[] resultRightFace = null;

                    if (currentindex + 1 < indexcount)
                    {
                        elevationRight = heights[currentindex + 1].elevation;
                        resultFrontFace = calculateLine(elevationCurrent, elevationRight, offset);
                    }

                    if (currentindex + (int)itemsperaxis.Y < indexcount)
                    {
                        elevationInfront = heights[currentindex + (int)itemsperaxis.Y].elevation;
                        resultLeftFace = calculateLine(elevationCurrent, elevationInfront, offset);
                    }
                    if (currentindex + (int)itemsperaxis.Y + 1 < indexcount)
                    {
                        elevationRightFront = heights[currentindex + (int)itemsperaxis.Y + 1].elevation;
                        resultBackFace = calculateLine(elevationInfront, elevationRightFront, offset);
                        resultRightFace = calculateLine(elevationRight, elevationRightFront, offset);
                    }

                    for (var i = 0; i < offset; i++)
                    {
                        int heightFront = 0, heightBack = 0, heightLeft = 0, heightRight = 0;
                        if (resultFrontFace != null) heightFront = resultFrontFace[i];
                        if (resultBackFace != null) heightBack = resultBackFace[i];
                        if (resultLeftFace != null) heightLeft = resultLeftFace[i];
                        if (resultRightFace != null) heightRight = resultRightFace[i];

                        var heightsAlongZ = calculateLine(heightFront, heightBack, offset);
                        var heightsAlongX = calculateLine(heightLeft, heightRight, offset);

                        for (var forward = 0; forward < offset; forward++)
                        {

                            for (var y = 0; y < heightsAlongZ[forward]; y++)
                            {
                                var colorr = color(y, deltaheight);
                                vol.SetVoxel(x * offset + i, y, z * offset + forward, colorr);
                            }

                            for (var y = 0; y < heightsAlongX[forward]; y++)
                            {
                                var colorr = color(y, deltaheight);
                                vol.SetVoxel(x * offset + forward, y, z * offset + i, colorr);
                            }
                        }
                    }
                }
            return vol;
        }

        public Volume generateLineFillLower(int y0, int y1, int lengthInVoxels, bool fill = false,
            Vector3 direction = default(Vector3))
        {
            Volume vol = null;
            var slope = (y1 - y0) / (double)lengthInVoxels;
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