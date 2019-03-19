using BulletSharp.Math;
using OpenTK;
using Quaternion = OpenTK.Quaternion;
using Vector3 = OpenTK.Vector3;

namespace MyEngine
{
    public static class MathHelpers
    {
        public static Matrix4 getRotation(float yaw, float pitch,float roll)
        {
            Quaternion qPitch = Quaternion.FromAxisAngle(Vector3.UnitX, pitch);
            Quaternion qYaw = Quaternion.FromAxisAngle(Vector3.UnitY, yaw);
            Quaternion qRoll = Quaternion.FromAxisAngle(Vector3.UnitZ, roll);

            //For a FPS Camera we can omit roll
            Quaternion orientation = qPitch * qYaw * qRoll;
            orientation = Quaternion.Normalize(orientation);
            Matrix4 rotate = Matrix4.CreateFromQuaternion(orientation);
            return rotate;
        }

        public static Matrix Matrix4toMatrix(Matrix4 matrix)
        {
            Matrix result = Matrix.Zero;
            result.M11 = matrix.M11;
            result.M12 = matrix.M12;
            result.M13 = matrix.M13;
            result.M14 = matrix.M14;
            result.M21 = matrix.M21;
            result.M22 = matrix.M22;
            result.M23 = matrix.M23;
            result.M24 = matrix.M24;
            result.M31 = matrix.M31;
            result.M32 = matrix.M32;
            result.M33 = matrix.M33;
            result.M34 = matrix.M34;
            result.M41 = matrix.M41;
            result.M42 = matrix.M42;
            result.M43 = matrix.M43;
            result.M44 = matrix.M44;

            return result;
        }
        public static Matrix4 MatrixtoMatrix4(Matrix matrix)
        {
            Matrix4 result = Matrix4.Zero;
            result.M11 = matrix.M11;
            result.M12 = matrix.M12;
            result.M13 = matrix.M13;
            result.M14 = matrix.M14;
            result.M21 = matrix.M21;
            result.M22 = matrix.M22;
            result.M23 = matrix.M23;
            result.M24 = matrix.M24;
            result.M31 = matrix.M31;
            result.M32 = matrix.M32;
            result.M33 = matrix.M33;
            result.M34 = matrix.M34;
            result.M41 = matrix.M41;
            result.M42 = matrix.M42;
            result.M43 = matrix.M43;
            result.M44 = matrix.M44;

            return result;
        }
    }
    
}
