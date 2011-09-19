using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Helpers;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyToolStripSeparator:IMyControl
    {
        ToolStripSeparator tssBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }

        private static Image _ToolStripSeparatorImage;
        public static Image ToolStripSeparatorImage
        {
            get
            {
                return _ToolStripSeparatorImage;
            }
            set
            {
                _ToolStripSeparatorImage = value;
                if (value != null)
                {
                    ChangeControlColor();
                }
            }
        }

        
        public static Image TrueToolStripSeparatorImage;

        public MyToolStripSeparator(ToolStripSeparator tss)
        {
            tssBase = tss;
        }

        public void StopControlSkin()
        {
            tssBase.Paint -= new PaintEventHandler(tssBase_Paint);
        }

        public void StartControlSkin()
        {
            tssBase.Paint += new PaintEventHandler(tssBase_Paint);
        }

        public static void ChangeControlColor()
        {
            TrueToolStripSeparatorImage = ImageHelper.ReplaceColor(ToolStripSeparatorImage, skinEng.BackColor);
        }

        public void tssBase_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //先画背景
            IMyContainer imctl = (IMyContainer)skinEng.GetInterface(tssBase.Owner);
            g.TranslateTransform(2, 2);
            imctl.InvokePaintBackground(tssBase, e);
            g.ResetClip();

            Rectangle drawRect = new Rectangle();
            drawRect.X = (tssBase.Width - ToolStripSeparatorImage.Width) / 2;
            drawRect.Y = 0;
            drawRect.Width = ToolStripSeparatorImage.Width;
            drawRect.Height = tssBase.Height;

            //画分隔条
            GraphicHelper.DrawImageWithoutBorder(g, TrueToolStripSeparatorImage, drawRect);
        }
    }
}
