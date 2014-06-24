using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace CSLab4
{
    class MyRectangle : Drawing
    {
        public Color color { set; get;  }
        public Bitmap map { set; get; }
        private Bitmap previousMap;
        private bool draw;
        private Point first;
        private Point second;

        public MyRectangle(Color COLOR, Bitmap MAP)
        {
            color = COLOR;
            map = MAP;
            draw = false;
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            first = e.Location;
            previousMap = new Bitmap(map);
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                second = e.Location;
                using (Graphics graph = Graphics.FromImage(map))
                {
                    graph.Clear(Color.White);
                    graph.DrawImageUnscaled(previousMap, 0, 0);
                    graph.DrawLine(new Pen(color, 1), first.X, first.Y, second.X, first.Y);
                    graph.DrawLine(new Pen(color, 1), second.X, first.Y, second.X, second.Y);
                    graph.DrawLine(new Pen(color, 1), second.X, second.Y, first.X, second.Y);
                    graph.DrawLine(new Pen(color, 1), first.X, first.Y, first.X, second.Y);
                }
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
            using (Graphics graph = Graphics.FromImage(map))
            {
                graph.DrawLine(new Pen(color, 1), first.X, first.Y, second.X, first.Y);
                graph.DrawLine(new Pen(color, 1), second.X, first.Y, second.X, second.Y);
                graph.DrawLine(new Pen(color, 1), second.X, second.Y, first.X, second.Y);
                graph.DrawLine(new Pen(color, 1), first.X, first.Y, first.X, second.Y);
            }
        }
    }
}
