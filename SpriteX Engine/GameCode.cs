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
        GameObject g = new GameObject(new Vector2(500, 100));
        GameObject gg = new GameObject(new Vector2(200, 400));
        GameObject ggg = new GameObject(new Vector2(1200, 400));
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
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080))));
            win.PlayAudio("Resources/Audio/sample_audio.wav");
        }

        public void btn2_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = (MainWindow)sender;
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080))));
            win.PlayAudio("Resources/Audio/sample_audio.wav");
        }

        public void btn3_ButtonPressed(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = (MainWindow)sender;
            win.world.SpawnGameObject(new GameObject(new Vector2(Rand.RangeFloat(0, 1920), Rand.RangeFloat(0, 1080))));
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
            win.world.SpawnGameObject(ggg);
            win.GetWorldCamera().SetEnableCameraBound(true);

            //PhysicsComponent pc2 = gg.AddComponent<PhysicsComponent>();
            //pc2.mass = 10f;
            //pc2.gravityEnabled = false;
            //pc2.isAirborne = false;
            ColliderComponent ccc = gg.AddComponent<ColliderComponent>();
            ccc.transform.scale = new Vector2(10, 1);
            ccc.friction = 0f;
            ColliderComponent ccc2 = ggg.AddComponent<ColliderComponent>();
            ccc2.transform.scale = new Vector2(10, 1);
            ccc2.friction = 0.1f;

            pc = g.AddComponent<PhysicsComponent>();
            pc.mass = 10f;
            //pc.gravityEnabled = false;
            //pc.isAirborne = false;

            ColliderComponent cc = g.AddComponent<ColliderComponent>();
            ColliderComponent cc2 = g.AddComponent<ColliderComponent>();
            cc2.transform.position = new Vector2(60, 80);
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
                    float b = Vec2D.Distance2D(new Vector2(i - ((1920 / 2) / 120), j - ((1080 / 2) / 120)), new Vector2(((g.GetPosition().X - 1920/2) / 120), ((g.GetPosition().Y - 1080 / 2) / 120))) / 5;
                    b = 1 - b; 
                    b = b > 1 ? 1 : b;
                    b = b <= 0.1f ? 0.1f : b;
                    Color4 c = Colors.Lerp(Color4.White, lightColor, b);
                    gfx.DrawImage(new Vector2(i * 120, j * 120), new Vector2(120, 120), img2, Colors.Multiply(new Color4(b, b, b, 1), c));
                }
            }
            gfx.DrawText(new Vector2(200, 300), "Slippery Floor", Color4.Cyan, 1, false);
            gfx.DrawText(new Vector2(1200, 300), "Normal Floor", Color4.Yellow, 1, false);
            foreach (GameObject obj in win.world.GetAllGameObjects())
            {
                float b = Vec2D.Distance2D(obj.GetPosition() / new Vector2(1920/2, 1080/2), g.GetPosition() / new Vector2(1920 / 2, 1080 / 2)) * 1.45f;
                b = 1 - b;
                b += 0.25f;
                b = b <= 0.1f ? 0.1f : b;
                b = b > 1 ? 1 : b;
                Color4 c = Colors.Lerp(Color4.White, lightColor, b);
                ColliderComponent cc = obj.GetComponent<ColliderComponent>();
                Vector2 s = cc != null ? cc.transform.scale * 50 : new Vector2(50, 50);
                gfx.DrawImage(obj.GetPosition() - s, s*2, img1, Colors.Multiply(new Color4(b, b, b, 1), c));
            }

            gfx.DrawText(new Vector2(16, 80), "Cam Pos = (" + Math.Round(win.world.cam.camPos.X, 2) + ", " + Math.Round(win.world.cam.camPos.Y, 2) + ")", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 112), "Obj Pos = (" + Math.Round(g.GetPosition().X, 2) + ", " + Math.Round(g.GetPosition().Y, 2) + ")", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 144), "Obj Speed = " + Math.Round(pc.velocity.Length * win.fixedFrameTime, 2) + "u/s", Color4.White, 1);
            gfx.DrawText(new Vector2(16, 176), "Count = " + win.world.audios.Count, Color4.White, 1);
        }

        public override void OnGameEnd(MainWindow win)
        {

        }
    }
}
