using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine
{
    internal partial class Camera
    {
        public const float YAW = 0f;
        public const float PITCH = 0f;

        public float Yaw = YAW;
        public float Pitch = PITCH;
        public float Sensitivity = SENSITIVITY;

        private Vector3 Target => Position + ViewDirection;

        private Matrix4 GetRotationMatrix()
        {
            Quaternion qYaw = Quaternion.FromAxisAngle(Vector3.UnitY, (float)(Yaw * (Math.PI / 180)));
            Quaternion qPitch = Quaternion.FromAxisAngle(Vector3.UnitX, (float) (Pitch * (Math.PI/180)));
            Quaternion orientation = qPitch * qYaw;
            orientation = Quaternion.Normalize(orientation);
            Matrix4 rotate = Matrix4.CreateFromQuaternion(orientation);
            Yaw = Pitch = 0;
            return rotate;
        }
    }
}
