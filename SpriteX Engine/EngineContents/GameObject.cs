using OpenTK.Mathematics;
using System.Drawing;

namespace SpriteX_Engine.EngineContents
{
    public class GameObject
    {
        static List<GameObject> gameObjects = new List<GameObject>();

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
            uint id = 0;
            while (gameObjects.Any(o => o.id == id))
            {
                id++;
            }
            this.id = id;
            this.position = position;
            this.size = size;
            this.mass = mass;
            this.friction = friction;
            this.simulatePhysics = simulatePhysics;
            this.collisionEnabled = collisionEnabled;

            gameObjects.Add(this);
        }

        void UpdateTick()
        {
            if (simulatePhysics)
            {
                position += velocity;
                velocity *= 1 / (((friction + 1) >= 1) ? (friction + 1) : 1);
            }
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





        /// <summary>
        /// Removes a GameObject by id
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveGameObjectByID(uint id)
        {
            gameObjects.RemoveAll(o => o.id == id);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="obj"></param>
        public static void RemoveGameObject(GameObject obj) 
        {
            gameObjects.Remove(obj);
        }

        /// <summary>
        /// Returns GameObject by the inputted ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GameObject GetGameObjectByID(uint id)
        {
            try { return gameObjects.Single(o => o.id == id); }
            catch { return null; }
        }

        /// <summary>
        /// Returns true if GameObject with the same ID exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DoesGameObjectExist(uint id)
        {
            return gameObjects.Any(o => o.id == id);
        }

        /// <summary>
        /// Returns the List of all GameObjects
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetAllGameObjects()
        {
            return gameObjects;
        }

        /// <summary>
        /// Will update tick all existing GameObjects and does the collision between them
        /// </summary>
        public static void TickAllGameObjects()
        {
            List<GameObject> collidableGameObjects = gameObjects.FindAll(o => o.collisionEnabled);
            if (collidableGameObjects.Count > 1) foreach (GameObject obj in collidableGameObjects)
            {
                obj.UpdateTick();

                foreach (GameObject obj2 in collidableGameObjects)
                {
                    if (obj != obj2 && obj.collisionEnabled && obj2.collisionEnabled && obj.velocity.Length + obj2.velocity.Length > 0 && obj.IsIntersectingWith(obj2))
                    {
                        // Calculate the collision vector based on the GameObjects' size and position
                        Vector2 cv = CalculateCollisionVector(obj, obj2);

                        // Move the objects apart along the MTV to prevent overlapping
                        if (obj.IsSimulatingPhysics()) obj.SetPosition(obj.GetPosition() + cv);

                        // Pushes Colliding GameObjects if simulating physics, pushing force depending on their mass
                        if (obj.IsSimulatingPhysics()) obj.OverrideVelocity(obj.GetVelocity() + (cv / (obj2.IsSimulatingPhysics() ? obj.mass : 1f)));
                        if (obj2.IsSimulatingPhysics()) obj2.OverrideVelocity(obj2.GetVelocity() - (cv / (obj.IsSimulatingPhysics() ? obj2.mass : 1f)));
                    }
                }
            }
        }
        
        // collision vector code
        private static Vector2 CalculateCollisionVector(GameObject obj1, GameObject obj2)
        {
            // This'll store the center positions of the GameObjects
            Vector2 center1 = obj1.GetCenterPosition();
            Vector2 center2 = obj2.GetCenterPosition();

            // Calculate the half sizes of the objects
            Vector2 halfSize1 = obj1.GetSize() * 0.5f;
            Vector2 halfSize2 = obj2.GetSize() * 0.5f;

            // Calculate the overlap on each axis
            float overlapX = halfSize1.X + halfSize2.X - Math.Abs(center1.X - center2.X);
            float overlapY = halfSize1.Y + halfSize2.Y - Math.Abs(center1.Y - center2.Y);

            // Determine the axis of the collision and just return the collision vectors
            if (overlapX < overlapY) return center1.X < center2.X ? new Vector2(-overlapX, 0) : new Vector2(overlapX, 0);
            else return center1.Y < center2.Y ? new Vector2(0, -overlapY) : new Vector2(0, overlapY);
        }

    }
}
