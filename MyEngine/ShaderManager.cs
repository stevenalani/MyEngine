using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyEngine.ShaderImporter;

namespace MyEngine
{
    internal class ShaderManager
    {
        private readonly Dictionary<string, ShaderProgram> _shaders = new Dictionary<string, ShaderProgram>();
        private readonly Dictionary<string, List<int>> shaderModelMapping = new Dictionary<string, List<int>>();
        private bool _hasShaderUpdates;

        private void InitPrograms()
        {
            if (!_hasShaderUpdates)
                return;
            var shaderProgs = _shaders.Values.Where(X => !X.IsCompiled);
            foreach (var shaderProgram in shaderProgs) shaderProgram.SetupShader();
            _hasShaderUpdates = false;
        }
        public void AddShader(string vsShaderPath, string fsShaderPath = "")
        {
            var shader = ShaderImporter.ShaderImporter.LoadShader(vsShaderPath, fsShaderPath);
            _shaders.Add(Path.GetFileNameWithoutExtension(vsShaderPath), shader);
            _hasShaderUpdates = true;
        }
        public void AssignModelToShader(int model_id, string vertexshaderpath)
        {
            if (vertexshaderpath == "")
                return;

            var filename = Path.GetFileNameWithoutExtension(vertexshaderpath);
            if (shaderModelMapping.ContainsKey(filename)) shaderModelMapping[filename].Add(model_id);
        }

        public ShaderProgram GetFirst()
        {
            return _shaders.Values.First();
        }

        public void Update(object sender, EventArgs e)
        {
            InitPrograms();
        }

    }
}