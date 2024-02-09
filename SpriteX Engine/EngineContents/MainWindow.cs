using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.ComponentModel;
using static SpriteX_Engine.EngineContents.Utilities;

namespace SpriteX_Engine.EngineContents
{
    class MainWindow : GameWindow
    {
        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
            /* --- Initial Window Parameters --- */

            Title = "SpriteX Game"; // Window Title
            ClientSize = new Vector2i(1280, 720); // Window Resolution
            AspectRatio = (16, 9); // Window Aspect Ratio
            WindowBorder = WindowBorder.Resizable; // Window Border type
            WindowState = WindowState.Normal; // Decides window state (can be used to set fullscreen) 
            UpdateFrequency = 120; // Window Framerate (setting to 0 and turning off VSync will unlock FPS)
            fixedFrameTime = 60; // How many times per second OnFixedGameUpdate() is Called
            VSync = VSyncMode.On; // Control the window's VSync
            CenterWindow(); // Will center the window in the middle of the screen
            allowAltEnter = true; // Controls whether you can toggle fullscreen when pressing Alt+Enter
            showDebugHitbox = true; // Controls whether to show all GameObjects' hitboxes
        }

        /*
         * DO NOT TOUCH ANYTHING BELOW!
         * well unless if you know what you're doing
         */

        // Shader stuff
        private int vertexArrayObject;
        private int vertexBufferObject;
        private int shaderProgram;

        private bool allowAltEnter; // Alt+Enter Control
        private double targetFrameTime; // fixed time for OnFixedGameUpdate()
        private GameCode gameCode = new GameCode(); // Declares and instantiate GameCode
        private double accumulatedTime = 0.0; // Used for OnFixedGameUpdate()

        /// <summary>
        /// Controls whether the game is paused or not.
        /// </summary>
        public bool isGamePaused { get; set; }
        /// <summary>
        /// Returns the Window's current FPS.
        /// </summary>
        public double FPS { get { return 1 / UpdateTime; } } 
        /// <summary>
        /// controls how many times OnFixedGameUpdate() method is executed per second.
        /// </summary>
        public double fixedFrameTime { get { return 1 / targetFrameTime; } set { targetFrameTime = 1 / (value < 0 ? 0 : value); } }
        /// <summary>
        /// controls whether to render the GameObject hitboxes.
        /// </summary>
        public bool showDebugHitbox { get; set; }
        /// <summary>
        /// Hold time in seconds since Window was created
        /// </summary>
        public double time { get; private set; }

        protected override void OnLoad()
        {
            gameCode.Awake(this); // Awake() from GameCode gets executed here

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
            gameCode.OnGameStart(this);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y); // Fixes viewport whenever the window changes size
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Will execute Physics/collision code, OnPrePhysicsUpdate(), and OnFixedGameUpdate()
            accumulatedTime += UpdateTime;
            while (accumulatedTime >= targetFrameTime)
            {
                if (!isGamePaused)
                {
                    gameCode.OnPrePhysicsUpdate(this);
                    GameObject.TickAllGameObjects();
                    gameCode.OnFixedGameUpdate(this);
                }
                accumulatedTime -= targetFrameTime;
            }

            time += UpdateTime; // Counts up time since game has been launched

            // Does Alt+Enter Fullscreen toggle if enabled
            if ((IsKeyDown(Keys.LeftAlt) || IsKeyDown(Keys.RightAlt)) && IsKeyPressed(Keys.Enter) && allowAltEnter)
            {
                WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
            }

            if (!isGamePaused) gameCode.OnGameUpdate(this); // OnGameUpdate() from GameCode is executed here
            gameCode.OnGameUpdateNoPause(this);
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

            gameCode.OnGraphicsUpdate(this); // OnGraphicsUpdate() in GameCode gets executed here

            // Will render the Rectangles representing the hitbox of the GameObject
            if (showDebugHitbox) foreach (GameObject obj in GameObject.GetAllGameObjects())
                if (obj.GetSize().X > 0 && obj.GetSize().Y > 0) DrawRect(obj.GetPosition(), obj.GetSize(), Color4.White, DrawType.Outline);

            GL.DisableVertexAttribArray(0);
            SwapBuffers(); // Switches buffer
        }

        protected override void OnUnload()
        {
            gameCode.OnGameEnd(this); // OnGameEnd method will get executed when game is about to close

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

        public void DrawScaledPixel(double x, double y, Color4 color)
        {
            DrawRect(new Vector2((float)x, (float)y), new Vector2(1, 1), color, DrawType.Filled);
        }

        /// <summary>
        /// Draws a triangle on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="color"></param>
        public void DrawTri(Vector2 a, Vector2 b, Vector2 c, Color4 color, DrawType drawType = DrawType.Filled)
        {
            if (drawType == DrawType.Outline) // Will draw the triangle as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, a, color);
                return;
            }
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
        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color4 color, DrawType drawType = DrawType.Filled)
        {
            if (drawType == DrawType.Outline) // Will draw the quad as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, d, color);
                DrawLine(d, a, color);
                return;
            }
            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Specify the vertex data for quad
            float[] vertices = {
                b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f,
                a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f,
                c.X / (1920 * 0.5f) - 1f, -c.Y / (1080 * 0.5f) + 1f,
                d.X / (1920 * 0.5f) - 1f, -d.Y / (1080 * 0.5f) + 1f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            // Draw the quad
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        /// <summary>
        /// Draws a simple rectangle on the game window
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="color"></param>
        public void DrawRect(Vector2 pos, Vector2 dimension, Color4 color, DrawType drawType = DrawType.Filled)
        {
            DrawQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, color, drawType);
        }

        /// <summary>
        /// Draws a line between two points on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector2 a, Vector2 b, Color4 color, double width = 1)
        {
            if (width <= 1)
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
            else
            {
                // Calculate the normalized direction between two sides of the line
                Vector2 direction = b - a;
                direction.Normalize();

                // Calculate the perpendicular vector
                Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

                // Calculate the half-width offset
                Vector2 offset = (float)width * 0.5f * perpendicular;

                // Calculate the four corners of the quad
                Vector2 topLeft = a - offset;
                Vector2 topRight = a + offset;
                Vector2 bottomLeft = b - offset;
                Vector2 bottomRight = b + offset;

                // Draws Line as a Quad to have thickness
                DrawQuad(topRight, topLeft, bottomLeft, bottomRight, color, DrawType.Filled);
            }
        }
    }
}