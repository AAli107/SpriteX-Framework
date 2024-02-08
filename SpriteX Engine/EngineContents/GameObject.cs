using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

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
        bool simulatePhysics;

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="simulatePhysics"></param>
        /// <param name="friction"></param>
        public GameObject(Vector2 position, Vector2 size, bool simulatePhysics = false, float friction = 0.1f)
        {
            Construct(position, size, simulatePhysics, friction);
        }

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        /// <param name="hitbox"></param>
        /// <param name="simulatePhysics"></param>
        /// <param name="friction"></param>
        public GameObject(RectangleF hitbox, bool simulatePhysics = false, float friction = 0.1f) 
        {
            Construct(new Vector2(hitbox.X, hitbox.Y), new Vector2(hitbox.Width, hitbox.Height), simulatePhysics, friction);
        }

        void Construct(Vector2 position, Vector2 size, bool simulatePhysics = false, float friction = 0.1f)
        {
            uint id = 0;
            while (gameObjects.Any(o => o.id == id))
            {
                id++;
            }
            this.id = id;
            this.position = position;
            this.size = size;
            this.friction = friction;
            this.simulatePhysics = simulatePhysics;

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
        /// Removes a GameObject by id
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveGameObjectByID(uint id)
        {
            gameObjects.RemoveAll(o => o.id == id);
        }

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
        /// Will update tick all existing GameObjects
        /// </summary>
        public static void TickAllGameObjects()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.UpdateTick();
            }
        }

    }
}
