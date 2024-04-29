using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteX_Engine.EngineContents.GameFeatures.GameObjects
{
    public class SideScrollerCharacter : CharacterBase
    {
        Vector2 gravityVector = Vector2.UnitY;
        float jumpStrength = 16f;
        PhysicsComponent pc;
        ColliderComponent cc;
        ColliderComponent groundCollider;

        public SideScrollerCharacter(Vector2 position) : base(position)
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();
            cc.friction = 0f;
            groundCollider = AddComponent<ColliderComponent>();
            groundCollider.isSolidCollision = false;
            OnGameObjectUpdate += SideScrollerCharacter_Update;
        }

        private void SideScrollerCharacter_Update(object sender, EventArgs e)
        {
            if (groundCollider != null && cc != null)
            {
                groundCollider.transform.scale = new Vector2(cc.transform.scale.X, 0.001f);
                groundCollider.transform.position.Y = ((cc.transform.scale.Y * 100 * 0.999f) / 2) + 0.001f;
            }
        }

        /// <summary>
        /// Sets which direction the character falls towards
        /// </summary>
        /// <param name="gravityVector"></param>
        public void SetGravityVector(Vector2 gravityVector) 
        {
            this.gravityVector = gravityVector;
        }

        /// <summary>
        /// Returns the direction at which the character falls towards
        /// </summary>
        /// <returns></returns>
        public Vector2 GetGravityVector() {  return gravityVector; }

        public void Jump(float jumpMultiplier = 1f, bool requireGrounded = true) 
        {
            if (!IsDead)
            {
                if (requireGrounded && IsGrounded || !requireGrounded)
                    pc.AddVelocity(-gravityVector * (jumpStrength * jumpMultiplier));
            }
        }

        public bool IsGrounded { get {  return groundCollider != null && groundCollider.IsOverlapping(); } }
    }
}
