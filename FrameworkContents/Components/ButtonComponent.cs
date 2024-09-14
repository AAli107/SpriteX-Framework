using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SpriteX_Framework.FrameworkContents.Structs;
using System;
using System.Drawing;

namespace SpriteX_Framework.FrameworkContents.Components
{
    public class ButtonComponent : Component
    {
        /// <summary>
        /// box collider transform, holds position and scale
        /// </summary>
        public Transform transform = new();
        public Box2d buttonRect { get; private set; }
        public Color4 currentColor { get { return IsPressed ? PressColor : IsHovered ? HoverColor : NormalColor; } }
        public Color4 NormalColor { get; set; }
        public Color4 HoverColor { get; set; }
        public Color4 PressColor { get; set; }
        public Texture Texture { get; set; }
        public bool IsHovered { get; set; }
        public bool IsPressed { get; set; }

        public event EventHandler<MouseButtonEventArgs> OnButtonPressed;

        public ButtonComponent(GameObject parent) : base(parent) 
        {
            NormalColor = Color.LightGray;
            float intensity = 0.6f * NormalColor.R + 1.18f * NormalColor.G + 0.22f * NormalColor.B;
            float s = 0.4f;
            HoverColor = new Color4(intensity * s + NormalColor.R * (1 - s), intensity * s + NormalColor.G * (1 - s), intensity * s + NormalColor.B * (1 - s), NormalColor.A);
            PressColor = new Color4(NormalColor.R * 0.6f, NormalColor.G * 0.6f, NormalColor.B * 0.6f, NormalColor.A);
            Texture = Texture.GetPlainWhiteTexture();
        }

        public override void RenderTick(MainWindow win, gfx gfx)
        {
            base.RenderTick(win, gfx);

            if (isEnabled) gfx.DrawImage(buttonRect.Center - buttonRect.HalfSize, buttonRect.Size, Texture, currentColor, true);
        }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            buttonRect = new Box2d(parent.GetPosition() + World.WorldInst.cam.camPos - (new Vector2d(-50, -50) * transform.scale) + transform.position, parent.GetPosition() + World.WorldInst.cam.camPos - (new Vector2d(50, 50) * transform.scale) + transform.position);
        }

        public void InvokeButtonPress(MainWindow win, MouseButtonEventArgs e)
        {
            OnButtonPressed?.Invoke(win, e);
        }
    }
}
