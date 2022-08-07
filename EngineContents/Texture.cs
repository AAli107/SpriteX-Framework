// If you want to draw an image/texture on screen, you need to place it in the same directory as the game's executable file
// Currently supports the following file formats: BMP, GIF, EXIF, JPG, PNG and TIFF

using System.Drawing;
using System.Numerics;
using System;

namespace SpriteX_Engine.EngineContents
{
    class Texture
    {
        private readonly string fileName; // Stores the Texture's file name
        private readonly Bitmap img; // loads the image in a variable
        public readonly Vector2 imageResolution;

        public Texture(string _fileName) // constructor for initializing the Texture class
        {
            fileName = _fileName; // Sets the file's name/path

            img = new Bitmap(fileName); // Assigning the Bitmap class into a variable with the fileName
            imageResolution = new Vector2(img.Width, img.Height); // Saves the Resolution of the image
        }

        /// <summary>
        /// Will draw the loaded texture file
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="scale"></param>
        public void DrawImage(float x, float y, float scale = 1)
        {
            gfx.graphics.DrawImage(img, x, y, imageResolution.X * scale, imageResolution.Y * scale);
        }

        /// <summary>
        /// Will draw the loaded texture file cropped based on the offset and size Vectors.
        /// </summary>
        /// <param name="imageLoc"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="blackTransparent"></param>
        /// <param name="scale"></param>
        /// <param name="isStatic"></param>
        public void DrawCroppedImage(Vector2 imageLoc, Vector2 offset, Vector2 size, float scale = 1)
        {
            // Loops between pixels
            //for (int i = 0; i < img.Width; i++)
            //{
            //    for (int j = 0; j < img.Height; j++)
            //    {
            //        // Draws the cropped Image only within the offset and size
            //        if ((i > offset.X && i < size.X + offset.X) && (j > offset.Y && j < size.Y + offset.Y))
            //            gfx.DrawRectangle(((int)(i * scale) - (int)offset.X) + (int)imageLoc.X, ((int)(j * scale) - (int)offset.Y) + (int)imageLoc.Y, (int)Math.Ceiling(scale), (int)Math.Ceiling(scale), img.GetPixel(i, j)); // Draws the image
            //    }
            //}

            gfx.graphics.DrawImage(img, new RectangleF(imageLoc.X, imageLoc.Y, imageResolution.X * scale, imageResolution.Y * scale), new RectangleF(offset.X, offset.Y, size.X, size.Y), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Gets the color of all pixels from the texture
        /// </summary>
        /// <returns></returns>
        public Color[] GetColorData()
        {
            Color[] Data = new Color[img.Width * img.Height];

            for (int i = 0; i < img.Width; i++)
                for (int j = 0; j < img.Height; j++)
                    Data[img.Width * j + i] = img.GetPixel(i, j); // Returns an array of colors from the loaded Bitmap

            return Data;
        }

        /// <summary>
        /// Gets the Color at a chosen pixel
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetColorOfPixel(int x, int y)
        {
            return img.GetPixel(x, y);
        }
    }
}
