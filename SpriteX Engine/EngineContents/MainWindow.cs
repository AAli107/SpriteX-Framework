using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;

namespace SpriteX_Engine.EngineContents
{
    class MainWindow : GameWindow
    {
        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
            Title = "SpriteX Game"; // Window Title
            ClientSize = new Vector2i(1280, 720); // Window Resolution
            AspectRatio = (16, 9); // Window Aspect Ratio
            WindowBorder = WindowBorder.Resizable; // Window Border type
            WindowState = WindowState.Normal; // Decides window state (can be used to set fullscreen) 
            UpdateFrequency = 120; // Window Framerate
            VSync = VSyncMode.On; // Control the window's VSync
            CenterWindow(); // Will center the window in the middle of the screen
            allowAltEnter = true; // Controls whether you can toggle fullscreen when pressing Alt+Enter
        }

        // Shader stuff
        private int vertexArrayObject;
        private int vertexBufferObject;
        private int shaderProgram;

        private bool allowAltEnter; // Alt+Enter Control

        public double FPS { get { return 1 / UpdateTime; } }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Create the vertex array object (VAO)
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // Create the vertex buffer object (VBO)
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            // The Vertex shader code
            string vertexShaderSource = @"
                #version 330 core

                layout (location = 0) in vec2 aPosition;

                void main()
                {
                    gl_Position = vec4(aPosition, 0.0, 1.0);
                }
            ";

            // The Fragment shader code
            string fragmentShaderSource = @"
                #version 330 core

                uniform vec4 uColor;

                out vec4 FragColor;

                void main()
                {
                    FragColor = uColor;
                }
            ";

            // Compile Vertex Shader code
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            // Compile Fragment shader code
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            // Create Shader Program
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            // OnGameStart() gets executed from GameCode
            GameCode.OnGameStart(this);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // Fixes viewport whenever the window changes size
            base.OnResize(e);
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        }

        // Game Update per frame 
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            if ((IsKeyDown(Keys.LeftAlt) || IsKeyDown(Keys.RightAlt)) && IsKeyPressed(Keys.Enter) && allowAltEnter)
            {
                WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
            }
            GameCode.OnGameUpdate(this);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Does stuff that allows the game window to be able to render whatever you wrote in OnGraphicsUpdate()
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vertexArrayObject);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

            GameCode.OnGraphicsUpdate(this); // OnGraphicsUpdate() in GameCode gets executed here

            GL.DisableVertexAttribArray(0);
            SwapBuffers(); // Switches buffer
        }

        protected override void OnUnload()
        {
            // Clean up resources
            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
            GL.DeleteProgram(shaderProgram);

            base.OnUnload();
        }

        /// <summary>
        /// Draws a single pixel on the game window (exact pixel position)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(double x, double y, Color4 color)
        {
            // Set the color uniform in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Specify the vertex data for pixel
            float[] position = { (float)x / (ClientSize.X * 0.5f) - 1f, (float)-y / (ClientSize.Y * 0.5f) + 1f };
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * position.Length, position, BufferUsageHint.DynamicDraw);

            // Draws the pixel
            GL.DrawArrays(PrimitiveType.Points, 0, 1);
        }

        /// <summary>
        /// Draws a single pixel on the game window with Vector2 as parameter (exact pixel position)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void DrawPixel(Vector2 position, Color4 color)
        {
            DrawPixel(position.X, position.Y, color);
        }

        /// <summary>
        /// Draws a triangle on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="color"></param>
        public void DrawTri(Vector2 a, Vector2 b, Vector2 c, Color4 color)
        {
            // Triangle verticies
            float[] vertices = {
                a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f,
                b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f,
                c.X / (1920 * 0.5f) - 1f, -c.Y / (1080 * 0.5f) + 1f
            };

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        /// <summary>
        /// Draws a Quad on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color4 color)
        {
            DrawTri(a, b, c, color);
            DrawTri(a, d, c, color);
        }

        /// <summary>
        /// Draws a simple rectangle on the game window
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="color"></param>
        public void DrawRect(Vector2 pos, Vector2 dimension, Color4 color)
        {
            DrawQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, color);
        }

        /// <summary>
        /// Draws a line between two points on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector2 a, Vector2 b, Color4 color)
        {
            // Line vertices, point a and point b
            float[] vertices = {
                a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f,
                b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f
            };

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }
    }
}