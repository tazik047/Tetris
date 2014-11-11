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
        public string Date { set; get; }

        public Gamer (string n, int s, string d)
        {
            Name = n;
            Score = s;
            Date = d;
        }

        public override string ToString()
        {
            return String.Format("{0}       {1}         {2}", Name, Date, Score);
        }

    }
}
