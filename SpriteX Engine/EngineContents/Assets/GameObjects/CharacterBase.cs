using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Utilities;
using static SpriteX_Engine.EngineContents.Utilities.Enums;

namespace SpriteX_Engine.EngineContents.Assets.GameObjects
{
    public abstract class CharacterBase : GameObject
    {
        private float hitPoints;
        private float maxHitPoints = 100;
        private uint iframes = 0;
        private Vector2 spawnpoint;

        public bool Invincibility { get; set; }
        public bool IsDead { get { return hitPoints <= 0; } }
        public bool IsInvulnerable { get { return iframes > 0 || Invincibility; } }

        public CharacterBase(Vector2 position) : base(position) 
        {
            OnGameObjectSpawn += CharacterBase_Spawn;
            OnGameObjectUpdate += CharacterBase_Update;
        }

        private void CharacterBase_Update(object sender, EventArgs e)
        {
            if (iframes > 0) iframes--;
        }

        private void CharacterBase_Spawn(object sender, EventArgs e)
        {
            hitPoints = maxHitPoints;
            if (sender == null)
                spawnpoint = GetPosition();
        }

        public void Respawn()
        {
            CharacterBase_Spawn(null, null);
            SetPosition(spawnpoint);
        }

        public virtual void DeathSequence(DamageType damageType) { }

        public void SetHP(float hp)
        {
            hitPoints = hp; 
            if (IsDead) DeathSequence(DamageType.None);
        }

        public void SetMaxHP(float maxHP)
        {
            if (maxHP > 0) { maxHitPoints = maxHP; }
        }

        public void SetSpawnpoint(Vector2 spawnpoint, bool willRespawn = false)
        {
            this.spawnpoint = spawnpoint;
            if (willRespawn) Respawn();
        }

        public float GetHP() { return hitPoints; }

        public float GetMaxHP() { return maxHitPoints; }

        public Vector2 GetSpawnpoint() { return spawnpoint; }

        public bool DealDamage(float amount, uint iframes = 60, DamageType damageType = DamageType.Generic)
        {
            if (!IsDead && !IsInvulnerable)
            {
                hitPoints = Numbers.ClampN(hitPoints - amount, 0, maxHitPoints);
                this.iframes = iframes < 0 ? 0 : iframes;
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
