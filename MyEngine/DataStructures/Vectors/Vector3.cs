using System;

namespace MyEngine.DataStructures.Vectors
{

    public class Vector2 
    {
        internal OpenTK.Vector2 _depVector2;
        public float X => _depVector2.X;
        public float Y => _depVector2.Y;

        public Vector2()
        {
            _depVector2 = new OpenTK.Vector2();
        }
        public Vector2(OpenTK.Vector2 vector)
        {
            _depVector2 = vector;
        }

        public Vector2(float value)
        {
            _depVector2 = new OpenTK.Vector2(value);
        }

        public Vector2(float x, float y)
        {
            _depVector2 = new OpenTK.Vector2(x, y);
        }
        public static Vector2 UnityX => new Vector2(1, 0);
        public static Vector2 UnityY => new Vector2(0, 1);
        public static Vector2 Zero => new Vector2(0);
    }

    public class Vector3
    {
        internal OpenTK.Vector3 _depVector3;
        public float X => _depVector3.X;
        public float Y => _depVector3.Y;
        public float Z => _depVector3.Z;
        public Vector3()
        {
            _depVector3 = new OpenTK.Vector3();
        }

        public Vector3(float value)
        {
            _depVector3 = new OpenTK.Vector3(value);
        }

        public Vector3(float x, float y)
        {
            _depVector3 = new OpenTK.Vector3(x,y,0);
        }
        public Vector3(float x, float y,float z)
        {
            _depVector3 = new OpenTK.Vector3(x,y,z);
        }

        public static Vector3 UnityX => new Vector3(1f, 0, 0);
        public static Vector3 UnityY => new Vector3(0, 1f, 0);
        public static Vector3 UnityZ => new Vector3(0, 0, 1f);
        public static Vector3 Zero => new Vector3(0);
        public static Vector3 One => new Vector3(1f);
        
    }

    public class Vector4
    {
        internal OpenTK.Vector4 _depVector4;
        public float X => _depVector4.X;
        public float Y => _depVector4.Y;
        public float Z => _depVector4.Z;
        public float W => _depVector4.W;
        public Vector4()
        {
            _depVector4 = new OpenTK.Vector4();
        }

        public Vector4(float value)
        {
            _depVector4 = new OpenTK.Vector4(value);
        }

        public Vector4(float x, float y)
        {
            _depVector4 = new OpenTK.Vector4(x,y,0,0);
        }
        public Vector4(float x, float y, float z)
        {
            _depVector4 = new OpenTK.Vector4(x,y,z,0);
        }
        public Vector4(float x, float y, float z, float w)
        {
            _depVector4 = new OpenTK.Vector4(x,y,z,w);
        }
        public static Vector4 Zero => new Vector4(0);
    }   
}