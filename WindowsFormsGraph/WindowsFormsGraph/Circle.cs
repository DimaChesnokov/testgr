using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsGraph
{
    public class Circlee
    {
        public Circlee()
        {
        }

        int Radius;
        Point Center;
        public Circlee(int R, int x, int y)
        {
            Radius = R;
            Center = new Point(x, y);
            
        }



        public void Show(Graphics graphics, Pen pen, Brush brush)
        {
            int X = Center.X - Radius;
            int Y = Center.Y - Radius;
            
            Rectangle rect = new Rectangle(X, Y, Radius * 2, Radius * 2);
            graphics.DrawEllipse(pen, rect);
            graphics.FillEllipse(brush, rect);
        }


    }

}
