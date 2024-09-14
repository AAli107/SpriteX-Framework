namespace SpriteX_Framework.EngineContents.Utilities
{
    public static class Enums
    {
        /// <summary>
        /// Enumeration for most common damage types
        /// </summary>
        public enum DamageType // You could add or remove damage types if necessary
        {
            None,
            Generic,
            Piercing,
            Falling,
            Burning,
            Magic,
            Drowning,
            Suffocation,
            Freezing,
            Bleeding,
            Explosion,
            Projectile,
            Lightning,
            Poisoning,
            Arrow,
            Prickling,
            Stabbing,
            Kinetic,
            Shooting,
            Curse,
            Acid,
            Rotting
        }

        /// <summary>
        /// Enumeration for the directions in 2D space
        /// </summary>
        public enum CardinalDirection
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        }
    }

}
