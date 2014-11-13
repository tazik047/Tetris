﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        Form1 form;
        Records records;
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            form = new Form1();
            form.ShowDialog();
            this.Show();
            form.WMP.controls.pause();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            records = new Records();
            records.ShowDialog();
            this.Show();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
