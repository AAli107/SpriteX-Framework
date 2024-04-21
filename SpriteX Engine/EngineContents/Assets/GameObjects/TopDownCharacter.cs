using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;

namespace SpriteX_Engine.EngineContents.Assets.GameObjects
{
    public class TopDownCharacter : CharacterBase
    {
        PhysicsComponent pc;
        ColliderComponent cc;

        public TopDownCharacter(Vector2 position) : base(position)
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();

            pc.gravityEnabled = false;
            pc.isAirborne = false;
        }
    }
}
