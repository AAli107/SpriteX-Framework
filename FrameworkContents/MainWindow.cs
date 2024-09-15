using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SpriteX_Framework.FrameworkContents.Components;
using System;
using System.IO;

namespace SpriteX_Framework.FrameworkContents
{
    public class MainWindow : GameWindow
    {
        /*
         * DO NOT TOUCH ANYTHING BELOW!
         * well unless if you know what you're doing
         */

        public MainWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
            CenterWindow();

            // Will input all the values from InitialGameWindowConfig into MainWindow
            Title = InitialGameWindowConfig.Title;
            ClientSize = InitialGameWindowConfig.ClientSize;
            AspectRatio = InitialGameWindowConfig.AspectRatio;
            WindowBorder = InitialGameWindowConfig.WindowBorder;
            WindowState = InitialGameWindowConfig.WindowState;
            UpdateFrequency = InitialGameWindowConfig.UpdateFrequency;
            fixedFrameTime = InitialGameWindowConfig.fixedFrameTime;
            VSync = InitialGameWindowConfig.VSync;
            bgColor = InitialGameWindowConfig.bgColor;
            allowAltEnter = InitialGameWindowConfig.allowAltEnter;
            showDebugHitbox = InitialGameWindowConfig.showDebugHitbox;
            showStats = InitialGameWindowConfig.showStats;
            font = InitialGameWindowConfig.font;
            startLevel = InitialGameWindowConfig.startLevel;
        }

        // Shader stuff
        public int vertexArrayObject { get; private set; }
        public int vertexBufferObject { get; private set; }
        public int shaderProgram { get; private set; }

        private bool allowAltEnter;
        private double targetFrameTime;
        private gfx gfx;
        private GameLevelScript gameLevel;
        private GameLevelScript startLevel; 
        private double accumulatedTime = 0.0;
        private int gcTimer = 0;

        /// <summary>
        /// Controls whether the game is paused or not
        /// </summary>
        public bool isGamePaused { get; set; }
        /// <summary>
        /// Returns the Window's current FPS
        /// </summary>
        public double FPS { get { return 1 / UpdateTime; } } 
        /// <summary>
        /// controls how many times FixedUpdate method is executed per second
        /// </summary>
        public double fixedFrameTime { get { return 1 / targetFrameTime; } set { targetFrameTime = 1 / (value < 0 ? 0 : value); } }
        /// <summary>
        /// controls whether to render the GameObject hitboxes
        /// </summary>
        public bool showDebugHitbox { get; set; }
        /// <summary>
        /// Displays Stats showing FPS and Update time(ms) when true
        /// </summary>
        public bool showStats { get; set; }
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

        public Vector2d mouseScreenPos { get { return MousePosition / (ClientSize / new Vector2d(1920, 1080)); } }
        public Vector2d mouseWorldPos { get { return (MousePosition / (ClientSize / new Vector2d(1920, 1080))) + new Vector2d(GetWorldCamera().camPos.X - 960, GetWorldCamera().camPos.Y - 540); } }

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
                GL.ShaderSource(vertexShader, File.ReadAllText("Resources/Framework/vertexShader.vert"));
                GL.ShaderSource(fragmentShader, File.ReadAllText("Resources/Framework/fragmentShader.frag"));
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
            gfx?.ScreenRefresh();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (gcTimer > fixedFrameTime * 5) { GC.Collect(); gcTimer = 0; }
  
            world.WorldUpdate();

            // Will execute Physics/collision code, PrePhysicsUpdate, and FixedUpdate
            accumulatedTime += UpdateTime;
            while (accumulatedTime >= targetFrameTime)
            {
                if (!isGamePaused)
                {
                    gameLevel.PrePhysicsUpdate(this);
                    world.TickAllGameObjects(this);
                    gameLevel.FixedUpdate(this);
                }
                gcTimer++;
                accumulatedTime -= targetFrameTime;
            }

            time += UpdateTime; // Counts up time since game has been launched

            // Does Alt+Enter Fullscreen toggle if enabled
            if ((IsKeyDown(Keys.LeftAlt) || IsKeyDown(Keys.RightAlt)) && IsKeyPressed(Keys.Enter) && allowAltEnter)
            {
                WindowState = IsFullscreen ? WindowState.Normal : WindowState.Fullscreen;
            }

