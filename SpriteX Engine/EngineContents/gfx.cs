﻿using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using static SpriteX_Engine.EngineContents.Utilities;


namespace SpriteX_Engine.EngineContents
{
    public class gfx
    {
        MainWindow win;
        public Texture tex;
        public Texture fontTex;

        public gfx(MainWindow win) 
        {
            this.win = win;
            tex = new Texture();
            fontTex = new Texture(Font.GetDefaultFont().fontPath);
        }

        /// <summary>
        /// Draws a single pixel on the game window (exact pixel position)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(double x, double y, Color4 color)
        {
            if ((x < 0) || (y < 0) || (x > 1920) || (y > 1080))
                return;

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();
            // Specify the vertex data for pixel
            float[] position = { (float)x / (win.ClientSize.X * 0.5f) - 1f, (float)-y / (win.ClientSize.Y * 0.5f) + 1f, 0, 0 };
            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * position.Length, position, BufferUsageHint.DynamicDraw);

            // Draws the pixel
            GL.DrawArrays(PrimitiveType.Points, 0, 1);

            tex.Unbind();
        }

        /// <summary>
        /// Draws a single pixel on the game window (exact pixel position)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void DrawPixel(Vector2 position, Color4 color)
        {
            DrawPixel(position.X, position.Y, color);
        }

        /// <summary>
        /// Draws a single pixel that scales with Game Window
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawScaledPixel(Vector2 position, Color4 color, bool isStatic = false)
        {
            DrawRect(position, new Vector2(1, 1), color, Enums.DrawType.Filled, isStatic);
        }

        /// <summary>
        /// Draws a single pixel that scales with Game Window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawScaledPixel(double x, double y, Color4 color, bool isStatic = false)
        {
            DrawScaledPixel(new Vector2((float)x, (float)y), color, isStatic);
        }

