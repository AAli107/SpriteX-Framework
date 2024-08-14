using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;


namespace SpriteX_Engine.EngineContents
{
    public class gfx
    {
        MainWindow win;
        public Texture tex;
        public Texture fontTex;
        public int ScaledPixelSize { get { return scaledPixelSize; } }
        
        private int scaledPixelSize = 0;

        public void ScreenRefresh()
        {
            scaledPixelSize = (int)MathF.Round(Vector2.Distance(new Vector2((0 - (win.world.cam.camPos.X - 960)) / 960 - 1f, -(0 - (win.world.cam.camPos.Y - 540)) / 540 + 1f),
                new Vector2((0 - (win.world.cam.camPos.X - 960)) / 960 - 1f, -(1 - (win.world.cam.camPos.Y - 540)) / 540 + 1f)) * (win.ClientSize.Y * 0.5f));
        }

        public gfx(MainWindow win) 
        {
            this.win = win;
            tex = new Texture();
            fontTex = new Texture(Font.defaultFont.fontPath);
        }

        /// <summary>
        /// Draw Types
        /// </summary>
        public enum DrawType
        {
            Filled,
            Outline
        }

        /// <summary>
        /// Shapes
        /// </summary>
        public enum Shape
        {
            Quad,
            Rect,
            Tri,
            Line,
            Pixel
        }

        /// <summary>
        /// Draws a single pixel on the game window (exact pixel position)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(double x, double y, Color4 color)
        {
            if (x < 0 || y < 0) return;

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

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
        /// Draws a single pixel on the game window based on given data
        /// </summary>
        /// <param name="vertexData"></param>
        /// <param name="color"></param>
        public void DrawPixel(float[] data, Color4 color)
        {
            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            tex.Bind();
            // Specify the vertex data for pixel
            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.DynamicDraw);

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
        /// Draws many pixels on the game window (exact pixel position)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void DrawPixels(Vector2[] position, Color4 color)
        {
            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            float xs = 1 / (win.ClientSize.X * 0.5f);
            float ys = 1 / (win.ClientSize.Y * 0.5f);

            tex.Bind();
            float[] vertexData = new float[position.Length * 4];
            int vi = 0;
            for (int i = 0; i < position.Length; i++)
            {
                // Specify the vertex data for each pixel
                vertexData[vi] = position[i].X * xs - 1f;
                vertexData[vi + 1] = -position[i].Y * ys + 1f;
                vertexData[vi + 2] = 0;
                vertexData[vi + 3] = 0;
                vi += 4;
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertexData.Length, vertexData, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Points, 0, position.Length);

            tex.Unbind();
        }

        public void DrawPixels(float[] vertexData, Color4 color)
        {
            if (vertexData.Length % 4 != 0) return;

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            tex.Bind();

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertexData.Length, vertexData, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Points, 0, vertexData.Length / 4);

            tex.Unbind();
        }

        private void BufferPixels(float[] vertexData)
        {
            if (vertexData.Length % 4 != 0) return;

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertexData.Length, vertexData, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Points, 0, vertexData.Length / 4);
        }

        /// <summary>
        /// Draws a single pixel that scales with Game Window
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawScaledPixel(Vector2 position, Color4 color, bool isStatic = false)
        {
            DrawRect(position, new Vector2(1, 1), color, DrawType.Filled, isStatic);
        }

        /// <summary>
        /// Draws many pixels that scales with Game Window
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="isStatic"></param>
        public void DrawScaledPixels(Vector2[] position, Color4 color, bool isStatic = false)
        {
            float[] v = new float[position.Length * scaledPixelSize * scaledPixelSize * 4];
            int vi = 0;
            for (int j = 0; j < position.Length; j++)
            {
                for (int i = 0; i < scaledPixelSize * scaledPixelSize; i++)
                {
                    v[vi] = (position[j].X + (i / scaledPixelSize) - (isStatic ? 0 : win.world.cam.camPos.X - 960)) / 960 - 1f;
                    v[vi + 1] = -(position[j].Y + (i % scaledPixelSize) - (isStatic ? 0 : win.world.cam.camPos.Y - 540)) / 540 + 1f;
                    vi += 4;
                }
            }

            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            tex.Bind();
            BufferPixels(v);
            tex.Unbind();
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
        public void DrawTri(Vector2 a, Vector2 b, Vector2 c, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080)
                ) return;

