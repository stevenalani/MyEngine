using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyEngine.Assets;
using MyEngine.ShaderImporter;
using OpenTK;

namespace MyEngine
{
    internal class ModelManager
    {
        private readonly Dictionary<int,Model> _models = new Dictionary<int, Model>();
        private ConcurrentQueue<Model> UninitializedModels = new ConcurrentQueue<Model>();

        public Model[] GetModels(bool joined = true)
        {
            if(!joined)
                return this._models.Values.ToArray();
            List<Model> res = new List<Model>();
            res.AddRange(UninitializedModels);
            res.AddRange(_models.Values);
            return res.ToArray();
        }

        public void AddModel(Model model)
        {
            UninitializedModels.Enqueue(model);
            HasModelUpdates = true;
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
                    _models.Add(model.ID,model);
                }
            }
            else
            {
                HasModelUpdates = false;
            }
        }

        public bool HasModelUpdates { get; set; } = false;

        public void DrawModels(ShaderProgram shader,int[] modelIDs = null)
        {
            if (modelIDs == null)
                foreach (var model in _models.Values)
                {
                    model.Draw(shader);
                    if (!model.IsInitialized)
                    {
                        model.InitBuffers();
                    } 
                }
                   
            else
            {
                foreach (var modelID in modelIDs)
                {
                    if (_models.ContainsKey(modelID))
                    {
                        _models[modelID].Draw(shader);
                    }
                }
            }
            
        }

        public void LoadModelFromFile(string modelpath,string name)
        {
            if (name == "") name = Path.GetFileNameWithoutExtension(modelpath);
            var models = ModelImporter.LoadFromFile(modelpath);
            int i = 0;
            foreach (var model in models)
            {
                model.name = i > 0 ? name + i : name;
                UninitializedModels.Enqueue(model);
                HasModelUpdates = true;
            }
        }


        public void Update(object sender, EventArgs e)
        {
            if(HasModelUpdates)
                InitModels();
        }


        public List<Model> GetModel(string name)
        {
            var l1 = _models.Values.Where(x => x.name == name).ToList();
            var l2 = UninitializedModels.Where(x => x.name == name).ToList();
            l1.AddRange(l2);
            return l1;
        }
    }
}