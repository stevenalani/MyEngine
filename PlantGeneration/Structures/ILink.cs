using System.Collections.Generic;
using OpenTK;

namespace PlantGeneration.Structures
{
    public interface ILink
    {
        List<IBone> ChildBones { get; set; }
        IBone ParentBone { get; set; }
        Vector3 Position { get; }
        void AddBone(IBone bone);
    }
}