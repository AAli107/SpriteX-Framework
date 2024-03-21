using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Structs
{
    public struct Transform
    {
        public Vector2 position = new Vector2(0, 0);
        public Vector2 scale = new Vector2(1, 1);

        public Transform() { }
        public Transform(Vector2 position, Vector2 scale)
        {
            this.position = position;
            this.scale = scale;
        }
    }
}
