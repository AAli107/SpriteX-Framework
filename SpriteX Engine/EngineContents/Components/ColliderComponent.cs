using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Structs;

namespace SpriteX_Engine.EngineContents.Components
{
    public class ColliderComponent : Component
    {
        /// <summary>
        /// box collider transform, holds position and scale
        /// </summary>
        public Transform transform = new ();
        /// <summary>
        /// controls whether to allow overlapping with other colliders
        /// </summary>
        public bool isSolidCollision = true;
        /// <summary>
        /// friction between colliders [0-1]. Lower values will cause ice-like friction
        /// </summary>
        public float friction = 0.1f;

        private Box2d rect;
        private List<ColliderComponent> overlappingColliders = new List<ColliderComponent>();

        public ColliderComponent(GameObject parent) : base(parent) { }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);

            rect = new Box2d(new Vector2d(parent.GetPosition().X, parent.GetPosition().Y), new Vector2d(transform.position.X + parent.GetPosition().X, transform.position.Y + parent.GetPosition().Y));
            rect.Inflate(transform.scale * 50);

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

                        Vector2d mtv = CalculateMTV(cc);

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

        private Vector2d CalculateMTV(ColliderComponent otherCollider)
        {
            Vector2d centerDifference = transform.position + parent.GetPosition() - (otherCollider.transform.position + otherCollider.parent.GetPosition());
            Vector2d overlap = GetHalfSize() + otherCollider.GetHalfSize() - new Vector2d(Math.Abs(centerDifference.X), Math.Abs(centerDifference.Y));

            if (overlap.X > 0 && overlap.Y > 0)
            {
                return (overlap.X < overlap.Y) ? 
                    new Vector2d(Math.Sign(centerDifference.X) * overlap.X, 0) :
                    new Vector2d(0, Math.Sign(centerDifference.Y) * overlap.Y); 
            }

            return Vector2d.Zero;
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
        public Box2d GetHitbox() { return rect; }

        /// <summary>
        /// Returns true when Collider intersects with another one
        /// </summary>
        /// <returns></returns>
        public bool IsIntersectingWith(ColliderComponent cc) 
        {
            return GetHitbox().Contains(cc.GetHitbox()); 
        }

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
            for (int i = 0; i < oc.Count; i++)
            {
                for (int j = 0; j < ignoredColliders.Length; j++)
                {
                    if (oc[i].Equals(ignoredColliders[j]))
                    {
                        oc.RemoveAt(i);
                        break;
                    }
                }
            }

            return oc.Count > 0;
        }
        
        /// <summary>
        /// Returns Half-size of the collider
        /// </summary>
        /// <returns></returns>
        public Vector2d GetHalfSize() { return rect.HalfSize; }
    }
}
