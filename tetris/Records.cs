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
    public partial class Records : Form
    {
        List<Gamer> gamers = new List<Gamer>();

        public Records()
        {
            InitializeComponent();
        }

        private void Records_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            label1.Size = new System.Drawing.Size(35, 350);
            this.Open();
            for (int i = 0; i < gamers.Count; i++)
            {
                label1.Text += gamers[i].ToString() + "\n";
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
    }
}
