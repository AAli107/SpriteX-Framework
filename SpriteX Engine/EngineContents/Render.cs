using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents
{
    public struct Render
    {
        public bool isVisible = true;
        public Vector2[] vertex = { new Vector2(0, 0), new Vector2(100, 100) };
        public gfx.Shape shape = gfx.Shape.Rect;
        public gfx.DrawType drawType = gfx.DrawType.Filled;
        public Texture tex = Texture.GetPlainWhiteTexture();
        public Color4 color = Color4.White;
        public float size = 1;

        public Render() { }

        public Render(gfx.Shape shape, Vector2[] vertex, Texture tex, Color4 color)
        {
            Construct(shape, vertex, tex, color);
        }
        public Render(gfx.Shape shape, Vector2[] vertex, Texture tex)
        {
            Construct(shape, vertex, tex, Color4.White);
        }
        public Render(gfx.Shape shape, Vector2[] vertex, Color4 color)
        {
            Construct(shape, vertex, Texture.GetPlainWhiteTexture(), color);
        }

        private void Construct(gfx.Shape shape, Vector2[] vertex, Texture tex, Color4 color)
        {
            this.shape = shape;
            this.vertex = vertex;
            this.tex = tex;
            this.color = color;
        }

        public void DrawRender(gfx gfx, Vector2 pos)
        {
            if (!isVisible) return;
            Vector2[] v = vertex.ToArray();
            for (int i = 0; i < v.Length; i++)
            {
                if (shape == gfx.Shape.Rect && i == 1) continue;
                v[i] = v[i] + pos;
            }

            gfx.DrawShape(shape, v, color, tex, drawType, size, false);
        }
    }
}
