using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEngine.DataStructures;
using OpenTK;

namespace MyEngine.Models
{
    public class Line : PositionColorModel
    {
        private Vector3 _vector3;

        public Line(Vector3 start, Vector3 direction,float length, float thickness, string modelname = "unnnamed") : base(null, null, modelname)
        {
            direction.Normalize();
            var end = start + direction * length;
        }
        public Line(Vector3 start, Vector3 end,float thickness, string modelname = "unnnamed") : base(null, null, modelname)
        {
            var dir = (end - start).Normalized();
            var x = dir.X;
            var z = dir.Z;
            var y = (x * x + z * z) / -dir.Y;
            
            var orthdir = new Vector3(x,y,z).Normalized() * thickness;
            this.Vertices = new PositionColorVertex[4];
            var col = new Vector4(1f,0.5f,0,1);
            var tmppos0 = (start + orthdir);
            var tmppos1 = (start - orthdir);
            var tmppos2 = (end + orthdir);
            var tmppos3 = (end - orthdir);
            this.Vertices = new []
            {
                new PositionColorVertex
                {
                    Position = tmppos0,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos1,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos2,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos3,
                    Color = col,
                }
            };
            Indices = new uint[]
            {
                1,2,0,1,3,2,1,0,2,1,2,3
            };
        }

        public Line(DataStructures.Vectors.Vector3 start, DataStructures.Vectors.Vector3 end, float thickness, string modelname = "unnamed") : base(null,null,modelname)
        {
            
            var dir = (end._depVector3 - start._depVector3).Normalized();
            var x = dir.X;
            var z = dir.Z;
            var y = (x * x + z * z) / -dir.Y;

            var orthdir = new Vector3(x, y, z).Normalized() * thickness;
            this.Vertices = new PositionColorVertex[4];
            var col = new Vector4(1f, 0.5f, 0, 1);
            var tmppos0 = (start._depVector3 + orthdir);
            var tmppos1 = (start._depVector3 - orthdir);
            var tmppos2 = (end._depVector3 + orthdir);
            var tmppos3 = (end._depVector3 - orthdir);
            this.Vertices = new[]
            {
                new PositionColorVertex
                {
                    Position = tmppos0,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos1,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos2,
                    Color = col,
                },
                new PositionColorVertex
                {
                    Position = tmppos3,
                    Color = col,
                }
            };
            Indices = new uint[]
            {
                1,2,0,1,3,2,1,0,2,1,2,3
            };
        }
    }
}
