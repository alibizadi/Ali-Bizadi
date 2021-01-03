
/*Nibbles Game Made By Ali Bizadi [alibizi]
 Commect By The Author: C# is good but I more Love C++*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nibbles
{
    public partial class Form1 : Form
    {
        List<Square> Snakes = new List<Square>();
        Square fruit;

        public int keyChar = 0;

        public Form1()
        {
            InitializeComponent();

            new Settings();

            timer1.Interval = Settings.Speed;
            timer1.Tick += new EventHandler(UpdateScreen);
            timer1.Start();

            screen.Width = 20 * Settings.Width;
            screen.Height = 20 * Settings.Height;

            StartGame();
        }

        private void StartGame()
        {
            resaultLBL.Visible = false;

            new Settings();
            timer1.Interval = Settings.Speed;

            Snakes.Clear();

            Square head = new Square();
            head.X = screen.Width / 2;
            head.Y = screen.Height / 2;
            Snakes.Add(head);

            GenerateFruit();
        }

        private void GenerateFruit()
        {
            int maxX = screen.Width;
            int maxY = screen.Height;

            Random rand = new Random();
            fruit = new Square();
            bool flag1 = false, flag2 = false;
            while (true)
            {
                int x = rand.Next(0, maxX);
                int y = rand.Next(0, maxY);
                if (x % Settings.Width == 0)
                {
                    fruit.X = x;
                    flag1 = true;
                }
                if (y % Settings.Height == 0)
                {
                    fruit.Y = y;
                    flag2 = true;
                }

                for(int i = 0; i < Snakes.Count; i++)
                {
                    if (x == Snakes[i].X && y == Snakes[i].Y)
                    {
                        flag1 = false;
                        break;
                    }
                }

                if (flag1 && flag2)
                    break;
            }
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (keyChar == 5)
                {
                    StartGame();
                    keyChar = 0;
                }
            }
            else
            {
                if (keyChar == 3/*100*/ && Settings.Dir != Direction.Left)         //RightArrow
                    Settings.Dir = Direction.Right;
                else if (keyChar == 1/*97*/ && Settings.Dir != Direction.Right)    //LeftArrow
                    Settings.Dir = Direction.Left;
                else if (keyChar == 2/*119*/ && Settings.Dir != Direction.Down)    //UpArrow
                    Settings.Dir = Direction.Up;
                else if (keyChar == 4/*115*/ && Settings.Dir != Direction.Up)      //DownArrow
                    Settings.Dir = Direction.Down;
                else if (keyChar == 6)
                    Settings.GameOver = true;

                MoveSnake();
            }

            screen.Invalidate();
        }

        private void Screen_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                for(int i = 0; i < Snakes.Count; i++)
                {
                    canvas.FillRectangle(Brushes.DarkGray, new Rectangle(Snakes[i].X,
                        Snakes[i].Y, Settings.Width, Settings.Height));

                    canvas.FillEllipse(Brushes.Red, new Rectangle(fruit.X,
                        fruit.Y, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string resaultMSG = "GameOver :)\nYou Earned " + Settings.Score + " SCORE\nPress ENTER For New PLaying.";

                resaultLBL.Text = resaultMSG;
                resaultLBL.Visible = true;
            }
        }

        private void MoveSnake()
        {
            for (int i = Snakes.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.Dir)
                    {
                        case Direction.Down:
                            Snakes[i].Y += Settings.Height;
                            break;
                        case Direction.Up:
                            Snakes[i].Y -= Settings.Height;
                            break;
                        case Direction.Right:
                            Snakes[i].X += Settings.Width;
                            break;
                        case Direction.Left:
                            Snakes[i].X -= Settings.Width;
                            break;
                    }

                    int maxX = screen.Width;
                    int maxY = screen.Height;

                    if(Snakes[i].X < 0 || Snakes[i].Y < 0
                        || Snakes[i].X >= maxX || Snakes[i].Y >= maxY)
                    {
                        Settings.GameOver = true;
                    }

                    for(int j = 1; j < Snakes.Count; j++)
                    {
                        if (Snakes[i].X == Snakes[j].X && Snakes[i].Y == Snakes[j].Y)
                        {
                            Settings.GameOver = true;
                        }
                    }

                    if (Snakes[0].X == fruit.X && Snakes[0].Y == fruit.Y)
                    {
                        EatFruit();
                    }
                }
                else
                {
                    Snakes[i].X = Snakes[i - 1].X;
                    Snakes[i].Y = Snakes[i - 1].Y;
                }
            }
        }

        private void EatFruit()
        {
            Square tail = new Square();
            tail.X = Snakes[Snakes.Count - 1].X;
            tail.Y = Snakes[Snakes.Count - 1].Y;
            Snakes.Add(tail);

            Settings.Score += Settings.Points;
            scoreLBL.Text = "SCORE = " + Settings.Score.ToString();

            if (Settings.Speed > 70)
            {
                Settings.Speed -= 5;
                timer1.Interval = Settings.Speed;
            }

            GenerateFruit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                keyChar = 3;
            else if (e.KeyCode == Keys.Left)
                keyChar = 1;
            else if (e.KeyCode == Keys.Up)
                keyChar = 2;
            else if (e.KeyCode == Keys.Down)
                keyChar = 4;
            else if (e.KeyCode == Keys.Enter)
                keyChar = 5;
            else if (e.KeyCode == Keys.Escape)
                keyChar = 6;
        }
    }
}
