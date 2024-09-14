namespace SpriteX_Engine.EngineContents.Utilities
{

    public static class Rand
    {
        static readonly System.Random rng = new(); // Variable that stores the Random Class

        /// <summary>
        /// Returns a random int in range (inclusive)
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RangeInt(int min, int max)
        {
            return rng.Next(min, max + 1);
        }

        /// <summary>
        /// Returns a random float in range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RangeFloat(float min, float max)
        {
            return (float)RangeDouble(min, max);
        }

        /// <summary>
        /// Returns random double in range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double RangeDouble(double min, double max)
        {
            return rng.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Returns a random bool, true or false
        /// </summary>
        /// <returns></returns>
        public static bool RandBool()
        {
            return rng.Next(0, 2) == 1;
        }

        /// <summary>
        /// returns true randomly based on the decimal chance it would do it
        /// </summary>
        /// <param name="chance"></param>
        /// <returns></returns>
        public static bool RandBoolByChance(float chance = 0.5f)
        {
            return chance >= rng.NextDouble();
        }

        /// <summary>
        /// Returns a random int between 0 to max
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandInt(int max)
        {
            return rng.Next(max + 1);
        }
    }

}
