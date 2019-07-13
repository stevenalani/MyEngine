using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BulletSharp;
using BulletSharp.Math;
using MyEngine.Models.Voxel;
using OpenTK;
using Vector3 = BulletSharp.Math.Vector3;

namespace MyEngine
{
    internal class Physics
    {

        private DiscreteDynamicsWorld World;
        private CollisionDispatcher dispatcher;
        private DbvtBroadphase broadphase;
        private ConcurrentQueue<CollisionObject> CollisionObjectQueue = new ConcurrentQueue<CollisionObject>();
        private CollisionConfiguration collisionConf;
        private List<Model> Models = new List<Model>();


        public Physics()
        {
            collisionConf = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConf);

            broadphase = new DbvtBroadphase();
            World = new DiscreteDynamicsWorld(dispatcher, broadphase, null, collisionConf);
            World.Gravity = new Vector3(0, -3, 0);
            
        }


        public void Update(float elapsedTime)
        {
            CollisionObject co =null;
            while (!CollisionObjectQueue.IsEmpty)
            {
                CollisionObjectQueue.TryDequeue(out co);
                if(co != null)
                    World.AddRigidBody((RigidBody)co);
            }
            World.StepSimulation(elapsedTime);
        }

        public void ExitPhysics()
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


            World.Dispose();
            broadphase.Dispose();
            if (dispatcher != null)
            {
                dispatcher.Dispose();
            }
            collisionConf.Dispose();
        }

        public void UpdateModelPhysicsModelmatrix(Model model)
        {
            var Rigidbody =  World.CollisionObjectArray.FirstOrDefault(x => x.UserObject == model.name);
            if (Rigidbody == null)
            {
                return;
            }
            var matrix = ((RigidBody)Rigidbody).MotionState.WorldTransform;
            model.PhysicsModelmatrix = MathHelpers.MatrixtoMatrix4(matrix);
        }
        public void AddRigidBody(Model model)
        {
            if (model.CalculatePhysics)
                CollisionObjectQueue.Enqueue(model.GetRigitBody());

        }
        public void RemoveRigidBody(Model model)
        {
            //var Rigidbody = World.CollisionObjectArray.First(x => x.UserObject == model.name);
            World.RemoveRigidBody(model.GetRigitBody());
        }
    }
}