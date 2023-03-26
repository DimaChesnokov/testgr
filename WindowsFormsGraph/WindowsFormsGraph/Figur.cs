using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace WindowsFormsGraph
{
  

    /// <summary>
    /// Абстрактный класс описывающий геометрическую фигуру в плоскости
    /// </summary>
    public abstract class Figure
    {
        /// <summary>
        /// Точка в которой находится центр фигуры
        /// </summary>
        protected Point center;
        /// <summary>
        /// Размерность фигуры
        /// </summary>
        protected double scale;
        /// <summary>
        /// Булевая переменная хранящая в себе информацию перемещается ли фигура
        /// </summary>
        protected bool dragged;
        /// <summary>
        /// Цвет фигуры
        /// </summary>
        protected Color color;

        /// <summary>
        /// Точка в которой находится центр фигуры
        /// </summary>
        public Point Center_of_figure
        {
            get { return center; }
            set { center = value; }
        }

        /// <summary>
        /// Размерность фигуры 
        /// </summary>
        public double Scale_of_figure
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Перетаскивается ли фигура в данный момент
        /// </summary>
        public bool Dragging
        {
            get { return dragged; }
            set { dragged = value; }
        }

        /// <summary>
        /// Цвет фигуры
        /// </summary>
        public Color Color_of_figure
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Базовый конструктор фигуры
        /// </summary>
        /// <param name="x"> Координата на оси X </param>
        /// <param name="y"> Координата на оси Y </param>
        public Figure(int x, int y)
        {
            center = new Point(x, y);
            scale = 1;
            dragged = false;
            color = Color.Black;
        }

        /// <summary>
        /// Метод позволяющий двигать фигуру в плоскости
        /// </summary>
        /// <param name="x"> Значение на которое нужно переместить фигуру вдоль оси X </param>
        /// <param name="y"> Значение на которое нужно переместить фигуру вдоль оси Y </param>
        public void Moving(int x, int y)
        {
            center.X += x;
            center.Y += y;
        }

        /// <summary>
        /// Метод позволяющий менять размер фигуры
        /// </summary>
        /// <param name="multipler_of_scale"> Аргумент хранящий в себе число на которое нужно умножить размер фигуры </param>
        public void Scaling(double multipler_of_scale)
        {
            scale *= multipler_of_scale;
        }

        /// <summary>
        /// Абстрактный метод позволяющий отобразить фигуру в интерфейсе пользователя
        /// </summary>
        /// <param name="graphics"> Поверхность рисования </param>
        /// <param name="pen"> Объект для рисования (перо) </param>
        /// <param name="brush"> Объект для заливки графических фигур (кисть) </param>
        public abstract void Show(Graphics graphics, Pen pen, Brush brush);

        public abstract Rectangle Region_Capture();
    }
}
