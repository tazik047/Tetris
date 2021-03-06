﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    class ZrevFigure : Figure
    {
        public ZrevFigure(Point p, Heap heap, int sizeItem)
            : base(heap, sizeItem)
        {
            elements = new Point[4];
            elements[0] = p;
            elements[1] = new Point(elements[0].X, elements[0].Y - SizeItem);
            elements[2] = new Point(elements[1].X - SizeItem, elements[1].Y);
            elements[3] = new Point(elements[2].X, elements[2].Y - SizeItem);
            central = 2;
        }
    }
}
