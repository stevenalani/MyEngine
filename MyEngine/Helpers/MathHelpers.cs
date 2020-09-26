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
    }
    
}
