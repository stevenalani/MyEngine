using System;
using MyEngine.Logging;
using OpenTK;

namespace MyEngine
{
    public enum PROJECTIONTYPE
    {
        Perspective,
        Orthogonal
    }

    public partial class Camera
    {
        
        public Vector3 Position = new Vector3(0,0,5);

        public const float SPEED = 10f;
        public const float SENSITIVITY = 0.1f;
        public const float ZOOM = 45.0f;

        private readonly float fov = 45;

        public Vector3 ViewDirection = -Vector3.UnitZ;
        private Vector3 Up = Vector3.UnitY;
        private Vector3 Right => Vector3.Normalize(Vector3.Cross(ViewDirection,Up));
        


        public float Speed = SPEED;
        
        public float Zoom = ZOOM;

        private float aspect;
        private float near;
        internal float far;

        private Matrix4 pProjection;
        private Matrix4 oProjection;
        private PROJECTIONTYPE _projectionType;
        private Vector3 WorldUP = Vector3.UnitY;

        public EventHandler<CameraMovedEventArgs> CameraMoved = (param1, param2) => { };
        public Camera(int width, int height, PROJECTIONTYPE projection = PROJECTIONTYPE.Perspective)
        {
            this.aspect = width/height;
            this.near = 0.1f;
            this.far = 100f;
            this._projectionType = projection;
            Update();
            pProjection = Matrix4.CreatePerspectiveFieldOfView((float) (fov * (Math.PI / 180)), aspect, near, far);
            oProjection = Matrix4.CreateOrthographic(width, height, near, far);
        }
        public Camera(int width, int height, float near, float far, PROJECTIONTYPE projection = PROJECTIONTYPE.Perspective)
        {
            this.aspect = width/height;
            this.near = near;
            this.far = far;
            this._projectionType = projection;
            Update();
            pProjection = Matrix4.CreatePerspectiveFieldOfView((float)(fov * (Math.PI / 180)), aspect, near, far);
            oProjection = Matrix4.CreateOrthographic(width, height, near, far);
        }

        public Matrix4 GetView()
        {
            if (_projectionType == PROJECTIONTYPE.Perspective)
            return Matrix4.LookAt(Position,Target,Up);
            return Matrix4.Identity * Matrix4.CreateTranslation(Position);
        }
        public Matrix4 GetProjection(PROJECTIONTYPE projectiontype)
        {
            if (projectiontype == PROJECTIONTYPE.Perspective)
            {
                return pProjection;
            }

            return oProjection;
        }
        public Matrix4 GetProjection()
        {
            if (_projectionType == PROJECTIONTYPE.Perspective)
            {
                return pProjection;
            }

            return oProjection;
        }

        void Update()
        {
            var rotate = GetRotationMatrix();
            ViewDirection = Vector3.Normalize(Vector3.TransformNormal(ViewDirection, rotate));
            OnCameraMoved(new CameraMovedEventArgs(){Orientation = ViewDirection,Orgin = Position,ViewMatrix = GetView()});
        }
        
        public void ProcessKeyboard(CameraMovement direction, float deltaTime)
        {
            switch (direction)
            {
                case CameraMovement.FORWARD:
                    Position += ViewDirection * deltaTime * Speed;
                    break;
                case CameraMovement.RIGHT:
                    Position += Vector3.Cross(ViewDirection,Up)* deltaTime * Speed;
                    break;
                case CameraMovement.BACKWARD:
                    Position -= ViewDirection * deltaTime * Speed;
                    break;
                case CameraMovement.LEFT:
                    Position -= Vector3.Cross(ViewDirection, Up) * deltaTime * Speed;
                    break;
                case CameraMovement.UP:
                    Position += Up * deltaTime * Speed;
                    break;
                case CameraMovement.DOWN:
                    Position -= Up * deltaTime * Speed;
                    break;
            }
            Update();
        }

        public void ProcessMouseMovement(float xoffset, float yoffset)
        {
            
            Yaw -= xoffset * Sensitivity;
            Pitch -= yoffset * Sensitivity * ViewDirection.Z/Math.Abs(ViewDirection.Z);
           
            Update();
        }

        public void ProcessMouseScroll(float yoffset)
        {
            if (Zoom >= 1.0f && Zoom <= 45.0f)
                Zoom -= yoffset;
            if (Zoom <= 1.0f)
                Zoom = 1.0f;
            if (Zoom >= 45.0f)
                Zoom = 45.0f;
        }
        protected virtual void OnCameraMoved(CameraMovedEventArgs e)
        {
            EventHandler<CameraMovedEventArgs> handler = CameraMoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }


    public class CameraMovedEventArgs : EventArgs
    {
        public Vector3 Orgin;
        public Vector3 Orientation;
        public Matrix4 ViewMatrix;

    }
}