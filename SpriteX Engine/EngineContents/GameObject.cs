using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public GameObject(Vector2 position, Vector2 size, bool simulatePhysics = false, float friction = 1f)
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
                velocity *= 0.9f / friction;
            }
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
        /// Removes a GameObject by id
        /// </summary>
        /// <param name="id"></param>
        public static void RemoveGameObjectByID(uint id)
        {
            gameObjects.RemoveAll(o => o.id == id);
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
