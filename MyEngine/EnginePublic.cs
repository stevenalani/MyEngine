using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEngine.Assets.Models;
using MyEngine.Logging;
using MyEngine.Models.Voxel;
using MyEngine.ShaderImporter;
using OpenTK;

namespace MyEngine
{
    public partial class Engine
    {
        public Camera Camera;
        public static Logger EngineLogger { get; set; }
        public event Action Update;

        public void enableCrossHair(MyEngine.DataStructures.Vectors.Vector4 color)
        {
            CrossHair = new CrossHair(this, color._depVector4);
        }

        public void LoadModelFromFile(string modelPath, string name = "")
        {
            var model = modelManager.LoadModelFromFile(modelPath, name);
        }

        public void AddShader(string vsShaderPath, string fsShaderPath = "")
        {
            shaderManager.AddShader(vsShaderPath, fsShaderPath);
        }

        public void AddModel(Model model)
        {
            modelManager.AddModel(model);
        }

        public void SetWorld(VoxelMap world)
        {
            if (this.physics != null)
            {
                 physics.AddRigidBody(world);
            }
            modelManager.World = world;
        }

        public void ClearWorld()
        {
            modelManager.ClearWorld();
        }

        public List<RayHitResult> CheckHit()
        {
            var hitResults = new List<RayHitResult>();
            var BoundingBoxes = modelManager.GetModelsAndWorld().Where(x => x is PositionColorModel)
                .Select(x =>
                {
                    var bb = ((PositionColorModel)x).BoundingBox;
                    return new KeyValuePair<PositionColorModel, BoundingBox>((PositionColorModel)x, bb);
                });

            foreach (var values in BoundingBoxes)
            {
                var boundingbox = values.Value;
                for (var i = 0.1f; i <= 100f; i += 0.01f)
                {
                    var ray = Camera.Position + Camera.ViewDirection * i;
                    var isinx = ray.X >= boundingbox.leftlownear.X && ray.X <= boundingbox.rightlownear.X;
                    var isiny = ray.Y >= boundingbox.leftlownear.Y && ray.Y <= boundingbox.rightupnear.Y;
                    var isinz = boundingbox.leftlownear.Z >= ray.Z && ray.Z >= boundingbox.rightlowfar.Z;
                    if (isinx && isiny && isinz)
                    {
                        while (isinx && isiny && isinz)
                        {
                            isinx = ray.X >= boundingbox.leftlownear.X && ray.X <= boundingbox.rightlownear.X;
                            isiny = ray.Y >= boundingbox.leftlownear.Y && ray.Y <= boundingbox.rightupnear.Y;
                            isinz = boundingbox.leftlownear.Z >= ray.Z && ray.Z >= boundingbox.rightlowfar.Z;
                            ray -= Camera.ViewDirection * 0.5f;
                        }

                        //var bb2 = new BoundingBox(values.Key, new Vector4(0.8f, 0.2f, 0.2f, 0.1f));
                        //bb2.series = "Checkhit";
                        //bb2.purgesiblings = true;
                        modelManager.AddModel(values.Key.BoundingBox);
                        var hit = new RayHitResult
                        {
                            model = values.Key,
                            HitPositionWorld = ray,
                            RayDirectionWorld = Camera.ViewDirection
                        };
                        hitResults.Add(hit);
                        break;
                    }
                }
            }

            return hitResults;
        }

        public void DrawModel(Model model,ShaderProgram shader)
        {
            if (!model.IsReady){ model.InitBuffers();}
            //var matrix = _engine.physics.UpdateModelPhysicsModelmatrix(model);
            var matrix = MathHelpers.MatrixtoMatrix4(model.collisionObject.WorldTransform);
            shader.SetUniformMatrix4X4("model", matrix);
            //shader.SetUniformMatrix4X4("model",model.Modelmatrix);
            model.Draw(shader);
        }

        public void DrawWorld(Model model, ShaderProgram shader)
        {
            if (!model.IsReady) { model.InitBuffers(); }
            shader.SetUniformMatrix4X4("model", model.Modelmatrix);
            //shader.SetUniformMatrix4X4("model",model.Modelmatrix);
            model.Draw(shader);
        }
    }

}
