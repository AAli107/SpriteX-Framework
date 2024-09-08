using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents.Utilities
{

    public static class Vec2D
    {
        /// <summary>
        /// Returns the center between two 2D vectors
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Vector2d Midpoint2D(Vector2 point1, Vector2 point2)
        {
            return (point1 + point2) * 0.5f;
        }

        /// <summary>
        /// Returns the position of a point after being rotated by a given degrees around a given point
        /// </summary>
        /// <param name="pointToRotate"></param>
        /// <param name="centerPoint"></param>
        /// <param name="degreesAngle"></param>
        /// <returns></returns>
        public static Vector2 RotateAroundPoint(Vector2 pointToRotate, Vector2 centerPoint, float degreesAngle)
        {
            float radianAngle = Numbers.DegreeToRad(degreesAngle); // Converts Degrees angle to Radian angle
            float cosTheta = MathF.Cos(radianAngle);
            float sinTheta = MathF.Sin(radianAngle);

            return new Vector2((cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X), (sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y));
        }

        /// <summary>
        /// Converts the Cardinal Directions into a normalized vector 2D
        /// </summary>
        /// <param name="cardinalDirection"></param>
        /// <returns></returns>
        public static Vector2 DirectionToVec2D(Enums.CardinalDirection cardinalDirection)
        {
            switch (cardinalDirection)
            {
                case Enums.CardinalDirection.North:
                    return new Vector2(0, -1);
                case Enums.CardinalDirection.NorthEast:
                    return new Vector2(1, -1);
                case Enums.CardinalDirection.East:
                    return new Vector2(1, 0);
                case Enums.CardinalDirection.SouthEast:
                    return new Vector2(1, 1);
                case Enums.CardinalDirection.South:
                    return new Vector2(0, 1);
                case Enums.CardinalDirection.SouthWest:
                    return new Vector2(-1, 1);
                case Enums.CardinalDirection.West:
                    return new Vector2(-1, 0);
                case Enums.CardinalDirection.NorthWest:
                    return new Vector2(-1, -1);
            }
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Lerps two Vector2s based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float alpha)
        {
            return new Vector2(Numbers.Lerp(a.X, b.X, alpha), Numbers.Lerp(a.Y, b.Y, alpha));
        }
    }

}
