namespace SpriteX_Engine.EngineContents
{
    internal class GameScript
    {
        /// <summary>
        /// Gets Executed right after the Game Window starts loading
        /// </summary>
        public virtual void Awake(MainWindow win) { }
        /// <summary>
        /// Gets Executed when game starts running, after the game Window finishes loading
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameStart(MainWindow win) { }
        /// <summary>
        /// Gets Executed before Physics and collision updates for fixed amount of time per second
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnPrePhysicsUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed fixed amount of time per second
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnFixedGameUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions (cannot be paused; executed right after OnGameUpdate())
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameUpdateNoPause(MainWindow win) { }
        /// <summary>
        /// Used to put everything related to rendering stuff
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGraphicsUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed when game is about to close
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameEnd(MainWindow win) { }
    }
}
