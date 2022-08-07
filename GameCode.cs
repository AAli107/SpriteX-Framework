using SpriteX_Engine.EngineContents;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Drawing;

namespace SpriteX_Engine
{
    class GameCode // Here is where you make most of your game's code!
    {
         // Insert static Variables here \\
        // \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/ \\
        public static Vector2 loc = new Vector2(0, 0);
        public static Vector2 width = new Vector2(100, 100);
        public static Vector3 dir = new Vector3(20, 20, 1);
        public static byte blue = 0;
        // /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\ \\

        public static void OnGameStart() // Gets Executed when game starts running/when the game begins
        {

        }

        public static void OnGameUpdate() // Gets Executed every frame as long as the game is running
        {
            loc.X += dir.X;
            loc.Y += dir.Y;
            blue += (byte)dir.Z;

            if (loc.X >= gfx.drawWidth - width.X || loc.X <= 0)
                dir.X *= -1;
            if (loc.Y >= gfx.drawHeight - width.Y || loc.Y <= 0)
                dir.Y *= -1;
            if (blue >= 255 || blue <= 0)
                dir.Z *= -1;

        }

        public static void OnGraphicsUpdate() // Will be used to draw graphics related items per frame
        {
            gfx.DrawRectangle(loc.X, loc.Y, width.X, width.Y, Color.FromArgb((int)loc.X / 5, (int)(loc.Y / 2.8125f), blue));

            gfx.GameUI.DrawProgressBar(10, 300, 100, 400, Color.Blue, Color.DarkBlue, blue/255.0f, false);
            gfx.GameUI.DrawText(10, 280, (blue/255.0f).ToString(), new Font(FontFamily.GenericMonospace, 12), Color.Red);
        }

        public static void OnGameEnd() // Gets Executed when Engine.gameRunning = false which is basically when the game ends
        {

        }
    }
}
