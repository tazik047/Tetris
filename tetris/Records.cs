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
        List<Label> record = new List<Label>();

        public Records()
        {
            InitializeComponent();
        }

        private void Records_Load(object sender, EventArgs e)
        {
            this.Open();
            int x = 10;
            int y = 50;

            for (int i = 0; i < gamers.Count; i++)
            {
                record.Add(new Label
                {
                    Text = (i + 1).ToString() + ")   " + gamers[i].Name,
                    Size = new System.Drawing.Size(200, 25),
                    Location = new System.Drawing.Point(x, y),
                    Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))),
                });
                this.Controls.Add((Label)record[record.Count - 1]);
                x += 240;

                record.Add(new Label
                {
                    Text = gamers[i].Score.ToString(),
                    Size = new System.Drawing.Size(50, 25),
                    Location = new System.Drawing.Point(x, y),
                    Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))),
                    TextAlign = System.Drawing.ContentAlignment.TopRight,
                });
                this.Controls.Add((Label)record[record.Count - 1]);
                x = 165;

                /*record.Add(new Label
                {
                    Text = "..................................",
                    Size = new System.Drawing.Size(350, 25),
                    Location = new System.Drawing.Point(x, y),
                    Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))),
                });
                this.Controls.Add((Label)record[record.Count - 1]);*/
                x = 10;
                y += 30;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
