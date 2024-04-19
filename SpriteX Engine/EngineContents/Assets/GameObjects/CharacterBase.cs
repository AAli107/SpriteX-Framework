using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpriteX_Engine.EngineContents.Assets.GameObjects
{
    public abstract class CharacterBase : GameObject
    {
        private float hitPoints;
        private float maxHitPoints = 100;
        private int iframes = 0;

        public bool Invincibility { get; set; }
        public bool IsDead { get { return hitPoints <= 0; } }
        public bool IsInvulnerable { get { return iframes > 0 || Invincibility; } }

        public CharacterBase(Vector2 position) : base(position) 
        {
            hitPoints = maxHitPoints;
        }

        public bool DealDamage(float amount)
        {
            if (!IsDead && !IsInvulnerable)
            {
                hitPoints = Utilities.Numbers.ClampN(hitPoints - amount, 0, maxHitPoints);
                return true;
            } else return false;
        }

        public bool Heal(float amount)
        {
            if (!IsDead)
            {
                hitPoints = Utilities.Numbers.ClampN(hitPoints + amount, 0, maxHitPoints);
                return true;
            }
            else return false;
        }
    }
}
