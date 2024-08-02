namespace SpriteX_Engine.EngineContents
{
    public class World
    {
        public Camera cam;
        public List<GameObject> gameObjects = new List<GameObject>();
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
        public bool SpawnGameObject(GameObject obj)
        {
            uint id = 0;

            while (gameObjects.Any(o => o.GetID() == id)) id++;

            if (obj.SetID(id, this))
            {
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
            gameObjects.RemoveAll(o => o.GetID() == id);
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
            try { return gameObjects.Single(o => o.GetID() == id); }
            catch { return null; }
        }

        /// <summary>
        /// Returns true if GameObject with the same ID exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DoesGameObjectExist(uint id)
        {
            return gameObjects.Any(o => o.GetID() == id);
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
                gameObjects[i].UpdateTick(win);
        }
    }
}
