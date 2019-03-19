using System;
using System.Collections.Generic;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using MyEngine.Models.Voxel;
using OpenTK;
using Vector3 = BulletSharp.Math.Vector3;

namespace MyEngine
{
    internal static class Physics
    {

        public static DiscreteDynamicsWorld World { get; set; }
        static CollisionDispatcher dispatcher;
        static DbvtBroadphase broadphase;
        public static List<CollisionShape> CollisionShapes { get; set; } = new List<CollisionShape>();
        static CollisionConfiguration collisionConf;



        static Physics()
        {
            collisionConf = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConf);

            broadphase = new DbvtBroadphase();
            World = new DiscreteDynamicsWorld(dispatcher, broadphase, null, collisionConf);
            World.Gravity = new Vector3(0, -1, 0);
            
        }


        public static void Update(float elapsedTime)
        {
            World.StepSimulation(elapsedTime);
        }

        public static void ExitPhysics()
        {
            //remove/dispose constraints
            int i;
            for (i = World.NumConstraints - 1; i >= 0; i--)
            {
                TypedConstraint constraint = World.GetConstraint(i);
                World.RemoveConstraint(constraint);
                constraint.Dispose();
            }

            //remove the rigidbodies from the dynamics world and delete them
            for (i = World.NumCollisionObjects - 1; i >= 0; i--)
            {
                CollisionObject obj = World.CollisionObjectArray[i];
                RigidBody body = obj as RigidBody;
                if (body != null && body.MotionState != null)
                {
                    body.MotionState.Dispose();
                }
                World.RemoveCollisionObject(obj);
                obj.Dispose();
            }

            //delete collision shapes
            foreach (CollisionShape shape in CollisionShapes)
                shape.Dispose();
            CollisionShapes.Clear();

            World.Dispose();
            broadphase.Dispose();
            if (dispatcher != null)
            {
                dispatcher.Dispose();
            }
            collisionConf.Dispose();
        }

        public static Matrix4 GetPhysicsModelmatrix(Model model)
        {
             var Rigidbody =  World.CollisionObjectArray.First(x => x.UserObject == model.name);
                 
                var matrix = Rigidbody.WorldTransform;
            return MathHelpers.MatrixtoMatrix4(matrix);
        }
        public static void AddRigidBody(Model model)
        {
            if (World.CollisionObjectArray.Count > 0)
            {
                var Rigidbody = World.CollisionObjectArray.First(x => x.UserObject == model.name);
                World.CollisionObjectArray.Remove(Rigidbody);
            }
           
            World.AddRigidBody(model.GetRigitBody());
        }

    }
}