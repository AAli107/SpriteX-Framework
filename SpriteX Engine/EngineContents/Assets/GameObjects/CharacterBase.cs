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

        /// <summary>
        /// Determines whether the character is immune to damage or not
        /// </summary>
        public bool Invincibility { get; set; }
        /// <summary>
        /// Returns true when hitPoints is 0, aka when the character dies
        /// </summary>
        public bool IsDead { get { return hitPoints <= 0; } }
        /// <summary>
        /// Returns whether the character is currently invulnerable to attacks determined by iframes and Invincibility
        /// </summary>
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

        /// <summary>
        /// Respawns the character by moving it to its spawnpoint and refilling the hitpoints to full
        /// </summary>
        public void Respawn()
        {
            CharacterBase_Spawn(null, null);
            SetPosition(spawnpoint);
        }

        /// <summary>
        /// Kills the character even when invulnerable to damage
        /// </summary>
        public void Kill()
        {
            bool inv = Invincibility;
            Invincibility = false;
            iframes = 0;
            DealDamage(float.MaxValue, 0, DamageType.Generic);
            Invincibility = inv;
        }

        /// <summary>
        /// Executed when the character dies / hitpoints reaches 0
        /// </summary>
        /// <param name="damageType"></param>
        public virtual void DeathSequence(DamageType damageType) { }

        /// <summary>
        /// Sets the current HP of the character
        /// </summary>
        /// <param name="hp"></param>
        public void SetHP(float hp)
        {
            hitPoints = hp; 
            if (IsDead) DeathSequence(DamageType.None);
        }

        /// <summary>
        /// Sets the maximum HP of the character
        /// </summary>
        /// <param name="maxHP"></param>
        public void SetMaxHP(float maxHP)
        {
            if (maxHP > 0) { maxHitPoints = maxHP; }
        }

        /// <summary>
        /// Sets where the character will respawn by changing its spawnpoint
        /// </summary>
        /// <param name="spawnpoint"></param>
        /// <param name="willRespawn"></param>
        public void SetSpawnpoint(Vector2 spawnpoint, bool willRespawn = false)
        {
            this.spawnpoint = spawnpoint;
            if (willRespawn) Respawn();
        }

        /// <summary>
        /// Returns current HP of the character
        /// </summary>
        /// <returns></returns>
        public float GetHP() { return hitPoints; }

        /// <summary>
        /// returns maximum HP of the character
        /// </summary>
        /// <returns></returns>
        public float GetMaxHP() { return maxHitPoints; }

        /// <summary>
        /// returns character spawnpoint
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSpawnpoint() { return spawnpoint; }

        /// <summary>
        /// Deducts Character's hitpoints
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="iframes"></param>
        /// <param name="damageType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Increases Character's hitpoints
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
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
