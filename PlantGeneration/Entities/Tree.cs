using System;
using System.Collections.Generic;
using MyEngine;
using MyEngine.Models.Voxel;
using OpenTK;
using PlantGeneration.Structures;

namespace PlantGeneration
{
    public class Tree : ColorVolume
    {
        private int age = 0;
        private TreeSkeleton skeleton;
        private Vector3 RootPosition;
        private Vector2 LeafsLeftControlPoint(Vector2 startPoint, Vector2 tipPoint, float diameter) => startPoint + (tipPoint - startPoint) / 3 - Vector2.UnitX * diameter;
        private Vector2 LeafsRightControlPoint(Vector2 startPoint, Vector2 tipPoint, float diameter) => startPoint + (tipPoint - startPoint) / 3 + Vector2.UnitX * diameter;
        public Tree(int DimensionsX, int DimensionsY, int DimensionsZ) : base(DimensionsX, DimensionsY, DimensionsZ, 0.01f)
        {
            skeleton = new TreeSkeleton();
            RootPosition = new Vector3(DimensionsX / 2, 0, DimensionsZ / 2);
            AddMaterial(new Material() { Color = new Vector4(139, 69, 19, 255) });
            AddMaterial(new Material() { Color = new Vector4(154, 205, 50, 255) });
            AddMaterial(new Material() { Color = new Vector4(58, 95, 11, 255) });
            SetVoxel(RootPosition, 1);
            SetVoxel(RootPosition + Vector3.UnitY, 1);

        }

        public void AddLeafVoxels(Leaf leaf)
        {
            var startPoint = new Vector2(leaf.Position.X, leaf.Position.Y + leaf.StemLength);
            var tipPoint = new Vector2(startPoint.X,startPoint.Y + leaf.LeafLength);
            BezierCurve leftSide = new BezierCurve(new[] { leaf.Position.Xy, LeafsLeftControlPoint(startPoint,tipPoint,leaf.LeafDiameter), tipPoint });
            BezierCurve rightSide = new BezierCurve(new[] { startPoint, LeafsRightControlPoint(startPoint, tipPoint, leaf.LeafDiameter), tipPoint });
            for (int i = (int)leaf.Position.Y; i < leaf.StemLength; i++)
            {
                base.SetVoxel((int)leaf.Position.Z, i, 1, 2);
            }
            for (float y = 0;
                y <= 1;
                y += 0.1f)
            {
                var point1 = leftSide.CalculatePoint(y);
                var point2 = rightSide.CalculatePoint(y);
                for (int i = (int)point1.X; i <= (int)point2.X; i++)
                {
                    base.SetVoxel(i, (int)point1.Y, 1, 1);
                }
            }
        }

    }

    public interface IGrowable
    {
        void Grow();
    }

    public class Leaf : ILink
    {
        public int StemLength = 1;
        public float LeafLength = 3;
        public float LeafDiameter = 1.5f;

        public List<IBone> ChildBones { get; set; }
        public IBone ParentBone { get; set; }
        public Vector3 Position { get; }
        public void AddBone(IBone bone)
        {
            this.ChildBones.Add(bone);
        }
    }

    public class Branch : IBone
    {
        public Branch()
        {
            this.ChildLink = new Leaf();
            ChildLink.ParentBone = this;
        }

        public ILink ParentLink { get; set; }
        public ILink ChildLink { get; set; }
        public double Length { get; set; }
        public Vector3 Direction { get; set; }
        public string Name { get; set; }
        public void AddBone(IBone bone)
        {
            ChildLink.AddBone(bone);
        }
    }
    public class TreeSkeleton : Skeleton, IGrowable
    {
        public override IBone Root => Trunk;
        public Branch Trunk { get; set; } = new Branch() { Name = "Stamm", Length = 2, Direction = Vector3.UnitY };
        public TreeSkeleton()
        {
            base.SetRootLink();
            
        }

        public void Grow()
        {
            Random rand = new Random();
            var Bones = bonesRecursive(Root);
            foreach (var bone in Bones)
            {
                bone.Length += 0.1;
                if ((int)(bone.Length % 5) == 0)
                {
                    var yaw = rand.Next(0, 150);
                    var pitch = rand.Next(0, 150);
                    var roll = rand.Next(0, 150);

                    bone.ChildLink.AddBone(new Bone() { Name = "Ast", Length = 0.1f, Direction = Vector3.TransformPosition(bone.Direction, MathHelpers.GetRotation(yaw, pitch, roll)) });
                }
            }
        }
    }
}
