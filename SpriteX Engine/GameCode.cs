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
using static SpriteX_Engine.EngineContents.Utilities;
using System.Drawing;

namespace SpriteX_Engine
{
    class GameCode : GameScript
    {
        /* Insert Variables here */
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(200, 200), true, true);
        GameObject otherG = new GameObject(new Vector2(800, 300), new Vector2(300, 100), true, false);
        GameObject otherGG = new GameObject(new Vector2(1000, 600), new Vector2(300, 100), true, true);
        float s;

        Texture img1;
        Texture img2;

        public override void OnGameStart(MainWindow win)
        {
            img1 = new Texture("img.png");
            img2 = new Texture("afton.png");
        }

        public override void OnFixedGameUpdate(MainWindow win)
        {
            if (win.IsKeyDown(Keys.W)) g.AddVelocity(new Vector2(0, -s));
            if (win.IsKeyDown(Keys.A)) g.AddVelocity(new Vector2(-s, 0));
            if (win.IsKeyDown(Keys.S)) g.AddVelocity(new Vector2(0, +s));
            if (win.IsKeyDown(Keys.D)) g.AddVelocity(new Vector2(+s, 0));
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
            //win.DrawTexturedQuad(new Vector2(100, 100), new Vector2(1380, 400), new Vector2(1380, 1120), new Vector2(100, 820), img1);
            win.DrawTexturedQuad(new Vector2(100, 820), new Vector2(1380, 1120), new Vector2(1380, 400), new Vector2(100, 100), img1);

            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.SteelBlue);
            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.Red, Utilities.DrawType.Outline);
            win.DrawRect(new Vector2(250, 250), new Vector2(400, 100), Color4.Gold);
            win.DrawLine(new Vector2(1920/2, 1080/2), g.GetCenterPosition(), Color4.Magenta, 5);

            win.DrawImage(g.GetPosition(), g.GetSize(), img2);

            Console.WriteLine(Math.Round(win.FPS, 2) + " FPS                                       ");
            Console.WriteLine("time since start = " + Math.Round(win.time, 2) + " s                                       ");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Position = (" + Math.Round(g.GetCenterPosition().X, 2) + ", " + Math.Round(g.GetCenterPosition().Y, 2) + ")                                       ");
            Console.WriteLine("Speed = " + Math.Round(g.GetVelocity().Length, 2) + "                                       ");
            Console.WriteLine("Count = " + GameObject.GetAllGameObjects().Count + "                                       ");
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.CursorVisible = false;
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
