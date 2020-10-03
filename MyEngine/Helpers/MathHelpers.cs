using Assimp;
using MyEngine.DataStructures;
using OpenTK;
using Quaternion = OpenTK.Quaternion;
using Vector3 = OpenTK.Vector3;

namespace MyEngine
{
    public static class MathHelpers
    {
        public static Matrix4 GetRotation(float yaw, float pitch, float roll)
        {
            Quaternion qPitch = Quaternion.FromAxisAngle(Vector3.UnitX, pitch);
            Quaternion qYaw = Quaternion.FromAxisAngle(Vector3.UnitY, yaw);
            Quaternion qRoll = Quaternion.FromAxisAngle(Vector3.UnitZ, roll);
            Quaternion orientation = qPitch * qYaw * qRoll;
            orientation = Quaternion.Normalize(orientation);
            Matrix4 rotate = Matrix4.CreateFromQuaternion(orientation);
            return rotate;
        }
        public static Vector3 GetSurfaceNormalNewell(PositionColorVertex[] orderedVertices)
        {
            Vector3 normal = Vector3.Zero;
            for (int i = 0; i < orderedVertices.Length-1; i++)
            {
                Vector3 CurrentVertex = orderedVertices[i].Position;
                Vector3 NextVertex = orderedVertices[(i + 1)].Position;

                normal.X += (CurrentVertex.Y - NextVertex.Y) * (CurrentVertex.Z + NextVertex.Z);
                normal.Y += (CurrentVertex.Z - NextVertex.Z) * (CurrentVertex.X + NextVertex.X);
                normal.X += (CurrentVertex.X - NextVertex.X) * (CurrentVertex.Y + NextVertex.Y);
            }
            return normal.Normalized();
        }

        public static Vector3 GetSurfaceNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 u = p2 - p1;
            Vector3 v = p3 - p1;
            Vector3 normal = Vector3.Cross(u,v);
            
            return normal.Normalized();
        }

        public static Vector3 GetIntersection(Vector3 rayDirection, Vector3 rayPoint,Vector3 planePoint, Vector3 planeNormal)
        {
            
            var difference = rayDirection - planePoint;
            var dot1 = Vector3.Dot(difference, planeNormal);
            var dot2 = Vector3.Dot(rayDirection, planeNormal);
            var length = dot1 / dot2;
            Vector3 intersection = rayPoint - rayDirection * length;
            return intersection;
        }
        public static Vector3 GetIntersection2(Vector3 rayDirection, Vector3 rayPoint, Vector3 planePoint, Vector3 planeNormal)
        {
            rayDirection.Normalize();
            double t = (Vector3.Dot(planeNormal,planePoint) - Vector3.Dot(planeNormal, rayPoint)) / Vector3.Dot(planeNormal,rayDirection);
            return rayPoint + (Vector3.Multiply(rayDirection, (float) t));

            var difference = rayDirection - planePoint;
            var dot1 = Vector3.Dot(difference, planeNormal);
            var dot2 = Vector3.Dot(rayDirection, planeNormal);
            var length = dot1 / dot2;
            Vector3 intersection = rayPoint - rayDirection * length;
            return intersection;
        }
    }

    

}
