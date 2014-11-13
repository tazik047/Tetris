using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class NewBestScore : Form
    {
        List<Gamer> gamers = new List<Gamer>();
        Records recs;

        public NewBestScore()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            recs = new Records();
            for (int i = 0; i < gamers.Count; i++)
            {
                if (gamers[i].Name == "Winner")
                {
                    gamers[i].Name = textBox1.Text;
                    gamers[i].IsLast = true;
                    this.Save();
                    break;
                }
            }
            this.Close();
            recs.ShowDialog();
        }

        private void NewBestScore_Load(object sender, EventArgs e)
        {
            this.Open();
            for (int i = 0; i < gamers.Count; i++)
            {
                if (gamers[i].Name == "Winner")
                {
                    scoreText.Text = "Вас счет: " + gamers[i].Score;
                    break;
                }
            }
           
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

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gamers.Count; i++)
            {
                if (gamers[i].Name == "Winner")
                {
                    gamers[i].Name = "Безымянный";
                    this.Save();
                    break;
                }
            }
            this.Close();
            this.Close();
        }
    }
}
