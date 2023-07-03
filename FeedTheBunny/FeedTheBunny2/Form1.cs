using FeedTheBunny.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FeedTheBunny.Form1;

namespace FeedTheBunny
{

    public partial class Form1 : Form
    {
        private PictureBox rabbitPictureBox;
        private List<FallingObject> fallingObjects;
        private int RabbitSpeed = 10;
        private Random random = new Random();
        public int misses;
        public int points;
        public int timerTick = 0;
        private bool isRedbullActive;
        private int redbullTimer;
        int spawnInterval = 20;
        bool goLeft = false;
        bool goRight = false;
        int jumps = 0;
        int carrotCount = 0;
        int lives = 3;
        int cabbageCount = 0;
        public int level = 1;
        private bool isGameRunning = false;
        private SoundPlayer catchSound;
        private SoundPlayer redbullSound;
        private SoundPlayer bombSound;
        private SoundPlayer loseSound;
        private SoundPlayer lifeGain;
        private SoundPlayer startGame;
        private SoundPlayer jump;


        public Form1()
        {
            InitializeComponent();
            InitializeGame();

            catchSound = new SoundPlayer("quick-pop.wav");
            lifeGain = new SoundPlayer("lifeGain.wav");
            redbullSound = new SoundPlayer("redbull.wav");
            bombSound = new SoundPlayer("Small Bomb Explosion Sound Effect.wav");
            loseSound = new SoundPlayer("Lose sound effects.wav");
            startGame = new SoundPlayer("startGame.wav");
            jump = new SoundPlayer("jump.wav");
            startGame.Play();
            DoubleBuffered = true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (!isGameRunning)
                {
                    StartGame();
                    isGameRunning = true;
                }
                return true;
            }
            else if (keyData == Keys.P)
            {
                if (isGameRunning)
                {
                    PauseGame();
                    isGameRunning = false;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void StartGame()
        {
            timer1.Start();
            label3.Text = "";
            isGameRunning = true;

        }

        private void PauseGame()
        {
            timer1.Stop();          
            label3.Text = "PRESS ENTER\nTO UNPAUSE\nPRESS R FOR RULES";
            label3.Location = new Point((ClientSize.Width / 2) - label3.Width / 2, (ClientSize.Height / 2) - label3.Height);
            isGameRunning = false;

        }
        public void InitializeGame()
        {
            rabbitPictureBox = new PictureBox();
            rabbitPictureBox.Image = Properties.Resources.rabbit;
            rabbitPictureBox.Size = new Size(100, 100);
            rabbitPictureBox.Location = new Point((ClientSize.Width - 100) / 2, ClientSize.Height - 120);
            rabbitPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            rabbitPictureBox.BackColor = Color.Transparent;
            Controls.Add(rabbitPictureBox);
            fallingObjects = new List<FallingObject>();
            label3.Text = "PRESS ENTER \r\nTO START GAME\r\nPRESS R FOR RULES\r\n";
            label3.Location = new Point((ClientSize.Width / 2)-label3.Width/2, (ClientSize.Height / 2)-label3.Height);
            timer1.Start();
            jumps = 0;
            carrotCount = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = $"Points: {points}";
            label2.Text = $"Jumps: {jumps}";
            if (isGameRunning)
            { 
                timerTick++;
                if (timerTick % spawnInterval == 0)
                {
                    SpawnObject();
                }
                if (goLeft)
                {
                    if (rabbitPictureBox.Left > 0)
                    {
                        rabbitPictureBox.Left -= RabbitSpeed;
                    }
                }
                if (goRight)
                {
                    if (rabbitPictureBox.Right < ClientSize.Width)
                    {
                        rabbitPictureBox.Left += RabbitSpeed;
                    }
                }
                CheckLevel();
                MoveObjects();
                CheckCollision();
                UpdateRedbull();
                CheckGameOver();
                CheckLives();
            }
          
        }

        private void CheckLevel()
        {
            if (level < 6)
            {
                level = points / 250;
            }               
            switch (level)
            {
                case 0: this.BackColor = Color.LightGreen; break;
                case 1: this.BackColor = Color.LightBlue; break;
                case 2: this.BackColor = Color.Orange; spawnInterval = 17; break;
                case 3: this.BackColor = Color.PaleVioletRed; spawnInterval = 15; break;
                case 4: this.BackColor = Color.IndianRed; spawnInterval = 12; break;
                default: this.BackColor = Color.OrangeRed; spawnInterval = 10; break;
            }
        }
        

        public void CheckLives()
        {
            if (lives == 3)
            {
                pictureBox1.Image = Properties.Resources.heart_pixel_art_254x254;
                pictureBox2.Image = Properties.Resources.heart_pixel_art_254x254;
                pictureBox3.Image = Properties.Resources.heart_pixel_art_254x254;
            }
            if (lives == 2)
            {
                pictureBox1.Image = Properties.Resources.heart_pixel_art_254x254;
                pictureBox2.Image = Properties.Resources.heart_pixel_art_254x254;
                pictureBox3.Image = Properties.Resources.blackHeart;

            } else if(lives == 1)
            {
                pictureBox1.Image = Properties.Resources.heart_pixel_art_254x254;
                pictureBox2.Image = Properties.Resources.blackHeart;
                pictureBox3.Image = Properties.Resources.blackHeart;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                
                if (rabbitPictureBox.Left > 0)
                {
                    goLeft = true;
                    goRight = false;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
               
                if (rabbitPictureBox.Right < ClientSize.Width)
                {
                    goRight = true;
                    goLeft = false;
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                if (jumps > 0)
                {
                    
                    if (goLeft && rabbitPictureBox.Location.X - 150 > 50)
                    {
                        jump.Play();
                        rabbitPictureBox.Location = new Point(rabbitPictureBox.Location.X - 150, rabbitPictureBox.Location.Y);
                        jumps--;
                    }
                    if (goRight && rabbitPictureBox.Location.X + 150 < ClientSize.Width - 65)
                    {
                        jump.Play();
                        rabbitPictureBox.Location = new Point(rabbitPictureBox.Location.X + 150, rabbitPictureBox.Location.Y);
                        jumps--;
                    }
                    
                }
                
            }
            if(e.KeyCode == Keys.R) 
            {
                if (!isGameRunning)
                {
                    MessageBox.Show($"Navigate with LEFT and RIGHT keys\nPress SPACE to jump\n5 carrots = 1 jump\n5 cabbages = 1 life\nPress P to pause\nPress ESCAPE to restart", "Rules", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (e.KeyCode == Keys.Escape) {
                isGameRunning = false;
                DialogResult answer = MessageBox.Show($"Do you want to restart?", "Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(answer == DialogResult.Yes)
                {
                    RestartGame();
                } else
                {
                    PauseGame();
                }
            }
            
        }
        private void CheckCollision()
        {
            foreach (FallingObject fallingObject in fallingObjects)
            {
                if (rabbitPictureBox.Bounds.IntersectsWith(fallingObject.Bounds))
                {
                    switch (fallingObject)
                    {
                        case Carrot carrot:
                            catchSound.Play();
                            points += 5;
                            carrotCount++;
                            if (carrotCount % 5 == 0)
                            {
                                jumps++;
                            }
                            break;
                        case Cabbage cabbage:
                            catchSound.Play();
                            points += 2;
                            cabbageCount++;
                            if ( cabbageCount % 5 == 0)
                            {
                                if (lives < 3)
                                {
                                    lives++;
                                    lifeGain.Play();
                                }
                            }
                            break;
                        case Bomb bomb:
                            bombSound.Play();
                            misses += 5;
                            lives--;
                            break;
                        case Redbull redbull:
                            isRedbullActive = true;
                            redbullSound.Play();
                            redbullTimer = 250;
                            if (rabbitPictureBox.Image == Properties.Resources.rabbitLeft)
                            {
                                rabbitPictureBox.Image = Properties.Resources.rabbitRedBullLeft;
                                isRedbullActive = true;
                            }
                            else if (rabbitPictureBox.Image == Properties.Resources.rabbit)
                            {
                                rabbitPictureBox.Image = Properties.Resources.rabbitRedBullRight;
                                isRedbullActive = true;
                            }
                            UpdateRedbull();
                            break;
                    }

                    fallingObject.Dispose();
                }
            }

            fallingObjects.RemoveAll(f => rabbitPictureBox.Bounds.IntersectsWith(f.Bounds));
        }
        private void CheckGameOver()
        {
            if (lives == 0)
            {
                timer1.Stop();
                loseSound.Play();
                DialogResult result = MessageBox.Show($"Game Over! Your score: {points}\nDo you want to play again?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    RestartGame();
                }
                else
                {
                    Close();
                }
            }
        }

        private void RestartGame()
        {
            timerTick = 0;
            misses = 0;
            points = 0;
            redbullTimer = 0;
            isRedbullActive = false;
            lives = 3;
            jumps = 0;
            carrotCount = 0;
            cabbageCount = 0;
            level = 1;
            spawnInterval = 20;
            isGameRunning = false;
            rabbitPictureBox.Location = new Point((ClientSize.Width - 100) / 2, ClientSize.Height - 120);
            rabbitPictureBox.Image = Properties.Resources.rabbit;
            rabbitPictureBox.Size = new Size(100, 100);
            rabbitPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            rabbitPictureBox.BackColor = Color.Transparent;
            Controls.Add(rabbitPictureBox);
            fallingObjects.ForEach(obj => { obj.Dispose(); });
            fallingObjects = new List<FallingObject>();
            timer1.Start();
            label3.Text = "PRESS ENTER \r\nTO START GAME\r\nPRESS R FOR RULES\r\n";
            label3.Location = new Point((ClientSize.Width / 2) - label3.Width / 2, (ClientSize.Height / 2) - label3.Height);
            this.BackColor = Color.LightGreen;
            pictureBox1.Image = Properties.Resources.heart_pixel_art_254x254;
            pictureBox2.Image = Properties.Resources.heart_pixel_art_254x254;
            pictureBox3.Image = Properties.Resources.heart_pixel_art_254x254;
        }
        private void SpawnObject()
        {
            int k = random.Next(1, 22);
            Point point = new Point(random.Next(1, ClientSize.Width-60), 0);
            if (k < 13)
            {
                Carrot c = new Carrot(point);
                c.Speed += level;
                if (points > 1500)
                {
                    c.Speed++;
                }
                fallingObjects.Add(c);
                Controls.Add(c.GetPictureBox()); 
            }
            else if (k >= 13 && k < 16)
            {
                Cabbage cabbage = new Cabbage(point);
                cabbage.Speed += level;
                if (points > 1500)
                {
                    cabbage.Speed++;
                }
                fallingObjects.Add(cabbage);
                Controls.Add(cabbage.GetPictureBox()); 
            }
            else if (k >= 16 && k < 21)
            {
                Bomb bomb = new Bomb(point);
                bomb.Speed += level;
                if (points > 1500)
                {
                    bomb.Speed++;
                }
                fallingObjects.Add(bomb);
                Controls.Add(bomb.GetPictureBox()); 
            }
            else
            {
                Redbull rd = new Redbull(point);
                rd.Speed += level;
                if (points > 1500)
                {
                    rd.Speed++;
                }
                fallingObjects.Add(rd);
                Controls.Add(rd.GetPictureBox()); 
            }
        }
        private void UpdateRedbull()
        {
            if (isRedbullActive)
            {
                rabbitPictureBox.Size = new Size(120, 100);
                if (goLeft)
                {
                    rabbitPictureBox.Image = Resources.rabbitRedBullLeft;
                }
                if (goRight)
                {
                    rabbitPictureBox.Image= Resources.rabbitRedBullRight;
                }
                redbullTimer--;
                RabbitSpeed = 20;
                if (redbullTimer == 0)
                {
                    isRedbullActive = false;
                    
                }
            } else if (!isRedbullActive)
            {
                if (goLeft)
                {
                    rabbitPictureBox.Image = Properties.Resources.rabbitLeft;
                }
                else if (goRight)
                    rabbitPictureBox.Image = Properties.Resources.rabbit;
                RabbitSpeed = 10;
                rabbitPictureBox.Size = new Size(100, 100);
            }
        }
        private void MoveObjects()
        {
            int i = 0;
            foreach (FallingObject fallingObject in fallingObjects)
            {
                i++;
                fallingObject.UpdatePosition();

                if (fallingObject.Bounds.Bottom >= ClientSize.Height)
                {
                    if (fallingObject is Bomb)
                    {
                        fallingObject.Dispose();
                        break;
                    }
                    misses += 1;
                    fallingObject.Dispose();
                }
            }

            fallingObjects.RemoveAll(f => f.Bounds.Bottom >= ClientSize.Height);
        }

        public abstract class FallingObject : IDisposable
        {
            protected PictureBox pictureBox;
            public abstract int Speed { get; set;  }
            public abstract Image GetImage();
            public abstract Rectangle Bounds { get; }
            public abstract void UpdatePosition();
            public PictureBox GetPictureBox() => pictureBox;
            public abstract void Dispose();
            
        }

        public class Carrot : FallingObject
        {
            Random random = new Random();
            private int speed = 5;
            public Carrot(Point point)
            {
                pictureBox = new PictureBox();
                pictureBox.Image = Properties.Resources.carrot;
                pictureBox.Size = new Size(60, 60);
                pictureBox.Location = point;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BackColor = Color.Transparent;
            }
            
            public override void UpdatePosition()
            {
                pictureBox.Top += speed;
            }


            public override Image GetImage()
            {
                return pictureBox.Image;
            }

            public override Rectangle Bounds => pictureBox.Bounds;

            public override int Speed { 
                get { return speed; }
                set { speed = value; }
            }

            public override void Dispose()
            {
                pictureBox.Dispose();
            }
        }

        public class Cabbage : FallingObject
        {
            public int speed = 3;
            public Cabbage(Point point)
            {
                pictureBox = new PictureBox();
                pictureBox.Image = Properties.Resources.cabbage;
                pictureBox.Size = new Size(60, 60);
                pictureBox.Location = point;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BackColor = Color.Transparent;

            }
            public override int Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public override void UpdatePosition()
            {
                pictureBox.Top += Speed;
            }

            

            public override Image GetImage()
            {
                return pictureBox.Image;
            }

            public override Rectangle Bounds => pictureBox.Bounds;

            public override void Dispose()
            {
                pictureBox.Dispose();
            }
        }

        public class Bomb : FallingObject
        {
            private int speed = 7;
            public Bomb(Point point)
            {
                pictureBox = new PictureBox();
                pictureBox.Image = Properties.Resources.bomb;
                pictureBox.Size = new Size(55, 55);
                pictureBox.Location = point;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BackColor = Color.Transparent;

            }
            public override int Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public override void UpdatePosition()
            {
                pictureBox.Top += Speed;
            }

            

            public override Image GetImage()
            {
                return pictureBox.Image;
            }

            public override Rectangle Bounds => pictureBox.Bounds;

            public override void Dispose()
            {
                pictureBox.Dispose();
            }
        }

        public class Redbull : FallingObject
        {
            public int speed = 6;
            public Redbull(Point point)
            {
                pictureBox = new PictureBox();
                pictureBox.Image = Properties.Resources.redbull;
                pictureBox.Size = new Size(40, 40);
                pictureBox.Location = point;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.BackColor = Color.Transparent;

            }
            public override int Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public override void UpdatePosition()
            {
                pictureBox.Top += Speed;
            }

            

            public override Image GetImage()
            {
                return pictureBox.Image;
            }

            public override Rectangle Bounds => pictureBox.Bounds;

            public override void Dispose()
            {
                pictureBox.Dispose();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }
}
