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
                voxel.ComputeVertices();
                voxel.ComputeIndices();
                PositionColorModel model = new PositionColorModel(voxel.Vertices,voxel.Indices);
                importResult.Add(model);
            }
            else
            {
                using (var importer = new AssimpContext())
                {
                    var scene = importer.ImportFile(file);
                    foreach (var sceneMesh in scene.Meshes)
                    {
                        /*var vertices = sceneMesh.Vertices.Select(x => new PositionColorNormalVertex(x.X, x.Y, x.Z))
                            .Orgin();
                        importResult.Add(new DefaultModel(sceneMesh.Vertices.Orgin(),sceneMesh.Indices,sceneMesh.Normals));*/
                    }
                }
            }
            return importResult;
        }
    }
}
