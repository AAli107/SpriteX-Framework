using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Audio.OpenAL.ALC;
using SpriteX_Engine.EngineContents;

namespace SpriteX_Engine
{
    static class GameCode
    {
        // Insert static Variables here \\
        // \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/ \\
        static Vector2 pos = new Vector2(100, 100);
        static Vector2 velocity = new Vector2(0, 0);
        static float s = 0.2f;
        // /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\ \\

        public static void OnGameStart(MainWindow win) // Gets Executed when game starts running, after the main Window loads
        {

        }

        public static void OnGameUpdate(MainWindow win) // Gets Executed every frame, used for Game related actions
        {
            if (win.IsKeyPressed(Keys.Escape)) win.Close();

            s = win.IsKeyDown(Keys.LeftControl) ? 0.1f : (win.IsKeyDown(Keys.LeftShift) ? 0.4f : 0.2f);
            if (win.IsKeyDown(Keys.W)) velocity.Y -= s;
            if (win.IsKeyDown(Keys.A)) velocity.X -= s;
            if (win.IsKeyDown(Keys.S)) velocity.Y += s;
            if (win.IsKeyDown(Keys.D)) velocity.X += s;

            pos.X += velocity.X;
            pos.Y += velocity.Y;

            velocity *= 0.95f;

            Console.WriteLine("Position = (" + Math.Round(pos.X, 2) + ", " + Math.Round(pos.Y, 2) + ")                                       ");
            Console.WriteLine("Speed = " + Math.Round(velocity.Length, 2) + "                                       ");
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.CursorVisible = false;
        }

        public static void OnGraphicsUpdate(MainWindow gfx) // Used to put everything related to rendering stuff
        {
            gfx.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.SteelBlue);
            gfx.DrawRect(new Vector2(250, 250), new Vector2(200, 100), Color4.Gold);
            gfx.DrawQuad(pos + new Vector2(0, -100), pos + new Vector2(100, 0), pos + new Vector2(0, 100), pos + new Vector2(-100, 0), Color4.Red);

            // Draws large crosshair
            gfx.DrawLine(pos + new Vector2(-100, 0), pos + new Vector2(100, 0), Color4.White);
            gfx.DrawLine(pos + new Vector2(0, -100), pos + new Vector2(0, 100), Color4.White);
            gfx.DrawPixel(pos, Color4.White);
            for (float i = 1; i <= 100; i += 0.1f)
            {
                gfx.DrawPixel(pos + new Vector2(0, i), Color4.White);
                gfx.DrawPixel(pos + new Vector2(i, 0), Color4.White);
                gfx.DrawPixel(pos + new Vector2(0, -i), Color4.White);
                gfx.DrawPixel(pos + new Vector2(-i, 0), Color4.White);
            }
            gfx.DrawLine(new Vector2(1280/2, 720/2), pos, Color4.Magenta);
        }

        public static void OnGameEnd() // Gets Executed after Game Window Closes
        {

        }
    }
}
