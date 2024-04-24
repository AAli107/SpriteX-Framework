﻿using OpenTK.Mathematics;
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
        PhysicsComponent pc;
        ColliderComponent cc;

        public SideScrollerCharacter(Vector2 position) : base(position)
        {
            pc = AddComponent<PhysicsComponent>();
            cc = AddComponent<ColliderComponent>();
            cc.friction = 0f;
        }
    }
}
