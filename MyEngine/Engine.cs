using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MyEngine.Assets.Models;
using MyEngine.Logging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MyEngine

{
    internal class Engine : GameWindow
    {
        private readonly ModelManager modelManager = new ModelManager();
        private bool _firstMouse = true;

        private MousePositionState _lastPositionState;

        public Camera Camera;
        private CrossHair CrossHair;
        private double lasttime;
        internal ShaderManager shaderManager = new ShaderManager();
        private bool IsWireframe;
        private DateTime nextWireframeSwitch = DateTime.Now;


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
            Title += GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;
            _lastPositionState = new MousePositionState(Width / 2f, Height / 2f);
            CursorVisible = false;
            if (camera == null)
                Camera = new Camera(Width, Height);
            else
                Camera = camera;

            Load += modelManager.Update;
            Load += shaderManager.Update;
            UpdateFrame += modelManager.Update;
            UpdateFrame += shaderManager.Update;
            EngineLogger = new Logger();
            //EngineLogger.Start();
        }

        public static Logger EngineLogger { get; set; }

        public void AddShader(string vsShaderPath, string fsShaderPath = "")
        {
            shaderManager.AddShader(vsShaderPath, fsShaderPath);
        }

        public void AddModel(Model model)
        {
            modelManager.AddModel(model);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            EngineLogger?.Stop();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);

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
            shader.SetUniformVector3("lightPos", Camera.Position);
            shader.SetUniformVector3("lightColor", new Vector3(1, 1, 1));
            shader.SetUniformVector3("viewpos", Camera.Position);
            shader.SetUniformFloat("ambientStrength", 1);
            shader.SetUniformFloat("diffuseStrength", 1f);
            shader.SetUniformFloat("specularStrength", 1f);
            modelManager.DrawModels(shader);
            CrossHair?.Draw();
            SwapBuffers();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            HandleKeyboard((float) e.Time);
            lasttime = e.Time;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var ray = new VisualRay(Camera);
            AddModel(ray);
            CheckHit();
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
            base.OnMouseWheel(e);
            Camera.ProcessMouseScroll(e.Y);
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
            if ( nextWireframeSwitch < now)
            {
                IsWireframe = !IsWireframe;
                if (IsWireframe)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
                else
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
                }
                nextWireframeSwitch = nextWireframeSwitch.AddSeconds((now - nextWireframeSwitch).Seconds +0.5);
            }
        }

        public void enableCrossHair(Vector4 color)
        {
            CrossHair = new CrossHair(this, color);
        }

        public void LoadModelFromFile(string modelPath, string name = "")
        {
            modelManager.LoadModelFromFile(modelPath, name);
        }

        public bool CheckHit()
        {
            var BoundingBoxes = modelManager.GetModels().Where(x => x is PositionColorModel && !(x is VisualRay))
                .Select(x =>
                {
                    var bb = new BoundingBox((PositionColorModel) x);
                    modelManager.AddModel(bb);
                    return bb;
                });
            return true;
        }

        public List<Model> GetModel(string name)
        {
            return modelManager.GetModel(name);
        }
    }

    internal struct MousePositionState
    {
        public float X;
        public float Y;

        public MousePositionState(float value)
        {
            X = Y = value;
        }

        public MousePositionState(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    internal enum CameraMovement
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
}