using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents
{
    public class World
    {
        public Camera cam;
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<Audio> audios = new List<Audio>();

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

        public void PlayAudio(string path) 
        {
            Audio a = new Audio(path);
            audios.Add(a);
            a.Play();
        }

        public void WorldUpdate()
        {
            List<Audio> stoppedSounds = audios.FindAll(a => a.IsStopped());
            foreach (Audio a in stoppedSounds)
            {
                a.Dispose();
                audios.Remove(a);
            }

            if (audios.Count > 500)
            {
                audios[0].Dispose();
                audios.RemoveAt(0);
            }
        }

        /// <summary>
        /// Will spawn this GameObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool InstantiateGameObject(GameObject obj)
        {
            uint id = 0;
            while (gameObjects.Any(o => o.GetID() == id))
            {
                id++;
            }
            if (obj.SetID(id, this))
            {
                gameObjects.Add(obj);
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
        public void TickAllGameObjects()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject obj = gameObjects[i];
                obj.UpdateTick();

                if (!obj.IsCollisionEnabled())
                    continue;

                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    GameObject obj2 = gameObjects[j];
                    if (!obj2.IsCollisionEnabled())
                        continue;

                    if (obj.GetVelocity().Length + obj2.GetVelocity().Length <= 0)
                        continue;

                    if (!obj.IsIntersectingWith(obj2))
                        continue;

                    Vector2 cv = CalculateCollisionVector(obj, obj2);

                    if (obj.IsSimulatingPhysics())
                    {
                        obj.SetPosition(obj.GetPosition() + cv);
                        obj.OverrideVelocity(obj.GetVelocity() + (cv / (obj2.IsSimulatingPhysics() ? obj.GetMass() : 1f)));
                    }

                    if (obj2.IsSimulatingPhysics())
                        obj2.OverrideVelocity(obj2.GetVelocity() - (cv / (obj.IsSimulatingPhysics() ? obj2.GetMass() : 1f)));
                }
            }
        }

        // collision vector code
        private Vector2 CalculateCollisionVector(GameObject obj1, GameObject obj2)
        {
            Vector2 center1 = obj1.GetCenterPosition();
            Vector2 center2 = obj2.GetCenterPosition();

            Vector2 halfSize1 = obj1.GetSize() * 0.5f;
            Vector2 halfSize2 = obj2.GetSize() * 0.5f;

            float diffX = center1.X - center2.X;
            float diffY = center1.Y - center2.Y;

            float overlapX = halfSize1.X + halfSize2.X - Math.Abs(diffX);
            float overlapY = halfSize1.Y + halfSize2.Y - Math.Abs(diffY);

            float collisionX = overlapX < overlapY ? (center1.X < center2.X ? -overlapX : overlapX) : 0;
            float collisionY = overlapX < overlapY ? 0 : (center1.Y < center2.Y ? -overlapY : overlapY);

            return new Vector2(collisionX, collisionY);
        }
    }
}
