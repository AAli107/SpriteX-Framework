using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Structs
{
    public struct Transform
    {
        public Vector2d position = new (0, 0);
        public Vector2d scale = new (1, 1);

        public Transform() { }
        public Transform(Vector2d position, Vector2d scale)
        {
            this.position = position;
            this.scale = scale;
        }

        public override string ToString()
        {
            return "[ position=" + position + " / scale=" + scale + " ]";
        }
    }
}
