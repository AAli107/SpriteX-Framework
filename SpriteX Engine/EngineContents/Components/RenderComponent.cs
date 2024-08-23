﻿using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Components
{
    public class RenderComponent : Component
    {
        public bool isVisible = true;
        public Vector2[] vertex = { new Vector2(0, 0), new Vector2(100, 100) };
        public gfx.Shape shape = gfx.Shape.Rect;
        public gfx.DrawType drawType = gfx.DrawType.Filled;
        public Texture tex = Texture.GetPlainWhiteTexture();
        public Color4 color = Color4.White;
        public float size = 1;

        public RenderComponent(GameObject parent) : base(parent) { }

        public override void RenderTick(MainWindow win, gfx gfx)
        {
            base.RenderTick(win, gfx);
            if (!isVisible) return;
            Vector2[] v = vertex.ToArray();
            for (int i = 0; i < v.Length; i++)
            {
                if (shape == gfx.Shape.Rect && i == 1) continue;
                v[i] = v[i] + parent.GetPosition();
            }

            gfx.DrawShape(shape, v, color, tex, drawType, size, false);
        }
    }
}
