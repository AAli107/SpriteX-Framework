namespace SpriteX_Engine.EngineContents.Utilities
{
    public static class Numbers
    {
        /// <summary>
        /// calculates the distance between two single numbers
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static float Distance1D(float num1, float num2)
        {
            return MathF.Sqrt(MathF.Pow(num1 - num2, 2));
        }

        /// <summary>
        /// Will return the average of numbers inside an array
        /// </summary>
        /// <param name="floatArray"></param>
        /// <returns></returns>
        public static float AverageNum(float[] floatArray)
        {
            float value = 0.0f;
            for (int i = 0; i < floatArray.Length; i++)
                value += floatArray[i];

            return value / floatArray.Length;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static float DegreeToRad(float degree)
        {
            return (degree * MathF.PI) / 180.0f;
        }

        /// <summary>
        /// Converts radian to degrees
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static float RadToDegree(float radian)
        {
            return (radian * 180.0f) / MathF.PI;
        }

        /// <summary>
        /// Fixes the value number between the minimum and maximum
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampN(float val, float min, float max)
        {
            return val > max ? max : val < min ? min : val;
        }
        
        /// <summary>
        /// Fixes the value number between the minimum and maximum in double
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double ClampND(double val, double min, double max)
        {
            return val > max ? max : val < min ? min : val;
        }

        /// <summary>
        /// Returns the largest number in an array of floats
        /// </summary>
        /// <param name="floatArray"></param>
        /// <returns></returns>
        public static float MaxVal(float[] floatArray)
        {
            float max = 0;
            for (int i = 0; i < floatArray.Length; i++)
                if (floatArray[i] > max)
                {
                    max = floatArray[i];
                }

            return max;
        }

        /// <summary>
        /// Returns the smallest number in an array of floats
        /// </summary>
        /// <param name="floatArray"></param>
        /// <returns></returns>
        public static float MinVal(float[] floatArray)
        {
            float min = floatArray[0];
            for (int i = 0; i < floatArray.Length; i++)
                if (floatArray[i] < min)
                {
                    min = floatArray[i];
                }

            return min;
        }

        /// <summary>
        /// Lerps two numbers based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }
    }

}
