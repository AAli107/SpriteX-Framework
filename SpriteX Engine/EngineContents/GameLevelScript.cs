namespace SpriteX_Engine.EngineContents
{
    /// <summary>
    /// GameLevelScript is an abstract class used to make levels.
    /// To make a level, create a new class that uses GameLevelScript as base class.
    /// </summary>
    public abstract class GameLevelScript
    {
        /// <summary>
        /// Gets Executed before the world is loaded
        /// </summary>
        public virtual void Awake(MainWindow win) { }
        /// <summary>
        /// Gets Executed after the world is loaded
        /// </summary>
        /// <param name="win"></param>
        public virtual void LevelStart(MainWindow win) { }
        /// <summary>
        /// Gets Executed before Physics and collision updates for fixed amount of time per second
        /// </summary>
        /// <param name="win"></param>
        public virtual void PrePhysicsUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed fixed amount of time per second after Physics and collision updates
        /// </summary>
        /// <param name="win"></param>
        public virtual void FixedUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions (cannot be paused; executed right after OnGameUpdate())
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameUpdateNoPause(MainWindow win) { }
        /// <summary>
        /// Used for rendering shapes and images on screen
        /// </summary>
        /// <param name="win"></param>
        public virtual void GraphicsUpdate(MainWindow win, gfx gfx) { }
        /// <summary>
        /// Gets Executed when current level is about to be switched
        /// </summary>
        /// <param name="win"></param>
        public virtual void LevelEnd(MainWindow win) { }
        /// <summary>
        /// Gets Executed when game is about to close
        /// </summary>
        /// <param name="win"></param>
        public virtual void GameEnd(MainWindow win) { }
    }
}
