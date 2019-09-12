using OpenTK;

namespace MyEngine
{
    public struct RayHitResult
    {
        public Model model;
        public Vector3 HitPositionWorld;
        public Vector3 RayDirectionWorld;
    }
}