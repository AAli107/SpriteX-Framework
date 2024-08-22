using OpenTK.Mathematics;
using SpriteX_Engine.EngineContents.Components;
using System.Reflection;

namespace SpriteX_Engine.EngineContents
{
    public class GameObject
    {
        uint id;
        Vector2 position;

        private List<Component> components = new List<Component>();

        public event EventHandler<EventArgs> OnUpdate;
        public event EventHandler<EventArgs> OnSpawn;
        public event EventHandler<EventArgs> OnRender;
        public bool isEnabled = true;

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        /// <param name="position"></param>
        public GameObject(Vector2 position)
        {
            Construct(position);
        }

        void Construct(Vector2 position)
        {
            this.position = position;
        }

        public T AddComponent<T>() where T : Component
        {
            Type classType = typeof(T);
            ConstructorInfo constructor = classType.GetConstructor(new Type[] { typeof(GameObject) });
            Component component = (T)constructor.Invoke(new object[] { this });
            components.Add(component);
            if (component.isEnabled) if (component is ScriptComponent) (component as ScriptComponent).Start();
            return component as T;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == typeof(T))
                    return components[i] as T;
            }
            return null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            List<T> comps = new List<T>();
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == typeof(T))
                    comps.Add(components[i] as T);
            }
            return comps.ToArray();
        }

        public Component[] GetAllComponents()
        {
            Component[] _components = new Component[components.Count];
            components.CopyTo(_components);
            return _components;
        }

        public void RemoveComponent(Component c)
        {
            components.Remove(c);
        }

        public void Spawn()
        {
            OnSpawn?.Invoke(this, new EventArgs());
        }

        public void UpdateTick(MainWindow win)
        {
            if (isEnabled)
            {
                OnUpdate?.Invoke(this, new EventArgs());
                foreach (Component c in components) if (c.isEnabled) c.UpdateTick(win);
            }
        }

        public void Render(MainWindow win, gfx gfx)
        {
            OnRender?.Invoke(this, new EventArgs());
            foreach (Component c in components) if (c.isEnabled) c.RenderTick(win, gfx);
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
        /// Returns GameObject's ID
        /// </summary>
        /// <returns></returns>
        public uint GetID() { return id; }

        /// <summary>
        /// Returns GameObject's position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition() { return position; }
    }
}