using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Helpers;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyToolStrip : IMyControl, IMyContainer
    {
        ToolStrip tsBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }

        Dictionary<Object, IMyControl> dictItems = new Dictionary<object, IMyControl>();

        private static Image _ToolbarImage;
        public static Image ToolbarImage
        {
            get
            {
                return _ToolbarImage;
            }
            set
            {
                _ToolbarImage = value;
                if (value != null)
                {
                    ChangeControlColor();
                }
            }
        }

        public static Image TrueToolbarImage;

        public MyToolStrip(ToolStrip ts)
        {
            tsBase = ts;

            foreach (ToolStripItem tsi in tsBase.Items)
            {
                if (tsi is ToolStripButton)
                {
                    if (!dictItems.ContainsKey(tsi))
                    {
                        dictItems.Add(tsi, new MyToolStripButton(tsi as ToolStripButton));
                    }
                }
                else if (tsi is ToolStripSeparator)
                {
                    if (!dictItems.ContainsKey(tsi))
                    {
                        dictItems.Add(tsi, new MyToolStripSeparator(tsi as ToolStripSeparator));
                    }
                }
            }
        }

        public void StopControlSkin()
        {
            tsBase.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            tsBase.Paint -= new PaintEventHandler(tsBase_Paint);
            tsBase.PaintGrip -= new PaintEventHandler(tsBase_PaintGrip);

            foreach (IMyControl imc in dictItems.Values)
            {
                imc.StopControlSkin();
            }
        }

        public void StartControlSkin()
        {
            tsBase.RenderMode = ToolStripRenderMode.System;
            tsBase.Paint += new PaintEventHandler(tsBase_Paint);
            tsBase.PaintGrip += new PaintEventHandler(tsBase_PaintGrip);

            foreach (IMyControl imc in dictItems.Values)
            {
                imc.StartControlSkin();
            }
        }

        #region ToolStrip部分
        public static void ChangeControlColor()
        {
            TrueToolbarImage = ImageHelper.ReplaceColor(ToolbarImage, skinEng.BackColor);
        }

        void tsBase_PaintGrip(object sender, PaintEventArgs e)
        {
            tsBase_Paint(sender, e);
        }

        void tsBase_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GraphicHelper.DrawImageWithoutBorder(g, TrueToolbarImage, new Rectangle(new Point(0, 0), tsBase.Size));
        }
        #endregion

        #region IMyContainer 成员

        public void InvokePaintBackground(System.ComponentModel.Component c, PaintEventArgs e)
        {
            int borderWidth = 2;
            e.Graphics.TranslateTransform(0 - borderWidth, 0 - borderWidth);
            tsBase_Paint(c, e);
            e.Graphics.ResetTransform();
        }

        public void InvokePaint(System.ComponentModel.Component c, PaintEventArgs e)
        {
            
        }

        #endregion
    }
}
