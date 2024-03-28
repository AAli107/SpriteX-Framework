using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteX_Engine.EngineContents
{
    public static class InitialGameWindowConfig
    {
        public static readonly string Title = "SpriteX Game"; // Window Title
        public static readonly Vector2i ClientSize = new Vector2i(1280, 720); // Default Window Resolution when not in fullscreen
        public static readonly (int numerator, int denominator) AspectRatio = (16, 9); // Window Aspect Ratio
        public static readonly WindowBorder WindowBorder = WindowBorder.Resizable; // Window Border type
        public static readonly WindowState WindowState = WindowState.Maximized; // Decides window state (can be used to set fullscreen) 
        public static readonly double UpdateFrequency = 120; // Window Framerate (setting to 0 will unlock FPS if VSync is off)
        public static readonly double fixedFrameTime = 60; // How many times per second the game updates
        public static readonly VSyncMode VSync = VSyncMode.On; // Control the window's VSync
        public static readonly Color bgColor = Color.Black; // Controls the windows background color
        public static readonly bool allowAltEnter = true; // Controls whether you can toggle fullscreen when pressing Alt+Enter
        public static readonly bool showDebugHitbox = true; // Controls whether to show all GameObjects' hitboxes
        public static readonly bool showStats = true; // Displays FPS and UpdateTime(ms) Stat
        public static readonly Font font = Font.GetDefaultFont(); // Contains game font
        public static readonly GameLevelScript startLevel = new GameCode(); // The Level to load when game launches
    }
}
