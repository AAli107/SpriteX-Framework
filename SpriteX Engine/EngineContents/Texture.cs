using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

#pragma warning disable CA1416

namespace SpriteX_Engine.EngineContents
{
    public struct Texture
    {
        private int textureId;
        public string path { get; private set; }

        public Texture(string filePath)
        {
            path = filePath;
            Bitmap image;
            try
            {
                // Loads the image using Bitmap
                image = new Bitmap(filePath);
            } 
            catch 
            {   // If Texture is missing, put this instead
                image = GetMissingBitmap();
            }

            // Generate a new texture ID
            textureId = GL.GenTexture();

            // Bind the texture and set its parameters
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, image.Width, image.Height, PixelFormat.Bgra, PixelType.UnsignedByte, BitmapData(image));

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public Texture()
        {
            path = "";
            // Load the image using Bitmap
            Bitmap image = new Bitmap(1, 1);
            image.SetPixel(0, 0, Color.White);

            // Generate a new texture ID
            textureId = GL.GenTexture();

            // Bind the texture and set its parameters
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Bgra, PixelType.Float, IntPtr.Zero);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, image.Width, image.Height, PixelFormat.Bgra, PixelType.UnsignedByte, BitmapData(image));

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Helper method to convert Bitmap to byte array
        private byte[] BitmapData(Bitmap image)
        {
            System.Drawing.Imaging.BitmapData bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] data = new byte[bmpData.Stride * bmpData.Height];
            Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
            image.UnlockBits(bmpData);
            return data;
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public int getTextureID()
        {
            return textureId;
        }

        public static Bitmap GetMissingBitmap()
        {
            Bitmap image = new Bitmap(2, 2);
            image.SetPixel(0, 0, Color.Magenta);
            image.SetPixel(1, 0, Color.Black);
            image.SetPixel(0, 1, Color.Black);
            image.SetPixel(1, 1, Color.Magenta);
            return image;
        }

        public static Texture GetMissingTexture() 
        {
            return new Texture("");
        }
    }
}
