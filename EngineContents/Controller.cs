using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Forms;

namespace SpriteX_Engine.EngineContents
{
    public static class Controller
    {
        public static Vector2 mousePos = new Vector2(0, 0);
        public static List<MouseButtons> pressedMouseButtons = new List<MouseButtons>();
        public static List<Keys> pressedkeys = new List<Keys>();

        /// <summary>
        /// Checks whether if the specified mouse button is being pressed
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMouseButtonPressed(MouseButtons input)
        {
            for (int i = 0; i < pressedMouseButtons.Count; i++)
                if (pressedMouseButtons[i] == input)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks whether if the specified key is being pressed
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsKeyPressed(Keys input)
        {
            for (int i = 0; i < pressedkeys.Count; i++)
                if (pressedkeys[i] == input)
                    return true;
            return false;
        }
    }
}
