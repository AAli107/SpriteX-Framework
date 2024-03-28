using System.Drawing;
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
        /// <summary>
        /// friction between colliders [0-1]. Lower values will cause ice-like friction
        /// </summary>
        public float friction = 0.1f;

        private RectangleF rectf;

        public ColliderComponent(GameObject parent) : base(parent) { }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            rectf = new RectangleF(new PointF(transform.position.X + parent.GetPosition().X, transform.position.Y + parent.GetPosition().Y), SizeF.Empty);
            rectf.Inflate(transform.scale.X * 50, transform.scale.Y * 50);
            topLeft = new Vector2(rectf.Left, rectf.Top) - parent.GetPosition();
            bottomRight = new Vector2(rectf.Right, rectf.Bottom);

            friction = Utilities.Numbers.ClampN(friction, 0, 1);

            foreach (GameObject obj in win.world.gameObjects)
            {
                if (obj == parent) continue;

                foreach (ColliderComponent cc in obj.GetComponents<ColliderComponent>())
                {
                    if (!cc.isEnabled) continue;
                    if (!IsIntersectingWith(cc)) continue;

                    if (isSolidCollision && cc.isSolidCollision)
                    {
                        PhysicsComponent pc = parent.GetComponent<PhysicsComponent>();
                        PhysicsComponent pc2 = obj.GetComponent<PhysicsComponent>();

                        float pcMass = pc != null ? pc.mass : float.MaxValue;
                        float pc2Mass = pc2 != null ? pc2.mass : float.MaxValue;

                        float totalMass = pcMass + pc2Mass;
                        
                        float thisMassProportion = pcMass / totalMass;
                        float otherMassProportion = pc2Mass / totalMass;

                        Vector2 mtv = CalculateMTV(cc);

                        if (pc != null)
                        {
                            if (pc.isEnabled)
                            {
                                Vector2 frictionForce = -pc.velocity * cc.friction;

                                parent.SetPosition(parent.GetPosition() + mtv * (1 - thisMassProportion));
                                pc.AddVelocity(mtv * (1 - thisMassProportion) + frictionForce);
                                if (pc2 != null)
                                {
                                    if (pc2.isEnabled)
                                    {
                                        Vector2 frictionForce2 = -pc2.velocity * friction;

                                        obj.SetPosition(obj.GetPosition() - mtv * (1 - otherMassProportion));
                                        pc2.AddVelocity(-mtv * (1 - otherMassProportion) + frictionForce2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private Vector2 CalculateMTV(ColliderComponent otherCollider)
        {
            Vector2 thisHalfSize = GetHalfSize();
            Vector2 otherHalfSize = otherCollider.GetHalfSize();

            Vector2 thisCenter = transform.position + parent.GetPosition();
            Vector2 otherCenter = otherCollider.transform.position + otherCollider.parent.GetPosition();

            Vector2 centerDifference = thisCenter - otherCenter;

            float xOverlap = thisHalfSize.X + otherHalfSize.X - Math.Abs(centerDifference.X);
            float yOverlap = thisHalfSize.Y + otherHalfSize.Y - Math.Abs(centerDifference.Y);

            if (xOverlap > 0 && yOverlap > 0)
            {
                return (xOverlap < yOverlap) ? 
                    new Vector2(Math.Sign(centerDifference.X) * xOverlap, 0) :
                    new Vector2(0, Math.Sign(centerDifference.Y) * yOverlap); 
            }

            return Vector2.Zero;
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
        public Vector2 GetHalfSize() { return bottomRight - (parent.GetPosition() + transform.position); }
    }
}
