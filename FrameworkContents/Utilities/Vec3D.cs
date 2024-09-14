using OpenTK.Mathematics;
using System;

namespace SpriteX_Framework.FrameworkContents.Utilities
{

    public static class Vec3D
    {
        /// <summary>
        /// Calculates the 3D distance between two points/vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double Distance3D(Vector3d point1, Vector3d point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2) + Math.Pow(point2.Z - point1.Z, 2));
        }

        /// <summary>
        /// Returns the center between two 3D vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Vector3d Midpoint3D(Vector3d point1, Vector3d point2)
        {
            return (point1 + point2) * 0.5;
        }

        /// <summary>
        /// Converts 3D Vectors into 2D vectors with depth (Can be used to render simple 3D graphics)
        /// </summary>
        /// <param name="vec3D"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static Vector2d Vec3DToVec2D(Vector3d vec3D, double depth = 100)
        {
            return new Vector2d((vec3D.X * (depth / vec3D.Z)) + 960, (vec3D.Y * (depth / vec3D.Z)) + 540);
        }

        /// <summary>
        /// Lerps two Vector3s based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector3d Lerp(Vector3d a, Vector3d b, double alpha)
        {
            return new Vector3d(Numbers.LerpD(a.X, b.X, alpha), Numbers.LerpD(a.Y, b.Y, alpha), Numbers.LerpD(a.Z, b.Z, alpha));
        }
    }

}
