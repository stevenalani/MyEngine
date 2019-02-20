using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MyEngine.ShaderImporter
{
    internal class ShaderImporter
    {
        public static ShaderProgram LoadShader(string vertexshader, string fragmentshader)
        {
            return new ShaderProgram(vertexshader, fragmentshader);
        }

        private static Dictionary<string, string> getInParametersFromShaderFile(string shader)
        {
            var inVars = new Dictionary<string, string>();
            using (var reader = new StreamReader(shader))
            {
                string line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var rregex = Regex.Match(line, "(?<=in )((\\w+) (\\w+))");
                    if (rregex.Success)
                    {
                        var inVar = rregex.Value.Trim().Split(' ');
                        inVars.Add(inVar[0], inVar[1]);
                    }
                }

                return inVars;
            }
        }





    }
}