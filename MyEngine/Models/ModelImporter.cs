using Assimp;
using System.Collections.Generic;

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
                //voxel.ComputeVerticesAndIndices();
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
                            //PositionColorNormalVertex positionColorVertex = new PositionColorNormalVertex(){ Position = vertices[i], Color = new Color4D(0, 0.5f, 0.7f), Normal = normals[i])};
                        }
                    }
                }
            }
            return importResult;
        }
    }
}
