using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;

namespace SpriteX_Engine.EngineContents.GameFeatures.GameObjects
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

        public void MoveForward(float speed = 1f)
        {
            if (pc != null)
                pc.AddVelocity(ForwardDirection * speed);
        }

        public void StrafeRight(float speed = 1f)
        {
            if (pc != null)
                pc.AddVelocity(RightDirection * speed);
        }

        public void SetSimulatePhysics(bool simulatePhysics)
        {
            if (pc != null)
                pc.isEnabled = simulatePhysics;
        }

        public bool IsSimulatingPhysics()
        {
            return pc != null && pc.isEnabled;
        }
    }
}