using SpriteX_Framework.FrameworkContents.Components;

namespace SpriteX_Framework.FrameworkContents.GameFeatures.GameObjects
{
    public class TopDownCharacter : CharacterBase
    {
        PhysicsComponent pc;
        ColliderComponent cc;

        public TopDownCharacter() : base()
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();

            pc.gravityEnabled = false;
            pc.isAirborne = false;
        }

        /// <summary>
        /// Causes the Character to move forward
        /// </summary>
        /// <param name="speed"></param>
        public void MoveForward(float speed = 1f)
        {
            if (pc != null)
                pc.AddVelocity(ForwardDirection * speed);
        }

        /// <summary>
        /// Causes the Character to strafe right. (negative speed will make it strafe left)
        /// </summary>
        /// <param name="speed"></param>
        public void StrafeRight(float speed = 1f)
        {
            if (pc != null)
                pc.AddVelocity(RightDirection * speed);
        }

        /// <summary>
        /// Allows you to set whether to enable or disable character physics
        /// </summary>
        /// <param name="simulatePhysics"></param>
        public void SetSimulatePhysics(bool simulatePhysics)
        {
            if (pc != null)
                pc.isEnabled = simulatePhysics;
        }

        /// <summary>
        /// Returns whether the character is simulating physics or not
        /// </summary>
        /// <returns></returns>
        public bool IsSimulatingPhysics()
        {
            return pc != null && pc.isEnabled;
        }
    }
}