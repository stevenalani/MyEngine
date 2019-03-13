using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using MyEngine.Assets.Models;

namespace MyEngine.Assets
{
    static class ModelImporter
    {
        public static List<Model> LoadFromFile(string file)
        {
            List<Model> importResult = new List<Model>();
            if (file.ToLower().Contains(".vox"))
            {
                var voxel = VoxelImporter.VoxelImporter.LoadVoxelModelFromVox(file);
                voxel.Init();
                importResult.Add(voxel);
            }
            else
            {
                using (var importer = new AssimpContext())
                {
                    var scene = importer.ImportFile(file);
                    foreach (var sceneMesh in scene.Meshes)
                    {
                        if (!sceneMesh.HasVertices)
                            return null;

                        Vector3D[] vertices = sceneMesh.Vertices.ToArray();
                        int[] indices = sceneMesh.GetIndices();
                        Vector3D[] normals = sceneMesh.Normals.ToArray();
                        
                        for (int i = 0; i < sceneMesh.Vertices.Count; i++)
                        {
                            var cc = sceneMesh.VertexColorChannels;
                            PositionColorNormalVertex positionColorVertex = new PositionColorNormalVertex(vertices[i],new Color4D(0,0.5f,0.7f),normals[i]);
                        }
                    }
                }
            }
            return importResult;
        }
    }
}
