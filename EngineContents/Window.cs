﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace SpriteX_Engine.EngineContents
{
    public partial class Window : Form
    {
        public Timer timer = new Timer();
        public Graphics graphics;

        public Window()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size((int)Engine.resolution.X, (int)Engine.resolution.Y); // Sets Window resolution

            DoubleBuffered = true; // Enabled Double Buffering

            Text = Engine.gameTitle; // Sets the Window title

            MaximizeBox = false; // Disables the maximize button
            
            graphics = CreateGraphics(); // Creates the graphics

            gfx.graphics.InterpolationMode = InterpolationMode.NearestNeighbor; // Disables blurring of scaled textures

            GameCode.OnGameStart(); // Calls GameCode.OnGameStart() when the game begins to run

            // Starts the game loop
            timer.Interval = 1; // Sets the framerate the game loop will run in
            timer.Tick += new EventHandler(Engine.Tick);
            timer.Start();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Controller.mousePos = new Vector2(e.X, e.Y);
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            Controller.pressedMouseButtons.Add(e.Button);
        }

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Controller.pressedMouseButtons.Count; i++)
            {
                if (Controller.pressedMouseButtons[i] == e.Button)
                    Controller.pressedMouseButtons.RemoveAt(i);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Controller.IsKeyPressed(e.KeyCode))
                Controller.pressedkeys.Add(e.KeyCode);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < Controller.pressedkeys.Count; i++)
            {
                if (Controller.pressedkeys[i] == e.KeyCode)
                    Controller.pressedkeys.RemoveAt(i);
            }
        }
    }
}
