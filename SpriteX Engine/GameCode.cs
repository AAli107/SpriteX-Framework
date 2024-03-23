using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SpriteX_Engine.EngineContents;
using SpriteX_Engine.EngineContents.Components;
using SpriteX_Engine.EngineContents.Utilities;
using OpenTK.Windowing.Common;

namespace SpriteX_Engine
{
    public class GameCode : GameLevelScript // All classes with GameLevelScript as base class acts like a script for a level
    {
        /* Insert Variables here */
        GameObject g = new GameObject(new Vector2(500, 400), new Vector2(150, 150), true, true, 0.1f);
        GameObject gg = new GameObject(new Vector2(200, 400), new Vector2(150, 150), true, true, 0.1f);
        PhysicsComponent pc;

        float s;

        Texture img1;
        Texture img2;

        Button btn;
        Button btn2;
        Button btn3;

        public void btn_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = (MainWindow)sender;
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(100, 100), true, true, 0.1f, 1f));
            win.PlayAudio("Resources/Audio/sample_audio.wav");
        }

        public void btn2_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = (MainWindow)sender;
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(150, 150), true, true, 0.1f));
            win.PlayAudio("Resources/Audio/sample_audio.wav");
        }

        public void btn3_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = (MainWindow)sender;
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080)), new Vector2(200, 200), true, true, 0.1f, 20f));
            win.PlayAudio("Resources/Audio/sample_audio.wav");
        }

        public override void OnGameStart(MainWindow win)
        {
            img1 = new Texture("Resources/Textures/sample_texture.png");
            img2 = Texture.GetMissingTexture();
            btn = new Button(new Vector2(1920 / 1.25f, 50), new Vector2(300, 100), Color4.Red);
            btn2 = new Button(new Vector2(1920 / 1.25f, 200), new Vector2(300, 100), Color4.Lime);
            btn3 = new Button(new Vector2(1920 / 1.25f, 350), new Vector2(300, 100), Color4.Blue);
            btn.OnButtonPressed += btn_ButtonPressed;
            btn2.OnButtonPressed += btn2_ButtonPressed;
            btn3.OnButtonPressed += btn3_ButtonPressed;
            win.world.SpawnGameObject(g);
            win.world.SpawnGameObject(gg);
            win.GetWorldCamera().SetEnableCameraBound(true);

            //(gg.AddComponent<PhysicsComponent>() as PhysicsComponent).gravityEnabled = false;
            gg.AddComponent<ColliderComponent>();

            pc = g.AddComponent<PhysicsComponent>() as PhysicsComponent;
            pc.gravityEnabled = false;

            ColliderComponent cc = g.AddComponent<ColliderComponent>() as ColliderComponent;
        }

        public override void OnFixedGameUpdate(MainWindow win)
        {
            if (pc != null)
            {
                if (win.IsKeyDown(Keys.W)) pc.AddVelocity(new Vector2(0, -s));
                if (win.IsKeyDown(Keys.A)) pc.AddVelocity(new Vector2(-s, 0));
                if (win.IsKeyDown(Keys.S)) pc.AddVelocity(new Vector2(0, +s));
                if (win.IsKeyDown(Keys.D)) pc.AddVelocity(new Vector2(+s, 0));
            }
        }

        public override void OnGameUpdate(MainWindow win)
        {
            if (!win.world.DoesGameObjectExist(g.GetID())) win.world.SpawnGameObject(g);
            s = win.IsKeyDown(Keys.LeftControl) ? 0.4f : (win.IsKeyDown(Keys.LeftShift) ? 1.6f : 0.8f);
            win.world.cam.SetCameraPosition(g.GetPosition());
        }

        public override void OnGameUpdateNoPause(MainWindow win)
        {
            if (win.IsKeyPressed(Keys.Escape)) win.Close();
            if (win.IsKeyPressed(Keys.P)) win.isGamePaused = !win.isGamePaused;
            if (win.IsKeyPressed(Keys.N)) win.LoadLevel(new GameCode());
        }

        public override void OnGraphicsUpdate(MainWindow win, gfx gfx)
        {
            Color4 lightColor = new Color4(1f, 0.75f, 0.15f, 1f);
            for (int i = -8; i < 24; i++)
            {
                for (int j = -5; j < 14; j++)
                {
                    float b = Vec2D.Distance2D(new Vector2(i - ((1920 / 2) / 120), j - ((1080 / 2) / 120)), new Vector2(((g.GetCenterPosition().X - 1920/2) / 120), ((g.GetCenterPosition().Y - 1080 / 2) / 120))) / 5;
                    b = 1 - b; 
                    b = b > 1 ? 1 : b;
                    b = b <= 0.1f ? 0.1f : b;
                    Color4 c = Colors.Lerp(Color4.White, lightColor, b);
                    gfx.DrawImage(new Vector2(i * 120, j * 120), new Vector2(120, 120), img2, Colors.Multiply(new Color4(b, b, b, 1), c));
                }
            }
            gfx.DrawText(new Vector2(500, 600), "ZA WARUDO!", Color4.Yellow, 1, false);
            foreach (GameObject obj in win.world.GetAllGameObjects())
            {
                float b = Vec2D.Distance2D(obj.GetCenterPosition() / new Vector2(1920/2, 1080/2), g.GetCenterPosition() / new Vector2(1920 / 2, 1080 / 2)) * 1.45f;
                b = 1 - b;
                b += 0.25f;
                b = b <= 0.1f ? 0.1f : b;
                b = b > 1 ? 1 : b;
                Color4 c = Colors.Lerp(Color4.White, lightColor, b);
                gfx.DrawImage(obj.GetPosition(), obj.GetSize(), img1, Colors.Multiply(new Color4(b, b, b, 1), c));
            }

            gfx.DrawText(new Vector2(16, 80), "Cam Pos = (" + Math.Round(win.world.cam.camPos.X, 2) + ", " + Math.Round(win.world.cam.camPos.Y, 2) + ")", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 112), "Obj Pos = (" + Math.Round(g.GetCenterPosition().X, 2) + ", " + Math.Round(g.GetCenterPosition().Y, 2) + ")", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 144), "World Mouse Pos = (" + Math.Round(win.mouseWorldPos.X, 2) + ", " + Math.Round(win.mouseWorldPos.Y, 2) + ")", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 176), "Count = " + win.world.audios.Count, Color4.White, 1);
            gfx.DrawText(new Vector2(16, 256), (g.GetComponent<ColliderComponent>() as ColliderComponent).GetHalfSize().ToString(), Color4.White, 1);
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
