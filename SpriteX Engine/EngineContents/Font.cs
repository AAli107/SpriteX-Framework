using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteX_Engine.EngineContents
{
    public class Font
    {
        public string fontPath = "Resources/Engine/SpriteX_Font.png";
        public Vector2 charSize = new Vector2(16, 32);

        public static List<char> charSheet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ".ToList();

        public Font() { }

        public Font(string fontPath, Vector2 charSize) 
        {
            this.fontPath = fontPath;
            this.charSize = charSize;
        }

        public static Font GetDefaultFont()
        {
            return new Font();
        }
    }
}
