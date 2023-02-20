using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugsAndBunnyChallenge
{
    public partial class Form1 : Form
    {
        int timerBotInterval;
        int timer2Interval;
        String name;
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                timer2Interval = 3000;
                timerBotInterval = 100;
            }
            else if(radioButton2.Checked == true)
            {
                timer2Interval = 2300;
                timerBotInterval = 70;
            }
            else
            {
                timerBotInterval = 50;
                timer2Interval = 1700;
            }
            name = textBox1.Text;
            this.Hide();
            Form2 formPlay = new Form2(timer2Interval, timerBotInterval, name);
            formPlay.Show();
            formPlay.StartPosition = FormStartPosition.Manual;
            formPlay.Location = this.Location;
            formPlay.Size = this.Size;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 formStats = new Form3();
            formStats.Show();
            formStats.StartPosition = FormStartPosition.Manual;
            formStats.Location = this.Location;
            formStats.Size = this.Size;
        }

        private void buttoneExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
