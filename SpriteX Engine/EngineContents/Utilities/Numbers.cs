using System;

namespace SpriteX_Framework.EngineContents.Utilities
{
    public static class Numbers
    {
        /// <summary>
        /// calculates the distance between two single numbers
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static double Distance1D(double num1, double num2)
        {
            return Math.Sqrt(Math.Pow(num1 - num2, 2));
        }

        /// <summary>
        /// Will return the average of numbers inside an array
        /// </summary>
        /// <param name="floatArray"></param>
        /// <returns></returns>
        public static double AverageNum(double[] arr)
        {
            double value = 0;
            for (int i = 0; i < arr.Length; i++)
                value += arr[i];

            return value / arr.Length;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreeToRad(double degree)
        {
            return (degree * Math.PI) / 180.0f;
        }

        /// <summary>
        /// Converts radian to degrees
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadToDegree(double radian)
        {
            return (radian * 180.0f) / Math.PI;
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
        public static double MaxVal(double[] arr)
        {
            double max = 0;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] > max)
                    max = arr[i];
            return max;
        }

        /// <summary>
        /// Returns the smallest number in an array
        /// </summary>
        /// <param name="floatArray"></param>
        /// <returns></returns>
        public static double MinVal(double[] arr)
        {
            double min = arr[0];
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] < min)
                    min = arr[i];
            return min;
        }

        /// <summary>
        /// Lerps two floats based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        /// <summary>
        /// Lerps two doubles based on "alpha"
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static double LerpD(double a, double b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }
    }

}
