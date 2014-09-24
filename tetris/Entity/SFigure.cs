using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris.Entity
{
    class SFigure : Figure
    {
        public SFigure(Point p, Heap heap, int sizeItem)
            : base(heap, sizeItem)
        {
            elements = new Point[4];
            elements[0] = p;
            elements[1] = new Point(p.X + SizeItem, p.Y);
            elements[2] = new Point(p.X, p.Y - SizeItem);
            elements[3] = new Point(elements[1].X,
                elements[2].Y);
        }

        public override void Turn()
        {
            return;
        }
    }
}
