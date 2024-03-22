namespace SpriteX_Engine.EngineContents.Components
{
    /// <summary>
    /// Abstract base class of components used only to create other component classes.
    /// </summary>
    public abstract class Component
    {
        public GameObject parent;
        public bool isEnabled = true;

        public Component(GameObject parent)
        {
            this.parent = parent;
        }

        public virtual void UpdateTick(MainWindow win) { }
        public virtual void RenderTick(MainWindow win, gfx gfx) { }
    }
}