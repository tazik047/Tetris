﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    abstract class Figure : IEnumerable<Point>
    {
        /// <summary>
        /// Положение квадратиков фигурки. 
        /// </summary> 
        protected Point[] elements;

        /// <summary>
        /// Куча элементов.
        /// </summary>
        public Heap downLine;

        /// <summary>
        /// Размер квадратика.
        /// </summary>
        public int SizeItem;

        /// <summary>
        /// Номер центрального квадратика.
        /// </summary>
        protected int central;

        /// <summary>
        /// Размер поля.
        /// Width это минимальный придел.
        /// Height это максимальный придел.
        /// </summary>
        private Size sizePole;

        /// <summary>
        /// Заполняет нижнюю линию(конструктор).
        /// </summary>
        protected Figure(Heap heap, int sizeItem)
        {
            sizePole = heap == null ? new Size() : heap.SizePole;
            SizeItem = sizeItem;
            downLine = heap;
        }

        public event EventHandler AddToHeap;
        /// <summary>
        /// Проход по каждому верхнему левому углу квадратика фигуры.
        /// </summary>
        /// <param name="n"> номер квадратика.</param>
        /// <returns> значение левой верхней точки.</returns>
        public Point this[int n]
        {
            get { return elements[n]; }
        }

        /// <summary>
        /// Количество квадратиков в фигуре.
        /// </summary>
        public int Length
        {
            get { return elements.Length; }
        }

        /// <summary>
        /// Вспомогательная функция поворота.
        /// </summary>
        /// <param name="test">Массив, который нужно повернуть. </param>
        /// <returns>Повернутый массив.</returns>
        private Point[] turn(Point[] test)
        {
            for (int i = 0; i < test.Length; ++i)
            {
                if (i == central)
                    continue;
                int x = (test[central].X - test[i].X) / SizeItem;
                int y = (test[central].Y - test[i].Y) / SizeItem;
                if ((x > 0 && y > 0) || (x < 0 && y < 0))
                {
                    test[i].X += x * SizeItem * 2;
                }
                else if ((x < 0 && y > 0) || (x > 0 && y < 0))
                {
                    test[i].Y += y * SizeItem * 2;
                }
                else if (x < 0 && y == 0)
                {
                    test[i].Y += (-x) * SizeItem;
                    test[i].X += x * SizeItem;
                }
                else if (x > 0 && y == 0)
                {
                    test[i].Y -= x * SizeItem;
                    test[i].X += x * SizeItem;
                }
                else if (x == 0)
                {
                    test[i].X += y * SizeItem;
                    test[i].Y += y * SizeItem;
                }
            }
            return test;
        }
        /// <summary>
        /// Поворот вокруг по часовой стрелки вокруг центра фигуры.
        /// </summary>
        public virtual void Turn()
        {
            SaveStep(turn);
        }

        /// <summary>
        /// Проверка на сталкновение.
        /// </summary>
        /// <param name="test">Последовательность для проверки.</param>
        /// <returns>true - нужно откатить действия.
        /// false - не нужно.</returns>
        private bool check(IEnumerable<Point> test, bool moveDown = false)
        {
            foreach (var i in test)
            {
                if (i.X > sizePole.Height || i.X < sizePole.Width)
                    return false;
                if (downLine.Count(p => p == i) != 0)
                {
                    if (AddToHeap != null && moveDown)
                        AddToHeap(this, EventArgs.Empty);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Фугкция для хода, при неудачном ходе откатывает действие назад.
        /// </summary>
        /// <param name="doSomethings"> Функция для хода.</param>
        private void SaveStep(Func<Point[], Point[]> doSomethings, bool moveDown = false)
        {
            var res = doSomethings((Point[])elements.Clone());
            if (check(res, moveDown))
                elements = res;
        }

        /// <summary>
        /// Перемещение фигуры вниз.
        /// </summary>
        public void MoveDown()
        {
            SaveStep((temp) =>
            {
                for (int i = 0; i < temp.Length; ++i)
                    temp[i].Y += SizeItem;
                return temp;
            }, true);
        }
        /// <summary>
        /// Перемещение фигуры вправо
        /// </summary>
        public void MoveRight()
        {
            SaveStep((temp) =>
            {
                for (int i = 0; i < temp.Length; ++i)
                    temp[i].X += SizeItem;
                return temp;
            });
        }

        /// <summary>
        /// Перемещение фигуры влево.
        /// </summary>
        public void MoveLeft()
        {
            SaveStep((temp) =>
            {
                for (int i = 0; i < temp.Length; ++i)
                    temp[i].X -= SizeItem;
                return temp;
            });
        }

        public void Draw(Graphics g)
        {
            Brush[] col = new Brush[] { Brushes.DodgerBlue, Brushes.OrangeRed, Brushes.Yellow, Brushes.Gold };
            for (int i = 0; i < elements.Length; ++i)
                g.FillRectangle(col[i % col.Length], elements[i].X, elements[i].Y,
                    SizeItem, SizeItem);
        }

        /// <summary>
        /// Для прохода по foreach.
        /// </summary>
        /// <returns></returns>
        IEnumerator<Point> IEnumerable<Point>.GetEnumerator()
        {
            foreach (var i in elements)
                yield return i;
        }

        /// <summary>
        /// Для прохода по foreach
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }
}
