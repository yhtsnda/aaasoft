using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Win32;
using aaaSoft.Helpers;
using aaaSoft.SkinEngine.SkinHelpers;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyHScrollBar: NativeWindow ,IMyControl
    {
        //假滚动条
        Control FakeScrollBar;
        MessagePortal mp;
        Bitmap bmpBuffer;

        //鼠标是否按下
        private bool IsMouseDown = false;
        //0:左侧按钮；1：左侧空白；2：滚动按钮；3：右侧空白；4：右侧按钮
        private int MouseDownIndex = -1;
        private Point MouseDownLocation;

        //滚动按钮最小宽度
        public static int ScrollButtonMinWidth = 15;

        private static Image _HScrollbarImage;
        public static Image HScrollbarImage
        {
            get
            {
                return _HScrollbarImage;
            }
            set
            {
                _HScrollbarImage = value;
                if (value != null)
                {
                    InitScrollbarImage();
                    ChangeControlColor();
                }
            }
        }

        #region 按钮边框设置
        //左侧边框宽度
        public static int LeftBorderWidth = 2;
        //右侧边框宽度
        public static int RightBorderWidth = 2;
        //上方边框高度
        public static int TopBorderHeight = 2;
        //下方边框高度
        public static int BottomBorderHeight = 2;
        #endregion

        //原始图片
        public static Image[,] SourceLeftButtonImages = new Image[3, 9];
        public static Image[,] SourceRightButtonImages = new Image[3, 9];
        public static Image[,] SourceScrollButtonImages = new Image[3, 9];
        public static Image SourceBackImage;

        //真实图片
        public static Image[,] TrueLeftButtonImages = new Image[3, 9];
        public static Image[,] TrueRightButtonImages = new Image[3, 9];
        public static Image[,] TrueScrollButtonImages = new Image[3, 9];
        public static Image TrueBackImage;

        //矩形区域
        private Rectangle LeftButtonRect;
        private Rectangle RightButtonRect;
        private Rectangle ScrollButtonRect;
        private Rectangle LeftBlankRect;
        private Rectangle RightBlankRect;

        //分块矩形区域
        private Rectangle[] LeftButtonRects;
        private Rectangle[] RightButtonRects;
        private Rectangle[] ScrollButtonRects;

        #region 初始化相关图片
        public static void InitScrollbarImage()
        {
            Image[] tmpImages = ImageHelper.CutImage(HScrollbarImage, 4, true);

            Image[] tmpRightImages = ImageHelper.CutImage(tmpImages[0], 3, false);
            Image[] tmpLeftImages = ImageHelper.CutImage(tmpImages[1], 3, false);
            Image[] tmpScrollButtonImages = ImageHelper.CutImage(tmpImages[2], 3, false);

            Rectangle[] srcLeftButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpLeftImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            Rectangle[] srcRightButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpRightImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            Rectangle[] srcScrollButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpScrollButtonImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);

            for (int i = 0; i <= tmpLeftImages.Length - 1; i++)
            {
                for (int j = 0; j <= srcLeftButtonRect.Length - 1; j++)
                {
                    SourceRightButtonImages[i, j] = ImageHelper.GetImage(tmpRightImages[i], srcRightButtonRect[j]);
                    SourceLeftButtonImages[i, j] = ImageHelper.GetImage(tmpLeftImages[i], srcLeftButtonRect[j]);
                    SourceScrollButtonImages[i, j] = ImageHelper.GetImage(tmpScrollButtonImages[i], srcScrollButtonRect[j]);

                    //加边框
                    SourceRightButtonImages[i, j] = ImageHelper.AddBorder(SourceRightButtonImages[i, j]);
                    SourceLeftButtonImages[i, j] = ImageHelper.AddBorder(SourceLeftButtonImages[i, j]);
                    SourceScrollButtonImages[i, j] = ImageHelper.AddBorder(SourceScrollButtonImages[i, j]);
                }
            }
            
            SourceBackImage = tmpImages[3];
            SourceBackImage = ImageHelper.AddBorder(SourceBackImage);
        }
        #endregion

        #region 改变相关图片颜色
        public static void ChangeControlColor()
        {
            TrueBackImage = ImageHelper.ReplaceColor(SourceBackImage, skinEng.BackColor);

            for (int i = 0; i <= 3 - 1; i++)
            {
                for (int j = 0; j <= 9 - 1; j++)
                {
                    TrueLeftButtonImages[i, j] = ImageHelper.ReplaceColor(SourceLeftButtonImages[i, j], skinEng.BackColor);
                    TrueRightButtonImages[i, j] = ImageHelper.ReplaceColor(SourceRightButtonImages[i, j], skinEng.BackColor);
                    TrueScrollButtonImages[i, j] = ImageHelper.ReplaceColor(SourceScrollButtonImages[i, j], skinEng.BackColor);
                }
            }
        }
        #endregion

        HScrollBar hsbBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }
        public MyHScrollBar(HScrollBar hsb)
        {
            hsbBase = hsb;
            hsbBase_SizeChanged(hsbBase, null);
        }
        
        public void StopControlSkin()
        {
            if (mp == null)
            {
                return;
            }
            mp.ClosePortal();
            ReleaseHandle();
            
            hsbBase.SizeChanged -= new EventHandler(hsbBase_SizeChanged);

            if (hsbBase.Parent == null)
            {
                return;
            }
            hsbBase.Parent.Controls.Remove(FakeScrollBar);
            FakeScrollBar.Dispose();
        }

        public void StartControlSkin()
        {
            CalcRect();
            AssignHandle(hsbBase.Handle);
            hsbBase.SizeChanged += new EventHandler(hsbBase_SizeChanged);
            hsbBase.LocationChanged += new EventHandler(hsbBase_LocationChanged);
            hsbBase.VisibleChanged += new EventHandler(hsbBase_VisibleChanged);

            //构造一个假的滚动条
            FakeScrollBar = new Control();
            hsbBase.Parent.Controls.Add(FakeScrollBar);
            FakeScrollBar.Location = hsbBase.Location;
            FakeScrollBar.Size = hsbBase.Size;
            FakeScrollBar.Visible = hsbBase.Visible;

            FakeScrollBar.Paint += new PaintEventHandler(FakeScrollBar_Paint);
            
            FakeScrollBar.BringToFront();
            hsbBase.SendToBack();
            mp = new MessagePortal(FakeScrollBar.Handle, hsbBase.Handle);
            mp.OpenPortal();
        }

        #region FakeScrollBar相关事件
        void FakeScrollBar_MouseLeave()
        {
            Point pt = Form.MousePosition;
            pt = hsbBase.Parent.PointToClient(pt);

            if (!FakeScrollBar.Bounds.Contains(pt))
            {
                IsMouseDown = false;
            }
        }

        void FakeScrollBar_Paint(object sender, PaintEventArgs e)
        {
            DrawScrollbar();
        }

        void FakeScrollBar_MouseDown()
        {
            IsMouseDown = true;
            Point pt = Form.MousePosition;
            pt = hsbBase.PointToClient(pt);
            MouseDownLocation = pt;

            if (LeftButtonRect.Contains(pt))
            {
                MouseDownIndex = 0;
            }
            else if (LeftBlankRect.Contains(pt))
            {
                MouseDownIndex = 1;
            }
            else if (ScrollButtonRect.Contains(pt))
            {
                MouseDownIndex = 2;
            }
            else if (RightBlankRect.Contains(pt))
            {
                MouseDownIndex = 3;
            }
            else if (RightButtonRect.Contains(pt))
            {
                MouseDownIndex = 4;
            }

            if (LeftButtonRect.Contains(pt) || RightButtonRect.Contains(pt) || ScrollButtonRect.Contains(pt))
            {
                DrawScrollbar();
            }
        }

        void FakeScrollBar_MouseUp()
        {
            IsMouseDown = false;
            DrawScrollbar();
            MouseDownIndex = -1;
            NativeMethods.ReleaseCapture();
        }
        #endregion

        #region hsbBase相关事件
        void hsbBase_LocationChanged(object sender, EventArgs e)
        {
            FakeScrollBar.Location = hsbBase.Location;
        }

        void hsbBase_SizeChanged(object sender, EventArgs e)
        {
            bmpBuffer = new Bitmap(hsbBase.Width, hsbBase.Height);
            //计算矩形
            CalcRect();
            if (FakeScrollBar != null)
            {
                FakeScrollBar.Size = hsbBase.Size;
            }
        }

        void hsbBase_VisibleChanged(object sender, EventArgs e)
        {
            FakeScrollBar.Visible = hsbBase.Visible;
        }
        #endregion

        #region 计算矩形
        private void CalcRect()
        {
            //算矩形
            LeftButtonRect = new Rectangle(new Point(0, 0), new Size(hsbBase.Height, hsbBase.Height));
            RightButtonRect = new Rectangle(new Point(hsbBase.Width - hsbBase.Height, 0), new Size(hsbBase.Height, hsbBase.Height));
            ScrollButtonRect = new Rectangle();

            if (hsbBase.Maximum <= hsbBase.Minimum)
            {
                return;
            }
            
            //中间滚动区域宽度
            int MiddleWidth = hsbBase.Width - 2 * hsbBase.Height;
            //可见客户区高度(像素)
            //int TrueViewedClientHeight = 0;
            //客户区宽度
            int TrueViewedClientWidth;
            
            //滚动条Max,Min,Value三个值对应的像素值
            int TrueClientMax;
            int TrueClientMin;
            int TrueClientValue;

            //客户区减少的量
            int ClientWidthOffset = 0;
            if (hsbBase.Parent is DataGridView)
            {
                DataGridView dgv = (DataGridView)hsbBase.Parent;
                if (dgv.RowHeadersVisible)
                {
                    ClientWidthOffset = dgv.RowHeadersWidth;
                }
            }
            TrueViewedClientWidth = hsbBase.Width - ClientWidthOffset;

            if (hsbBase.Parent is DataGridView)
            {
                TrueClientMax = hsbBase.Maximum;
                TrueClientMin = hsbBase.Minimum;
                TrueClientValue = hsbBase.Value;
            }
            else if (hsbBase.Parent.GetType().ToString() == "System.Windows.Forms.PropertyGridInternal.PropertyGridView")
            {
                //经验系数
                int specValue = 17;
                TrueClientMax = (hsbBase.Maximum + 1) * specValue;
                TrueClientMin = hsbBase.Minimum * specValue;
                TrueClientValue = hsbBase.Value * specValue;
            }
            else
            {
                TrueClientMax = hsbBase.Maximum;
                TrueClientMin = hsbBase.Minimum;
                TrueClientValue = hsbBase.Value;
            }

            //=====关键是算对滑块宽度
            //滑块宽度
            ScrollButtonRect.Width = TrueViewedClientWidth * MiddleWidth / TrueClientMax;
            if (ScrollButtonRect.Width > MiddleWidth)
            {
                ScrollButtonRect.Width = MiddleWidth;
                ScrollButtonRect.X = MiddleWidth - ScrollButtonRect.Width + hsbBase.Height;
            }
            else
            {
                if (ScrollButtonRect.Width < ScrollButtonMinWidth)
                {
                    ScrollButtonRect.Width = ScrollButtonMinWidth;
                }

                ScrollButtonRect.X = Convert.ToInt32(((MiddleWidth - ScrollButtonRect.Width) * TrueClientValue
                                    / (TrueClientMax - TrueViewedClientWidth)))
                                    + hsbBase.Height;
                if (ScrollButtonRect.Right > MiddleWidth + hsbBase.Height)
                {
                    ScrollButtonRect.X = MiddleWidth + hsbBase.Height - ScrollButtonRect.Width;
                }
            }

            ScrollButtonRect.Height = hsbBase.Height;
            ScrollButtonRect.Y = 0;

            LeftBlankRect = new Rectangle(LeftButtonRect.Right, LeftButtonRect.Top, ScrollButtonRect.Left - LeftButtonRect.Right, LeftButtonRect.Height);
            RightBlankRect = new Rectangle(ScrollButtonRect.Right, RightButtonRect.Top, RightButtonRect.Left - ScrollButtonRect.Right, RightButtonRect.Height);

            //算分块矩形
            LeftButtonRects = ShapeHelper.GetRectangles(LeftButtonRect, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            RightButtonRects = ShapeHelper.GetRectangles(RightButtonRect, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            ScrollButtonRects = ShapeHelper.GetRectangles(ScrollButtonRect, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
        }
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m)
        {
            switch ((NativeConsts.WindowMessage)m.Msg)
            {
                case NativeConsts.WindowMessage.SBM_SETSCROLLINFO:
                    {
                        CalcRect();
                        DrawScrollbar();
                        break;
                    }
                case NativeConsts.WindowMessage.WM_PAINT:
                    {
                        DrawScrollbar();
                        break;
                    }
                case NativeConsts.WindowMessage.WM_LBUTTONDOWN:
                    {
                        FakeScrollBar_MouseDown();
                        NativeMethods.SetCapture(hsbBase.Handle);
                        break;
                    }
                case NativeConsts.WindowMessage.WM_MOUSELEAVE:
                    {
                        FakeScrollBar_MouseLeave();
                        if (!IsMouseDown)
                        {
                            NativeMethods.ReleaseCapture();
                        }
                        break;
                    }
                case NativeConsts.WindowMessage.WM_CAPTURECHANGED:
                    {
                        if (m.LParam == IntPtr.Zero)
                        {
                            FakeScrollBar_MouseUp();
                        }
                        break;
                    }
                case NativeConsts.WindowMessage.WM_LBUTTONUP:
                    {
                        break;
                    }
            }

            base.WndProc(ref m);
        }
        #endregion

        #region 画滚动条
        private void DrawScrollbar()
        {
            Graphics g = Graphics.FromImage(bmpBuffer);

            Point pt = Form.MousePosition;
            pt = hsbBase.PointToClient(pt);
            //背景
            GraphicHelper.DrawImageWithoutBorder(g, TrueBackImage, new Rectangle(new Point(0, 0), hsbBase.Size));

            //左按钮
            int leftButtonIndex = 0;
            if (LeftButtonRect.Contains(pt) && MouseDownIndex == 0)
            {
                if (IsMouseDown)
                {
                    leftButtonIndex = 2;
                }
                else
                {
                    leftButtonIndex = 1;
                }
            }
            //右按钮
            int rightButtonIndex = 0;
            if (RightButtonRect.Contains(pt) && MouseDownIndex == 4)
            {
                if (IsMouseDown)
                {
                    rightButtonIndex = 2;
                }
                else
                {
                    rightButtonIndex = 1;
                }
            }
            //滚动按钮
            int scrollButtonIndex = 0;
            if (ScrollButtonRect.Contains(pt) && MouseDownIndex == 2)
            {
                if (IsMouseDown)
                {
                    scrollButtonIndex = 2;
                }
                else
                {
                    scrollButtonIndex = 1;
                }
            }

            for (int i = 0; i <= LeftButtonRects.Length - 1; i++)
            {
                GraphicHelper.DrawImageWithoutBorder(g, TrueLeftButtonImages[leftButtonIndex, i], LeftButtonRects[i]);
                GraphicHelper.DrawImageWithoutBorder(g, TrueRightButtonImages[rightButtonIndex, i], RightButtonRects[i]);
                GraphicHelper.DrawImageWithoutBorder(g, TrueScrollButtonImages[scrollButtonIndex, i], ScrollButtonRects[i]);
            }

            g = FakeScrollBar.CreateGraphics();
            g.DrawImage(bmpBuffer, 0, 0);
        }
        #endregion
    }
}
