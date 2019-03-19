using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    class VoxelMap : Volume
    {
        public CollisionObject CollisionObject { get; }
         public CollisionShape CollisionShape { get; set; }

        public VoxelMap(Vector2 itemsperaxis, int offset,int[] data)
        {
            var minh = data.Min(x => x);
            var maxh = data.Max(x => x);
            var deltaheight = maxh - minh;
            size = new Vector3((int)itemsperaxis.X * offset, deltaheight, (int)itemsperaxis.Y * offset);
            VolumeData = new Voxel[(int)itemsperaxis.X * offset, deltaheight,(int)itemsperaxis.Y * offset];
            InitializeVolumeData();
            
            name = "Ground";
            mass = 0.0f;
            CollisionShape = new HeightfieldTerrainShape((int)itemsperaxis.X* offset, (int) (itemsperaxis.Y * offset), new IntPtr(data[0]), 100f, minh, maxh, 1, PhyScalarType.Int32, false);
            bool isDynamic = (mass != 1.0f);

            BulletSharp.Math.Vector3 localInertia = BulletSharp.Math.Vector3.Zero;
            if (isDynamic)
                CollisionShape.CalculateLocalInertia(mass, out localInertia);

            DefaultMotionState myMotionState = new DefaultMotionState(MathHelpers.Matrix4toMatrix(Modelmatrix));

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, CollisionShape, localInertia);
            CollisionObject = new RigidBody(rbInfo);
            CollisionObject.UserObject = name;
        }

        public VoxelMap(int witdh, int height, int depth) : base(witdh, height, depth)
        {
        }

        public override RigidBody GetRigitBody()
        {
            return (RigidBody)CollisionObject;
        }

    }
}
