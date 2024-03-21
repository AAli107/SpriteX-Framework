namespace SpriteX_Engine.EngineContents.Components
{
    public abstract class Component
    {
        public GameObject parent;

        public Component(GameObject parent)
        {
            this.parent = parent;
        }

        public virtual void GameTick(MainWindow win) { }
        public virtual void RenderTick(MainWindow win, gfx gfx) { }
    }
}