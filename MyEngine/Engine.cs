using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyEngine.Assets;
using MyEngine.Assets.Models;
using MyEngine.Logging;
using MyEngine.ShaderImporter;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MyEngine
{
    internal class Engine : GameWindow
    {
        private ModelManager modelManager = new ModelManager();
        internal ShaderManager shaderManager = new ShaderManager();

        private MousePositionState _lastPositionState;
        
        public Camera Camera { get; set; }
        private bool _firstMouse = true;
        private readonly float lasttime = 0f;
        private CrossHair CrossHair;
        public ShaderProgram Shader;


        public Engine() : base(
            600,
            400,
            GraphicsMode.Default,
            "OpenGl Version:", GameWindowFlags.Default, DisplayDevice.GetDisplay(DisplayIndex.Second),
            4,
            0,
            GraphicsContextFlags.ForwardCompatible)
        {
            Title += GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;
            _lastPositionState = new MousePositionState(Width/2,Height/2);
            CursorVisible = false;
            EngineLogger = new Logger();
            EngineLogger.Start();
        }

        public static Logger EngineLogger { get; set; }

        public Engine(int height, int width, Camera camera = null) : base(
            height,
            width,
            GraphicsMode.Default,
            "OpenGl Version:", GameWindowFlags.Default, DisplayDevice.GetDisplay(DisplayIndex.Second),
            4,
            0,
            GraphicsContextFlags.ForwardCompatible)
        {
            Title += GL.GetString(StringName.Version);
            VSync = VSyncMode.Off;
            _lastPositionState = new MousePositionState(Width / 2f, Height / 2f);
            CursorVisible = false;
            if (camera == null)
            {
                Camera = new Camera(Width,Height);
            }
            else
            {
                Camera = camera;
            }
            
            Load += modelManager.Update;
            Load += shaderManager.Update;
            UpdateFrame += modelManager.Update;
            UpdateFrame += shaderManager.Update;
            EngineLogger = new Logger();
            EngineLogger.Start();
        }

        public void AddShader(string vsShaderPath, string fsShaderPath = "")
        {
            shaderManager.AddShader(vsShaderPath,fsShaderPath);
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
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var projection = Camera.GetProjection();
            var shader = shaderManager.GetFirst();

            shader.use();
            shader.SetUniformMatrix4X4("projection", projection);
            var view = Camera.GetView();
            shader.SetUniformMatrix4X4("view", view);
            shader.SetUniformVector4("ambientLight",new Vector4(0.1f,0.3f,1f,0.5f));
            shader.SetUniformVector4("diffuseLight",new Vector4(0.1f,0.2f,0.3f,0.7f));
            shader.SetUniformVector3("diffuseLightpos",new Vector3(1f,10f,-10f));
            modelManager.DrawModels(shader);
            //ShaderProgram.unuse();
            CrossHair?.Draw();
            SwapBuffers();
        }



        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            HandleKeyboard((float) (e.Time - lasttime));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
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
            Mouse.SetPosition(X + Width/2f,Y + Height/2f);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
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
        }

        public void enableCrossHair(Vector4 color)
        {

            this.CrossHair = new CrossHair(this, color);
            
        }

        public void LoadModelFromFile(string modelPath)
        {
            modelManager.LoadModelFromFile(modelPath);
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
        RIGHT
    }
}