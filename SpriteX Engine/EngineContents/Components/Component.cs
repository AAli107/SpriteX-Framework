/*
 * Abstract base class of components used only to create other component classes.
 */
namespace SpriteX_Engine.EngineContents.Components
{
    public abstract class Component
    {
        public GameObject parent;

        public Component(GameObject parent)
        {
            this.parent = parent;
        }

        public virtual void UpdateTick(MainWindow win) { }
        public virtual void RenderTick(MainWindow win, gfx gfx) { }
    }
}