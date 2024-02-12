using OpenTK.Windowing.Desktop;

namespace SpriteX_Engine.EngineContents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            *   If you wanna use the engine properly, go to GameCode, not here!
            *                                                   See ya! :3
            */

            // Will create and run the Main Window
            using (var window = new MainWindow(new GameWindowSettings(), new NativeWindowSettings()))
            {
                window.Run();
            }

        }
    }
}