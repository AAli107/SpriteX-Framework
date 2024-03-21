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
        public Transform transform = new Transform();
        /// <summary>
        /// controls whether to allow overlapping with other colliders
        /// </summary>
        public bool isSolidCollision = true;

        private RectangleF rectf;

        public ColliderComponent(GameObject parent) : base(parent) 
        {
            rectf = new RectangleF(new PointF(transform.position.X, transform.position.Y), SizeF.Empty);
            rectf.Inflate(transform.scale.X * 50, transform.scale.Y * 50);
        }

        /// <summary>
        /// Returns Center Position of the collider
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCenterPosition() { return new Vector2(transform.position.X + (transform.scale.X / 2), transform.position.Y + (transform.scale.Y / 2)); }

        /// <summary>
        /// Returns the collider dimensions
        /// </summary>
        /// <returns></returns>
        public RectangleF GetHitbox() { return rectf; }

        /// <summary>
        /// Returns true when GameObject hitbox intersects with another one
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool IsIntersectingWithGameObject(GameObject gameObject) { return GetHitbox().IntersectsWith(gameObject.GetHitbox()); }
    }
}
