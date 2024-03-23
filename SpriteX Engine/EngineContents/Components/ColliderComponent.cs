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
        /// <summary>
        /// top left corner of the collider
        /// </summary>
        public Vector2 topLeft;
        /// <summary>
        /// bottom right corner of the collider
        /// </summary>
        public Vector2 bottomRight;

        private RectangleF rectf;

        public ColliderComponent(GameObject parent) : base(parent) { }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            rectf = new RectangleF(new PointF(transform.position.X + parent.GetPosition().X, transform.position.Y + parent.GetPosition().Y), SizeF.Empty);
            rectf.Inflate(transform.scale.X * 50, transform.scale.Y * 50);
            topLeft = new Vector2(rectf.Left, rectf.Top) - parent.GetPosition();
            bottomRight = new Vector2(rectf.Right, rectf.Bottom);

            foreach (GameObject obj in win.world.gameObjects)
            {
                if (obj == parent) continue;

                foreach (ColliderComponent cc in obj.GetComponents<ColliderComponent>())
                {
                    if (!cc.isEnabled) continue;
                    if (!IsIntersectingWith(cc)) continue;

                    // TODO: Collision implementation here...
                }
            }
        }


        /// <summary>
        /// Sets the collider's relative position to its parent game object
        /// </summary>
        /// <param name="pos"></param>
        public void SetRelativePosition(Vector2 pos) { transform.position = pos; }

        /// <summary>
        /// Sets the collider's scale
        /// </summary>
        /// <param name="scale"></param>
        public void SetRelativeScale(Vector2 scale) { transform.scale = scale; }

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
        
        /// <summary>
        /// Returns Half-size of the collider
        /// </summary>
        /// <returns></returns>
        public Vector2 GetHalfSize() { return bottomRight - (parent.GetPosition() - transform.position); }
    }
}
