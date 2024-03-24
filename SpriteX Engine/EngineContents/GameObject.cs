using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SpriteX_Engine.EngineContents.Components;
using System.Drawing;
using System.Reflection;

namespace SpriteX_Engine.EngineContents
{
    public class GameObject
    {
        uint id;
        Vector2 position;

        bool isVisible = true;

        private List<Component> components = new List<Component>();

        public readonly List<Render> renders = new List<Render>(255);

        public event EventHandler<EventArgs> OnGameObjectUpdate;
        public event EventHandler<EventArgs> OnGameObjectSpawn;
        public event EventHandler<EventArgs> OnGameObjectRender;

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

        public Component AddComponent<T>() where T : Component
        {
            Type classType = typeof(T);
            ConstructorInfo constructor = classType.GetConstructor(new Type[] { typeof(GameObject) });
            Component component = (T)constructor.Invoke(new object[] { this });
            components.Add(component);
            if (component.isEnabled) if (component is ScriptComponent) (component as ScriptComponent).Start();
            return component;
        }

        public Component GetComponent<T>() where T : Component
        {
            return components.FirstOrDefault(c => c.GetType() == typeof(T));
        }

        public Component[] GetComponents<T>() where T : Component
        {
            return components.Where(c => c.GetType() == typeof(T)).ToArray();
        }

        public Component[] GetAllComponents()
        {
            return components.ToArray();
        }

        public void RemoveComponent(Component c)
        {
            components.Remove(c);
        }

        public void OnSpawn()
        {
            OnGameObjectSpawn?.Invoke(this, new EventArgs());
        }

        public void UpdateTick(MainWindow win)
        {
            OnGameObjectUpdate?.Invoke(this, new EventArgs());
            foreach (Component c in components) if (c.isEnabled) c.UpdateTick(win);
        }

        public void Render(MainWindow win, gfx gfx)
        {
            OnGameObjectRender?.Invoke(this, new EventArgs());
            foreach (Component c in components) if (c.isEnabled) c.RenderTick(win, gfx);

            if (isVisible) for (int i = 0; i < renders.Count; i++)
                renders[i].DrawRender(gfx, GetPosition());
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