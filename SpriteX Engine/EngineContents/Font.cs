using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents
{
    public class Font
    {
        public string fontPath = "Resources/Engine/SpriteX_Font.png";
        public Vector2 charSize = new Vector2(16, 32);

        public static readonly char[] charSheet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ".ToCharArray();
        public static readonly Font defaultFont = new();

        public Font() { }

        public Font(string fontPath, Vector2 charSize) 
        {
            this.fontPath = fontPath;
            this.charSize = charSize;
        }
    }
}
