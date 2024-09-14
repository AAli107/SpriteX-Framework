namespace SpriteX_Framework.EngineContents.Structs
{
    public struct Constraint2D
    {
        public bool X = false;
        public bool Y = false;
        public Constraint2D() { }
        public Constraint2D(bool X, bool Y)
        { this.X = X; this.Y = Y; }
    }
}
