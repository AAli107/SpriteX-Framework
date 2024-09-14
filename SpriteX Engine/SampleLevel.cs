using OpenTK.Mathematics;
using SpriteX_Framework.FrameworkContents;

namespace SpriteX_Framework
{
    public class SampleLevel : GameLevelScript // All classes with GameLevelScript as base class acts like a script for a level
    {
        // LevelStart is called when the level begins
        public override void LevelStart(MainWindow win)
        {

        }

        // GameUpdate is called once per frame as long as the game is not paused
        public override void GameUpdate(MainWindow win)
        {

        }

        // GraphicsUpdate is called once per frame to render graphics
        public override void GraphicsUpdate(MainWindow win, gfx gfx)
        {
            gfx.DrawText(new Vector2d(100, 100), "Hello World!", Color4.White);
        }
    }
}