            if (!isGamePaused)
            {
                gameLevel.GameUpdate(this);
                GameObject[] _gameObjects = new GameObject[world.gameObjects.Count];
                world.gameObjects.Values.CopyTo(_gameObjects, 0);
                foreach (GameObject obj in _gameObjects)
                {
                    var cs = obj.GetAllComponents();

                    if (cs.Length <= 0) continue;

                    foreach (var c in cs)
                    {
                        if (c is ScriptComponent)
                            if (c.isEnabled) 
                                (c as ScriptComponent).Update(this);
                    }
                }
            }
            gameLevel.GameUpdateNoPause(this);    
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Does stuff that allows the game window to be able to render whatever you wrote in GraphicsUpdate()
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vertexArrayObject);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);

            gameLevel.GraphicsUpdate(this, gfx);

            GameObject[] _gameObjects = new GameObject[world.gameObjects.Count];
            world.gameObjects.Values.CopyTo(_gameObjects, 0);
            foreach (GameObject obj in _gameObjects)
            {
                if (!obj.isEnabled) continue;

                obj.Render(this, gfx); // Will render all game objects

                if (showDebugHitbox) // Will render the Rectangles representing the hitbox of the GameObject
                    foreach (ColliderComponent cc in obj.GetComponents<ColliderComponent>())
                        if (cc.isEnabled)
                        { 
                            var hb = cc.GetHitbox();
                            gfx.DrawRect(hb.Center - hb.HalfSize, hb.Size, cc.isSolidCollision ? Color4.White : Color4.Blue, gfx.DrawType.Outline, false); 
                        }
            }
            
            if (showStats) // Will display FPS and UpdateTime (ms)
                gfx.DrawText(new Vector2(16, 16), string.Format("{0:0.00}", Math.Round(FPS, 2)) + " FPS\n"
                    + string.Format("{0:0.00}", Math.Round(UpdateTime * 1000, 2)) + " ms", 
                    FPS > 48 ? Color4.Lime : (FPS > 24 ? Color4.Yellow : Color4.Red), 0.5f);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1); 
            GL.ClearColor(bgColor);
            SwapBuffers();
        }

        protected override void OnUnload()
        {
            gameLevel.GameEnd(this);

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
                GameObject[] _gameObjects = new GameObject[world.gameObjects.Count];
                world.gameObjects.Values.CopyTo(_gameObjects, 0);
                foreach (GameObject gameObj in _gameObjects)
                {
                    if (!gameObj.isEnabled) continue;

                    ButtonComponent[] buttonComponents = gameObj.GetComponents<ButtonComponent>();

                    for (int j = 0; j < buttonComponents.Length; j++)
                    {
                        ButtonComponent btn = buttonComponents[j];

                        if (btn.buttonRect.ContainsInclusive(mouseScreenPos))
                        {
                            btn.IsPressed = false;
                            btn.InvokeButtonPress(this, e);
                        }

                        btn.IsPressed = false;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                GameObject[] _gameObjects = new GameObject[world.gameObjects.Count];
                world.gameObjects.Values.CopyTo(_gameObjects, 0);
                foreach (GameObject gameObj in _gameObjects)
                {
                    if (!gameObj.isEnabled) continue;

                    ButtonComponent[] buttonComponents = gameObj.GetComponents<ButtonComponent>();

                    for (int j = 0; j < buttonComponents.Length; j++)
                    {
                        if (!buttonComponents[j].buttonRect.ContainsInclusive(mouseScreenPos)) continue;
                        buttonComponents[j].IsPressed = true;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            // Will determine whether button is being hovered on or not
            GameObject[] _gameObjects = new GameObject[world.gameObjects.Count];
            world.gameObjects.Values.CopyTo(_gameObjects, 0);
            foreach (GameObject gameObj in _gameObjects)
            {
                if (!gameObj.isEnabled) continue;

                ButtonComponent[] buttonComponents = gameObj.GetComponents<ButtonComponent>();

                for (int j = 0; j < buttonComponents.Length; j++)
                    buttonComponents[j].IsHovered = buttonComponents[j].buttonRect.ContainsInclusive(mouseScreenPos);
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
        /// Plays Audio from file, volume ranges from 0-1
        /// </summary>
        /// <param name="path"></param>
        public void PlayAudio(string path, float volume = 1f)
        {
            world.PlayAudio(path, volume);
        }

        /// <summary>
        /// Will Load level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="cam"></param>
        public void LoadLevel(GameLevelScript level, Camera cam)
        {
            gameLevel?.LevelEnd(this);
            gameLevel = level; 
            GC.Collect();
            gameLevel.Awake(this);
            var auds = world?.audios;
            if (auds != null)
                foreach (var a in auds)
                    a.Stop();
            world = new World(cam);
            World.SetWorldInst(world);
            gameLevel.LevelStart(this);
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