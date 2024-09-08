using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;

namespace SpriteX_Engine.EngineContents.GameFeatures.GameObjects
{
    public class SideScrollerCharacter : CharacterBase
    {
        Vector2 gravityVector = Vector2.UnitY;
        float jumpStrength = 16f;
        PhysicsComponent pc;
        ColliderComponent cc;
        ColliderComponent groundCollider;

        public SideScrollerCharacter() : base()
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();
            cc.friction = 0f;
            groundCollider = AddComponent<ColliderComponent>();
            groundCollider.isSolidCollision = false;
            OnUpdate += SideScrollerCharacter_Update;
        }

        private void SideScrollerCharacter_Update(object sender, EventArgs e)
        {
            if (groundCollider != null && cc != null)
            {
                groundCollider.transform.scale = new Vector2d(cc.transform.scale.X * 0.99, 0.001);
                groundCollider.transform.position.Y = ((cc.transform.scale.Y * 100 * 0.999) / 2d) + 1;
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

        /// <summary>
        /// Will cause the character to jump
        /// </summary>
        /// <param name="jumpMultiplier"></param>
        /// <param name="requireGrounded"></param>
        public void Jump(float jumpMultiplier = 1f, bool requireGrounded = true) 
        {
            if (!IsDead)
            {
                if (requireGrounded && IsGrounded || !requireGrounded)
                    pc.AddVelocity(-gravityVector * (jumpStrength * jumpMultiplier));
            }
        }

        /// <summary>
        /// Sets the character's jump strength
        /// </summary>
        /// <param name="jumpStrength"></param>
        public void SetJumpStrength(float jumpStrength) { this.jumpStrength = jumpStrength; }

        /// <summary>
        /// Returns the character's jump strength
        /// </summary>
        /// <returns></returns>
        public float GetJumpStrength() { return jumpStrength; }

        /// <summary>
        /// Returns true if character is on the ground
        /// </summary>
        public bool IsGrounded { get { return groundCollider != null && groundCollider.IsOverlapping(); } }
    }
}
