using MyEngine.Assets;
using MyEngine.Models.Voxel;
using MyEngine.ShaderImporter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyEngine
{
    internal class ModelManager
    {
        private readonly Dictionary<int, Model> _models = new Dictionary<int, Model>();

        private readonly Dictionary<string, List<IEngineModel>> engineModels =
            new Dictionary<string, List<IEngineModel>>();

        private readonly ConcurrentQueue<IEngineModel> UninitializedEngineModels = new ConcurrentQueue<IEngineModel>();
        private readonly ConcurrentQueue<Model> UninitializedModels = new ConcurrentQueue<Model>();
        private bool hasWorld;

        public bool HasModelUpdates { get; set; }

        public List<Model> GetModels(bool joined = true)
        {
            if (!joined)
                return _models.Values.ToList();
            var res = new List<Model>();
            res.AddRange(UninitializedModels);
            res.AddRange(_models.Values);
            return res.ToList();
        }
        public List<Model> GetModelsAndWorld(bool joined = true)
        {
            if (!joined)
            {
                var models = _models.Values.ToList();
                models.Insert(0, World);
                return models;
            }

            var res = new List<Model>();
            res.AddRange(UninitializedModels);
            res.AddRange(_models.Values);
            if (hasWorld)
            {
                res.Add(World);
            }

            return res.ToList();
        }

        public void AddModel(Model model)
        {
            HasModelUpdates = true;
            if (model is IEngineModel)
            {
                UninitializedEngineModels.Enqueue((IEngineModel)model);
                return;
            }

            UninitializedModels.Enqueue(model);
        }

        public void InitModels()
        {
            if (!UninitializedModels.IsEmpty)
            {
                Model model = null;
                while (!UninitializedModels.IsEmpty)
                {
                    UninitializedModels.TryDequeue(out model);
                    model?.InitBuffers();
                    _models.Add(model.ID, model);
                }
            }
            else if (!UninitializedEngineModels.IsEmpty)
            {
                IEngineModel model = null;
                while (!UninitializedEngineModels.IsEmpty)
                {
                    UninitializedEngineModels.TryDequeue(out model);
                    ((Model)model)?.InitBuffers();
                    if (!engineModels.ContainsKey(model.series))
                        engineModels.Add(model.series, new List<IEngineModel>());
                    if (model.purgesiblings)
                        engineModels[model.series] = new List<IEngineModel>();
                    engineModels[model.series].Add(model);
                }
            }
            else
            {
                HasModelUpdates = false;
            }
        }

        public void DrawModels(ShaderProgram shader, int[] modelIDs = null)
        {
            if (this.World != null)
            {
                if (!World.IsInitialized) { World.InitBuffers(); }
                shader.SetUniformMatrix4X4("model", World.Modelmatrix);
                World.Draw(shader);
            }
            if (modelIDs == null)
                foreach (var model in _models.Values)
                {
                    if (!model.IsInitialized) { model.InitBuffers(); }
                    shader.SetUniformMatrix4X4("model", model.Modelmatrix);
                    model.Draw(shader);

                }
            else
                foreach (var modelID in modelIDs)
                    if (_models.ContainsKey(modelID))
                    {
                        if (_models[modelID].IsInitialized)
                            _models[modelID].InitBuffers();
                        shader.SetUniformMatrix4X4("model", _models[modelID].Modelmatrix);
                        _models[modelID].Draw(shader);
                    }

            foreach (var engineModels in engineModels.Values)
                foreach (var model in engineModels)
                {
                    if (!((Model)model).IsInitialized) ((Model)model).InitBuffers();
                    ((Model)model).Draw(shader);
                }
        }

        public List<Model> LoadModelFromFile(string modelpath, string name)
        {
            if (name == "") name = Path.GetFileNameWithoutExtension(modelpath);
            var models = ModelImporter.LoadFromFile(modelpath);
            var i = 0;
            foreach (var model in models)
            {
                model.name = i > 0 ? name + i : name;
                UninitializedModels.Enqueue(model);
                HasModelUpdates = true;
                if (model is Volume)
                {
                    HasModelUpdates = true;
                }
            }

            return models;
        }


        public void Update(object sender, EventArgs eventArgs)
        {
            if (HasModelUpdates)
                InitModels();
        }


        public List<Model> GetModel(string name)
        {
            var l1 = _models.Values.Where(x => x.name == name).ToList();
            var l2 = UninitializedModels.Where(x => x.name == name).ToList();
            l1.AddRange(l2);
            return l1;
        }

        public void Update()
        {
            Update(null, null);
        }

        public void SetWorld(ColorVolume world)
        {
            this.hasWorld = true;
            this.World = world;
        }

        public ColorVolume World { get; set; }

        public void ClearWorld()
        {
            this.hasWorld = false;
            World = null;
        }
    }
}