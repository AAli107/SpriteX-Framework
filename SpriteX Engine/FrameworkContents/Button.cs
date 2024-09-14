using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpriteX_Framework.FrameworkContents
{
    public class Button
    {
        public static List<Button> buttons = new List<Button>();

        public RectangleF buttonRect { get; private set; }
        public Color4 currentColor { get { return isPressed ? colorPressed : (isHovered ? colorHover : color); } }
        public Color4 color { get; set; }
        public Color4 colorHover { get; set; }
        public Color4 colorPressed { get; set; }
        public Texture tex { get; set; }
        public bool isHovered { get; set; }
        public bool isPressed { get; set; }
        public bool isVisible { get; set; }

        public event EventHandler<MouseButtonEventArgs> OnButtonPressed;

        public Button(Vector2 pos, Vector2 size, Color4 color, Texture tex, bool isVisible = true)
        {
            buttonRect = new RectangleF(new PointF(pos.X, pos.Y), new SizeF(size.X, size.Y));
            this.color = color;
            this.tex = tex;
            this.isVisible = isVisible;
            float intensity = 0.6f * color.R + 1.18f * color.G + 0.22f * color.B;
            float s = 0.4f;
            colorHover = new Color4(intensity * s + color.R * (1 - s), intensity * s + color.G * (1 - s), intensity * s + color.B * (1 - s), color.A);
            colorPressed = new Color4(color.R * 0.6f, color.G * 0.6f, color.B * 0.6f, color.A);
            buttons.Add(this);
        }

        public Button(Vector2 pos, Vector2 size, Color4 color, bool isVisible = true)
        {
            buttonRect = new RectangleF(new PointF(pos.X, pos.Y), new SizeF(size.X, size.Y));
            this.color = color;
            this.isVisible = isVisible;
            float intensity = 0.6f * color.R + 1.18f * color.G + 0.22f * color.B;
            float s = 0.4f;
            colorHover = new Color4(intensity * s + color.R * (1 - s), intensity * s + color.G * (1 - s), intensity * s + color.B * (1 - s), color.A);
            colorPressed = new Color4(color.R * 0.6f, color.G * 0.6f, color.B * 0.6f, color.A);
            tex = Texture.GetPlainWhiteTexture();
            buttons.Add(this);
        }

        public Button(Vector2 pos, Vector2 size, Texture tex, bool isVisible = true)
        {
            buttonRect = new RectangleF(new PointF(pos.X, pos.Y), new SizeF(size.X, size.Y));
            color = Color4.White;
            this.tex = tex;
            this.isVisible = isVisible;
            float intensity = 0.6f * color.R + 1.18f * color.G + 0.22f * color.B;
            float s = 0.4f;
            colorHover = new Color4(intensity * s + color.R * (1 - s), intensity * s + color.G * (1 - s), intensity * s + color.B * (1 - s), color.A);
            colorPressed = new Color4(color.R * 0.6f, color.G * 0.6f, color.B * 0.6f, color.A);
            buttons.Add(this);
        }

        public void InvokeButtonPress(MainWindow win, MouseButtonEventArgs e)
        {
            OnButtonPressed?.Invoke(win, e);
        }
    }
}