            if (drawType == DrawType.Outline) // Will draw the triangle as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, a, color);
                return;
            }
            // Triangle verticies
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 1.0f, 0.0f
            };

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

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
        public void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)
        {
            if (
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080)
                ) return;

            if (drawType == DrawType.Outline) // Will draw the quad as an outline
            {
                DrawLine(a, b, color);
                DrawLine(b, c, color);
                DrawLine(c, d, color);
                DrawLine(d, a, color);
                return;
            }

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            tex.Bind();

            // Specify the vertex data for quad
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.world.cam.camPos.X - 960)) / 960 - 1f, -(b.Y - (isStatic ? 0 : win.world.cam.camPos.Y - 540)) / 540 + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.world.cam.camPos.X - 960)) / 960 - 1f, -(a.Y - (isStatic ? 0 : win.world.cam.camPos.Y - 540)) / 540 + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.world.cam.camPos.X - 960)) / 960 - 1f, -(c.Y - (isStatic ? 0 : win.world.cam.camPos.Y - 540)) / 540 + 1f, 1.0f, 0.0f,
                (d.X - (isStatic ? 0 : win.world.cam.camPos.X - 960)) / 960 - 1f, -(d.Y - (isStatic ? 0 : win.world.cam.camPos.Y - 540)) / 540 + 1f, 0.0f, 0.0f
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
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 &&
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 &&
                (c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080)
                ) return;

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            // Bind the texture
            texture.Bind();

            // Specify the vertex data for the quad
            float[] vertices = {
                (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 1.0f, 1.0f,
                (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 0.0f, 1.0f,
                (c.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(c.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 1.0f, 0.0f,
                (d.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(d.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 0.0f, 0.0f
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
        public void DrawRect(Vector2 pos, Vector2 dimension, Color4 color, DrawType drawType = DrawType.Filled, bool isStatic = false)
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
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0) ||
                ((a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 && (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920) ||
                ((a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080 && (b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080)
                ) return;

            if (width <= 1)
            {
                // Line vertices, point a and point b
                float[] vertices = {
                    (b.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(b.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 0f, 0f,
                    (a.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(a.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f, 0f, 0f
                };


                // Set the ucolor in the shader
                GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

                // Set the texture uniform in the shader
                GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

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
                DrawQuad(topRight, topLeft, bottomLeft, bottomRight, color, DrawType.Filled);
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
        public void DrawSingleChar(Vector2 pos, char character, Color4 color, float size = 1, bool isStatic = true)
        {
            if (
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 ||
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080
                ) return;

            Vector2 charVec = win.font.charSize * size;

            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            // Bind the texture
            fontTex.Bind();

            int charCount = Font.charSheet.Length;
            int charIndex = Array.IndexOf(Font.charSheet, character);

            // Specify the vertex data for the quad
            float[] vertices = {
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540))) / 540 + 1f,
                (1.0f / charCount) * charIndex, 0.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) + charVec.X) / 960 - 1f, -(pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f,
                (1.0f / charCount) * charIndex + (1.0f / charCount), 0.0f,
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(pos.Y + charVec.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f,
                (1.0f / charCount) * charIndex, 1.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) + charVec.X) / 960 - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) + charVec.Y ) / 540 + 1f,
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
            // Set the ucolor in the shader
            GL.Uniform4(GL.GetUniformLocation(win.shaderProgram, "uColor"), color);

            // Set the texture uniform in the shader
            GL.Uniform1(GL.GetUniformLocation(win.shaderProgram, "uTexture"), 0);

            // Bind the texture
            fontTex.Bind();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals('\n') && i + 1 < text.Length)
                {
                    DrawText(pos + new Vector2(0, win.font.charSize.Y * size), text[(i + 1)..], color, size, isStatic);
                    return;
                }
                DrawCharactor(pos + new Vector2(win.font.charSize.X * i * size, 0), text[i], size, isStatic);
            }

            // Unbind the texture
            fontTex.Unbind();
        }

        /// <summary>
        /// Used only for drawing text
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="character"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="isStatic"></param>
        private void DrawCharactor(Vector2 pos, char character, float size = 1, bool isStatic = true)
        {
            if (
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) < 0 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) < 0 ||
                (pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) > 1920 ||
                (pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) > 1080
                ) return;

            Vector2 charVec = win.font.charSize * size;

            int charCount = Font.charSheet.Length;
            int charIndex = Array.IndexOf(Font.charSheet, character);

            // Specify the vertex data for the quad
            float[] vertices = {
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540))) / 540 + 1f,
                (1.0f / charCount) * charIndex, 0.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) + charVec.X) / 960 - 1f, -(pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f,
                (1.0f / charCount) * charIndex + (1.0f / charCount), 0.0f,
                (pos.X  - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) / 960 - 1f, -(pos.Y + charVec.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) / 540 + 1f,
                (1.0f / charCount) * charIndex, 1.0f,
                ((pos.X - (isStatic ? 0 : win.GetWorldCamera().camPos.X - 960)) + charVec.X) / 960 - 1f, -((pos.Y - (isStatic ? 0 : win.GetWorldCamera().camPos.Y - 540)) + charVec.Y ) / 540 + 1f,
                (1.0f / charCount) * charIndex + (1.0f / charCount), 1.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, win.vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        public void DrawShape(Shape shape, Vector2[] pos, Color4 color, Texture texture, DrawType drawType = DrawType.Filled, float size = 1, bool isStatic = false)
        {
            switch (shape)
            {
                case Shape.Quad:
                    if (drawType == DrawType.Filled) DrawTexturedQuad(pos[0], pos[1], pos[2], pos[3], texture, color, isStatic);
                    else if (drawType == DrawType.Outline) DrawQuad(pos[0], pos[1], pos[2], pos[3], color, drawType, isStatic);
                    break;
                case Shape.Rect:
                    if (drawType == DrawType.Filled) DrawImage(pos[0], pos[1], texture, color, isStatic);
                    else if (drawType == DrawType.Outline) DrawRect(pos[0], pos[1], color, drawType, isStatic);
                    break;
                case Shape.Tri:
                    DrawTri(pos[0], pos[1], pos[2], color, drawType, isStatic);
                    break;
                case Shape.Line:
                    DrawLine(pos[0], pos[1], color, size, isStatic);
                    break;
                case Shape.Pixel:
                    DrawScaledPixel(pos[0], color, isStatic);
                    break;
            }
        }
    }
}
