using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Audio.OpenAL.ALC;
using SpriteX_Engine.EngineContents;
using System.Runtime.CompilerServices;

namespace SpriteX_Engine
{
    class GameCode : GameScript
    {
        /* Insert Variables here */
        Vector2 pos = new Vector2(100, 100);
        Vector2 velocity = new Vector2(0, 0);
        float s;
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(40, 200), true);

        public override void OnGameStart(MainWindow win)
        {

        }

        public override void OnFixedGameUpdate(MainWindow win)
        {
            if (win.IsKeyDown(Keys.W)) velocity.Y -= s;
            if (win.IsKeyDown(Keys.A)) velocity.X -= s;
            if (win.IsKeyDown(Keys.S)) velocity.Y += s;
            if (win.IsKeyDown(Keys.D)) velocity.X += s;


            if (win.IsKeyDown(Keys.W)) g.AddVelocity(new Vector2(0, -s));

            pos.X += velocity.X;
            pos.Y += velocity.Y;

            velocity *= 0.9f;
        }

        public override void OnGameUpdate(MainWindow win)
        {
            s = win.IsKeyDown(Keys.LeftControl) ? 0.4f : (win.IsKeyDown(Keys.LeftShift) ? 1.6f : 0.8f);
        }

        public override void OnGameUpdateNoPause(MainWindow win)
        {
            if (win.IsKeyPressed(Keys.Escape)) win.Close();
            if (win.IsKeyPressed(Keys.P)) win.isGamePaused = !win.isGamePaused;
        }

        public override void OnGraphicsUpdate(MainWindow win)
        {
            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.SteelBlue);
            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.Red, Utilities.DrawType.Outline);
            win.DrawRect(new Vector2(250, 250), new Vector2(400, 100), Color4.Gold);
            win.DrawQuad(pos + new Vector2(0, -100), pos + new Vector2(100, 0), pos + new Vector2(0, 100), pos + new Vector2(-100, 0), Color4.Lime);
            win.DrawQuad(pos + new Vector2(0, -100), pos + new Vector2(100, 0), pos + new Vector2(0, 100), pos + new Vector2(-100, 0), Color4.Magenta, Utilities.DrawType.Outline);
            win.DrawLine(new Vector2(1920/2, 1080/2), pos, Color4.Magenta, 5);

            Console.WriteLine(Math.Round(win.FPS, 2) + " FPS                                       ");
            Console.WriteLine("time since start = " + Math.Round(win.GetTimeSinceStart(), 2) + " s                                       ");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Position = (" + Math.Round(pos.X, 2) + ", " + Math.Round(pos.Y, 2) + ")                                       ");
            Console.WriteLine("Speed = " + Math.Round(velocity.Length, 2) + "                                       ");
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.CursorVisible = false;
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
