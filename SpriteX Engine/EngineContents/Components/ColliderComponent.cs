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
        private List<ColliderComponent> overlappingColliders = new List<ColliderComponent>();

        public ColliderComponent(GameObject parent) : base(parent) { }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            rectf = new RectangleF(new PointF(transform.position.X + parent.GetPosition().X, transform.position.Y + parent.GetPosition().Y), SizeF.Empty);
            rectf.Inflate(transform.scale.X * 50, transform.scale.Y * 50);
            topLeft = new Vector2(rectf.Left, rectf.Top) - parent.GetPosition();
            bottomRight = new Vector2(rectf.Right, rectf.Bottom);

            friction = Utilities.Numbers.ClampN(friction, 0, 1);

            if (overlappingColliders.Count > 0) overlappingColliders.Clear();

            PhysicsComponent pc = parent.GetComponent<PhysicsComponent>();
            foreach (GameObject obj in win.world.gameObjects)
            {
                if (obj == parent) continue;
                PhysicsComponent pc2 = obj.GetComponent<PhysicsComponent>();

                foreach (ColliderComponent cc in obj.GetComponents<ColliderComponent>())
                {
                    if (!cc.isEnabled) continue;
                    if (!IsIntersectingWith(cc)) continue;
                    
                    if (!overlappingColliders.Contains(cc)) overlappingColliders.Add(cc);

                    if (isSolidCollision && cc.isSolidCollision)
                    {
                        float pcMass = pc != null ? pc.mass : float.MaxValue;
                        float pc2Mass = pc2 != null ? pc2.mass : float.MaxValue;

                        float totalMass = pcMass + pc2Mass;
                        
                        float thisMassProportion = pcMass / totalMass;
                        float otherMassProportion = pc2Mass / totalMass;

                        Vector2 mtv = CalculateMTV(cc);

                        if (pc != null && pc.isEnabled)
                        {
                            parent.SetPosition(parent.GetPosition() + mtv * (1 - thisMassProportion));
                            pc.AddVelocity(mtv * (1 - thisMassProportion) + -pc.velocity * cc.friction);
                            if (pc2 != null && pc2.isEnabled)
                            {
                                obj.SetPosition(obj.GetPosition() - mtv * (1 - otherMassProportion));
                                pc2.AddVelocity(-mtv * (1 - otherMassProportion) + -pc2.velocity * friction);
                            }
                        }
                    }
                }
            }
        }

        private Vector2 CalculateMTV(ColliderComponent otherCollider)
        {
            Vector2 centerDifference = transform.position + parent.GetPosition() - (otherCollider.transform.position + otherCollider.parent.GetPosition());
            Vector2 overlap = GetHalfSize() + otherCollider.GetHalfSize() - new Vector2(Math.Abs(centerDifference.X), Math.Abs(centerDifference.Y));

            if (overlap.X > 0 && overlap.Y > 0)
            {
                return (overlap.X < overlap.Y) ? 
                    new Vector2(Math.Sign(centerDifference.X) * overlap.X, 0) :
                    new Vector2(0, Math.Sign(centerDifference.Y) * overlap.Y); 
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
        /// Will return true if this collider is overlapping with another collider
        /// </summary>
        /// <returns></returns>
        public bool IsOverlapping() { return overlappingColliders.Count > 0; }

        /// <summary>
        /// Will return true if this collider is overlapping with another collider ignoring the array of ignored colliders
        /// </summary>
        /// <param name="ignoredColliders"></param>
        /// <returns></returns>
        public bool IsOverlapping(ColliderComponent[] ignoredColliders)
        {
            List<ColliderComponent> oc = overlappingColliders;
            oc.RemoveAll(x => ignoredColliders.Contains(x));
            return oc.Count > 0;
        }
        
        /// <summary>
        /// Returns Half-size of the collider
        /// </summary>
        /// <returns></returns>
        public Vector2 GetHalfSize() { return bottomRight - (parent.GetPosition() + transform.position); }
    }
}
