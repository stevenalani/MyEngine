using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace PlantGeneration.Structures
{
    public abstract class Skeleton : ISkeleton
    {
        
        public abstract IBone Root { get; }

        public virtual IBone[] Bones => bonesRecursive(Root);

        public virtual IBone[] bonesRecursive(IBone bone)
        {
            List<IBone> bones = new List<IBone>() { bone };
            foreach (var _bone in bone.ChildLink.ChildBones)
            {
                bones.AddRange(bonesRecursive(_bone));
            }
            return bones.ToArray();
        }
        public void SetRootLink()
        {
            Root.ParentLink = new RootLink() { ParentBone = Root };
        }
        public virtual Vector3 Dimensions  
        {
            get
            {
                List<IBone> bones = bonesRecursive(Root).ToList();
                var minxVal = bones.Min(x => x.ChildLink.Position.X);
                var minx = bones.First(x => x.ChildLink.Position.X == minxVal);
                var maxxVal = bones.Max(x => x.ChildLink.Position.X);
                var maxx = bones.First(x => x.ChildLink.Position.X == maxxVal);
                var minyVal = bones.Min(x => x.ChildLink.Position.Y);
                var miny = bones.First(x => x.ChildLink.Position.Y == minyVal);
                var maxyVal = bones.Max(x => x.ChildLink.Position.Y);
                var maxy = bones.First(x => x.ChildLink.Position.Y == maxyVal);
                var minzVal = bones.Min(x => x.ChildLink.Position.Z);
                var minz = bones.First(x => x.ChildLink.Position.Z == minzVal);
                var maxzVal = bones.Max(x => x.ChildLink.Position.Z);
                var maxz = bones.First(x => x.ChildLink.Position.Z == maxzVal);

                return new Vector3(maxxVal - minxVal, maxyVal - minyVal, maxzVal - minzVal);
            }
        }

    }
}