        /// <summary>
        /// Draws a triangle on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawTri(Vector2 a, Vector2 b, Vector2 c, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080)
                ) return;

            if (drawType == Enums.DrawType.Outline) // Will draw the triangle as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, a, color);
                return;
            }
            // Triangle verticies
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 0.0f
            };

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            tex.Unbind();
        }

        /// <summary>
        /// Draws a Quad on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080)
                ) return;

            if (drawType == Enums.DrawType.Outline) // Will draw the quad as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, d, color);
                DrawLine(d, a, color);
                return;
            }

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            tex.Bind();

            // Specify the vertex data for quad
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 0.0f,
                (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0.0f, 0.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            tex.Unbind();
        }

        /// <summary>
        /// Draws a Quad with texture
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        /// <param name="isStatic"></param>
        public void DrawTexturedQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Texture texture, Color4 color, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080)
                ) return;

            // Set the ucolor in the shader
            int colorUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uColor");
            GL.Uniform4(colorUniformLocation, color);

            // Set the texture uniform in the shader
            int textureUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uTexture");
            GL.Uniform1(textureUniformLocation, 0);

            // Bind the texture
            texture.Bind();

            // Specify the vertex data for the quad
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 1.0f, 0.0f,
                (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0.0f, 0.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            // Unbind the texture
            texture.Unbind();
        }

        /// <summary>
        /// Draws a Quad with texture
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="texture"></param>
        /// <param name="isStatic"></param>
        public void DrawTexturedQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Texture texture, bool isStatic = false)
        {
            DrawTexturedQuad(a, b, c, d, texture, Color4.White, isStatic);
        }

        /// <summary>
        /// Draws a simple rectangle on the game window
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawRect(Vector2 pos, Vector2 dimension, Color4 color, Enums.DrawType drawType = Enums.DrawType.Filled, bool isStatic = false)
        {
            DrawQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, color, drawType, isStatic);
        }

        /// <summary>
        /// Draws an image texture on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawImage(Vector2 pos, Vector2 dimension, Texture texture, Color4 color, bool isStatic = false)
        {
            DrawTexturedQuad(pos + new Vector2(0, dimension.Y), pos + new Vector2(dimension.X, dimension.Y), pos + new Vector2(dimension.X, 0), pos, texture, color, isStatic);
        }

        /// <summary>
        /// Draws an image texture on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dimension"></param>
        /// <param name="texture"></param>
        /// <param name="isStatic"></param>
        public void DrawImage(Vector2 pos, Vector2 dimension, Texture texture, bool isStatic = false)
        {
            DrawImage(pos, dimension, texture, Color4.White, isStatic);
        }

        /// <summary>
        /// Draws a line between two points on the game window
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public void DrawLine(Vector2 a, Vector2 b, Color4 color, double width = 1, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080)
                ) return;

            if (width <= 1)
            {
                // Line vertices, point a and point b
                float[] vertices = {
                    (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0f, 0f,
                    (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f, 0f, 0f
                };


                // Set the ucolor in the shader
                int colorUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uColor");
                GL.Uniform4(colorUniformLocation, color);

                // Set the texture uniform in the shader
                int textureUniformLocation = GL.GetUniformLocation(win.shaderProgram, "uTexture");
                GL.Uniform1(textureUniformLocation, 0);

                tex.Bind();

                GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

                GL.DrawArrays(PrimitiveType.Lines, 0, 2);

                tex.Unbind();
            }
            else
            {
                // Calculate the normalized direction between two sides of the line
                Vector2 direction = b - a;
                direction.Normalize();

                // Calculate the perpendicular vector
                Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

                // Calculate the half-width offset
                Vector2 offset = (float)width * 0.5f * perpendicular;

                // Calculate the four corners of the quad
                Vector2 topLeft = a - offset;
                Vector2 topRight = a + offset;
                Vector2 bottomLeft = b - offset;
                Vector2 bottomRight = b + offset;

                // Draws Line as a Quad to have thickness
                DrawQuad(topRight, topLeft, bottomLeft, bottomRight, color, Enums.DrawType.Filled);
            }
        }

        /// <summary>
        /// Draws a Single character on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="character"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="isStatic"></param>
        public void DrawChar(Vector2 pos, char character, Color4 color, float size = 1, bool isStatic = true)
        {
            if (
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) < 0 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) < 0 ||
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) > 1920 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) > 1080
                ) return;

            Vector2 charVec = (win.font.charSize * size);

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            // Bind the texture
            fontTex.Bind();

            int charCount = Font.charSheet.Count;
            int charIndex = Font.charSheet.IndexOf(character);

            // Specify the vertex data for the quad
            float[] vertices = {
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f)))) / (1080 * 0.5f) + 1f,
                (1.0f / charCount) * charIndex, 0.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) + charVec.X) / (1920 * 0.5f) - 1f, -(pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f,
                (1.0f / charCount) * charIndex + (1.0f / charCount), 0.0f,
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) / (1920 * 0.5f) - 1f, -((pos.Y) + charVec.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) / (1080 * 0.5f) + 1f,
                (1.0f / charCount) * charIndex, 1.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - (1920 * 0.5f))) + charVec.X) / (1920 * 0.5f) - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - (1080 * 0.5f))) + charVec.Y ) / (1080 * 0.5f) + 1f,
                (1.0f / charCount) * charIndex + (1.0f / charCount), 1.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

            // Unbind the texture
            fontTex.Unbind();
        }

        /// <summary>
        /// Draws Text on screen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="isStatic"></param>
        public void DrawText(Vector2 pos, string text, Color4 color, float size = 1, bool isStatic = true)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('\n') && i + 1 < text.Length)
                {
                    DrawText(pos + new Vector2(0, win.font.charSize.Y * size), text.Substring(i + 1), color, size, isStatic);
                    return;
                }
                DrawChar(pos + new Vector2(win.font.charSize.X * i * size, 0), text[i], color, size, isStatic);
            }
        }

        public void DrawShape(Enums.Shape shape, Vector2[] pos, Color4 color, Texture texture, Enums.DrawType drawType = Enums.DrawType.Filled, float size = 1, bool isStatic = false)
        {
            switch (shape)
            {
                case Enums.Shape.Quad:
                    if (drawType == Enums.DrawType.Filled) DrawTexturedQuad(pos[0], pos[1], pos[2], pos[3], texture, color, isStatic);
                    else if (drawType == Enums.DrawType.Outline) DrawQuad(pos[0], pos[1], pos[2], pos[3], color, drawType, isStatic);
                    break;
                case Enums.Shape.Rect:
                    if (drawType == Enums.DrawType.Filled) DrawImage(pos[0], pos[1], texture, color, isStatic);
                    else if (drawType == Enums.DrawType.Outline) DrawRect(pos[0], pos[1], color, drawType, isStatic);
                    break;
                case Enums.Shape.Tri:
                    DrawTri(pos[0], pos[1], pos[2], color, drawType, isStatic);
                    break;
                case Enums.Shape.Line:
                    DrawLine(pos[0], pos[1], color, size, isStatic);
                    break;
                case Enums.Shape.Pixel:
                    DrawScaledPixel(pos[0], color, isStatic);
                    break;
            }
        }
    }
}