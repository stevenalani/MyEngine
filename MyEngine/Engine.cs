using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MyEngine.Assets.Models;
using MyEngine.Logging;
using MyEngine.Models.Voxel;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MyEngine

{
    public partial class Engine : GameWindow
    {
        private readonly ModelManager modelManager;

        private bool _firstMouse = true;

        private MousePositionState _lastPositionState;


        private CrossHair CrossHair;
        private bool IsWireframe;
        private double lasttime;
        private DateTime nextWireframeSwitch = DateTime.Now;
        internal Physics physics;
        internal ShaderManager shaderManager = new ShaderManager();


        public Engine(int width, int height, Camera camera = null) : base(
            width,
            height,
            GraphicsMode.Default,
            "OpenGl Version:",
            GameWindowFlags.Default,
            DisplayDevice.GetDisplay(DisplayIndex.Default),
            4,
            0,
            GraphicsContextFlags.Default)
        {
            modelManager = new ModelManager(this);
            Title += GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;
            _lastPositionState = new MousePositionState(Width / 2f, Height / 2f);
            CursorVisible = false;
            if (camera == null)
                Camera = new Camera(Width, Height);
            else
                Camera = camera;


            Load += OnUpdate;
            Update += modelManager.Update;
            Update += shaderManager.Update;
            UpdateFrame += OnUpdate;
            EngineLogger = new Logger();
            EngineLogger.Start();
        }


        private void OnUpdate(object sender, EventArgs e)
        {
            Update?.Invoke();

        }

        private void OnUpdate(object sender, FrameEventArgs e)
        {
            Update?.Invoke();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Cw);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.2f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var projection = Camera.GetProjection();
            var shader = shaderManager.GetFirst();

            shader.use();
            shader.SetUniformMatrix4X4("projection", projection);
            var view = Camera.GetView();
            shader.SetUniformMatrix4X4("view", view);
            shader.SetUniformVector3("lightPos", new Vector3(0, 100, 0));
            shader.SetUniformVector3("lightColor", new Vector3(1, 1, 1));
            shader.SetUniformVector3("viewpos", Camera.Position);
            shader.SetUniformFloat("ambientStrength", 1);
            shader.SetUniformFloat("diffuseStrength", 2f);
            shader.SetUniformFloat("specularStrength", 1f);
            var world = modelManager.World;
            if (world != null)
            {
                shader.SetUniformMatrix4X4("model", world.Modelmatrix);
                world.Draw(shader);
            }

            //modelManager.DrawModels(shader);
            var models = modelManager.GetModels();
            foreach (var model in models)
            {
                if (physics != null)
                {
                    physics?.UpdateModelPhysicsModelmatrix(model);
                    model.IsReady = false;
                }
                shader.SetUniformMatrix4X4("model",model.Modelmatrix);
                model.Draw(shader);
            }

            var dictEngineModels = modelManager.GetEngineModels();
            
            foreach (var listEnginemodel in dictEngineModels)
            {
                foreach (var engineModel in listEnginemodel)
                {
                    
                    shader.SetUniformMatrix4X4("model",engineModel.Modelmatrix);
                    ((Model) engineModel).Draw(shader);
                }
            }
            
            CrossHair?.Draw();
            SwapBuffers();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            HandleKeyboard((float) e.Time);
            physics?.Update((float)e.Time);
            lasttime = e.Time;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var visualray = new VisualRay(Camera.Position, Camera.ViewDirection);
            visualray.series = "showray";
            visualray.purgesiblings = true;
            AddModel(visualray);

            var results = CheckHit();
            var volumes = results.Where(x => x.model is Volume);
            
            var colorval = 240;
            foreach (var volumehit in volumes)
            {
                var volume = (Volume) volumehit.model;
                var hitinobjectspace =
                    Vector3.TransformPosition(volumehit.HitPositionWorld, volume.Modelmatrix.Inverted()) +
                    volume.size / 2;
                var directionModelSpace =
                    Vector3.Normalize(Vector3.TransformVector(volumehit.RayDirectionWorld,
                        volume.Modelmatrix.Inverted()));

                var IsVoxelSet = false;
                for (double i = 0; i < Camera.far * 2; i += 0.1)
                {
                    var ray = hitinobjectspace + directionModelSpace * (float) i;
                    if (volume.IsValidVoxelPosition((int) ray.X, (int) ray.Y, (int) ray.Z))
                    {
                        volume.SetVoxel((int) ray.X, (int) ray.Y,
                            (int) ray.Z, new Vector4(colorval, colorval, colorval, 255f));
                        IsVoxelSet = true;
                        volume.IsReady = false;
                        break;
                    }
                }

                if (IsVoxelSet) volume.IsReady = false;
            }
        }


        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (_firstMouse)
            {
                _lastPositionState.X = Mouse.GetState().X;
                _lastPositionState.Y = Mouse.GetState().Y;
                _firstMouse = false;
            }

            var xoffset = Mouse.GetState().X - _lastPositionState.X;
            var yoffset = _lastPositionState.Y - Mouse.GetState().Y;

            Camera.ProcessMouseMovement(xoffset, yoffset);
            _lastPositionState.X = Mouse.GetState().X;
            _lastPositionState.Y = Mouse.GetState().Y;
            Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Camera.ProcessMouseScroll(e.Y);
            base.OnMouseWheel(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            physics?.ExitPhysics();
            base.OnUnload(e);
        }

        private void HandleKeyboard(float deltaTime)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.Escape))
                Exit();
            if (keyState.IsKeyDown(Key.W))
                Camera.ProcessKeyboard(CameraMovement.FORWARD, deltaTime);
            if (keyState.IsKeyDown(Key.A))
                Camera.ProcessKeyboard(CameraMovement.LEFT, deltaTime);
            if (keyState.IsKeyDown(Key.S))
                Camera.ProcessKeyboard(CameraMovement.BACKWARD, deltaTime);
            if (keyState.IsKeyDown(Key.D))
                Camera.ProcessKeyboard(CameraMovement.RIGHT, deltaTime);
            if (keyState.IsKeyDown(Key.Space))
                Camera.ProcessKeyboard(CameraMovement.UP, deltaTime);
            if (keyState.IsKeyDown(Key.ControlLeft))
                Camera.ProcessKeyboard(CameraMovement.DOWN, deltaTime);
            if (keyState.IsKeyDown(Key.G))
                switchWireframe(deltaTime);
        }

        private void switchWireframe(float deltatime)
        {
            var now = DateTime.Now;
            if (nextWireframeSwitch < now)
            {
                IsWireframe = !IsWireframe;
                if (IsWireframe)
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                else
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                nextWireframeSwitch = nextWireframeSwitch.AddSeconds(0.5);
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            EngineLogger?.Stop();
            base.OnClosing(e);
        }

        public List<Model> GetModel(string name)
        {
            return modelManager.GetModel(name);
        }
    }
}