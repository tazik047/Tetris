using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    class Heap : IEnumerable<System.Drawing.Point>
    {
        public System.Drawing.Size SizePole;
        List<System.Drawing.Point> elements;

        public Heap(System.Drawing.Size size)
        {
            System.Drawing.Point[] downLine = new System.Drawing.Point[size.Width / tetris.Properties.Settings.Default.SizeItem];
            for (int i = 0; i < downLine.Length; ++i)
                downLine[i] = new System.Drawing.Point(i * tetris.Properties.Settings.Default.SizeItem, size.Height);
            elements = downLine.ToList();
            SizePole = new System.Drawing.Size(0, size.Width - tetris.Properties.Settings.Default.SizeItem);
        }

        public Heap(IEnumerable<System.Drawing.Point> p)
        {
            elements = p.ToList();
            SizePole = new System.Drawing.Size(elements.Min(i => i.X), elements.Max(i => i.X));
        }
        
        public List<int> Add(Figure f)
        {
            List<int> listToDelete = new List<int>();
            foreach (var p in f)
                elements.Add(p);
            int countElementInLine = (SizePole.Height - SizePole.Width) / f.SizeItem + 1;
            var res = elements.GroupBy(p => p.Y);
            foreach (var i in res)
                if (i.Key != elements.FirstOrDefault().Y && i.Count() == countElementInLine)
                    listToDelete.Add(i.Key);
            if(listToDelete.Count==0)
            {
                chekGame();
            }
            return listToDelete;
        }

        private void chekGame()
        {
            if (elements.Count(p => p.Y < 0) > 0 && HeapOverflow != null)
                HeapOverflow(this, EventArgs.Empty);
        }

        public IEnumerator<System.Drawing.Point> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public void DrawHeap(System.Drawing.Graphics g)
        {
            g.Clear(System.Drawing.Color.White);
            foreach(var i in elements)
                g.FillRectangle(System.Drawing.Brushes.Gray, i.X, i.Y,
                    tetris.Properties.Settings.Default.SizeItem, tetris.Properties.Settings.Default.SizeItem);
        }
        
        public void KillHeap(System.Drawing.Graphics g, List<int> list, System.Windows.Forms.PictureBox box)
        {
            var dell = elements.Where(p => list.Any(i => i == p.Y));
            for (int i = 0; i < 3; i++)
            {
                drawHeap(g, System.Drawing.Brushes.Red, dell);
                drawHeap(box.CreateGraphics(), System.Drawing.Brushes.Red, dell);
                System.Threading.Thread.Sleep(100);
                drawHeap(g, System.Drawing.Brushes.Orange, dell);
                drawHeap(box.CreateGraphics(), System.Drawing.Brushes.Orange, dell);
                System.Threading.Thread.Sleep(100);
                drawHeap(g, System.Drawing.Brushes.Blue, dell);
                drawHeap(box.CreateGraphics(), System.Drawing.Brushes.Blue, dell);
                System.Threading.Thread.Sleep(100);
            }
            elements.RemoveAll(p => list.Any(i => i == p.Y));
            DrawHeap(g);
            DrawHeap(box.CreateGraphics());
            System.Threading.Thread.Sleep(200);
            
            foreach(var i in list.OrderBy(j=>j))
            {
                for (int j = 0; j < elements.Count; j++)
                    if (elements[j].Y < i)
                        elements[j] = new System.Drawing.Point(elements[j].X,
                            elements[j].Y + tetris.Properties.Settings.Default.SizeItem);
            }

            DrawHeap(g);
            DrawHeap(box.CreateGraphics());
            System.Threading.Thread.Sleep(200);
            chekGame();
        }

        private void drawHeap(System.Drawing.Graphics g, System.Drawing.Brush br, IEnumerable<System.Drawing.Point> list)
        {
            foreach (var i in list)
                g.FillRectangle(br, i.X, i.Y,
                    tetris.Properties.Settings.Default.SizeItem, tetris.Properties.Settings.Default.SizeItem);
        }

        public event EventHandler HeapOverflow;
    }
}
