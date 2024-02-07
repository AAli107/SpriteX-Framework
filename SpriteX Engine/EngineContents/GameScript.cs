using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteX_Engine.EngineContents
{
    internal class GameScript
    {
        public virtual void OnGameStart(MainWindow win) // Gets Executed when game starts running, after the main Window loads
        {

        }

        public virtual void OnGameUpdate(MainWindow win) // Gets Executed every frame, used for Game related actions
        {
            
        }

        public virtual void OnGraphicsUpdate(MainWindow win) // Used to put everything related to rendering stuff
        {
            
        }

        public virtual void OnGameEnd(MainWindow win) // Gets Executed when game is about to close
        {

        }
    }
}
