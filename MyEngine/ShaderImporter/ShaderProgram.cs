using System;
using System.IO;
using GlmNet;
using MyEngine.Logging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace MyEngine.ShaderImporter
{
    public class ShaderProgram
    {
        public int ID = -1;
        private string vsPath;
        private string fsPath;
        public bool HasErrors { get; set; }
        public bool IsCompiled { get; set; }
        public ShaderProgram(string vertexPath, string fragmentPath)
        {
            this.vsPath = vertexPath;
            this.fsPath = fragmentPath;
        }

        public void SetupShader()
        {
            ID = GL.CreateProgram();
            var vertexShader = CompileVertexShader(vsPath);
            var fragmentShader = CompileFragmentShader(fsPath);
            LinkShadersToProgram(vertexShader, fragmentShader);
        }
        private int CompileVertexShader(string vertexPath)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            var shaderString = File.ReadAllText(vertexPath);
            GL.ShaderSource(vertexShader, shaderString);
            GL.CompileShader(vertexShader);
            int vsSuccess;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out vsSuccess);
            Console.WriteLine("Vertex Shader compiled: " + (vsSuccess == 1 ? "YES" : "NO"));
            if (vsSuccess == 0)
            {
                string errorLog;
                GL.GetShaderInfoLog(vertexShader, out errorLog);
                Console.WriteLine("Error:\n" + errorLog);
                HasErrors = true;
            }
            return vertexShader;
        }
        private int CompileFragmentShader(string fragmentPath)
        {
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            var shaderString = File.ReadAllText(fragmentPath);
            GL.ShaderSource(fragmentShader, shaderString);
            GL.CompileShader(fragmentShader);
            int vsSuccess;
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out vsSuccess);
            Console.WriteLine("Fragment Shader compiled: " + (vsSuccess == 1 ? "YES" : "NO"));
            if (vsSuccess == 0)
            {
                string errorLog;
                GL.GetShaderInfoLog(fragmentShader, out errorLog);
                Console.WriteLine("Error:\n" + errorLog);
                HasErrors = true;
            }

            return fragmentShader;
        }
        private void LinkShadersToProgram(int vertexShader, int fragmentShader)
        {
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);
            int linkSucceed;
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out linkSucceed);
#if DEBUG
            string vertexsource = String.Empty;
            string fragmentsource = String.Empty;

            GL.ShaderSource(vertexShader, vertexsource);
            GL.ShaderSource(fragmentShader, fragmentsource);
            DebugHelpers.Log("Vertex Source:", vertexsource);
            DebugHelpers.Log("Fragment Source:", fragmentsource);
#endif
            if (linkSucceed == 1)
            {
                HasErrors = false;
                IsCompiled = true;
            }
            else
            {
                string errorLog = String.Empty;
                errorLog = GL.GetProgramInfoLog(ID);
                DebugHelpers.Log("Linking Error:", errorLog);
                HasErrors = true;
            }
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void SetUniformMatrix4X4(string name,Matrix4 matrix)
        {
            if(!IsCompiled)return;
            
            var location = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix4(location,false,ref matrix);
        }

        public void SetUniformFloat(string name, float _float)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(this.ID, name);
            GL.Uniform1(location, _float);
        }
        public void SetUniformVector2(string name, Vector2 vec2)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(this.ID, name);
            GL.Uniform2(location, vec2);

        }
        public void SetUniformVector3(string name, Vector3 vec3)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(this.ID, name);
            GL.Uniform3(location, vec3);
        }

        public void SetUniformVector4(string name, Vector4 vec4)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(this.ID, name);
            GL.Uniform4(location,vec4);
        }

        public void use()
        {
            if(!IsCompiled) return;
            GL.UseProgram(this.ID);
        }

        public static void unuse()
        {
            GL.UseProgram(0);
        }

        
    }
}