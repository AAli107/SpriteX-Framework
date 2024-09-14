﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SpriteX_Engine.EngineContents.Components;
using System;
using System.Drawing;
using System.IO;

namespace SpriteX_Engine.EngineContents
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
            CenterWindow(); // Will center the window in the middle of the screen

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

        private bool allowAltEnter; // Alt+Enter Control
        private double targetFrameTime; // fixed time for FixedUpdate()
        private gfx gfx; // All graphics rendering method stored here
        private GameLevelScript gameLevel; // Declares Game-Level script
        private GameLevelScript startLevel; // Contains the start level when the game launches
        private double accumulatedTime = 0.0; // Used for FixedUpdate()
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
        /// controls how many times OnFixedGameUpdate() method is executed per second
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
        public Vector2d mouseWorldPos { get { return (MousePosition / (ClientSize / new Vector2d(1920, 1080))) + new Vector2d(GetWorldCamera().camPos.X - (1920 * 0.5f), GetWorldCamera().camPos.Y - (1080 * 0.5f)); } }

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
            gfx?.ScreenRefresh();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (gcTimer > fixedFrameTime * 5) { GC.Collect(); gcTimer = 0; }
  
            world.WorldUpdate(); // Updates world

            // Will execute Physics/collision code, PrePhysicsUpdate(), and FixedUpdate()
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
                gameLevel.GameUpdate(this); // OnGameUpdate() from GameCode is executed here
                foreach (GameObject obj in world.gameObjects)
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

            // Does stuff that allows the game window to be able to render whatever you wrote in OnGraphicsUpdate()
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vertexArrayObject);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);

            gameLevel.GraphicsUpdate(this, gfx); // OnGraphicsUpdate() in GameCode gets executed here

            foreach (GameObject obj in world.GetAllGameObjects())
            {
                if (!obj.isEnabled) continue;

                obj.Render(this, gfx); // Will do rendering tick on all Game Objects

                if (showDebugHitbox) // Will render the Rectangles representing the hitbox of the GameObject
                    foreach (ColliderComponent cc in obj.GetComponents<ColliderComponent>())
                        if (cc.isEnabled)
                        { 
                            var hb = cc.GetHitbox();
                            gfx.DrawRect(hb.Center - hb.HalfSize, hb.Size, cc.isSolidCollision ? Color4.White : Color4.Blue, gfx.DrawType.Outline, false); 
                        }
            }

            // Will render all the visible buttons
            foreach (Button btn in Button.buttons)
                if (btn.isVisible) gfx.DrawImage(new Vector2(btn.buttonRect.Location.X, btn.buttonRect.Location.Y), 
                    new Vector2(btn.buttonRect.Size.Width, btn.buttonRect.Size.Height), btn.tex, btn.currentColor, true);
            
            if (showStats) // Will display FPS and UpdateTime (ms)
                gfx.DrawText(new Vector2(16, 16), string.Format("{0:0.00}", Math.Round(FPS, 2)) + " FPS\n"
                    + string.Format("{0:0.00}", Math.Round(UpdateTime * 1000, 2)) + " ms", 
                    FPS > 48 ? Color4.Lime : (FPS > 24 ? Color4.Yellow : Color4.Red), 0.5f);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1); 
            GL.ClearColor(bgColor);
            SwapBuffers(); // Switches buffer
        }

        protected override void OnUnload()
        {
            gameLevel.GameEnd(this); // GameEnd method will get executed when game is about to close

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
                    if (btn.buttonRect.IntersectsWith(new RectangleF((float)mouseScreenPos.X, (float)mouseScreenPos.Y, 0, 0))) // Will invoke OnButtonPress event
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
                    if (btn.buttonRect.IntersectsWith(new RectangleF((float)mouseScreenPos.X, (float)mouseScreenPos.Y, 0, 0)))
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
                btn.isHovered = btn.buttonRect.IntersectsWith(new RectangleF((float)mouseScreenPos.X, (float)mouseScreenPos.Y, 0, 0));
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
            gameLevel.Awake(this); // Awake() from GameCode gets executed here
            var auds = world?.audios;
            if (auds != null)
                foreach (var a in auds)
                    a.Stop();
            world = new World(cam);
            World.SetWorldInst(world);
            Button.buttons.Clear();
            gameLevel.LevelStart(this); // Executes when world is Created
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