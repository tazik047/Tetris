using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using WMPLib;


namespace tetris
{
    public partial class Form1 : Form
    {
        Figure myFigure;
        Figure nextFigure;
        Heap allHeap;
        //Brush[] col = new Brush[] { Brushes.Blue, Brushes.Red, Brushes.Yellow, Brushes.Orange };
        Image imgHeap;
        int score;
        int nextUpper;

        public Form1()
        {
            InitializeComponent();

            startGame();
           
        }

        private void startGame()
        {
            allHeap = new Heap(pictureBox1.Size);
            allHeap.HeapOverflow += allHeap_HeapOverflow;
            
            createNextFigure();
            createNewFigure();

            score = 0;
            nextUpper = 50;
            scoreLabel.Text = score.ToString();

            imgHeap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            allHeap.DrawHeap(Graphics.FromImage(imgHeap));
        }

        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();

        private void RunMusic()
        {
            WMP.URL = @"Tetris.mp3";
            WMP.controls.play();
        }

        void allHeap_HeapOverflow(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            KeyDown -= Form1_KeyDown;
            var res = MessageBox.Show("Хотите начать заново?", "Вы проиграли:(", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            WMP.close();
            if(res == DialogResult.Yes)
            {
                startGame();
                timer1.Enabled = true;
                KeyDown += Form1_KeyDown;
            }
        }

        private void zf_AddToHeap(object sender, EventArgs e)
        {
            var list = allHeap.Add(myFigure);
            allHeap.DrawHeap(Graphics.FromImage(imgHeap));
            pictureBox1.Invalidate();
            if (list.Count != 0)
            {
                timer1.Enabled = false;
                KeyDown -= Form1_KeyDown;
                
                allHeap.KillHeap(Graphics.FromImage(imgHeap), list, pictureBox1);
                score += list.Count * (5 + list.Count - 1);
                if (score >= nextUpper)
                {
                    nextUpper += 50;
                    if (timer1.Interval != 0)
                        timer1.Interval -= 10;
                }
                scoreLabel.Text = score.ToString();
                timer1.Enabled = true;
                KeyDown += Form1_KeyDown;
            }
            createNewFigure();
        }

        private void createNewFigure()
        {
            switch (nextFigure.GetType().Name)
            {
                case "ZFigure":
                    myFigure = new ZFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "TFigure":
                    myFigure = new TFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "LFigure":
                    myFigure = new LFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "IFigure":
                    myFigure = new IFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "SFigure":
                    myFigure = new SFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "ZrevFigure":
                    myFigure = new ZrevFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
                case "LrevFigure":
                    myFigure = new LrevFigure(new Point(60, 0), allHeap, tetris.Properties.Settings.Default.SizeItem);
                    break;
            }
            myFigure.AddToHeap += zf_AddToHeap;
            createNextFigure();
        }

        private void createNextFigure()
        {
            switch (new Random().Next(7))
            {
                case 0:
                    nextFigure = new ZFigure(new Point(20, 50), null, 10);
                    break;
                case 1:
                    nextFigure = new TFigure(new Point(20, 40), null, 10);
                    break;
                case 2:
                    nextFigure = new LFigure(new Point(20, 50), null, 10);
                    break;
                case 3:
                    nextFigure = new IFigure(new Point(20, 50), null, 10);
                    break;
                case 4:
                    nextFigure = new SFigure(new Point(20, 40), null, 10);
                    break;
                case 5:
                    nextFigure = new ZrevFigure(new Point(30, 50), null, 10);
                    break;
                case 6:
                    nextFigure = new LrevFigure(new Point(30, 50), null, 10);
                    break;
            }
            pictureBox2.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (Convert.ToString(e.KeyCode))
            {
                case "W":
                case "Up":
                    step(myFigure.Turn);
                    break;
                case "S":
                case "Down":
                    step(myFigure.MoveDown);
                    break;
                case "D":
                case "Right":
                    step(myFigure.MoveRight);
                    break;
                case "A":
                case "Left":
                    step(myFigure.MoveLeft);
                    break;
            }
        }

        private void Form1_SystemKeyDown(object sender, KeyEventArgs e)
        {
            switch (Convert.ToString(e.KeyCode))
            {
                case "Escape":
                    label1_Click(this, EventArgs.Empty);
                    break;
                default:
                    Form1_KeyDown(sender, e);
                    break;
            }
        }

        private void step(Action actionStep)
        {
            actionStep();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(imgHeap, 0, 0);
            if (timer1.Enabled)
                myFigure.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            step(myFigure.MoveDown);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            label10.Visible = !label10.Visible;
            if (timer1.Enabled)
                KeyDown += Form1_KeyDown;
            else
                KeyDown -= Form1_KeyDown;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            KeyDown -= Form1_KeyDown;

            var res = MessageBox.Show("Вы точно хотите выйти?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Close();
                return;
            }
            timer1.Enabled = true;
            KeyDown += Form1_KeyDown;
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            nextFigure.Draw(e.Graphics);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            step(myFigure.Turn);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveLeft);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveRight);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveDown);
        }

        private void Run_Click(object sender, EventArgs e)
        {
        WMP.controls.play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WMP.controls.pause();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WMP.close();  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RunMusic();
        }

        bool music = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (music)
            {
                //RunMusic();
                WMP.controls.pause();
                music = false;
                buttonMus.Text = "OFF";
            }
            else if (!music)
            {
                WMP.controls.play();
                music = true;
                buttonMus.Text = "ON";
            }

        }
    }
}
