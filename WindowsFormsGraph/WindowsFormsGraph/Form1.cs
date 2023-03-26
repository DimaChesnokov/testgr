using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //graph = CreateGraphics();
            picture = new Bitmap(2000, 2000);
        }
        Graphics graph;
        Bitmap picture;
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Pen pen;
            pen = new Pen(Color.Black, 1);
            graph = Graphics.FromImage(picture);
            Brush brush = new SolidBrush(Color.Black);
            int CursorX = MousePosition.X - 188 ;
            int CursorY = MousePosition.Y - 20;

            Circlee circlee = new Circlee(25, CursorX, CursorY);
            circlee.Show(graph, pen, brush);
            pictureBox1.Image = picture;


        }
    }
}
