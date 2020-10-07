using System.Collections.Generic;
using OpenTK;

namespace PlantGeneration.Structures
{
    public class Link : ILink
    {
        public List<IBone> ChildBones { get; set; } = new List<IBone>();

        public IBone ParentBone { get; set; }

        public virtual Vector3 Position => (ParentBone.ParentLink != null? ParentBone.ParentLink.Position:Vector3.Zero) + (ParentBone.Direction * (float)ParentBone.Length);

        public void AddBone(IBone bone)
        {
            bone.ParentLink = this;
            ChildBones.Add(bone);
        }
    }
}
