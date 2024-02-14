using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using static SpriteX_Engine.EngineContents.Utilities;

namespace SpriteX_Engine.EngineContents
{
    public class MainWindow : GameWindow
    {
        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
            /* --- Initial Window Parameters --- */

            Title = "SpriteX Game"; // Window Title
            ClientSize = new Vector2i(1280, 720); // Default Window Resolution when not in fullscreen
            AspectRatio = (16, 9); // Window Aspect Ratio
            WindowBorder = WindowBorder.Resizable; // Window Border type
            WindowState = WindowState.Fullscreen; // Decides window state (can be used to set fullscreen) 
            UpdateFrequency = 120; // Window Framerate (setting to 0 and turning off VSync will unlock FPS)
            fixedFrameTime = 60; // How many times per second the game updates
            VSync = VSyncMode.On; // Control the window's VSync
            CenterWindow(); // Will center the window in the middle of the screen
            bgColor = Color.Black; // Controls the windows background color
            allowAltEnter = true; // Controls whether you can toggle fullscreen when pressing Alt+Enter
            showDebugHitbox = true; // Controls whether to show all GameObjects' hitboxes
            font = Font.GetDefaultFont(); // Contains game font
            startLevel = new GameCode(); // The Level to load when game launches
        }

        /*
         * DO NOT TOUCH ANYTHING BELOW!
         * well unless if you know what you're doing
         */

        // Shader stuff
        public int vertexArrayObject { get; private set; }
        public int vertexBufferObject { get; private set; }
        public int shaderProgram { get; private set; }

        private bool allowAltEnter; // Alt+Enter Control
        private double targetFrameTime; // fixed time for OnFixedGameUpdate()
        private gfx gfx; // All graphics rendering method stored here
        private GameLevelScript gameCode; // Declares Game-Level script
        private GameLevelScript startLevel; // Contains the start level when the game launches
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
        /// <summary>
        /// Controls the background Color of the window
        /// </summary>
        public Color4 bgColor { get; set; }
        /// <summary>
        /// Stores the Game's Font
        /// </summary>
        public Font font {  get; private set; }
        /// <summary>
        /// Stores the game world
        /// </summary>
        public World world { get; private set; }

        public Vector2 mouseScreenPos { get { return MousePosition / (ClientSize / new Vector2(1920, 1080)); } }
        public Vector2 mouseWorldPos { get { return (MousePosition / (ClientSize / new Vector2(1920, 1080))) + new Vector2(GetWorldCamera().camPos.X - (1920 * 0.5f), GetWorldCamera().camPos.Y - (1080 * 0.5f)); } }

        protected override void OnLoad()
        {
            GL.ClearColor(bgColor); // Sets Background Color
            SwapBuffers(); // Refreshes the screen on load
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
            gfx = new gfx(this);

            base.OnLoad();

            LoadLevel(startLevel); // Creates the World with default world camera
            startLevel = null;

            // Create the vertex array object (VAO)
            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            // Create the vertex buffer object (VBO)
            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);


            // Tries to Load Vertex and Fragment Shader
            try
            {
                GL.ShaderSource(vertexShader, File.ReadAllText("Resources/Engine/vertexShader.vert"));
                GL.ShaderSource(fragmentShader, File.ReadAllText("Resources/Engine/fragmentShader.frag"));
            } catch (Exception) { Close(); } // Closes the Game when it catches an exception

            // Compiles the Vertex and Fragment Shader
            GL.CompileShader(vertexShader);
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
                    world.TickAllGameObjects();
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

            gameCode.OnGraphicsUpdate(this, gfx); // OnGraphicsUpdate() in GameCode gets executed here

            // Will render the Rectangles representing the hitbox of the GameObject
            if (showDebugHitbox) foreach (GameObject obj in world.GetAllGameObjects())
                    if (obj.GetSize().X > 0 && obj.GetSize().Y > 0) gfx.DrawRect(obj.GetPosition(), obj.GetSize(), Color4.White, gfx.DrawType.Outline, false);

            // Will render all the visible buttons
            foreach (Button btn in Button.buttons)
                if (btn.isVisible) gfx.DrawImage(new Vector2(btn.buttonRect.Location.X, btn.buttonRect.Location.Y), 
                    new Vector2(btn.buttonRect.Size.Width, btn.buttonRect.Size.Height), btn.tex, btn.currentColor, true);

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

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButton.Left)
            {
                foreach (Button btn in Button.buttons)
                {
                    if (btn.buttonRect.IntersectsWith(new RectangleF(mouseScreenPos.X, mouseScreenPos.Y, 0, 0))) // Will invoke OnButtonPress event
                    {
                        btn.isPressed = false;
                        btn.InvokeButtonPress(this, e);
                    }

                    btn.isPressed = false;
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left) // Will determine whether button is being held
            {
                foreach (Button btn in Button.buttons)
                {
                    if (btn.buttonRect.IntersectsWith(new RectangleF(mouseScreenPos.X, mouseScreenPos.Y, 0, 0)))
                    {
                        btn.isPressed = true;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            foreach (Button btn in Button.buttons) // Will determine whether button is being hovered on or not
            {
                btn.isHovered = btn.buttonRect.IntersectsWith(new RectangleF(mouseScreenPos.X, mouseScreenPos.Y, 0, 0));
            }
        }

        /// <summary>
        /// Sets the Game's font
        /// </summary>
        /// <param name="font"></param>
        public void SetGameFont(Font font)
        {
            this.font = font;
            gfx.fontTex = new Texture(font.fontPath);
        }

        /// <summary>
        /// Returns the camera the world is using
        /// </summary>
        /// <returns></returns>
        public Camera GetWorldCamera()
        {
            return world.cam;
        }

        /// <summary>
        /// Switches the world's Camera
        /// </summary>
        /// <param name="cam"></param>
        public void SetWorldCamera(Camera cam)
        {
            world.cam = cam;
        }

        /// <summary>
        /// Will Load level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="cam"></param>
        public void LoadLevel(GameLevelScript level, Camera cam)
        {
            gameCode = level;
            gameCode.Awake(this); // Awake() from GameCode gets executed here
            world = new World(cam);
            Button.buttons.Clear();
            gameCode.OnGameStart(this); // Executes when world is Created
        }

        /// <summary>
        /// Will Load Level
        /// </summary>
        /// <param name="level"></param>
        public void LoadLevel(GameLevelScript level)
        {
            LoadLevel(level, new Camera());
        }
    }
}