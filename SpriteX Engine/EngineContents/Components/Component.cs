namespace SpriteX_Framework.EngineContents.Components
{
    /// <summary>
    /// Abstract base class of components used only to create other component classes.
    /// </summary>
    public abstract class Component
    {
        public GameObject parent;
        public bool isEnabled = true;

        public Component() {}

        public Component(GameObject parent)
        {
            this.parent = parent;
        }

        public virtual void UpdateTick(MainWindow win) 
        {
            if (parent == null || !isEnabled) return;
        }
        public virtual void RenderTick(MainWindow win, gfx gfx) 
        {
            if (parent == null || !isEnabled) return;
        }
    }
}