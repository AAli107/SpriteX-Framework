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
        PhysicsComponent pc;
        ColliderComponent cc;

        public SideScrollerCharacter(Vector2 position) : base(position)
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();
            cc.friction = 0f;
        }

        public void SetGravityVector(Vector2 gravityVector) 
        {
            this.gravityVector = gravityVector;
        }

        public Vector2 GetGravityVector() {  return gravityVector; }
    }
}
