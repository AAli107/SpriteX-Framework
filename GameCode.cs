using SpriteX_Engine.EngineContents;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;

namespace SpriteX_Engine
{
    class GameCode // Here is where you make most of your game's code!
    {
         // Insert static Variables here \\
        // \/\/\/\/\/\/\/\/\/\/\/\/\/\/\/ \\
        public static Vector2 loc = new Vector2(100, 100);
        public static Vector2 width = new Vector2(100, 100);
        public static int dir = 1;
        public static float speed = 10;
        public static byte blue = 0;
        public static float deg = 0;
        // /\/\/\/\/\/\/\/\/\/\/\/\/\/\/\ \\

        public static void OnGameStart() // Gets Executed when game starts running/when the game begins
        {

        }

        public static void OnGameUpdate() // Gets Executed every frame as long as the game is running
        {
            blue += (byte)dir;
            if (blue >= 255 || blue <= 0)
                dir *= -1;

            if (Controller.IsKeyPressed(Keys.W)) loc.Y -= speed;
            if (Controller.IsKeyPressed(Keys.A)) loc.X -= speed;
            if (Controller.IsKeyPressed(Keys.S)) loc.Y += speed;
            if (Controller.IsKeyPressed(Keys.D)) loc.X += speed;

            loc.X = loc.X < 0 ? 0 : (loc.X > gfx.drawWidth - width.X ? gfx.drawWidth - width.X : loc.X);
            loc.Y = loc.Y < 0 ? 0 : (loc.Y > gfx.drawHeight - width.Y ? gfx.drawHeight - width.Y : loc.Y);

            deg+=10;
            deg = deg > 360 ? 0 : (deg < 0 ? 360 : deg);

        }

        public static void OnGraphicsUpdate() // Will be used to draw graphics related items per frame
        {
            gfx.DrawRectangle(0, 0, gfx.drawWidth, gfx.drawHeight, Color.White);
            gfx.DrawCircle(gfx.drawWidth / 2, gfx.drawHeight / 2, 200, Color.Red);

            gfx.DrawRectangle(loc.X, loc.Y, width.X, width.Y, Color.FromArgb((int)loc.X / 5, (int)(loc.Y / 2.8125f), (byte)Utilities.Numbers.ClampN(255 / ((loc.X / 255.0f) + 1), 0, 255)));

            if (Controller.IsMouseButtonPressed(MouseButtons.Left))
                gfx.DrawCircle(Controller.mousePos.X, Controller.mousePos.Y, 50, Color.FromArgb((byte)Utilities.Numbers.ClampN(Controller.mousePos.X / 5, 0, 255), (byte)Utilities.Numbers.ClampN(Controller.mousePos.Y / 2.8125f, 0, 255), (byte)Utilities.Numbers.ClampN(255 / ((Controller.mousePos.X / 255.0f) + 1), 0, 255)));
            if (Controller.IsMouseButtonPressed(MouseButtons.Right))
                gfx.DrawRectangle(Controller.mousePos.X, Controller.mousePos.Y - 75, 50, 25, Color.FromArgb((byte)Utilities.Numbers.ClampN(Controller.mousePos.X / 5, 0, 255), (byte)Utilities.Numbers.ClampN(Controller.mousePos.Y / 2.8125f, 0, 255), (byte)Utilities.Numbers.ClampN(255 / ((Controller.mousePos.X / 255.0f) + 1), 0, 255)));

            Vector2 circleLoc = Utilities.Vec2D.RotateAroundPoint(Controller.mousePos + new Vector2(50, 0), Controller.mousePos, deg);
            gfx.DrawCircle(circleLoc.X, circleLoc.Y, 10, Color.Black);

            gfx.GameUI.DrawProgressBar(10, 300, 100, 400, Color.Blue, Color.DarkBlue, blue/255.0f, false);
            gfx.GameUI.DrawText(10, 280, (blue / 255.0f).ToString(), new Font(FontFamily.GenericMonospace, 12), Color.Red);
            gfx.GameUI.DrawText(10, 20, Controller.mousePos.ToString(), new Font(FontFamily.GenericMonospace, 12), Color.Cyan);
            gfx.GameUI.DrawText(10, 40, Controller.pressedkeys.Count.ToString(), new Font(FontFamily.GenericMonospace, 12), Color.Black);
        }

        public static void OnGameEnd() // Gets Executed when Engine.gameRunning = false which is basically when the game ends
        {

        }
    }
}
