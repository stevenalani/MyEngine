using System;
using System.Collections.Generic;
using System.Linq;
using BulletSharp;
using OpenTK;

namespace MyEngine.Models.Voxel
{
    public class VoxelMap : Volume
    {
        public VoxelMap(Vector2 itemsperaxis, int offset, int[] data)
        {
            //Position -= this.size / 2;// * new Vector3(1,0,1);

            var minh = data.Min(x => x);
            var maxh = data.Max(x => x);
            var deltaheight = maxh - minh;
            size = new Vector3((int) itemsperaxis.X * offset, deltaheight, (int) itemsperaxis.Y * offset);
            VolumeData = new Voxel[(int) itemsperaxis.X * offset, deltaheight, (int) itemsperaxis.Y * offset];
            InitializeVolumeData();
            name = "Ground";
            mass = 0.0f;
        }

        private new Matrix4 Modelmatrix => Matrix4.Identity;

        public CollisionShape CollisionShape { get; set; }

        public void UpdateColissionObject()
        {
            var datalist = new List<Voxel>();
            var x = 0;
            var z = 0;
            
            //for (x = (int) (size.X -1); x > -1 ; x--)
            //for (z = (int) (size.Z -1); z > -1; z--)
            for ( x = 0; x < size.X; x++)
                //for (z = 0; z < size.Z; z++)
            for (z = (int) (size.Z -1); z > -1; z--)
            for (var y = (int) size.Y - 1; y > -1; y--)
            if (VolumeData[x, y,z].Color != Vector4.Zero || y == 0)
            {
                datalist.Add(VolumeData[x, y,z]);
                break;
            }

            if (datalist.Count == 0) return;

            var data = datalist.ToArray();
            var rsize = Math.Sqrt(data.Length);
            var minh = data.Min(voxel => voxel.Posindices.Y);
            var maxh = data.Max(voxel => voxel.Posindices.Y);
            var floatdata = data.Select(voxel => voxel.Posindices.Y).ToArray();
            var deltaheight = maxh - minh;
            CollisionShape = new HeightfieldTerrainShape(
                (int) size.Z,
                (int) size.X,
                (IntPtr) floatdata[0],
                1, minh, maxh,
                1,
                PhyScalarType.Double,
                false
            );
            
            var isDynamic = mass != 1.0f;

            var localInertia = BulletSharp.Math.Vector3.Zero;
            if (isDynamic)
                CollisionShape.CalculateLocalInertia(mass, out localInertia);

            var myMotionState = new DefaultMotionState();
            var rbInfo = new RigidBodyConstructionInfo(mass, myMotionState, CollisionShape, localInertia);

            collisionObject = new RigidBody(rbInfo);
            collisionObject.UserObject = name;
        }


        public override RigidBody GetRigitBody()
        {
            return (RigidBody) collisionObject;
        }
    }
}