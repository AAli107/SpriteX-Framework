using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Diagnostics;

namespace SpriteX_Engine.EngineContents
{
    static class Engine
    {
        public static Window wnd; // Creates the window
        public static Vector2 resolution = new Vector2(1280, 720); // Resolution of the Window
        public static string gameTitle = "SpriteX Game"; // Title of the window
        public static Color bgColor = Color.Black; // The Color of the background

        public static bool gameRunning = true; // If set to false, game loop will stop and the program will close
        public static bool renderGraphics = true; // Controls whether you want to render the graphics or not
        public static bool gamePaused = false; // The GameCode.OnGameUpdate() function and other updates will not execute only if it's false
        public static bool showFPS = true; // Controls whether the currentFPS is displayed on the screen or not

        /// <summary>
        /// The amount in seconds it took to render the last frame (read only)
        /// </summary>
        public static float deltaTime
        { get { return deltaT; } }

        /// <summary>
        /// The frames per second the game is running at
        /// </summary>
        public static float currentFPS
        { get { return 1 / deltaT; } }

        private static float deltaT; // The amount in seconds it takes to render a frame
        private static Stopwatch stopwatch = new Stopwatch(); // stopwatch to measure time it takes to for every game loop


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            wnd = new Window();
            Application.Run(wnd);
        }
        public static void Tick(Object myObject, EventArgs myEventArgs)
        {
            if (gameRunning)
            {
                // Stops timer and sets deltaT to the time it took to render a frame
                stopwatch.Stop();
                deltaT = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Reset();
                stopwatch.Start(); // starts a timer to measure the delta time of the current frame

                if (!gamePaused)
                {
                    GameCode.OnGameUpdate(); // Updates the game
                }
                if (renderGraphics)
                {
                    GameCode.OnGraphicsUpdate(); // Executes all the graphics code
                    if (showFPS) // If showFPS is true, it will display a text showing the current FPS
                        gfx.GameUI.DrawText(0, 0, ((int)currentFPS).ToString() + " FPS", new Font(FontFamily.GenericMonospace, 12), Color.Lime);

                    
                    gfx.imageOnScreen = gfx.frameBuffer.Clone(new RectangleF(0, 0, gfx.drawWidth, gfx.drawHeight), PixelFormat.Format24bppRgb);
                    wnd.graphics.DrawImage(gfx.imageOnScreen, 0, 0); // Shows final rendered image on screen

                    gfx.ResetBuffer(bgColor); // Blanks the frameBuffer with the background color

                    // Clears memory of the image on screen
                    using (gfx.imageOnScreen)
                        gfx.imageOnScreen.Dispose();
                    gfx.imageOnScreen = null;
                }
            }
            else 
            {
                // Closes the game if gameRunning is set to false
                GameCode.OnGameEnd();
                wnd.timer.Stop();
                wnd.Close();
                Application.Exit();
            }
        }
    }
}
