using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SpriteX_Engine.EngineContents;
using static SpriteX_Engine.EngineContents.Utilities;
using OpenTK.Windowing.Common;

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
        float s;

        Texture img1;
        Texture img2;

        Button btn;
        Button btn2;
        Button btn3;

        public static void btn_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            new GameObject(new Vector2(Rand.RangeFloat(-500, 500), Rand.RangeFloat(-500, 500)), new Vector2(100, 100), true, true, 0.1f, 0.5f);
        }

        public static void btn2_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            new GameObject(new Vector2(Rand.RangeFloat(-500, 500), Rand.RangeFloat(-500, 500)), new Vector2(300, 100), true, true, 0.1f);
        }

        public static void btn3_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            new GameObject(new Vector2(Rand.RangeFloat(-500, 500), Rand.RangeFloat(-500, 500)), new Vector2(200, 200), true, true, 0.1f, 10f);
        }

        public override void OnGameStart(MainWindow win)
        {
            img1 = Texture.GetMissingTexture();
            img2 = new Texture("Resources/Textures/img.png");
            btn = new Button(new Vector2(1920 / 1.25f, 50), new Vector2(300, 100), Color4.Red);
            btn2 = new Button(new Vector2(1920 / 1.25f, 200), new Vector2(300, 100), Color4.Lime);
            btn3 = new Button(new Vector2(1920 / 1.25f, 350), new Vector2(300, 100), Color4.Blue);
            btn.OnButtonPressed += btn_ButtonPressed;
            btn2.OnButtonPressed += btn2_ButtonPressed;
            btn3.OnButtonPressed += btn3_ButtonPressed;
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

            foreach (GameObject obj in GameObject.GetAllGameObjects())
            {
                win.DrawImage(obj.GetPosition() - g.GetCenterPosition() + new Vector2(1920 / 2, 1080 / 2), obj.GetSize(), img2, obj.IsSimulatingPhysics() ? Color4.White : Color4.Blue);
            }

            win.DrawText(new Vector2(10, 10), Math.Round(win.FPS) + " FPS", Color4.Lime, 1);
            win.DrawText(new Vector2(10, 42), "time since start = " + Math.Round(win.time, 2) + " s", Color4.Cyan, 1);
            win.DrawText(new Vector2(10, 74), "-------------------------------------------", Color4.Yellow, 1);
            win.DrawText(new Vector2(10, 106), "Mouse Pos = (" + Math.Round(win.MousePosition.X, 2) + ", " + Math.Round(win.MousePosition.Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 138), "Game Pos = (" + Math.Round(win.mouseGamePos.X, 2) + ", " + Math.Round(win.mouseGamePos.Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 170), "Count = " + GameObject.GetAllGameObjects().Count, Color4.White, 1);
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
