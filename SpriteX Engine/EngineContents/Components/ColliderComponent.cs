using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Structs;

namespace SpriteX_Engine.EngineContents.Components
{
    public class ColliderComponent : Component
    {
        /// <summary>
        /// box collider transform, holds position and scale
        /// </summary>
        public Transform transform {
            get { return t; }
            set { SetTransform(value); }
        }
        /// <summary>
        /// controls whether to allow overlapping with other colliders
        /// </summary>
        public bool isSolidCollision = true;
        /// <summary>
        /// top left corner of the collider
        /// </summary>
        public Vector2 topLeft;
        /// <summary>
        /// bottom right corner of the collider
        /// </summary>
        public Vector2 bottomRight;

        private RectangleF rectf;
        private Transform t = new Transform();

        public ColliderComponent(GameObject parent) : base(parent) 
        {
            SetTransform(t);
        }

        /// <summary>
        /// Sets Transform of collision
        /// </summary>
        /// <param name="transform"></param>
        public void SetTransform(Transform transform)
        {
            t = transform;

            rectf = new RectangleF(new PointF(t.position.X, t.position.Y), SizeF.Empty);
            rectf.Inflate(t.scale.X * 50, t.scale.Y * 50);
            topLeft = new Vector2(rectf.Left, rectf.Top);
            bottomRight = new Vector2(rectf.Right, rectf.Bottom);
        }

        /// <summary>
        /// Sets the collider's relative position to its parent game object
        /// </summary>
        /// <param name="pos"></param>
        public void SetRelativePosition(Vector2 pos)
        {
            SetTransform(new Transform(pos, t.scale));
        }

        /// <summary>
        /// Sets the collider's scale
        /// </summary>
        /// <param name="scale"></param>
        public void SetRelativeScale(Vector2 scale)
        {
            SetTransform(new Transform(t.position, scale));
        }

        /// <summary>
        /// Returns Center Position of the collider
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCenterPosition() { return new Vector2(t.position.X + (t.scale.X / 2), t.position.Y + (t.scale.Y / 2)); }

        /// <summary>
        /// Returns the collider dimensions
        /// </summary>
        /// <returns></returns>
        public RectangleF GetHitbox() { return rectf; }

        /// <summary>
        /// Returns true when Collider intersects with another one
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool IsIntersectingWith(ColliderComponent cc) { return GetHitbox().IntersectsWith(cc.GetHitbox()); }
    }
}
