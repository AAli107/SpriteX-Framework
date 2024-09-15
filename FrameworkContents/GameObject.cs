using OpenTK.Mathematics;
using SpriteX_Framework.FrameworkContents.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpriteX_Framework.FrameworkContents
{
    public class GameObject
    {
        Vector2d position;

        private List<Component> components = new List<Component>();

        public event EventHandler<EventArgs> OnUpdate;
        public event EventHandler<EventArgs> OnSpawn;
        public event EventHandler<EventArgs> OnRender;
        public bool isEnabled = true;

        /// <summary>
        /// Creates a GameObject
        /// </summary>
        public GameObject() { }

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
            List<T> comps = new();
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
        /// Sets GameObject's Position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2d position)
        {
            this.position = position;
        }

        /// <summary>
        /// Returns GameObject's position
        /// </summary>
        /// <returns></returns>
        public Vector2d GetPosition() { return position; }

        /// <summary>
        /// Will try and get UUID of game object. It will work only when it is spawned in the world
        /// </summary>
        /// <returns></returns>
        public string GetUUID()
        {
            foreach (var item in World.WorldInst.gameObjects)
            {
                if (item.Value.Equals(this))
                    return item.Key;
            }
            return null;
        }
    }
}