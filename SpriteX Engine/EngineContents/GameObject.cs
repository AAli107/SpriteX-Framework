using OpenTK.Mathematics;
using System.Diagnostics;
using System.Drawing;

namespace SpriteX_Engine.EngineContents
{
    public class GameObject
    {
        uint id;
        Vector2 position;
        Vector2 size;
        Vector2 velocity;
        float friction;
        float mass;
        bool simulatePhysics;
        bool collisionEnabled;

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="simulatePhysics"></param>
        /// <param name="friction"></param>
        public GameObject(Vector2 position, Vector2 size, bool collisionEnabled = true, bool simulatePhysics = false, float friction = 0.1f, float mass = 10f)
        {
            Construct(position, size, collisionEnabled, simulatePhysics, friction, mass);
        }

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="simulatePhysics"></param>
        /// <param name="friction"></param>
        public GameObject(RectangleF hitbox, bool collisionEnabled = true, bool simulatePhysics = false, float friction = 0.1f, float mass = 1f) 
        {
            Construct(new Vector2(hitbox.X, hitbox.Y), new Vector2(hitbox.Width, hitbox.Height), collisionEnabled, simulatePhysics, friction, mass);
        }

        void Construct(Vector2 position, Vector2 size, bool collisionEnabled = true, bool simulatePhysics = false, float friction = 0.1f, float mass = 10f)
        {
            this.position = position;
            this.size = size;
            this.mass = mass;
            this.friction = friction;
            this.simulatePhysics = simulatePhysics;
            this.collisionEnabled = collisionEnabled;
        }

        public void UpdateTick()
        {
            if (simulatePhysics)
            {
                position += velocity;
                velocity *= 1 / (((friction + 1) >= 1) ? (friction + 1) : 1);
            }
        }

        /// <summary>
        /// Sets ID of GameObject if world has
        /// </summary>
        /// <param name="id"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public bool SetID(uint id, World world)
        {
            if (!world.DoesGameObjectExist(id))
            {
                this.id = id;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets GameObject's Position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Sets GameObject's Mass
        /// </summary>
        /// <param name="mass"></param>
        public void SetMass(float mass) 
        {
            this.mass = mass;
        }

        /// <summary>
        /// Sets GameObject's Friction
        /// </summary>
        /// <param name="friction"></param>
        public void SetFriction(float friction)
        {
            this.friction = friction;
        }

        /// <summary>
        /// Controls if GameObject will have physics or not
        /// </summary>
        /// <param name="simulatePhysics"></param>
        public void SetSimulatePhysics(bool simulatePhysics) 
        {
            this.simulatePhysics = simulatePhysics;
        }

        /// <summary>
        /// Will override current GameObject's Velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void OverrideVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        /// <summary>
        /// Adds directional velocity in the GameObject's current Velocity
        /// </summary>
        /// <param name="velocity"></param>
        public void AddVelocity(Vector2 velocity) 
        {
            this.velocity += velocity;
        }

        /// <summary>
        /// Controls whether GameObject collision is enabled or not
        /// </summary>
        /// <param name="collisionEnabled"></param>
        public void SetCollisionEnabled(bool collisionEnabled)
        {
            this.collisionEnabled = collisionEnabled;
        }

        /// <summary>
        /// Returns Center Position of the GameObject's Hitbox
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCenterPosition() { return new Vector2(position.X + (size.X / 2), position.Y + (size.Y / 2)); }

        /// <summary>
        /// Returns GameObject's ID
        /// </summary>
        /// <returns></returns>
        public uint GetID() { return id; }

        /// <summary>
        /// Returns GameObject's position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition() { return position; }

        /// <summary>
        /// Returns GameObject's size
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize() { return size; }

        /// <summary>
        /// Returns GameObject's Velocity
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVelocity() { return velocity; }

        /// <summary>
        /// Returns GameObject's mass
        /// </summary>
        /// <returns></returns>
        public float GetMass() { return mass; }

        /// <summary>
        /// Returns GameObject's Friction
        /// </summary>
        /// <returns></returns>
        public float GetFriction() { return friction; }

        /// <summary>
        /// Returns the GameObject's Hitbox
        /// </summary>
        /// <returns></returns>
        public RectangleF GetHitbox() { return new RectangleF(new PointF(position.X, position.Y), new SizeF(size.X, size.Y)); }

        /// <summary>
        /// Returns whether GameObject is Simulating physics or not
        /// </summary>
        /// <returns></returns>
        public bool IsSimulatingPhysics() { return simulatePhysics; }

        /// <summary>
        /// Returns true when GameObject hitbox intersects with another one
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool IsIntersectingWith(GameObject gameObject) { return GetHitbox().IntersectsWith(gameObject.GetHitbox()); }

        /// <summary>
        /// Returns true when GameObject collision is enabled
        /// </summary>
        /// <returns></returns>
        public bool IsCollisionEnabled() { return collisionEnabled; }
    }
}
