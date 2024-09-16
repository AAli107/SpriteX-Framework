namespace SpriteX_Framework.FrameworkContents
{
    /// <summary>
    /// GameLevelScript is an abstract class used to make levels.
    /// To make a level, create a new class that uses GameLevelScript as base class.
    /// </summary>
    public abstract class GameLevelScript
    {
        /// <summary>
        /// Awake is called before the world is loaded
        /// </summary>
        public virtual void Awake(MainWindow win) { }
        /// <summary>
        /// LevelStart is called when the level begins
        /// </summary>
        /// <param name="win"></param>
        public virtual void LevelStart(MainWindow win) { }
        /// <summary>
        /// PrePhysicsUpdate is called before Physics and collision updates for fixed amount of times per second
        /// </summary>
        /// <param name="win"></param>
        public virtual void PrePhysicsUpdate(MainWindow win) { }
        /// <summary>
        /// FixedUpdate is called for fixed amount of times per second (default: 60 per second)
        /// </summary>
        /// <param name="win"></param>
        public virtual void FixedUpdate(MainWindow win) { }
        /// <summary>
        /// GameUpdate is called once per frame as long as the game is not paused
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameUpdate(MainWindow win) { }
        /// <summary>
        /// GameUpdateNoPause is called once per frame (cannot be paused; executed right after GameUpdate)
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameUpdateNoPause(MainWindow win) { }
        /// <summary>
        /// GraphicsUpdate is called once per frame to render graphics
        /// </summary>
        /// <param name="win"></param>
        /// <param name="gfx"></param>
        public virtual void GraphicsUpdate(MainWindow win, gfx gfx) { }
        /// <summary>
        /// LevelEnd is called right before the currently running level gets switched
        /// </summary>
        /// <param name="win"></param>
        public virtual void LevelEnd(MainWindow win) { }
        /// <summary>
        /// GameEnd is called when the game is about to close
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameEnd(MainWindow win) { }
    }
}
