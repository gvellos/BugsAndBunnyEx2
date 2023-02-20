using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BugsAndBunnyChallenge
{
    public partial class Form2 : Form
    {
        List<PictureBox> bullets = new List<PictureBox>();
        List<PictureBox> bulletsBot = new List<PictureBox>();
        Random r;
        Player player = new Player();
        String winner;
        public Form2()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //Enable paint via message to reduce flicker. 
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
        public Form2(int timer2I, int timerBotI , String name)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //Enable paint via message to reduce flicker. 
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            timer2.Interval = timer2I;
            timerBot.Interval = timerBotI;
            player.Username = name;
        }

        int userCurrentScore = 0;
        int botCurrentScore = 0;
        bool flag = true;
        int x = 150;

        private void createBulletBot(int startBotX)
        {
            PictureBox bp = new PictureBox();
            bp.ImageLocation = "bullet.png";
            bp.InitialImage = null;
            bp.BackColor = Color.Transparent;
            bp.Location = new Point(startBotX + 5, pictureBox2.Location.Y );
            bp.Size = new Size(60, 50);
            bp.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(bp);
            bulletsBot.Add(bp);
        }

        private void createBullet(int startX)
        {
            PictureBox p = new PictureBox();
            p.ImageLocation = "carrot.png";
            p.InitialImage = null;
            p.BackColor = Color.Transparent;
            p.Location = new Point(startX + 5, pictureBox1.Location.Y );
            p.Size = new Size(60, 50);
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(p);
            bullets.Add(p);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (PictureBox p in bullets)
            {
                p.Location = new Point(p.Location.X + 10, p.Location.Y );
                labelCurrentScore.Text = "Score: " + userCurrentScore;
                if (p.Bounds.IntersectsWith(pictureBox2.Bounds))
                {
                    userCurrentScore += 1;
                    p.Hide();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int x = r.Next(this.ClientSize.Width - pictureBox2.Width);
            int y = r.Next(this.ClientSize.Height - pictureBox2.Height);
            pictureBox2.Location = new Point(x, y);
        }

        private void timerBot_Tick(object sender, EventArgs e)
        {
            progressBar1.Maximum = 150;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.ForeColor = Color.Red;
            if (flag == true)
            {
                progressBar1.Value = 150;
                flag = false;
            }

            foreach (PictureBox bp in bulletsBot)
            {
                bp.Location = new Point(bp.Location.X + 10, bp.Location.Y );
                if (bp.Bounds.IntersectsWith(pictureBox1.Bounds))
                {
                    botCurrentScore += 1;
                    x -= 1;
                    progressBar1.Value = x;
                    bp.Hide();
                    if (x < 2)
                    {
                        winner = "The winner is the bot";
                        player.Winner = winner;
                        player.Score = userCurrentScore;
                        flag = true;
                        this.Close();
                        Form3 form3 = new Form3(player);
                        form3.Show();
                        form3.StartPosition = FormStartPosition.Manual;
                        form3.Location = this.Location;
                        form3.Size = this.Size;
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = "bugsbunny.png";
            pictureBox2.ImageLocation = "yosemite.png";
            r = new Random();
            Stats stats = new Stats();
            stats.CreateTable();
            (string topScorer, int topScore) = stats.GetTopScorer();
            labelTopScore.Text = "All time top score: " + topScore.ToString();
            labelTopScorer.Text = "All time top scorer: " + topScorer;

        }
        private void timerBotFiring_Tick(object sender, EventArgs e)
        {
            createBulletBot(pictureBox2.Location.X);
        }

        private void timer60sec_Tick(object sender, EventArgs e)
        {
            if(userCurrentScore > botCurrentScore)
            {
                winner = "The winner is the user";
                player.Score = userCurrentScore;
                player.Winner = winner;
            }else if(userCurrentScore < botCurrentScore)
            {
                winner = "The winner is the bot";
                player.Score = botCurrentScore;
                player.Winner = winner;
            }
            else
            {
                winner = "There is no winner";
                player.Score = botCurrentScore;
                player.Winner = winner;
            }
            flag = true;
            this.Close();
            Form3 form3 = new Form3(player);
            form3.Show();
            form3.StartPosition = FormStartPosition.Manual;
            form3.Location = this.Location;
            form3.Size = this.Size;
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().Equals("Left"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X - 15, pictureBox1.Location.Y);
            }
            else if (e.KeyCode.ToString().Equals("Right"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X + 15, pictureBox1.Location.Y);
            }
            else if (e.KeyCode.ToString().Equals("Up"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 15);
            }
            else if (e.KeyCode.ToString().Equals("Down"))
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 15);
            }
            if (e.KeyCode.ToString().Equals("Space"))
            {
                createBullet(pictureBox1.Location.X);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }


        //Whether the animation starts.
        bool currentlyAnimating = false;
        //Indicate if update the animation.
        bool isAnimating = true;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (isAnimating)
            {
                //Begin the animation.
                AnimateImage();
                //Update the frames. The cell would paint the next frame of the image late on.
                ImageAnimator.UpdateFrames();
            }
            base.OnPaintBackground(e);
        }
        //This method begins the animation.
        public void AnimateImage()
        {
            if (!currentlyAnimating)
            {
                ImageAnimator.Animate(this.BackgroundImage, new EventHandler(this.OnFrameChanged));
                currentlyAnimating = true;
            }
        }
        private void OnFrameChanged(object o, EventArgs e)
        {
            //Force to redraw the form.
            this.Invalidate();
        }
    }
}
