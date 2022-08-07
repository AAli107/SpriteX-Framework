using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpriteX_Engine.EngineContents
{
    static class gfx
    {
        // Resolution width and height of the screen
        public static int drawWidth = (int)Engine.resolution.X;
        public static int drawHeight = (int)Engine.resolution.Y;

        public static Bitmap frameBuffer = new Bitmap(drawWidth, drawHeight, PixelFormat.Format24bppRgb);
        public static Bitmap imageOnScreen;

        public static Graphics graphics = Graphics.FromImage(frameBuffer);

        public static void ResetBuffer()
        {
            DrawRectangle(0, 0, drawWidth, drawHeight, Color.Black);
        }


        public static void DrawPixel(int x, int y, Color color)
        {
            graphics.FillRectangle(new SolidBrush(color), x, y, 1, 1);
        }

        public static void DrawLine(float x1, float y1, float x2, float y2, Color color, float thickness = 1)
        {
            graphics.DrawLine(new Pen(new SolidBrush(color), thickness), x1, y1, x2, y2);
        }

        public static void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            graphics.FillRectangle(new SolidBrush(color), x, y, width, height);
        }

        public static void DrawCircle(float x, float y, float radius, Color color) 
        {
            graphics.FillEllipse(new SolidBrush(color), x - radius, y - radius, radius + radius, radius + radius);
        }

        public class GameUI
        {
            public static void DrawText(float x, float y, string text, Font font, Color color)
            {
                graphics.DrawString(text, font, new SolidBrush(color), x, y);
            }
            public static void DrawProgressBar(int x, int y, int width, int height, Color fillColor, Color emptyColor, float percent, bool horizontal = true)
            {
                DrawRectangle(x, y, width, height, emptyColor); // Draws the Progress bar background

                // Limits the percentage fill between 0 to 1 so that the filled part of the progress bar doesn't get bigger than the background
                percent = Utilities.Numbers.ClampN(percent, 0, 1);

                if (horizontal) // Draws the fill background based on if the user wants vertical or horizontal bars
                    DrawRectangle(x, y, (int)(width * percent), height, fillColor);
                else
                    DrawRectangle(x, y - (int)(height * percent) + height, width, (int)(height * percent), fillColor);
            }

        }
    }
}
