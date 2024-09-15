using System.Drawing;
using OpenTK.Mathematics;

namespace SpriteX_Framework.FrameworkContents
{
    public class Camera
    {
        private Vector2d loc;
        public float speed;
        private RectangleF bounds;

        /// <summary>
        /// Controls whether the camera is locked within its bounds
        /// </summary>
        public bool isCameraBound { get; private set; }

        /// <summary>
        /// Gives the position of the camera
        /// </summary>
        public Vector2d camPos { get { return loc; } }

        public Camera() // Default Camera constructor
        {
            loc = new Vector2d(0,0);
            speed = 10;
            bounds = new RectangleF(new PointF(0, 0), new SizeF(1920, 1080));
            isCameraBound = false;
        }

        public Camera(Vector2d _loc) // Camera constructor to decide where to create the camera
        {
            loc = _loc;
            speed = 10;
            bounds = new RectangleF(new PointF(0, 0), new SizeF(1920, 1080));
            isCameraBound = false;
        }

        public Camera(Vector2d _loc, float _speed) // Camera constructor to decide where to create the camera and what speed to move
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
        public void MoveCamera(Vector2d offset)
        {
            loc = isCameraBound ? new Vector2d(Utilities.Numbers.ClampND(loc.X + offset.X, (double)bounds.X, (double)bounds.Width),
                Utilities.Numbers.ClampND(loc.Y + offset.Y, bounds.Y, bounds.Height)) : new Vector2d(loc.X + offset.X, loc.Y + offset.Y);
        }

        /// <summary>
        /// Offsets the camera location based on the dir(normalized vector) multiplied by the camera's speed
        /// </summary>
        /// <param name="dir"></param>
        public void MoveCameraBySpeed(Vector2d dir)
        {
            Vector2d clampedDir = new Vector2d(Utilities.Numbers.ClampND(dir.X, -1, 1), Utilities.Numbers.ClampND(dir.Y, -1, 1));

            loc = isCameraBound ? new Vector2d(Utilities.Numbers.ClampND(loc.X + (clampedDir.X * speed), bounds.X, bounds.Width),
                Utilities.Numbers.ClampND(loc.Y + (clampedDir.Y * speed), bounds.Y, bounds.Height)) : new Vector2d(loc.X + (clampedDir.X * speed), loc.Y + (clampedDir.Y * speed));
        }

        /// <summary>
        /// Sets the camera's position to whatever newPos is at
        /// </summary>
        /// <param name="newPos"></param>
        public void SetCameraPosition(Vector2d newPos)
        {
            loc = isCameraBound ? new Vector2d(Utilities.Numbers.ClampND(newPos.X, (double)bounds.X, (double)bounds.Width),
                Utilities.Numbers.ClampND(newPos.Y, bounds.Y, bounds.Height)) : newPos;
        }

        /// <summary>
        /// Gives Camera a Rectangle Bound where it cannot leave from
        /// </summary>
        /// <param name="start"></param>
        /// <param name="width"></param>
        public bool SetCameraBound(Vector2d start, Vector2d width, bool enableBounds = false)
        {
            if (start.X < width.X && start.Y < width.Y)
            {
                bounds = new RectangleF(new PointF((float)start.X, (float)start.Y), new SizeF((float)width.X, (float)width.Y));
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
