using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace CSLab4
{
    class MyPen : Drawing
    {
        public Color color { set; get; }
        public Bitmap map { set; get; }
        private Boolean draw;
        private Graphics graph;
        
        public MyPen(Color COLOR, Bitmap MAP)
        {
            color = COLOR;
            map = MAP;
            draw = false;
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            graph = Graphics.FromImage(map);
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                graph.DrawRectangle(new Pen(color, 2), e.X, e.Y, 1, 1);
            }
        }
    }
}
