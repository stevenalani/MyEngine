using OpenTK;

namespace PlantGeneration.Structures
{
    public class Bone : IBone
    {
        public ILink ParentLink { get; set; }

        public ILink ChildLink { get; set; }

        public double Length { get; set; }

        public Vector3 Direction { get; set; } = -Vector3.UnitY;
        public Vector3 AutoDirection => Vector3.Normalize(ChildLink.Position - ParentLink.Position);
        public string Name { get; set; }
      
        

        public Bone()
        {
            ChildLink = new Link();
            ChildLink.ParentBone = this;
        }
        public Bone(Bone from, Bone to)
        {
            ChildLink = new Link();
            ChildLink.ParentBone = this;
            if (to.ParentLink == null)
            {
                Direction = -from.ChildLink.Position;
            }
            else {
                Direction = to.ParentLink.Position - from.ChildLink.Position;
            }
            Length = Direction.Length;
            Direction = Vector3.Normalize(Direction);
        }

        public void AddBone(IBone bone)
        {
            ChildLink.AddBone(bone);
        }
        public void SetFromPositions(Vector3 pos1, Vector3 pos2)
        {
            Direction = pos2 - pos1;
            Length = Direction.Length;
            Direction.Normalize();
        }
    }
}
