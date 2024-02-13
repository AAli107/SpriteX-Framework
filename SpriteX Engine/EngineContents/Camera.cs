using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents
{
    public class Camera
    {
        private Vector2 loc;
        public float speed;
        private RectangleF bounds;

        public bool isCameraBound { get; private set; }
        public Vector2 camPos { get { return loc; } }

        public Camera() // Default Camera constructor
        {
            loc = new Vector2(0,0);
            speed = 10;
            bounds = new RectangleF(new PointF(0, 0), new SizeF(1920, 1080));
            isCameraBound = false;
        }

        public Camera(Vector2 _loc) // Camera constructor to decide where to create the camera
        {
            loc = _loc;
            speed = 10;
            bounds = new RectangleF(new PointF(0, 0), new SizeF(1920, 1080));
            isCameraBound = false;
        }

        public Camera(Vector2 _loc, float _speed) // Camera constructor to decide where to create the camera and what speed to move
        {
            loc = _loc;
            speed = _speed;
            bounds = new RectangleF(new PointF(0, 0), new SizeF(1920, 1080));
            isCameraBound = false;
        }

        /// <summary>
        /// Offsets the camera location based on the offset vector xy values.
        /// </summary>
        /// <param name="offset"></param>
        public void MoveCamera(Vector2 offset)
        {
            loc = isCameraBound ? new Vector2(Utilities.Numbers.ClampN(loc.X + offset.X, bounds.X, bounds.Width),
                Utilities.Numbers.ClampN(loc.Y + offset.Y, bounds.Y, bounds.Height)) : new Vector2(loc.X + offset.X, loc.Y + offset.Y);
        }

        /// <summary>
        /// Offsets the camera location based on the dir(normalized vector) multiplied by the camera's speed
        /// </summary>
        /// <param name="dir"></param>
        public void MoveCameraBySpeed(Vector2 dir)
        {
            Vector2 clampedDir = new Vector2(Utilities.Numbers.ClampN(dir.X, -1, 1), Utilities.Numbers.ClampN(dir.Y, -1, 1));

            loc = isCameraBound ? new Vector2(Utilities.Numbers.ClampN(loc.X + (clampedDir.X * speed), bounds.X, bounds.Width),
                Utilities.Numbers.ClampN(loc.Y + (clampedDir.Y * speed), bounds.Y, bounds.Height)) : new Vector2(loc.X + (clampedDir.X * speed), loc.Y + (clampedDir.Y * speed));
        }

        /// <summary>
        /// Sets the camera's position to whatever newPos is at
        /// </summary>
        /// <param name="newPos"></param>
        public void SetCameraPosition(Vector2 newPos)
        {
            loc = isCameraBound ? new Vector2(Utilities.Numbers.ClampN(newPos.X, bounds.X, bounds.Width),
                Utilities.Numbers.ClampN(newPos.Y, bounds.Y, bounds.Height)) : newPos;
        }

        /// <summary>
        /// Gives Camera a Rectangle Bound where it cannot leave from
        /// </summary>
        /// <param name="start"></param>
        /// <param name="width"></param>
        public bool SetCameraBound(Vector2 start, Vector2 width, bool enableBounds = false)
        {
            if (start.X < width.X && start.Y < width.Y)
            {
                bounds = new RectangleF(new PointF(start.X, start.Y), new SizeF(width.X, width.Y));
                isCameraBound = enableBounds;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Controls whether the camera is bound or not
        /// </summary>
        /// <param name="isCameraBound"></param>
        public void SetEnableCameraBound(bool isCameraBound)
        {
            this.isCameraBound = isCameraBound;
        }
    }
}
