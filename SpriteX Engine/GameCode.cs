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
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(150, 150), true, true, 0.1f);

        float s;

        Texture img1;
        Texture img2;

        Button btn;
        Button btn2;
        Button btn3;

        public void btn_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)sender).world.InstantiateGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(100, 100), true, true, 0.1f, 1f));
        }

        public void btn2_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)sender).world.InstantiateGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(150, 150), true, true, 0.1f));
        }

        public void btn3_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)sender).world.InstantiateGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(200, 200), true, true, 0.1f, 20f));
        }

        public override void OnGameStart(MainWindow win)
        {
            img1 = new Texture("Resources/Textures/img.png");
            img2 = Texture.GetMissingTexture();
            btn = new Button(new Vector2(1920 / 1.25f, 50), new Vector2(300, 100), Color4.Red);
            btn2 = new Button(new Vector2(1920 / 1.25f, 200), new Vector2(300, 100), Color4.Lime);
            btn3 = new Button(new Vector2(1920 / 1.25f, 350), new Vector2(300, 100), Color4.Blue);
            btn.OnButtonPressed += btn_ButtonPressed;
            btn2.OnButtonPressed += btn2_ButtonPressed;
            btn3.OnButtonPressed += btn3_ButtonPressed;
            win.world.InstantiateGameObject(g);
            win.GetWorldCamera().SetEnableCameraBound(true);
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
            win.world.cam.SetCameraPosition(g.GetCenterPosition());
        }

        public override void OnGameUpdateNoPause(MainWindow win)
        {
            if (win.IsKeyPressed(Keys.Escape)) win.Close();
            if (win.IsKeyPressed(Keys.P)) win.isGamePaused = !win.isGamePaused;
        }

        public override void OnGraphicsUpdate(MainWindow win)
        {
            //for (int i = 0; i < 32; i++)
            //{
            //    for (int j = 0; j < 18; j++)
            //    {
            //        float brightness = Vec2D.Distance2D(new Vector2(i - ((1920 / 2) / 120), j - ((1080 / 2) / 120)), new Vector2((g.GetCenterPosition().X / 120), (g.GetCenterPosition().Y / 120))) / 5;
            //        brightness = 1 - brightness; 
            //        brightness = brightness > 1 ? 1 : brightness;
            //        brightness = brightness <= 0.1f ? 0.1f : brightness;
            //
            //        Color4 c = new Color4(1f, 0.75f, 0.25f, 1f);
            //
            //        win.DrawImage(new Vector2(i * 120, j * 120) - g.GetCenterPosition(), new Vector2(120, 120), img1, new Color4(brightness * c.R, brightness * c.G, brightness * c.B, 1));
            //    }
            //}

            foreach (GameObject obj in win.world.GetAllGameObjects())
            {
                win.DrawImage(obj.GetPosition(), obj.GetSize(), img2, obj.IsSimulatingPhysics() ? Color4.White : Color4.Blue);
            }

            win.DrawText(new Vector2(10, 10), Math.Round(win.FPS) + " FPS", Color4.Lime, 1);
            win.DrawText(new Vector2(10, 42), "time since start = " + Math.Round(win.time, 2) + " s", Color4.Cyan, 1);
            win.DrawText(new Vector2(10, 74), "-------------------------------------------", Color4.Yellow, 1);
            win.DrawText(new Vector2(10, 106), "Cam Pos = (" + Math.Round(win.world.cam.camPos.X, 2) + ", " + Math.Round(win.world.cam.camPos.Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 138), "Obj Pos = (" + Math.Round(g.GetCenterPosition().X, 2) + ", " + Math.Round(g.GetCenterPosition().Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 170), "Game Pos = (" + Math.Round(win.mouseGamePos.X, 2) + ", " + Math.Round(win.mouseGamePos.Y, 2) + ")", Color4.White, 1);
            win.DrawText(new Vector2(10, 202), "Count = " + win.world.GetAllGameObjects().Count, Color4.White, 1);
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
