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
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(200, 200), true, true, 0.1f, 1);
        GameObject otherG = new GameObject(new Vector2(800, 300), new Vector2(300, 100), true, false);
        GameObject otherGG = new GameObject(new Vector2(1000, 600), new Vector2(300, 100), true, true, 0.1f);
        GameObject otherGGG = new GameObject(new Vector2(1400, 600), new Vector2(200, 200), true, true, 0.1f, 10);
        float s;

        Texture img1;
        Texture img2;

        public override void OnGameStart(MainWindow win)
        {
            img1 = Texture.GetMissingTexture();
            img2 = new Texture("Resources/Textures/img.png");
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
            win.DrawTexturedQuad(new Vector2(150, 510), new Vector2(790, 660), new Vector2(790, 300), new Vector2(150, 150), img1);

            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.SteelBlue);
            win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.Red, DrawType.Outline);
            win.DrawRect(new Vector2(250, 250), new Vector2(400, 100), Color4.Gold);
            win.DrawLine(g.GetCenterPosition(), otherGG.GetCenterPosition(), Color4.Red, 5);

            win.DrawImage(g.GetPosition(), g.GetSize(), img2);

            win.DrawRect(new Vector2(1200, 200), new Vector2(500, 500), new Color4(1f, 0.25f, 0.5f, 0.5f));

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
