using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace SpriteX_Framework.FrameworkContents
{
    public class World
    {
        public static World WorldInst { get; private set; }

        public Camera cam;
        public Dictionary<string, GameObject> gameObjects = new();
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
        public string SpawnGameObject(GameObject obj, Vector2d position = default)
        {
            string uuid = Guid.NewGuid().ToString();
            obj.SetPosition(position);
            gameObjects.Add(uuid, obj);
            obj.Spawn();
            return uuid;
        }

        public GameObject SpawnEmptyGameObject(Vector2d position = default)
        {
            GameObject gameObject = new();
            SpawnGameObject(gameObject, position);
            return gameObject;
        }

        /// <summary>
        /// Removes a GameObject by uuid
        /// </summary>
        /// <param name="uuid"></param>
        public void RemoveGameObjectByID(string uuid)
        {
            if (gameObjects.ContainsKey(uuid))
                gameObjects.Remove(uuid);
        }

        /// <summary>
        /// Removes a GameObject
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            foreach (var keyValue in gameObjects)
                if (keyValue.Value.Equals(obj))
                    gameObjects.Remove(keyValue.Key);
        }

        /// <summary>
        /// Returns GameObject by the inputted UUID
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public GameObject GetGameObjectByID(string uuid)
        {
            return gameObjects[uuid];
        }

        /// <summary>
        /// Returns true if GameObject with the same UUID exists
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public bool DoesGameObjectExist(string uuid)
        {
            return gameObjects.ContainsKey(uuid);
        }

        /// <summary>
        /// Returns true if given GameObject exists
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool DoesGameObjectExist(GameObject gameObject)
        {
            GameObject[] _gameObjects = new GameObject[gameObjects.Count];
            gameObjects.Values.CopyTo(_gameObjects, 0);
            foreach (GameObject gameObj in _gameObjects)
            {
                if (gameObj.Equals(gameObject))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Will update tick all existing GameObjects and does the collision between them
        /// </summary>
        public void TickAllGameObjects(MainWindow win)
        {
            foreach (var gameObj in gameObjects.Values)
                if (gameObj.isEnabled)
                    gameObj.UpdateTick(win);
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
