using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSLab4
{
    public partial class Form1 : Form
    {
        private Bitmap map;
        private Drawing painter;
        private Graphics graph;
        private List<Bitmap> mapList;
        private List<TabPage> pageList;

        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(tabControl1.SelectedTab.Width, tabControl1.SelectedTab.Height);
            graph = Graphics.FromImage(map);
            buttonColor.BackColor = Color.Black;
            mapList = new List<Bitmap>();
            pageList = new List<TabPage>();
            pageList.Add(tabPage1);
            mapList.Add(map);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonColor.BackColor = colorDialog1.Color;
                if (painter != null)
                {
                    painter.color = colorDialog1.Color;
                }                
            }
        }

        private void tabPage1_MouseDown(object sender, MouseEventArgs e)
        {
            if (painter != null)
            {
                painter.OnMouseDown(sender, e);
                tabControl1.SelectedTab.Invalidate();
            }
        }

        private void tabPage1_MouseMove(object sender, MouseEventArgs e)
        {
            if (painter != null && e.Button == MouseButtons.Left)
            {
                painter.OnMouseMove(sender, e);
                tabControl1.SelectedTab.Invalidate();
            }
        }

        private void tabPage1_MouseUp(object sender, MouseEventArgs e)
        {
            if (painter != null)
            {
                painter.OnMouseUp(sender, e);
                tabControl1.SelectedTab.Invalidate();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            graph = Graphics.FromImage(mapList[pageList.IndexOf(tabControl1.SelectedTab)]);
            graph.Clear(Color.White);
            tabControl1.SelectedTab.Invalidate();
        }

        private void radioButtonPen_CheckedChanged(object sender, EventArgs e)
        {
            painter = new MyPen(buttonColor.BackColor, mapList[pageList.IndexOf(tabControl1.SelectedTab)]);
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(mapList[pageList.IndexOf(tabControl1.SelectedTab)], Point.Empty);
        }

        private void radioButtonLine_CheckedChanged(object sender, EventArgs e)
        {
            painter = new MyLine(buttonColor.BackColor, mapList[pageList.IndexOf(tabControl1.SelectedTab)]);
        }

        private void radioButtonRectangle_CheckedChanged(object sender, EventArgs e)
        {
            painter = new MyRectangle(buttonColor.BackColor, mapList[pageList.IndexOf(tabControl1.SelectedTab)]);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "jpg (*.jpg)|*.jpg|bmp (*.bmp)|*.bmp";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mapList[pageList.IndexOf(tabControl1.SelectedTab)].Save(saveFileDialog1.FileName);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "bmp (*.bmp)|*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mapList[pageList.IndexOf(tabControl1.SelectedTab)] = new Bitmap(Image.FromFile(openFileDialog1.FileName), tabControl1.Width, tabControl1.Height);
                if (painter != null)
                {
                    painter.map = mapList[pageList.IndexOf(tabControl1.SelectedTab)];
                }
                tabControl1.SelectedTab.Invalidate();
            }
            
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            TabPage page = new TabPage("Page " + (tabControl1.TabCount + 1)); 
            tabControl1.TabPages.Add(page);
            page.Paint += new System.Windows.Forms.PaintEventHandler(tabPage1_Paint);
            page.MouseDown += new System.Windows.Forms.MouseEventHandler(tabPage1_MouseDown);
            page.MouseMove += new System.Windows.Forms.MouseEventHandler(tabPage1_MouseMove);
            page.MouseUp += new System.Windows.Forms.MouseEventHandler(tabPage1_MouseUp);
            page.BackColor = Color.White;
            Bitmap map = new Bitmap(tabControl1.SelectedTab.Width, tabControl1.SelectedTab.Height);
            pageList.Add(page);
            mapList.Add(map);
            tabControl1.SelectedTab = page;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount == 1)
            {
                Application.Exit();
            }
            var index = pageList.IndexOf(tabControl1.SelectedTab);
            mapList.RemoveAt(index);
            pageList.Remove(tabControl1.SelectedTab);
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            var i = 0;
            foreach (TabPage element in tabControl1.TabPages)
            {
                i++;
                element.Text = "Page " + i;    
            }
            if (index != pageList.Count)
            {
                tabControl1.SelectedTab = pageList[index];
            }
            else
            {
                if (index > 1)
                {
                    tabControl1.SelectedTab = pageList[index - 1];
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (painter != null && tabControl1.SelectedIndex > 0)
            {
                painter.map = mapList[pageList.IndexOf(tabControl1.SelectedTab)];
                graph = Graphics.FromImage(mapList[pageList.IndexOf(tabControl1.SelectedTab)]);
            }           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}
