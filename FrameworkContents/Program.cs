using OpenTK.Windowing.Desktop;

namespace SpriteX_Framework.FrameworkContents
{
    internal class Program
    {
        static void Main()
        {
            /*
            *   If you wanna use the framework properly, go to SampleLevel, not here!
            *                                                   See ya! :3
            */

            // Will create and run the Main Window
            using var win = new MainWindow(new GameWindowSettings(), new NativeWindowSettings());
            win.Run();

        }
    }
}