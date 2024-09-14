using OpenTK.Mathematics;
using SpriteX_Framework.FrameworkContents;

namespace SpriteX_Framework
{
    public class SampleLevel : GameLevelScript // All classes with GameLevelScript as base class acts like a script for a level
    {
        /* Insert Level Variables here */

        public override void LevelStart(MainWindow win)
        {

        }

        public override void GameUpdate(MainWindow win)
        {

        }

        public override void GraphicsUpdate(MainWindow win, gfx gfx)
        {
            gfx.DrawText(new Vector2d(100, 100), "Hello World!", Color4.White);
        }
    }
}