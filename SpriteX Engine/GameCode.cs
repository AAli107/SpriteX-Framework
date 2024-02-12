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

namespace SpriteX_Engine
{
    class GameCode : GameScript
    {
        /* Insert Variables here */
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(200, 200), true, true, 0.1f, 1);
        GameObject otherG = new GameObject(new Vector2(-1920 / 2, -1080 / 2), new Vector2(1920 * 2, 100), true, false);
        GameObject otherG2 = new GameObject(new Vector2(-1920 / 2, 1080 * 1.5f), new Vector2(1920 * 2, 100), true, false);
        GameObject otherG3 = new GameObject(new Vector2(-1920 / 2, (-1080 / 2) + 100), new Vector2(100, (1080 * 2) - 100), true, false);
        GameObject otherG4 = new GameObject(new Vector2(1920 * 1.5f, (-1080 / 2)), new Vector2(100, (1080 * 2) + 100), true, false);
        GameObject otherGG = new GameObject(new Vector2(800, 600), new Vector2(300, 100), true, true, 0.1f);
        GameObject otherGG2 = new GameObject(new Vector2(200, 300), new Vector2(100, 100), true, true, 0.1f, 0.5f);
        GameObject otherGGG = new GameObject(new Vector2(1200, 600), new Vector2(200, 200), true, true, 0.1f, 10);
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
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    float brightness = Vec2D.Distance2D(new Vector2(i - ((1920 / 2) / 120), j - ((1080 / 2) / 120)), new Vector2((g.GetCenterPosition().X / 120), (g.GetCenterPosition().Y / 120))) / 5;
                    brightness = 1 - brightness; 
                    brightness = brightness > 1 ? 1 : brightness;
                    brightness = brightness <= 0.1f ? 0.1f : brightness;

                    Color4 c = new Color4(1f, 0.75f, 0.25f, 1f);

                    win.DrawImage(new Vector2(i * 120, j * 120) - g.GetCenterPosition(), new Vector2(120, 120), img2, new Color4(brightness * c.R, brightness * c.G, brightness * c.B, 1));
                }
            }
            //win.DrawTexturedQuad(new Vector2(150, 510), new Vector2(790, 660), new Vector2(790, 300), new Vector2(150, 150), img1);
            //
            //win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.SteelBlue);
            //win.DrawTri(new Vector2(1280, 720), new Vector2(0.0f, 0.0f), new Vector2(1000, 100), Color4.Red, Enums.DrawType.Outline);
            //win.DrawRect(new Vector2(250, 250), new Vector2(400, 100), Color4.Gold);
            //win.DrawLine(g.GetCenterPosition(), otherGG.GetCenterPosition(), Color4.Red, 5);
            //

            win.DrawImage(otherG.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherG.GetSize(), img2, Color4.Blue);
            win.DrawImage(otherG2.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherG2.GetSize(), img2, Color4.Blue);
            win.DrawImage(otherG3.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherG3.GetSize(), img2, Color4.Blue);
            win.DrawImage(otherG4.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherG4.GetSize(), img2, Color4.Blue);
            win.DrawImage(otherGG.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherGG.GetSize(), img2);
            win.DrawImage(otherGG2.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherGG2.GetSize(), img2);
            win.DrawImage(otherGGG.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), otherGGG.GetSize(), img2);
            win.DrawImage(new Vector2((1920/2)-(g.GetSize().X/2), (1080/2) - (g.GetSize().Y/2)), g.GetSize(), img2);

            //win.DrawRect(new Vector2(1200, 200), new Vector2(500, 500), new Color4(1f, 0.25f, 0.5f, 0.5f));

            win.DrawText(new Vector2(10, 10), Math.Round(win.FPS) + " FPS", Color4.Lime, 1);
            win.DrawText(new Vector2(10, 42), "time since start = " + Math.Round(win.time, 2) + " s", Color4.Cyan, 1);
            win.DrawText(new Vector2(10, 74), "-------------------------------------------", Color4.Yellow, 1);
            win.DrawText(new Vector2(10, 106), "Position = (" + Math.Round(g.GetCenterPosition().X, 2) + ", " + Math.Round(g.GetCenterPosition().Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 138), "Speed = " + Math.Round(g.GetVelocity().Length, 2), Color4.White, 1);
            win.DrawText(new Vector2(10, 170), "Count = " + GameObject.GetAllGameObjects().Count, Color4.White, 1);
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
