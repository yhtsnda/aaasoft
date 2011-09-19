using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace aaaSoft.Helpers
{
    public class ControlHelper
    {
        public delegate void TraversalControlDelegate(Control ctl);
        /// <summary>
        /// 遍历控件及子控件
        /// </summary>
        /// <param name="ctl">控件</param>
        /// <param name="tcd">委托</param>
        public static void TraversalControl(Control ctl, TraversalControlDelegate tcd)
        {
            tcd(ctl);
            foreach (Control subCtl in ctl.Controls)
            {
                TraversalControl(subCtl, tcd);
            }
        }

        private static Dictionary<Control, Point> dictControlLocation = new Dictionary<Control, Point>();
        public static void MakeControlMovable(Control ctl)
        {
            dictControlLocation.Add(ctl, new Point());
            ctl.MouseDown += new MouseEventHandler(ctl_MouseDown);
            ctl.MouseMove += new MouseEventHandler(ctl_MouseMove);
            ctl.MouseUp += new MouseEventHandler(ctl_MouseUp);
        }

        static void ctl_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        static void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            var ctl = (Control)sender;
            if (e.Button == MouseButtons.Left)
            {
                var oldLocation = dictControlLocation[ctl];
                var newLocation = e.Location;
                var x = newLocation.X - oldLocation.X;
                var y = newLocation.Y - oldLocation.Y;
                ctl.Left += x;
                ctl.Top += y;
            }
        }

        static void ctl_MouseDown(object sender, MouseEventArgs e)
        {
            var ctl = (Control)sender;
            dictControlLocation[ctl] = e.Location;
        }
    }
}
