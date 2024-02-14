using OpenTK.Mathematics;

namespace SpriteX_Engine.EngineContents
{
    public class World
    {
        public Camera cam;
        public List<GameObject> gameObjects;

        public World()
        {
            cam = new Camera();
            gameObjects = new List<GameObject>();
        }

        public World(Camera cam)
        {
            this.cam = cam;
            gameObjects = new List<GameObject>();
        }

        ~World()
        {
            gameObjects.Clear();
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
            List<GameObject> collidableGameObjects = gameObjects.FindAll(o => o.IsCollisionEnabled());
            foreach (GameObject obj in collidableGameObjects)
            {
                obj.UpdateTick();
                if (collidableGameObjects.Count > 1)
                {
                    foreach (GameObject obj2 in collidableGameObjects)
                    {
                        if (obj != obj2 && obj.IsCollisionEnabled() && obj2.IsCollisionEnabled() && obj.GetVelocity().Length + obj2.GetVelocity().Length > 0 && obj.IsIntersectingWith(obj2))
                        {
                            // Calculate the collision vector based on the GameObjects' size and position
                            Vector2 cv = CalculateCollisionVector(obj, obj2);

                            // Move the objects apart along the MTV to prevent overlapping
                            if (obj.IsSimulatingPhysics()) obj.SetPosition(obj.GetPosition() + cv);

                            // Pushes Colliding GameObjects if simulating physics, pushing force depending on their mass
                            if (obj.IsSimulatingPhysics()) obj.OverrideVelocity(obj.GetVelocity() + (cv / (obj2.IsSimulatingPhysics() ? obj.GetMass() : 1f)));
                            if (obj2.IsSimulatingPhysics()) obj2.OverrideVelocity(obj2.GetVelocity() - (cv / (obj.IsSimulatingPhysics() ? obj2.GetMass() : 1f)));
                        }
                    }
                }
            }
        }

        // collision vector code
        private Vector2 CalculateCollisionVector(GameObject obj1, GameObject obj2)
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
