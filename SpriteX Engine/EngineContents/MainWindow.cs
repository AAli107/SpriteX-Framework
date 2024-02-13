using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static SpriteX_Engine.EngineContents.Utilities;
using System.Drawing;

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
            WindowState = WindowState.Fullscreen; // Decides window state (can be used to set fullscreen) 
            UpdateFrequency = 120; // Window Framerate (setting to 0 and turning off VSync will unlock FPS)
            fixedFrameTime = 60; // How many times per second OnFixedGameUpdate() is Called
            VSync = VSyncMode.On; // Control the window's VSync
            CenterWindow(); // Will center the window in the middle of the screen
            bgColor = Color.Black; // Controls the windows background color
            allowAltEnter = true; // Controls whether you can toggle fullscreen when pressing Alt+Enter
            showDebugHitbox = false; // Controls whether to show all GameObjects' hitboxes
            font = Font.GetDefaultFont(); // Contains game font
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
        private Texture tex; // Used for general rendering
        private Texture fontTex; // used for rendering text

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
        /// <summary>
        /// Controls the background Color of the window
        /// </summary>
        public Color4 bgColor { get; set; }
        /// <summary>
        /// Stores the Game's Font
        /// </summary>
        public Font font {  get; private set; }

        public Vector2 mouseGamePos { get { return MousePosition / (ClientSize / new Vector2(1920, 1080)); } }

        protected override void OnLoad()
        {
            GL.ClearColor(bgColor); // Sets Background Color
            SwapBuffers(); // Refreshes the screen on load
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            tex = new Texture();
            fontTex = new Texture(Font.GetDefaultFont().fontPath);

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
                layout (location = 1) in vec2 aTexCoord;
            
                out vec2 texCoords;
            
                void main()
                {
                    gl_Position = vec4(aPosition, 0.0, 1.0);
                    texCoords = aTexCoord;
                }
            ";

            // The Fragment shader code
            string fragmentShaderSource = @"
                #version 330 core

                uniform vec4 uColor;
                uniform sampler2D uTexture;
                
                in vec2 texCoords;
                
                out vec4 FragColor;
                
                void main()
                {
                    FragColor = texture(uTexture, texCoords) * uColor;
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

            // Allows Transparency for textures
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

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
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);

            gameCode.OnGraphicsUpdate(this); // OnGraphicsUpdate() in GameCode gets executed here

            // Will render the Rectangles representing the hitbox of the GameObject
            if (showDebugHitbox) foreach (GameObject obj in GameObject.GetAllGameObjects())
                if (obj.GetSize().X > 0 && obj.GetSize().Y > 0) DrawRect(obj.GetPosition(), obj.GetSize(), Color4.White, Enums.DrawType.Outline);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1); 
            GL.ClearColor(bgColor);
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
            if ((x < 0) || (y < 0) || (x > 1920) || (y > 1080))
                return;

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();
            // Specify the vertex data for pixel
            float[] position = { (float)x / (ClientSize.X * 0.5f) - 1f, (float)-y / (ClientSize.Y * 0.5f) + 1f, 0, 0 };
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * position.Length, position, BufferUsageHint.DynamicDraw);

            // Draws the pixel
            GL.DrawArrays(PrimitiveType.Points, 0, 1);

            tex.Unbind();
        }

        /// <summary>
        /// Draws a single pixel on the game window (exact pixel position)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void DrawPixel(Vector2 position, Color4 color)
        {
            DrawPixel(position.X, position.Y, color);
        }

        /// <summary>
        /// Draws a single pixel that scales with Game Window
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void DrawScaledPixel(Vector2 position, Color4 color)
        {
            DrawRect(position, new Vector2(1, 1), color, Enums.DrawType.Filled);
        }

        /// <summary>
        /// Draws a single pixel that scales with Game Window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawScaledPixel(double x, double y, Color4 color)
        {
            DrawScaledPixel(new Vector2((float)x, (float)y), color);
        }

        /// <summary>
        /// Draws a triangle on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="color"></param>
        public void DrawTri(Vector2 a, Vector2 b, Vector2 c, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled)
        {
            if (
                (a.X < 0 && b.X < 0 && c.X < 0)             ||
                (a.Y < 0 && b.Y < 0 && c.Y < 0)             ||
                (a.X > 1920 && b.X > 1920 && c.X > 1920)    ||
                (a.Y > 1080 && b.Y > 1080 && c.Y > 1080)
                ) return;

            if (drawType == Enums.DrawType.Outline) // Will draw the triangle as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, a, color);
                return;
            }
            // Triangle verticies
            float[] vertices = {
            a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f, 1.0f, 1.0f,
            b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f, 0.0f, 1.0f,
            c.X / (1920 * 0.5f) - 1f, -c.Y / (1080 * 0.5f) + 1f, 1.0f, 0.0f
            };

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            tex.Unbind();
        }

        /// <summary>
        /// Draws a Quad on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled)
        {
            if (
                (a.X < 0 && b.X < 0 && c.X < 0 && d.X < 0)              ||
                (a.Y < 0 && b.Y < 0 && c.Y < 0 && d.Y < 0)              ||
                (a.X > 1920 && b.X > 1920 && c.X > 1920 && d.X > 1920)  ||
                (a.Y > 1080 && b.Y > 1080 && c.Y > 1080 && d.Y > 1080)
                ) return;

            if (drawType == Enums.DrawType.Outline) // Will draw the quad as an outline
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

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();

            // Specify the vertex data for quad
            float[] vertices = {
                b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f, 1.0f, 1.0f,
                a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f, 0.0f, 1.0f,
                c.X / (1920 * 0.5f) - 1f, -c.Y / (1080 * 0.5f) + 1f, 1.0f, 0.0f,
                d.X / (1920 * 0.5f) - 1f, -d.Y / (1080 * 0.5f) + 1f, 0.0f, 0.0f 
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            tex.Unbind();
        }

        /// <summary>
        /// Draws a Quad with texture
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void DrawTexturedQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Texture texture, Color4 color)
        {
            if (
                (a.X < 0 && b.X < 0 && c.X < 0 && d.X < 0) ||
                (a.Y < 0 && b.Y < 0 && c.Y < 0 && d.Y < 0) ||
                (a.X > 1920 && b.X > 1920 && c.X > 1920 && d.X > 1920) ||
                (a.Y > 1080 && b.Y > 1080 && c.Y > 1080 && d.Y > 1080)
                ) return;

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            // Bind the texture
            texture.Bind();

            // Specify the vertex data for the quad
            float[] vertices = {
                b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f, 1.0f, 1.0f,                                         
                a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f, 0.0f, 1.0f,                                         
                c.X / (1920 * 0.5f) - 1f, -c.Y / (1080 * 0.5f) + 1f, 1.0f, 0.0f,                                         
                d.X / (1920 * 0.5f) - 1f, -d.Y / (1080 * 0.5f) + 1f, 0.0f, 0.0f                                          
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            // Unbind the texture
            texture.Unbind();
        }

        /// <summary>
        /// Draws a Quad with texture
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="texture"></param>
        public void DrawTexturedQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Texture texture)
        {
            DrawTexturedQuad(a, b, c, d, texture, Color4.White);
        }

        /// <summary>
        /// Draws a simple rectangle on the game window
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="color"></param>
        public void DrawRect(Vector2 pos, Vector2 dimension, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled)
        {
            DrawQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, color, drawType);
        }

        /// <summary>
        /// Draws an image texture on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        public void DrawImage(Vector2 pos, Vector2 dimension, Texture texture, Color4 color)
        {
            DrawTexturedQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, texture, color);
        }

        /// <summary>
        /// Draws an image texture on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="texture"></param>
        public void DrawImage(Vector2 pos, Vector2 dimension, Texture texture)
        {
            DrawImage(pos, dimension, texture, Color4.White);
        }

        /// <summary>
        /// Draws a line between two points on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector2 a, Vector2 b, Color4 color, double width = 1)
        {
            if (
                (a.X < 0 && b.X < 0)        ||
                (a.Y < 0 && b.Y < 0)        ||
                (a.X > 1920 && b.X > 1920)  ||
                (a.Y > 1080 && b.Y > 1080)
                ) return;

            if (width <= 1)
            {
                // Line vertices, point a and point b
                float[] vertices = {
                    a.X / (1920 * 0.5f) - 1f, -a.Y / (1080 * 0.5f) + 1f, 0, 0,
                    b.X / (1920 * 0.5f) - 1f, -b.Y / (1080 * 0.5f) + 1f, 0, 0
                };

                
                // Set the ucolor in the shader
                int colorUniformLocation = GL.GetUniformLocation(shaderProgram, "uColor");
                GL.Uniform4(colorUniformLocation, color);

                // Set the texture uniform in the shader
                int textureUniformLocation = GL.GetUniformLocation(shaderProgram, "uTexture");
                GL.Uniform1(textureUniformLocation, 0);

                tex.Bind();

                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

                GL.DrawArrays(PrimitiveType.Lines, 0, 2);
                
                tex.Unbind();
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
                DrawQuad(topRight, topLeft, bottomLeft, bottomRight, color, Enums.DrawType.Filled);
            }
        }

        /// <summary>
        /// Draws a Single character on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="character"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public void DrawChar(Vector2 pos, char character, Color4 color, float size = 1)
        {
            Vector2 charVec = (font.charSize * size);
            if (
                (pos.X < 0 && pos.X < 0 && pos.X < 0 && pos.X < 0) ||
                (pos.Y < 0 && pos.Y < 0 && pos.Y < 0 && pos.Y < 0) ||
                (pos.X > 1920 && pos.X > 1920 && pos.X > 1920 && pos.X > 1920) ||
                (pos.Y > 1080 && pos.Y > 1080 && pos.Y > 1080 && pos.Y > 1080)
                ) return;

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(shaderProgram, "uTexture"), 0);

            // Bind the texture
            fontTex.Bind();

            int charCount = Font.charSheet.Count;
            int charIndex = Font.charSheet.IndexOf(character);

            // Specify the vertex data for the quad
            float[] vertices = {
                (pos.X) / (1920 * 0.5f) - 1f, -(pos.Y) / (1080 * 0.5f) + 1f,                                (1.0f / charCount) * charIndex, 0.0f,
                (pos.X + charVec.X) / (1920 * 0.5f) - 1f, -(pos.Y) / (1080 * 0.5f) + 1f,                    (1.0f / charCount) * charIndex + (1.0f / charCount), 0.0f,
                (pos.X) / (1920 * 0.5f) - 1f, -(pos.Y + charVec.Y) / (1080 * 0.5f) + 1f,                    (1.0f / charCount) * charIndex, 1.0f,
                (pos.X + charVec.X) / (1920 * 0.5f) - 1f, -(pos.Y + charVec.Y ) / (1080 * 0.5f) + 1f,       (1.0f / charCount) * charIndex + (1.0f / charCount), 1.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            // Unbind the texture
            fontTex.Unbind();
        }

        /// <summary>
        /// Draws Text on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        public void DrawText(Vector2 pos, string text, Color4 color, float size = 1)
        {
            for (int i = 0; i < text.Length; i++) 
            {
                if (text[i].Equals('\n') && i+1 < text.Length) 
                {
                    DrawText(pos + new Vector2(0, font.charSize.Y * size), text.Substring(i+1), color, size);
                    return;
                }
                DrawChar(pos + new Vector2(font.charSize.X * i * size, 0), text[i], color, size);
            }
        }

        /// <summary>
        /// Sets the Game's font
        /// </summary>
        /// <param name="font"></param>
        public void SetGameFont(Font font)
        {
            this.font = font;
            fontTex = new Texture(font.fontPath);
        }
    }
}