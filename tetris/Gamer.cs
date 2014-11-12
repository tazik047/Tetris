using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    [Serializable]
    class Gamer
    {
        public string Name { set; get; }
        public int Score { set; get; }

        public Gamer()
        {
            Name = "NoName";
            Score = 0;
        }

        public Gamer (string n, int s)
        {
            Name = n;
            Score = s;
        }

        public override string ToString()
        {
            return String.Format("{0}       {1}", Name, Score);
        }

    }
}
