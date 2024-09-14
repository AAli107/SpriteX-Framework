using OpenTK.Mathematics;
using System;

namespace SpriteX_Engine.EngineContents.Utilities
{

    public static class Vec3D
    {
        /// <summary>
        /// Calculates the 3D distance between two points/vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static float Distance3D(Vector3 point1, Vector3 point2)
        {
            return MathF.Sqrt(MathF.Pow(point2.X - point1.X, 2) + MathF.Pow(point2.Y - point1.Y, 2) + System.MathF.Pow(point2.Z - point1.Z, 2));
        }

        /// <summary>
        /// Returns the center between two 3D vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Vector3 Midpoint3D(Vector3 point1, Vector3 point2)
        {
            return (point1 + point2) * 0.5f;
        }

        /// <summary>
        /// Converts 3D Vectors into 2D vectors (Can be used to render simple 3D graphics)
        /// </summary>
        /// <param name="vec3D"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Vector2 Vec3DToVec2D(Vector3 vec3D, float depth = 100)
        {
            return new Vector2((vec3D.X * (depth / vec3D.Z)) + 960, (vec3D.Y * (depth / vec3D.Z)) + 540);
        }

        /// <summary>
        /// Lerps two Vector3s based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector3 Lerp(Vector3 a, Vector3 b, float alpha)
        {
            return new Vector3(Numbers.Lerp(a.X, b.X, alpha), Numbers.Lerp(a.Y, b.Y, alpha), Numbers.Lerp(a.Z, b.Z, alpha));
        }
    }

}
