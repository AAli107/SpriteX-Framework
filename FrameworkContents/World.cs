using OpenTK.Mathematics;
using System.Collections.Generic;

namespace SpriteX_Framework.FrameworkContents
{
    public class World
    {
        public static World WorldInst { get; private set; }

        public Camera cam;
        public List<GameObject> gameObjects = new();
        public List<Audio> audios = new(256);

        public World()
        {
            cam = new Camera();
        }

        public World(Camera cam)
        {
            this.cam = cam;
        }

        ~World()
        {
            gameObjects.Clear();
            for (int i = 0; i < audios.Count; i++)
            {
                audios[i].Stop();
                audios[i].Dispose();
                audios[i] = null;
            }
            audios.Clear();
        }

        /// <summary>
        /// Plays Audio
        /// </summary>
        /// <param name="path"></param>
        /// <param name="volume"></param>
        public void PlayAudio(string path, float volume = 1f) 
        {
            volume = Utilities.Numbers.ClampN(volume, 0, 1);
            Audio a = new(path, volume);
            audios.Add(a);
            a.Play();
        }

        public void WorldUpdate()
        {
            foreach (Audio a in audios.ToArray())
            {
                if (a.IsStopped())
                {
                    a.Dispose();
                    audios.Remove(a);
                } 
            }
        }

        /// <summary>
        /// Will spawn this GameObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SpawnGameObject(GameObject obj, Vector2d position)
        {
            uint id = 0;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].GetID() == id) id++;
                else break;
            }

            if (obj.SetID(id, this))
            {
                obj.SetPosition(position);
                gameObjects.Add(obj);
                obj.Spawn();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a GameObject by id
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGameObjectByID(uint id)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].GetID() == id)
                { 
                    gameObjects.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            gameObjects.Remove(obj);
        }

        /// <summary>
        /// Returns GameObject by the inputted ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetGameObjectByID(uint id)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (id == gameObjects[i].GetID())
                    return gameObjects[i];
            }
            return null;
        }

        /// <summary>
        /// Returns true if GameObject with the same ID exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DoesGameObjectExist(uint id)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (id == gameObjects[i].GetID())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the List of all GameObjects
        /// </summary>
        /// <returns></returns>
        public List<GameObject> GetAllGameObjects()
        {
            return gameObjects;
        }

        /// <summary>
        /// Will update tick all existing GameObjects and does the collision between them
        /// </summary>
        public void TickAllGameObjects(MainWindow win)
        {
            for (int i = 0; i < gameObjects.Count; i++)
                if (gameObjects[i].isEnabled)
                    gameObjects[i].UpdateTick(win);
        }

        /// <summary>
        /// Sets the world inst
        /// </summary>
        /// <param name="world"></param>
        public static void SetWorldInst(World world)
        {
            WorldInst = world;
        }
    }
}
