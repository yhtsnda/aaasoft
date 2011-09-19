using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Helpers;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyStatusStrip : IMyControl
    {
        StatusStrip ssBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }

        private static Image _StatusBarImage;
        public static Image StatusBarImage
        {
            get
            {
                return _StatusBarImage;
            }
            set
            {
                _StatusBarImage = value;
                if (value != null)
                {
                    ChangeControlColor();
                }
            }
        }

        public static Image TrueStatusBar;

        public MyStatusStrip(StatusStrip ss)
        {
            ssBase = ss;

            foreach (ToolStripItem tsi in ssBase.Items)
            {
                try
                {
                    if (tsi is ToolStripProgressBar)
                        continue;
                    tsi.BackColor = Color.Transparent;
                }
                catch { }
            }
        }

        public void StopControlSkin()
        {
            ssBase.Paint -= new PaintEventHandler(ssBase_Paint);
        }

        public void StartControlSkin()
        {
            ssBase.Paint += new PaintEventHandler(ssBase_Paint);
        }

        public static void ChangeControlColor()
        {
            TrueStatusBar = ImageHelper.ReplaceColor(StatusBarImage, skinEng.BackColor);
        }

        void ssBase_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GraphicHelper.DrawImageWithoutBorder(g, TrueStatusBar, new Rectangle(new Point(0, 0), ssBase.Size));
        }
    }
}
