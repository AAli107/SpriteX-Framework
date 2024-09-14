namespace SpriteX_Framework.FrameworkContents.Components
{
    /// <summary>
    /// Abstract class to create your own script components
    /// </summary>
    public abstract class ScriptComponent : Component
    {
        public ScriptComponent(GameObject parent) : base(parent) 
        {
            if (isEnabled) Awake();
        }

        public override void UpdateTick(MainWindow win)
        {
            base.UpdateTick(win);
            FixedUpdate(win);
        }

        /// <summary>
        /// executed at script base constructor
        /// </summary>
        public virtual void Awake() { }

        /// <summary>
        /// executed after script component gets added to game object
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// executed every frame
        /// </summary>
        /// <param name="win"></param>
        public virtual void Update(MainWindow win) { }

        /// <summary>
        /// executed fixed amount of times per second
        /// </summary>
        /// <param name="win"></param>
        public virtual void FixedUpdate(MainWindow win) { }

    }
}
