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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace tetris
{
    public partial class GameField : Form
    {
        Figure myFigure;
        Figure nextFigure;
        Heap allHeap;
        Image imgHeap;
        int score;
        int nextUpper;
        List<Gamer> gamers = new List<Gamer>();
        NewBestScore nbs;
        bool gameover;

        public GameField()
        {
            InitializeComponent();

            startGame();
            //buttonMus.TabIndex = 0;
            buttonMus.Focus();
            //timer1.Enabled = true;
            //KeyDown += Form1_KeyDown;
           
        }

        private void startGame()
        {
            gameover = false;
            allHeap = new Heap(pictureBox1.Size);
            allHeap.HeapOverflow += allHeap_HeapOverflow;
            
            createNextFigure();
            createNewFigure();

            score = 0;
            nextUpper = 50;
            scoreLabel.Text = score.ToString();

            imgHeap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            allHeap.DrawHeap(Graphics.FromImage(imgHeap));
            //pictureBox1.Focus();
            Activate();
            //Focus();
            buttonMus.Focus();
            
        }

        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();

        private void RunMusic()
        {
            WMP.URL = @"Tetris.mp3";
            WMP.controls.play();
            buttonMus.Focus();
        }


        void allHeap_HeapOverflow(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            KeyDown -= Form1_KeyDown;
            gameover = true;
            if (this.checkBestScore())
            {
                nbs = new NewBestScore();
                nbs.ShowDialog();
            }

            var res = MessageBox.Show("Хотите начать заново?", "Вы проиграли :(", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            WMP.close();
            if (res == DialogResult.Yes)
            {
                startGame();
                timer1.Enabled = true;
                KeyDown += Form1_KeyDown;
            }
            else
            {
                this.Close();
            }
            buttonMus.Focus();
        }

        private bool checkBestScore()
        {
            bool res = false;
            this.Open();
            if (gamers.Count == 0)
            {
                gamers.Add(new Gamer("Winner", this.score));
                this.Save();
            }
            else if (gamers.Count == 1)
            {
                if (gamers[0].Score < this.score)
                    gamers.Insert(0, new Gamer("Winner", this.score));
                else
                    gamers.Add(new Gamer("Winner", this.score));
                this.Save();
            }
            else
                for (int i = 0; i < 9; i++)
                {
                
                    if (this.score >= gamers[i].Score)
                    {
                        gamers.Insert(i, new Gamer("Winner", this.score));
                        this.Save();
                        res = true;
                        break;
                    }
                }

            if (gamers.Count > 10)
                while (gamers.Count != 10)
                    gamers.RemoveAt(gamers.Count - 1);

            /*gamers.Clear();
            for (int i = 0; i < 10; i++)
                gamers.Add(new Gamer("Gamer", 0));*/

            this.Save();
            return res;
        }

        public void Save()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream("bestScores.dat", FileMode.OpenOrCreate))
            {
                binFormat.Serialize(fStream, gamers);
            }
        }

        public void Open()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            try
            {
                using (Stream fStream = new FileStream("bestScores.dat", FileMode.OpenOrCreate))
                {
                    gamers = (List<Gamer>)binFormat.Deserialize(fStream);
                }
            }
            catch { }
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
            buttonMus.Focus();
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
            buttonMus.Focus();
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
            buttonMus.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    step(myFigure.Turn);
                    break;
                case Keys.S:
                    step(myFigure.MoveDown);
                    break;
                case Keys.D:
                    step(myFigure.MoveRight);
                    break;
                case Keys.A:
                    step(myFigure.MoveLeft);
                    break;
            }
            buttonMus.Focus();
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

            if (e.KeyCode == Keys.Q)
                buttonMus.Focus();

        }

        private void step(Action actionStep)
        {
            actionStep();
            pictureBox1.Invalidate();
            buttonMus.Focus();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(imgHeap, 0, 0);
            if (timer1.Enabled)
                myFigure.Draw(e.Graphics);
            buttonMus.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            step(myFigure.MoveDown);
            buttonMus.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            label10.Visible = !label10.Visible;
            if (timer1.Enabled)
            {
                KeyDown += Form1_KeyDown;
                if (music) 
                    WMP.controls.play();
            }
            else
            {
                KeyDown -= Form1_KeyDown;
                WMP.controls.pause();
            }

            buttonMus.Focus();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            nextFigure.Draw(e.Graphics);
            buttonMus.Focus();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            step(myFigure.Turn);
            buttonMus.Focus();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveLeft);
            buttonMus.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveRight);
            buttonMus.Focus();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            step(myFigure.MoveDown);
            buttonMus.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RunMusic();
            startGame();
            timer1.Enabled = true;
            KeyDown += Form1_KeyDown;
            buttonMus.TabIndex = 0;
            buttonMus.Focus();
        }

        bool music = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (music)
            {
                WMP.controls.pause();
                music = false;
                this.pictureBox4.Image = global::tetris.Properties.Resources.volumeOFF1;
            }
            else if (!music)
            {
                WMP.controls.play();
                music = true;
                this.pictureBox4.Image = global::tetris.Properties.Resources.volumeON;
            }
            buttonMus.Focus();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            KeyDown -= Form1_KeyDown;
            WMP.controls.pause();

            if (!gameover)
            {
                var res = MessageBox.Show("Вы точно хотите выйти?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    timer1.Enabled = false;
                    KeyDown -= Form1_KeyDown;
                    return;
                }
                else if (res == DialogResult.No)
                {
                    e.Cancel = true;
                    timer1.Enabled = true;
                    KeyDown += Form1_KeyDown;
                    if (music)
                        WMP.controls.play();
                    return;
                }
            }

        }

        private void newlbl_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            KeyDown -= Form1_KeyDown;
            var res = MessageBox.Show("Вы точно хотите начать новую игру?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                startGame();
                timer1.Enabled = true;
                KeyDown += Form1_KeyDown;
                buttonMus.TabIndex = 0;
                buttonMus.Focus();
                return;
            }
            timer1.Enabled = true;
            KeyDown += Form1_KeyDown;
        }
    }
}
