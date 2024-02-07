namespace SpriteX_Engine.EngineContents
{
    internal class GameScript
    {
        /// <summary>
        /// Gets Executed when game starts running, after the main Window loads
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameStart(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions
        /// </summary>
        /// <param name="win"></param>
        public virtual void OnGameUpdate(MainWindow win) { }
        /// <summary>
        /// Gets Executed every frame, used for Game related actions (cannot be paused)
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
