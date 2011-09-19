using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Helpers;
using Microsoft.Win32;
using aaaSoft.SkinEngine.SkinHelpers;
using System.Diagnostics;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyVScrollBar : NativeWindow, IMyControl
    {
        //假滚动条
        Control FakeScrollBar;
        MessagePortal mp;
        Bitmap bmp;

        //经验系数
        private const double ScrollBarSpecValue = 2.5;

        //鼠标是否按下
        private bool IsMouseDown = false;
        //0:上方按钮；1：上方空白；2：滚动按钮；3：下方空白；4：下方按钮
        private int MouseDownIndex = -1;
        private Point MouseDownLocation;

        //滚动按钮最小宽度
        public static int ScrollButtonMinHeight = 15;


        private static Image _VScrollbarImage;
        public static Image VScrollbarImage
        {
            get
            {
                return _VScrollbarImage;
            }
            set
            {
                _VScrollbarImage = value;
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
        public static Image[,] SourceTopButtonImages = new Image[3, 9];
        public static Image[,] SourceBottomButtonImages = new Image[3, 9];
        public static Image[,] SourceScrollButtonImages = new Image[3, 9];
        public static Image SourceBackImage;

        //真实图片
        public static Image[,] TrueTopButtonImages = new Image[3, 9];
        public static Image[,] TrueBottomButtonImages = new Image[3, 9];
        public static Image[,] TrueScrollButtonImages = new Image[3, 9];
        public static Image TrueBackImage;

        //矩形区域
        private Rectangle TopButtonRect;
        private Rectangle BottomButtonRect;
        private Rectangle ScrollButtonRect;
        private Rectangle TopBlankRect;
        private Rectangle BottomBlankRect;

        //分块矩形区域
        private Rectangle[] TopButtonRects;
        private Rectangle[] BottomButtonRects;
        private Rectangle[] ScrollButtonRects;

        #region 初始化相关图片
        public static void InitScrollbarImage()
        {
            Image[] tmpImages = ImageHelper.CutImage(VScrollbarImage, 4, false);

            Image[] tmpBottomImages = ImageHelper.CutImage(tmpImages[0], 3, true);
            Image[] tmpTopImages = ImageHelper.CutImage(tmpImages[1], 3, true);
            Image[] tmpScrollButtonImages = ImageHelper.CutImage(tmpImages[2], 3, true);

            Rectangle[] srcTopButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpTopImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            Rectangle[] srcBottomButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpBottomImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            Rectangle[] srcScrollButtonRect = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tmpScrollButtonImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);

            for (int i = 0; i <= tmpTopImages.Length - 1; i++)
            {
                for (int j = 0; j <= srcTopButtonRect.Length - 1; j++)
                {
                    SourceBottomButtonImages[i, j] = ImageHelper.GetImage(tmpBottomImages[i], srcBottomButtonRect[j]);
                    SourceTopButtonImages[i, j] = ImageHelper.GetImage(tmpTopImages[i], srcTopButtonRect[j]);
                    SourceScrollButtonImages[i, j] = ImageHelper.GetImage(tmpScrollButtonImages[i], srcScrollButtonRect[j]);

                    //加边框
                    SourceBottomButtonImages[i, j] = ImageHelper.AddBorder(SourceBottomButtonImages[i, j]);
                    SourceTopButtonImages[i, j] = ImageHelper.AddBorder(SourceTopButtonImages[i, j]);
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
                    TrueTopButtonImages[i, j] = ImageHelper.ReplaceColor(SourceTopButtonImages[i, j], skinEng.BackColor);
                    TrueBottomButtonImages[i, j] = ImageHelper.ReplaceColor(SourceBottomButtonImages[i, j], skinEng.BackColor);
                    TrueScrollButtonImages[i, j] = ImageHelper.ReplaceColor(SourceScrollButtonImages[i, j], skinEng.BackColor);
                }
            }
        }
        #endregion
        
        VScrollBar vsbBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }
        public MyVScrollBar(VScrollBar vsb)
        {
            vsbBase = vsb;
            vsbBase_SizeChanged(vsbBase, null);
        }

        public void StopControlSkin()
        {
            if (mp == null)
            {
                return;
            }
            mp.ClosePortal();
            ReleaseHandle();

            vsbBase.SizeChanged -= new EventHandler(vsbBase_SizeChanged);

            if (vsbBase.Parent == null)
            {
                return;
            }
            vsbBase.Parent.Controls.Remove(FakeScrollBar);
            FakeScrollBar.Dispose();
        }

        public void StartControlSkin()
        {
            CalcRect();
            AssignHandle(vsbBase.Handle);

            vsbBase.SizeChanged += new EventHandler(vsbBase_SizeChanged);
            vsbBase.LocationChanged += new EventHandler(vsbBase_LocationChanged);
            vsbBase.VisibleChanged += new EventHandler(vsbBase_VisibleChanged);

            //构造一个假的滚动条
            FakeScrollBar = new Control();
            vsbBase.Parent.Controls.Add(FakeScrollBar);
            FakeScrollBar.Location = vsbBase.Location;
            FakeScrollBar.Size = vsbBase.Size;
            FakeScrollBar.Visible = vsbBase.Visible;

            FakeScrollBar.Paint += new PaintEventHandler(FakeScrollBar_Paint);

            FakeScrollBar.BringToFront();
            vsbBase.SendToBack();
            mp = new MessagePortal(FakeScrollBar.Handle, vsbBase.Handle);
            mp.OpenPortal();
        }

        #region FakeScrollBar相关事件
        void FakeScrollBar_MouseLeave()
        {
            Point pt = Form.MousePosition;
            pt = vsbBase.Parent.PointToClient(pt);

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
            pt = vsbBase.PointToClient(pt);
            MouseDownLocation = pt;

            if (TopButtonRect.Contains(pt))
            {
                MouseDownIndex = 0;
            }
            else if (TopBlankRect.Contains(pt))
            {
                MouseDownIndex = 1;
            }
            else if (ScrollButtonRect.Contains(pt))
            {
                MouseDownIndex = 2;
            }
            else if (BottomBlankRect.Contains(pt))
            {
                MouseDownIndex = 3;
            }
            else if (BottomButtonRect.Contains(pt))
            {
                MouseDownIndex = 4;
            }

            if (TopButtonRect.Contains(pt) || BottomButtonRect.Contains(pt) || ScrollButtonRect.Contains(pt))
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

        #region vsbBase相关事件
        void vsbBase_LocationChanged(object sender, EventArgs e)
        {
            FakeScrollBar.Location = vsbBase.Location;
        }

        void vsbBase_SizeChanged(object sender, EventArgs e)
        {
            bmp = new Bitmap(vsbBase.Width, vsbBase.Height);
            //计算矩形
            CalcRect();
            if (FakeScrollBar != null)
            {
                FakeScrollBar.Size = vsbBase.Size;
            }
        }

        void vsbBase_VisibleChanged(object sender, EventArgs e)
        {
            FakeScrollBar.Visible = vsbBase.Visible;
        }
        #endregion

        #region 计算矩形
        private void CalcRect()
        {
            //算矩形
            TopButtonRect = new Rectangle(new Point(0, 0), new Size(vsbBase.Width, vsbBase.Width));
            BottomButtonRect = new Rectangle(new Point(0, vsbBase.Height - vsbBase.Width), new Size(vsbBase.Width, vsbBase.Width));
            ScrollButtonRect = new Rectangle();

            if (vsbBase.Maximum <= vsbBase.Minimum)
            {
                return;
            }
            
            //中间滚动区域高度
            int MiddleHeight = vsbBase.Height - 2 * vsbBase.Width;
            
            //可见客户区高度(像素)
            int TrueViewedClientHeight = 0;
            
            //滚动条Max,Min,Value三个值对应的像素值
            int TrueClientMax;
            int TrueClientMin;
            int TrueClientValue;

            //客户区减少的量
            int ClientHeightOffset = 0;
            if (vsbBase.Parent is DataGridView)
            {
                DataGridView dgv = (DataGridView)vsbBase.Parent;
                if (dgv.ColumnHeadersVisible)
                {
                    ClientHeightOffset = dgv.ColumnHeadersHeight;

                }
            }

            //客户区高度
            TrueViewedClientHeight = vsbBase.Height - ClientHeightOffset;
            if (vsbBase.Parent is DataGridView)
            {
                TrueClientMax = vsbBase.Maximum;
                TrueClientMin = vsbBase.Minimum;
                TrueClientValue = vsbBase.Value;
            }
            else if (vsbBase.Parent.GetType().ToString() == "System.Windows.Forms.PropertyGridInternal.PropertyGridView")
            {
                //经验系数
                int specValue = 17;
                TrueClientMax = (vsbBase.Maximum + 1) * specValue;
                TrueClientMin = vsbBase.Minimum * specValue;
                TrueClientValue = vsbBase.Value * specValue;
            }
            else
            {
                TrueClientMax = vsbBase.Maximum;
                TrueClientMin = vsbBase.Minimum;
                TrueClientValue = vsbBase.Value;
            }

            //====关键是算对滑块高度====
            //滑块高度
            ScrollButtonRect.Height = Convert.ToInt32(TrueViewedClientHeight * MiddleHeight / (TrueClientMax - TrueClientMin));

            if (ScrollButtonRect.Height > MiddleHeight)
            {
                ScrollButtonRect.Height = MiddleHeight;
                ScrollButtonRect.Y = MiddleHeight - ScrollButtonRect.Height + vsbBase.Width;
            }
            else
            {
                if (ScrollButtonRect.Height < ScrollButtonMinHeight)
                {
                    ScrollButtonRect.Height = ScrollButtonMinHeight;
                }

                ScrollButtonRect.Y = Convert.ToInt32(((MiddleHeight - ScrollButtonRect.Height) * TrueClientValue
                    / Math.Abs((TrueClientMax - TrueViewedClientHeight))))
                    + vsbBase.Width;

                if (ScrollButtonRect.Bottom > MiddleHeight + vsbBase.Width)
                {
                    ScrollButtonRect.Y = MiddleHeight + vsbBase.Width - ScrollButtonRect.Height;
                }
            }

            ScrollButtonRect.Width = vsbBase.Width;
            ScrollButtonRect.X = 0;

            TopBlankRect = new Rectangle(TopButtonRect.Right, TopButtonRect.Top, ScrollButtonRect.Left - TopButtonRect.Right, TopButtonRect.Height);
            BottomBlankRect = new Rectangle(ScrollButtonRect.Right, BottomButtonRect.Top, BottomButtonRect.Left - ScrollButtonRect.Right, BottomButtonRect.Height);

            //算分块矩形
            TopButtonRects = ShapeHelper.GetRectangles(TopButtonRect, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            BottomButtonRects = ShapeHelper.GetRectangles(BottomButtonRect, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
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
                        NativeMethods.SetCapture(vsbBase.Handle);
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
            Graphics g = Graphics.FromImage(bmp);

            Point pt = Form.MousePosition;
            pt = vsbBase.PointToClient(pt);
            //背景
            GraphicHelper.DrawImageWithoutBorder(g, TrueBackImage, new Rectangle(new Point(0, 0), vsbBase.Size));

            //上方按钮
            int topButtonIndex = 0;
            if (TopButtonRect.Contains(pt) && MouseDownIndex == 0)
            {
                if (IsMouseDown)
                {
                    topButtonIndex = 2;
                }
                else
                {
                    topButtonIndex = 1;
                }
            }
            //正方按钮
            int bottomButtonIndex = 0;
            if (BottomButtonRect.Contains(pt) && MouseDownIndex == 4)
            {
                if (IsMouseDown)
                {
                    bottomButtonIndex = 2;
                }
                else
                {
                    bottomButtonIndex = 1;
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

            if (TopButtonRects != null)
            {
                for (int i = 0; i <= TopButtonRects.Length - 1; i++)
                {
                    GraphicHelper.DrawImageWithoutBorder(g, TrueTopButtonImages[topButtonIndex, i], TopButtonRects[i]);
                    GraphicHelper.DrawImageWithoutBorder(g, TrueBottomButtonImages[bottomButtonIndex, i], BottomButtonRects[i]);
                    GraphicHelper.DrawImageWithoutBorder(g, TrueScrollButtonImages[scrollButtonIndex, i], ScrollButtonRects[i]);
                }
            }

            g = FakeScrollBar.CreateGraphics();
            g.DrawImage(bmp, 0, 0);
        }
        #endregion
    }
}
