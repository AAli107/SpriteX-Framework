using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Utilities;

namespace SpriteX_Engine.EngineContents.Assets.GameObjects
{
    public abstract class CharacterBase : GameObject
    {
        private float hitPoints;
        private float maxHitPoints = 100;
        private int iframes = 0;
        private Vector2 spawnpoint;

        public bool Invincibility { get; set; }
        public bool IsDead { get { return hitPoints <= 0; } }
        public bool IsInvulnerable { get { return iframes > 0 || Invincibility; } }

        public CharacterBase(Vector2 position) : base(position) 
        {
            OnGameObjectSpawn += CharacterBase_OnGameObjectSpawn;
        }

        private void CharacterBase_OnGameObjectSpawn(object sender, EventArgs e)
        {
            hitPoints = maxHitPoints;
            if (sender == null)
                spawnpoint = GetPosition();
        }

        public void Respawn()
        {
            CharacterBase_OnGameObjectSpawn(null, null);
            SetPosition(spawnpoint);
        }

        public virtual void DeathSequence(Enums.DamageType damageType)
        {

        }

        public bool DealDamage(float amount, Enums.DamageType damageType = Enums.DamageType.Generic)
        {
            if (!IsDead && !IsInvulnerable)
            {
                hitPoints = Numbers.ClampN(hitPoints - amount, 0, maxHitPoints);
                if (IsDead) DeathSequence(damageType);
                return true;
            } else return false;
        }

        public bool Heal(float amount)
        {
            if (!IsDead)
            {
                hitPoints = Numbers.ClampN(hitPoints + amount, 0, maxHitPoints);
                return true;
            }
            else return false;
        }
    }
}
