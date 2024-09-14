using OpenTK.Mathematics;

namespace SpriteX_Framework.FrameworkContents.Utilities
{
    public static class Colors
    {
        /// <summary>
        /// Lerps two Colors based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color4 Lerp(Color4 a, Color4 b, float alpha)
        {
            return new Color4(Numbers.Lerp(a.R, b.R, alpha), Numbers.Lerp(a.G, b.G, alpha), Numbers.Lerp(a.B, b.B, alpha), Numbers.Lerp(a.A, b.A, alpha));
        }

        /// <summary>
        /// Multiplies two Colors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color4 Multiply(Color4 a, Color4 b)
        {
            return new Color4(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);
        }

        /// <summary>
        /// Divides two Colors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color4 Divide(Color4 a, Color4 b)
        {
            return new Color4(a.R / b.R, a.G / b.G, a.B / b.B, a.A / b.A);
        }

        /// <summary>
        /// Adds two Colors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color4 Add(Color4 a, Color4 b)
        {
            return new Color4(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
        }

        /// <summary>
        /// Subtracts two Colors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color4 Subtract(Color4 a, Color4 b)
        {
            return new Color4(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
        }

        /// <summary>
        /// Returns an inverted color
        /// </summary>
        /// <param name="c"></param>
        /// <param name="invertAlpha"></param>
        /// <returns></returns>
        public static Color4 Invert(Color4 c, bool invertAlpha = false)
        {
            return new Color4(1 - c.R, 1 - c.G, 1 - c.B, invertAlpha ? 1 - c.A : c.A);
        }
    }

}
