using OpenTK.Mathematics;
using System;

namespace SpriteX_Framework.FrameworkContents.Utilities
{

    public static class Vec2D
    {
        /// <summary>
        /// Returns the center between two 2D vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Vector2d Midpoint2D(Vector2d point1, Vector2d point2)
        {
            return (point1 + point2) * 0.5;
        }

        /// <summary>
        /// Returns the position of a point after being rotated by a given degrees around a given point
        /// </summary>
        /// <param name="pointToRotate"></param>
        /// <param name="centerPoint"></param>
        /// <param name="degreesAngle"></param>
        /// <returns></returns>
        public static Vector2d RotateAroundPoint(Vector2d pointToRotate, Vector2d centerPoint, double degreesAngle)
        {
            double radianAngle = Numbers.DegreeToRad(degreesAngle);
            double cosTheta = Math.Cos(radianAngle);
            double sinTheta = Math.Sin(radianAngle);

            return new Vector2d((cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X), (sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y));
        }

        /// <summary>
        /// Converts the Cardinal Directions into a normalized vector 2D
        /// </summary>
        /// <param name="cardinalDirection"></param>
        /// <returns></returns>
        public static Vector2d DirectionToVec2D(Enums.CardinalDirection cardinalDirection)
        {
            return cardinalDirection switch
            {
                Enums.CardinalDirection.North => new Vector2d(0, -1).Normalized(),
                Enums.CardinalDirection.NorthEast => new Vector2d(1, -1).Normalized(),
                Enums.CardinalDirection.East => new Vector2d(1, 0).Normalized(),
                Enums.CardinalDirection.SouthEast => new Vector2d(1, 1).Normalized(),
                Enums.CardinalDirection.South => new Vector2d(0, 1).Normalized(),
                Enums.CardinalDirection.SouthWest => new Vector2d(-1, 1).Normalized(),
                Enums.CardinalDirection.West => new Vector2d(-1, 0).Normalized(),
                Enums.CardinalDirection.NorthWest => new Vector2d(-1, -1).Normalized(),
                _ => new (0, 0),
            };
        }

        /// <summary>
        /// Lerps two Vector2s based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector2d Lerp(Vector2d a, Vector2d b, double alpha)
        {
            return new Vector2d(Numbers.LerpD(a.X, b.X, alpha), Numbers.LerpD(a.Y, b.Y, alpha));
        }
    }

}
