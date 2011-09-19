using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using aaaSoft.SkinEngine.SkinHelpers;
using System.Diagnostics;
using Microsoft.Win32;
using aaaSoft.Helpers;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;

namespace aaaSoft.SkinEngine.MyControls
{
    class MyForm : NativeWindow, IMyControl, IMyContainer
    {
        Form frmBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }
        Bitmap bmpBuffer;


        #region 窗体边框设置
        //左侧边框宽度
        public static int LeftBorderWidth = 3;
        //右侧边框宽度
        public static int RightBorderWidth = 3;
        //上方边框高度
        public static int TopBorderHeight = 3;
        //下方边框高度
        public static int BottomBorderHeight = 3;
        //标题栏高度(不包含上边框!)
        public static int TopicHeight = 27;
        //最大化后系统按钮下移像素
        public static int SystemButtonOffsetWhenMaxmize = 3;
        #endregion

        //窗体背景色
        public static Color FormBackColor = Color.FromArgb(248, 249, 253);
        //窗体的状态数量
        public static int FormStateCount = 1;

        #region 图标相关设置

        //图标大小
        public static Size IconSize = new Size(16, 16);

        #endregion
        
        #region 标题栏文本

        //标题栏文本颜色
        public static Color TitleTextColor = Color.White;
        //标题栏文本对齐方式
        public static StringAlignment TitleTextAlign = StringAlignment.Near;
        //标题栏字体
        public static Font TitleTextFont = new Font("微软雅黑", 9, FontStyle.Regular);

        #endregion




        //系统边框大小
        private int SystemBorder;
        //系统标题栏高度
        private int SystemTopicHeight;        
        //非客户区是否鼠标已按下
        private bool IsNCMouseDown = false;
        //非客户区鼠标按下时的坐标
        private Point NCMouseDownLocation;
        //非客户区鼠标按下时窗体的坐标
        private Point NCMouseDownFormLocation;

        //鼠标是否在系统按钮矩形中
        private bool IsMouseInSystemButtonRect = false;
        //按下鼠标时的系统按钮序号
        private int MouseDownSystemButtonIndex = -1;

        private enum DX:int
        {
            West = 0xF001,
            East = 0xF002,
            North = 0xF003,
            NorthWest = 0xF004,
            NorthEast = 0xF005,
            South = 0xF006,
            SouthWest = 0xF007,
            SouthEast = 0xF008
        }
            
        #region 属性 - 原始窗体标题栏及边框图片

        //原始窗体标题栏及边框图片
        private static Image _FormImage;
        public static Image FormImage
        {
            get
            {
                return _FormImage;
            }
            set
            {
                _FormImage = value;
                if (_FormImage != null)
                {
                    _FormImage = ImageHelper.ScaleImage(value, value.Size);
                    InitFormImages();
                    ChangeControlColor();
                }
            }
        }
        #endregion

        #region 边框及标题栏图片数组
        //原始图片数组
        public static Image[] SourceFormImages = new Image[FormStateCount];
        //左
        public static Image[] SourceLeftImages = new Image[FormStateCount];
        //右
        public static Image[] SourceRightImages = new Image[FormStateCount];
        //上
        public static Image[] SourceTopImages = new Image[FormStateCount];
        //下
        public static Image[] SourceBottomImages = new Image[FormStateCount];
        //左上
        public static Image[] SourceLeftTopImages = new Image[FormStateCount];
        //右上
        public static Image[] SourceRightTopImages = new Image[FormStateCount];
        //左下
        public static Image[] SourceLeftBottomImages = new Image[FormStateCount];
        //右下
        public static Image[] SourceRightBottomImages = new Image[FormStateCount];
        //标题栏
        public static Image[] SourceTopicImages = new Image[FormStateCount];
        //标题栏左
        public static Image[] SourceLeftTopicImages = new Image[FormStateCount];
        //标题栏右
        public static Image[] SourceRightTopicImages = new Image[FormStateCount];

        //真实图片数组
        public static Image[] TrueFormImages = new Image[FormStateCount];
        //左
        public static Image[] TrueLeftImages = new Image[FormStateCount];
        //右
        public static Image[] TrueRightImages = new Image[FormStateCount];
        //上
        public static Image[] TrueTopImages = new Image[FormStateCount];
        //下
        public static Image[] TrueBottomImages = new Image[FormStateCount];
        //左上
        public static Image[] TrueLeftTopImages = new Image[FormStateCount];
        //右上
        public static Image[] TrueRightTopImages = new Image[FormStateCount];
        //左下
        public static Image[] TrueLeftBottomImages = new Image[FormStateCount];
        //右下
        public static Image[] TrueRightBottomImages = new Image[FormStateCount];
        //标题栏
        public static Image[] TrueTopicImages = new Image[FormStateCount];
        //标题栏左
        public static Image[] TrueLeftTopicImages = new Image[FormStateCount];
        //标题栏右
        public static Image[] TrueRightTopicImages = new Image[FormStateCount];

        #endregion

        #region 边框及标题栏矩形划分
        Rectangle[] FormRects;
        #endregion


        #region 属性 - 原始系统按钮图片

        //原始系统按钮图片
        private static Image _SystemButtonImage;
        public static Image SystemButtonImage
        {
            get
            {
                return _SystemButtonImage;
            }
            set
            {
                _SystemButtonImage = value;
                if (value != null)
                {
                    InitSystemButton();
                    ChangeSystemButtonColor();
                }
            }
        }
        #endregion

        #region 系统按钮图片数组
        //4种按钮，3种状态
        public static Image[,] SourceSystemButtonImages = new Image[4, 3];
        public static Image[,] TrueSystemButtonImages = new Image[4, 3];
        #endregion

        #region 系统按钮矩形划分
        //3个位置
        private Rectangle[] SystemButtonRects = new Rectangle[3];
        #endregion

        //窗体菜单
        private ContextMenuStrip cms;
        ToolStripItem tsiRestore;
        ToolStripItem tsiMin;
        ToolStripItem tsiMax;
        ToolStripItem tsiClose;

        #region 构造函数
        List<String> typeNames = new List<String>();
        public MyForm(Form frm)
        {
            frmBase = frm;

            typeNames.Add(typeof(Label).ToString());
            typeNames.Add(typeof(Panel).ToString());
            typeNames.Add(typeof(GroupBox).ToString());
            typeNames.Add(typeof(TableLayoutPanel).ToString());
            typeNames.Add(typeof(PictureBox).ToString());

            cms = new ContextMenuStrip();
            tsiRestore = cms.Items.Add("还原(&R)");
            tsiRestore.Click += new EventHandler(tsiRestore_Click);
            tsiMin = cms.Items.Add("最小化(&N)");
            tsiMin.Click += new EventHandler(tsiMin_Click);
            tsiMax = cms.Items.Add("最大化(&X)");
            tsiMax.Click += new EventHandler(tsiMax_Click);
            cms.Items.Add("-");
            tsiClose = cms.Items.Add("关闭(&C)     Alt+F4");
            tsiClose.Click += new EventHandler(tsiClose_Click);

            frmBase_SizeChanged(frmBase, null);
        }

        #endregion
        
        #region 菜单
        //还原
        void tsiRestore_Click(object sender, EventArgs e)
        {
            frmBase.WindowState = FormWindowState.Normal;
        }
        //最小化
        void tsiMin_Click(object sender, EventArgs e)
        {
            frmBase.WindowState = FormWindowState.Minimized;
        }
        //最大化
        void tsiMax_Click(object sender, EventArgs e)
        {
            frmBase.WindowState = FormWindowState.Maximized;
        }
        //关闭
        void tsiClose_Click(object sender, EventArgs e)
        {
            frmBase.Close();
        }
        #endregion

        #region 开启和停止
        public void StopControlSkin()
        {
            ReleaseHandle();
            skinEng.BackColorChanged -= new EventHandler(BackColorChanged);
            frmBase.SizeChanged -= new EventHandler(frmBase_SizeChanged);
            frmBase.FormClosed -= new FormClosedEventHandler(frmBase_FormClosed);

            frmBase.BackColor = SystemColors.Control;
            frmBase.Region = null;
        }


        public void StartControlSkin()
        {
            Size BaseFormMinimumSize = new Size(LeftBorderWidth + RightBorderWidth, TopBorderHeight + TopicHeight + BottomBorderHeight);
            if (frmBase.MinimumSize.IsEmpty)
            {
                frmBase.MinimumSize = BaseFormMinimumSize;
            }
            else
            {
                Size newMinSize = frmBase.MinimumSize;
                if (newMinSize.Width < BaseFormMinimumSize.Width)
                {
                    newMinSize.Width = BaseFormMinimumSize.Width;
                }
                if (newMinSize.Height < BaseFormMinimumSize.Height)
                {
                    newMinSize.Height = BaseFormMinimumSize.Height;
                }
            }

            AssignHandle(frmBase.Handle);
            skinEng.BackColorChanged += new EventHandler(BackColorChanged);
            frmBase.SizeChanged += new EventHandler(frmBase_SizeChanged);
            frmBase.FormClosed += new FormClosedEventHandler(frmBase_FormClosed);

            //系统边框大小
            SystemBorder = (frmBase.Width - frmBase.ClientSize.Width) / 2;
            //系统标题栏高度
            SystemTopicHeight = frmBase.Height - frmBase.ClientSize.Height - 2 * SystemBorder;

            frmBase.BackColor = ColorHelper.ReplaceColor(FormBackColor, skinEng.BackColor);
            frmBase_SizeChanged(frmBase, null);
        }

        void frmBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmBase.Location = new Point(0 - frmBase.Width, 0 - frmBase.Height);
            StopControlSkin();
        }

        #endregion

        #region 窗体大小改变时
        //上次窗体状态
        private FormWindowState preWindowState = FormWindowState.Normal;
        //序号
        private int preWindowStateInt = 0;

        void frmBase_SizeChanged(object sender, EventArgs e)
        {
            //如果窗体没有边框
            if (frmBase.FormBorderStyle == FormBorderStyle.None)
            {
                return;
            }
            if (frmBase.WindowState != FormWindowState.Minimized)
            {
                bmpBuffer = new Bitmap(frmBase.Width, frmBase.Height);
            }
            CalcRects();

            //设置变量
            if (CurrentWindowState != frmBase.WindowState)
            {
                PreWindowState = CurrentWindowState;
                CurrentWindowState = frmBase.WindowState;
            }

            //设置区域
            Region rgn = new Region(new Rectangle(new Point(0, 0), frmBase.Size));
            GraphicsPath gp = GetUndrawGraphicsPath();
            rgn.Exclude(gp);
            frmBase.Region = rgn;
        }
        #endregion

        #region 计算矩形
        private void CalcRects()
        {
            try
            {
                //边框及标题栏矩形
                FormRects = ShapeHelper.GetRectangles(frmBase.Size, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight, TopicHeight);
                //系统按钮矩形
                SystemButtonRects[2] = new Rectangle(new Point(frmBase.Width - RightBorderWidth - SourceSystemButtonImages[3, 0].Width, 0), SourceSystemButtonImages[3, 0].Size);
                SystemButtonRects[1] = new Rectangle(new Point(SystemButtonRects[2].Left - SourceSystemButtonImages[1, 0].Width, 0), SourceSystemButtonImages[1, 0].Size);
                SystemButtonRects[0] = new Rectangle(new Point(SystemButtonRects[1].Left - SourceSystemButtonImages[0, 0].Width, 0), SourceSystemButtonImages[0, 0].Size);


                if (!frmBase.MinimizeBox)
                {
                    SystemButtonRects[0].Y = 0 - SystemButtonRects[0].Height;
                }
                if (!frmBase.MaximizeBox)
                {
                    if (frmBase.MinimizeBox)
                    {
                        SystemButtonRects[0] = SystemButtonRects[1];
                    }
                    SystemButtonRects[1].Y = 0 - SystemButtonRects[1].Height;
                }

                if (frmBase.WindowState == FormWindowState.Maximized)
                {
                    for (int i = 0; i <= SystemButtonRects.Length - 1; i++)
                    {
                        SystemButtonRects[i].Y += SystemButtonOffsetWhenMaxmize;
                    }
                }
            }
            catch { }
        }
        #endregion

        void BackColorChanged(object sender, EventArgs e)
        {
            frmBase.BackColor = ColorHelper.ReplaceColor(FormBackColor, skinEng.BackColor);
            ChangeSystemButtonColor();
            DrawWindowBorderAndTitle();
            DrawSystemButton();
        }

        #region 初始化窗体图片
        public static void InitFormImages()
        {
            SourceFormImages[0] = FormImage;

            //原始图片矩形划分
            Rectangle[] SourceRects = ShapeHelper.GetRectangles(FormImage.Size, LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight, TopicHeight);

            for (int i = 0; i <= FormStateCount - 1; i++)
            {
                Image tmpImage = SourceFormImages[i];

                //切分出十一个部分的图片
                SourceLeftTopImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[0]);
                SourceTopImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[1]);
                SourceRightTopImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[2]);

                SourceLeftTopicImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[3]);
                SourceTopicImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[4]);
                SourceRightTopicImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[5]);

                SourceLeftImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[6]);
                SourceRightImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[8]);

                SourceLeftBottomImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[9]);
                SourceBottomImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[10]);
                SourceRightBottomImages[i] = ImageHelper.GetImage(tmpImage, SourceRects[11]);

                //给图片加边框(因为GDI+的BUG)
                SourceLeftTopImages[i] = ImageHelper.AddBorder(SourceLeftTopImages[i]);
                SourceTopImages[i] = ImageHelper.AddBorder(SourceTopImages[i]);
                SourceRightTopImages[i] = ImageHelper.AddBorder(SourceRightTopImages[i]);

                SourceLeftTopicImages[i] = ImageHelper.AddBorder(SourceLeftTopicImages[i]);
                SourceTopicImages[i] = ImageHelper.AddBorder(SourceTopicImages[i]);
                SourceRightTopicImages[i] = ImageHelper.AddBorder(SourceRightTopicImages[i]);

                SourceLeftImages[i] = ImageHelper.AddBorder(SourceLeftImages[i]);
                SourceRightImages[i] = ImageHelper.AddBorder(SourceRightImages[i]);

                SourceLeftBottomImages[i] = ImageHelper.AddBorder(SourceLeftBottomImages[i]);
                SourceBottomImages[i] = ImageHelper.AddBorder(SourceBottomImages[i]);
                SourceRightBottomImages[i] = ImageHelper.AddBorder(SourceRightBottomImages[i]);
            }
        }
        #endregion
        
        #region 调整图片颜色
        public static void ChangeControlColor()
        {
            for (int i = 0; i <= FormStateCount - 1; i++)
            {
                TrueLeftTopImages[i] = ImageHelper.ReplaceColor(SourceLeftTopImages[i], skinEng.BackColor);
                TrueTopImages[i] = ImageHelper.ReplaceColor(SourceTopImages[i], skinEng.BackColor);
                TrueRightTopImages[i] = ImageHelper.ReplaceColor(SourceRightTopImages[i], skinEng.BackColor);

                TrueLeftTopicImages[i] = ImageHelper.ReplaceColor(SourceLeftTopicImages[i], skinEng.BackColor);
                TrueTopicImages[i] = ImageHelper.ReplaceColor(SourceTopicImages[i], skinEng.BackColor);
                TrueRightTopicImages[i] = ImageHelper.ReplaceColor(SourceRightTopicImages[i], skinEng.BackColor);

                TrueLeftImages[i] = ImageHelper.ReplaceColor(SourceLeftImages[i], skinEng.BackColor);
                TrueRightImages[i] = ImageHelper.ReplaceColor(SourceRightImages[i], skinEng.BackColor);

                TrueLeftBottomImages[i] = ImageHelper.ReplaceColor(SourceLeftBottomImages[i], skinEng.BackColor);
                TrueBottomImages[i] = ImageHelper.ReplaceColor(SourceBottomImages[i], skinEng.BackColor);
                TrueRightBottomImages[i] = ImageHelper.ReplaceColor(SourceRightBottomImages[i], skinEng.BackColor);
            }
        }
        #endregion

        #region 初始化系统按钮图片
        public static void InitSystemButton()
        {
            Image[] tmpImages = ImageHelper.CutImage(SystemButtonImage, 4, true);
            for (int i = 0; i <= tmpImages.Length - 1; i++)
            {
                Image tmpPlaceImage = ImageHelper.RemoveRightWhiteSpace(tmpImages[i], Color.FromArgb(255, 0, 255));
                Image[] tmpPlaceImages = ImageHelper.CutImage(tmpPlaceImage, 3, false);

                for (int j = 0; j <= tmpPlaceImages.Length - 1; j++)
                {
                    SourceSystemButtonImages[i, j] = tmpPlaceImages[j];
                    TrueSystemButtonImages[i, j] = SourceSystemButtonImages[i, j];
                }
            }
        }
        #endregion

        #region 改变系统按钮颜色
        public static void ChangeSystemButtonColor()
        {
            for (int i = 0; i <= 4 - 1; i++)
            {
                for (int j = 0; j <= 3 - 1; j++)
                {
                    TrueSystemButtonImages[i, j] = ImageHelper.ReplaceColor(SourceSystemButtonImages[i, j], skinEng.BackColor);
                }
            }
        }
        #endregion

        private void TrackMouse()
        {
            NativeStructs.LPTRACKMOUSEEVENT tme = new NativeStructs.LPTRACKMOUSEEVENT();
            tme.hwndTrack = frmBase.Handle;
            tme.cbSize = Convert.ToUInt32(Marshal.SizeOf(tme));
            tme.dwFlags = (int)NativeConsts.TME.NONCLIENT + (int)NativeConsts.TME.LEAVE;
            tme.dwHoverTime = 1000 * 3;
            NativeMethods.TrackMouseEvent(ref tme);
        }

        #region 得到透明部分图形路线
        private GraphicsPath GetUndrawGraphicsPath()
        {
            GraphicsPath gp = new GraphicsPath();
            Bitmap tmpBmp = (Bitmap)FormImage;
            //左上
            for (int i = 0; i <= LeftBorderWidth - 1; i++)
            {
                for (int j = 0; j <= TopBorderHeight - 1; j++)
                {
                    Color tmpColor = tmpBmp.GetPixel(i, j);
                    if (tmpColor == Color.FromArgb(255, 0, 255))
                    {
                        gp.AddRectangle(new Rectangle(i, j, 1, 1));
                    }
                }
            }
            //右上
            for (int i = tmpBmp.Width - RightBorderWidth; i <= tmpBmp.Width - 1; i++)
            {
                for (int j = 0; j <= TopBorderHeight - 1; j++)
                {
                    Color tmpColor = tmpBmp.GetPixel(i, j);
                    if (tmpColor == Color.FromArgb(255, 0, 255))
                    {
                        gp.AddRectangle(new Rectangle(i + FormRects[2].Left - (tmpBmp.Width - RightBorderWidth), j, 1, 1));
                    }
                }
            }


            //左下
            for (int i = 0; i <= LeftBorderWidth - 1; i++)
            {
                for (int j = tmpBmp.Height - BottomBorderHeight; j <= tmpBmp.Height - 1; j++)
                {
                    Color tmpColor = tmpBmp.GetPixel(i, j);
                    if (tmpColor == Color.FromArgb(255, 0, 255))
                    {
                        gp.AddRectangle(new Rectangle(i, j + FormRects[9].Top - (tmpBmp.Height - BottomBorderHeight), 1, 1));
                    }
                }
            }

            //右下
            for (int i = tmpBmp.Width - RightBorderWidth; i <= tmpBmp.Width - 1; i++)
            {
                for (int j = tmpBmp.Height - BottomBorderHeight; j <= tmpBmp.Height - 1; j++)
                {
                    Color tmpColor = tmpBmp.GetPixel(i, j);
                    if (tmpColor == Color.FromArgb(255, 0, 255))
                    {
                        gp.AddRectangle(new Rectangle(i + FormRects[11].Left - (tmpBmp.Width - RightBorderWidth), j + FormRects[11].Top - (tmpBmp.Height - BottomBorderHeight), 1, 1));
                    }
                }
            }
            return gp;
        }
        #endregion

        /*
        String preHashCode;
        String preMessage;
        */

        //上一次接收到WM_NCMOUSEMOVE消息时WParam的值
        private IntPtr PreNCMOUSEMOVE_WParam = IntPtr.Zero;
        //之前窗体状态
        private FormWindowState PreWindowState = FormWindowState.Minimized;
        //之后窗体状态
        private FormWindowState CurrentWindowState = FormWindowState.Minimized;

        #region 窗体消息处理
        String preHashCode;
        String preMessage;
        protected override void WndProc(ref Message m)
        {
            //如果窗体没有边框
            if (frmBase.FormBorderStyle == FormBorderStyle.None)
            {
                base.DefWndProc(ref m);
                return;
            }

            /*
            //if (!frmBase.Name.StartsWith("Form1"))
            {
                string msgStr = Enum.GetName(typeof(NativeConsts.WindowMessage), m.Msg);
                if (String.IsNullOrEmpty(msgStr))
                {
                    msgStr = m.Msg.ToString();
                }
                Debug.Print(frmBase.Name + " 消息:" + msgStr);
            }
            */

            /*
            string msgStr = Enum.GetName(typeof(NativeConsts.WindowMessage), m.Msg);
            if (String.IsNullOrEmpty(msgStr))
            {
                msgStr = m.Msg.ToString();
            }
#warning 拍照方式找到某一消息

            //if (m.Msg == (int)NativeConsts.WindowMessage.WM_NCPAINT)

            //if (frmBase.Name == "Form1")
            {
                {
                    Bitmap bmp = new Bitmap(SystemButtonRects[2].Width, SystemButtonRects[2].Height);
                    Graphics g = Graphics.FromImage(bmp);

                    Point scrPoint = SystemButtonRects[2].Location;
                    scrPoint.X += frmBase.Left;
                    scrPoint.Y += frmBase.Top;

                    g.CopyFromScreen(scrPoint, new Point(0, 0), SystemButtonRects[2].Size);
                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    String currentHashCode = Convert.ToBase64String(ms.ToArray());
                    if (currentHashCode != preHashCode)
                    {
                        Debug.Print("DIFF!!!");
                        Debug.Print(Control.FromHandle(m.HWnd).Name + " 消息：" + msgStr + " WPARAM:" + m.WParam + "   之前消息：" + preMessage);
                    }
                    else
                    {
                        Debug.Print(Control.FromHandle(m.HWnd).Name + " 消息：" + msgStr + " WPARAM:" + m.WParam);
                    }
                    preHashCode = currentHashCode;
                    preMessage = msgStr;
                }
            }
            */

            switch ((Microsoft.Win32.NativeConsts.WindowMessage)m.Msg)
            {
                #region NCHITTEST
                case NativeConsts.WindowMessage.WM_NCHITTEST:
                    {
                        base.WndProc(ref m);
                        switch ((Microsoft.Win32.NativeConsts.HT)m.Result)
                        {
                            case NativeConsts.HT.HTMAXBUTTON:
                            case NativeConsts.HT.HTMINBUTTON:
                            case NativeConsts.HT.HTCLOSE:

                            case NativeConsts.HT.HTLEFT:
                            case NativeConsts.HT.HTRIGHT:
                            case NativeConsts.HT.HTTOP:
                            case NativeConsts.HT.HTTOPLEFT:
                            case NativeConsts.HT.HTTOPRIGHT:
                            case NativeConsts.HT.HTBOTTOM:
                            case NativeConsts.HT.HTBOTTOMLEFT:
                            case NativeConsts.HT.HTBOTTOMRIGHT:
                                {
                                    m.Result = (IntPtr)Microsoft.Win32.NativeConsts.HT.HTCAPTION;
                                    return;
                                }
                        }
                        break;
                    }
                #endregion

                #region 此消息为Windows画系统按钮消息
                case NativeConsts.WindowMessage.WM_SYSCTLPAINT:
                    {
                        return;
                    }
                #endregion

                #region 改变大小消息
                case NativeConsts.WindowMessage.WM_SIZE:
                    {
                        if (preWindowStateInt == 1)
                        {
                            preWindowStateInt = 0;

                            int XOffset = (2 * SystemBorder) - (LeftBorderWidth + RightBorderWidth);
                            int YOffset = (2 * SystemBorder + SystemTopicHeight) - (TopicHeight + 2 * BottomBorderHeight);
                            Size newSize = new Size(frmBase.Width - XOffset, frmBase.Height - YOffset);
                            frmBase.Size = newSize;
                            
                            DrawWindowBorderAndTitle();
                            return;
                        }
                        if (frmBase.WindowState != preWindowState)
                        {
                            preWindowState = frmBase.WindowState;

                            if (frmBase.WindowState == FormWindowState.Normal)
                            {
                                preWindowStateInt += 1;
                                return;
                            }
                        }
                        break;
                    }
                #endregion

                #region 非客户区激活或非激活时
                case NativeConsts.WindowMessage.WM_ACTIVATE:
                case NativeConsts.WindowMessage.WM_NCACTIVATE:
                case NativeConsts.WindowMessage.WM_SETTEXT:
                    {
                        int MethodIndex = 0;
                        //两种处理方法
                        switch (MethodIndex)
                        {
                            case 0:
                                NativeMethods.LockWindowUpdate(frmBase.Handle);
                                base.WndProc(ref m);
                                NativeMethods.LockWindowUpdate(IntPtr.Zero);
                                break;
                            case 1:
                                base.WndProc(ref m);
                                DrawWindowBorderAndTitle();
                                frmBase.Invalidate(true);
                                break;
                        }
                        return;
                    }
                #endregion

                #region 非客户区重绘
                case NativeConsts.WindowMessage.WM_NCPAINT:
                    {
                        //画边框、标题栏和系统按钮
                        DrawWindowBorderAndTitle();
                        return;
                    }
                #endregion

                #region 非客户区计算大小
                case NativeConsts.WindowMessage.WM_NCCALCSIZE:
                    {
                        //非设计模式时才起作用
                        if (!skinEng.IsDesignMode)
                        {
                            Microsoft.Win32.NativeStructs.tagNCCALCSIZE_PARAMS ncSizeStruct;
                            ncSizeStruct = (Microsoft.Win32.NativeStructs.tagNCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(Microsoft.Win32.NativeStructs.tagNCCALCSIZE_PARAMS));
                            ncSizeStruct.rgrc[0].top -= (SystemBorder + SystemTopicHeight) - (TopBorderHeight + TopicHeight);
                            ncSizeStruct.rgrc[0].left -= SystemBorder - LeftBorderWidth;
                            ncSizeStruct.rgrc[0].right += SystemBorder - RightBorderWidth;
                            ncSizeStruct.rgrc[0].bottom += SystemBorder - BottomBorderHeight;

                            Marshal.StructureToPtr(ncSizeStruct, m.LParam, true);
                        }
                        break;
                    }
                #endregion

                #region 非客户区鼠标移动
                case NativeConsts.WindowMessage.WM_NCMOUSEMOVE:
                    {
                        TrackMouse();

                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;
                        if (SystemButtonRects[0].Contains(pt) || SystemButtonRects[1].Contains(pt) || SystemButtonRects[2].Contains(pt))
                        {
                            //如果鼠标在系统按钮矩形上
                            DrawSystemButton();

                            IsMouseInSystemButtonRect = true;
                        }
                        else
                        {
                            //鼠标不在系统按钮矩形上
                            if (PreWindowState != CurrentWindowState)
                            {
                                if (PreNCMOUSEMOVE_WParam == (IntPtr)NativeConsts.HT.HTCAPTION && 
                                    m.WParam == (IntPtr)NativeConsts.HT.HTBOTTOMRIGHT)
                                {
                                    DrawWindowBorderAndTitle();
                                    PreWindowState = CurrentWindowState;
                                    Debug.Print("MyForm.WndProc  WM_NCMOUSEMOVE:已处理一次画系统按钮行为！");
                                }
                            }
                            PreNCMOUSEMOVE_WParam = m.WParam;

                            if (IsMouseInSystemButtonRect)
                            {
                                DrawSystemButton();
                                IsMouseInSystemButtonRect = false;
                                MouseDownSystemButtonIndex = -1;
                            }

                            if (frmBase.FormBorderStyle == FormBorderStyle.Sizable || frmBase.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                            {
                                if (FormRects[0].Contains(pt))
                                {
                                    //左上角
                                    frmBase.Cursor = Cursors.SizeNWSE;
                                }
                                else if (FormRects[1].Contains(pt))
                                {
                                    //上边框
                                    frmBase.Cursor = Cursors.SizeNS;
                                }
                                else if (FormRects[2].Contains(pt))
                                {
                                    //右上角
                                    frmBase.Cursor = Cursors.SizeNESW;
                                }
                                else if (FormRects[3].Contains(pt) || FormRects[6].Contains(pt))
                                {
                                    //左边框
                                    frmBase.Cursor = Cursors.SizeWE;
                                }
                                else if (FormRects[4].Contains(pt))
                                {
                                    //标题栏
                                    frmBase.Cursor = Cursors.Default;
                                }
                                else if (FormRects[5].Contains(pt) || FormRects[8].Contains(pt))
                                {
                                    //右边框
                                    frmBase.Cursor = Cursors.SizeWE;
                                }
                                else if (FormRects[9].Contains(pt))
                                {
                                    //左下角
                                    frmBase.Cursor = Cursors.SizeNESW;
                                }
                                else if (FormRects[10].Contains(pt))
                                {
                                    //下边框
                                    frmBase.Cursor = Cursors.SizeNS;
                                }
                                else if (FormRects[11].Contains(pt))
                                {
                                    //右下角
                                    frmBase.Cursor = Cursors.SizeNWSE;
                                }
                                else
                                {
                                    frmBase.Cursor = Cursors.Default;
                                }
                            }
                            else
                            {
                                frmBase.Cursor = Cursors.Default;
                            }
                        }
                        break;
                    }
                #endregion

                #region 非客户区鼠标左键按下
                case NativeConsts.WindowMessage.WM_NCLBUTTONDOWN:
                    {
                        frmBase.BringToFront();
                        IsNCMouseDown = true;
                        NCMouseDownLocation = Form.MousePosition;
                        NCMouseDownFormLocation = frmBase.Location;

                        DrawSystemButton();

                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;
                        for (int i = 0; i <= SystemButtonRects.Length - 1; i++)
                        {
                            if (SystemButtonRects[i].Contains(pt))
                            {
                                MouseDownSystemButtonIndex = i;
                                break;
                            }
                        }

                        NativeMethods.ReleaseCapture();

                        if (frmBase.FormBorderStyle == FormBorderStyle.Sizable || frmBase.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                        {
                            if (SystemButtonRects[0].Contains(pt) || SystemButtonRects[1].Contains(pt) || SystemButtonRects[2].Contains(pt))
                            {
                            }
                            else
                            {
                                if (FormRects[0].Contains(pt))
                                {
                                    //左上角
                                    frmBase.Cursor = Cursors.SizeNWSE;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.NorthWest, IntPtr.Zero);
                                }
                                else if (FormRects[1].Contains(pt))
                                {
                                    //上边框
                                    frmBase.Cursor = Cursors.SizeNS;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.North, IntPtr.Zero);
                                }
                                else if (FormRects[2].Contains(pt))
                                {
                                    //右上角
                                    frmBase.Cursor = Cursors.SizeNESW;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.NorthEast, IntPtr.Zero);
                                }
                                else if (FormRects[3].Contains(pt) || FormRects[6].Contains(pt))
                                {
                                    //左边框
                                    frmBase.Cursor = Cursors.SizeWE;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.West, IntPtr.Zero);
                                }
                                else if (FormRects[4].Contains(pt))
                                {
                                    //标题栏
                                    frmBase.Cursor = Cursors.Default;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)(0xF012 + NativeConsts.HT.HTCAPTION), IntPtr.Zero);
                                }
                                else if (FormRects[5].Contains(pt) || FormRects[8].Contains(pt))
                                {
                                    //右边框
                                    frmBase.Cursor = Cursors.SizeWE;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.East, IntPtr.Zero);
                                }
                                else if (FormRects[9].Contains(pt))
                                {
                                    //左下角
                                    frmBase.Cursor = Cursors.SizeNESW;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.SouthWest, IntPtr.Zero);
                                }
                                else if (FormRects[10].Contains(pt))
                                {
                                    //下边框
                                    frmBase.Cursor = Cursors.SizeNS;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.South, IntPtr.Zero);
                                }
                                else if (FormRects[11].Contains(pt))
                                {
                                    //右下角
                                    frmBase.Cursor = Cursors.SizeNWSE;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)DX.SouthEast, IntPtr.Zero);
                                }
                                else
                                {
                                    frmBase.Cursor = Cursors.Default;
                                }
                            }
                        }
                        else
                        {
                            if (SystemButtonRects[0].Contains(pt) || SystemButtonRects[1].Contains(pt) || SystemButtonRects[2].Contains(pt))
                            {
                            }
                            else
                            {
                                if (FormRects[4].Contains(pt))
                                {
                                    //标题栏
                                    frmBase.Cursor = Cursors.Default;
                                    NativeMethods.SendMessage(m.HWnd, (int)NativeConsts.WindowMessage.WM_SYSCOMMAND, (IntPtr)(0xF012 + NativeConsts.HT.HTCAPTION), IntPtr.Zero);
                                }
                                else
                                {
                                    frmBase.Cursor = Cursors.Default;
                                }
                            }
                        }
                        return;
                    }
                #endregion

                #region 非客户区鼠标左键双击
                case NativeConsts.WindowMessage.WM_NCLBUTTONDBLCLK:
                    {
                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;

                        if (FormRects[4].Contains(pt))
                        {
                            if (!frmBase.MaximizeBox)
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                        break;
                    }
                #endregion

                #region 非客户区鼠标右键按下
                case NativeConsts.WindowMessage.WM_NCRBUTTONDOWN:
                    {
                        return;
                    }
                #endregion

                #region 非客户区鼠标右键弹起
                case NativeConsts.WindowMessage.WM_NCRBUTTONUP:
                    {
                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;
                        if (!FormRects[4].Contains(pt))
                        {
                            break;
                        }

                        foreach (ToolStripItem tsi in cms.Items)
                        {
                            tsi.Enabled = true;
                        }
                        switch (frmBase.WindowState)
                        {
                            case FormWindowState.Normal:
                                {
                                    tsiRestore.Enabled = false;
                                    break;
                                }
                            case FormWindowState.Maximized:
                                {
                                    tsiMax.Enabled = false;
                                    break;
                                }
                            case FormWindowState.Minimized:
                                {
                                    tsiMin.Enabled = false;
                                    break;
                                }
                        }

                        if (!frmBase.MaximizeBox)
                        {
                            tsiMax.Enabled = false;
                        }
                        if (!frmBase.MinimizeBox)
                        {
                            tsiMin.Enabled = false;
                        }

                        cms.Show(Form.MousePosition);
                        break;
                    }
                #endregion

                #region 非客户区鼠标离开
                case NativeConsts.WindowMessage.WM_NCMOUSELEAVE:
                    {
                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;
                        if (SystemButtonRects[0].Contains(pt) || SystemButtonRects[1].Contains(pt) || SystemButtonRects[2].Contains(pt))
                        {
                            //如果鼠标在系统按钮矩形上
                            DrawSystemButton();
                            IsMouseInSystemButtonRect = true;
                        }
                        else
                        {
                            //鼠标不在系统按钮矩形上

                            if (IsMouseInSystemButtonRect)
                            {
                                DrawSystemButton();
                                IsMouseInSystemButtonRect = false;
                                MouseDownSystemButtonIndex = -1;
                            }

                            if (FormRects[0].Contains(pt))
                            {
                                //左上角
                                frmBase.Cursor = Cursors.SizeNWSE;
                            }
                            else if (FormRects[1].Contains(pt))
                            {
                                //上边框
                                frmBase.Cursor = Cursors.SizeNS;
                            }
                            else if (FormRects[2].Contains(pt))
                            {
                                //右上角
                                frmBase.Cursor = Cursors.SizeNESW;
                            }
                            else if (FormRects[3].Contains(pt) || FormRects[6].Contains(pt))
                            {
                                //左边框
                                frmBase.Cursor = Cursors.SizeWE;
                            }
                            else if (FormRects[4].Contains(pt))
                            {
                                //标题栏
                                frmBase.Cursor = Cursors.Default;
                            }
                            else if (FormRects[5].Contains(pt) || FormRects[8].Contains(pt))
                            {
                                //右边框
                                frmBase.Cursor = Cursors.SizeWE;
                            }
                            else if (FormRects[9].Contains(pt))
                            {
                                //左下角
                                frmBase.Cursor = Cursors.SizeNESW;
                            }
                            else if (FormRects[10].Contains(pt))
                            {
                                //下边框
                                frmBase.Cursor = Cursors.SizeNS;
                            }
                            else if (FormRects[11].Contains(pt))
                            {
                                //右下角
                                frmBase.Cursor = Cursors.SizeNWSE;
                            }
                            else
                            {
                                frmBase.Cursor = Cursors.Default;
                            }
                        }
                        break;
                    }
                #endregion

                #region 非客户区鼠标左键释放
                case NativeConsts.WindowMessage.WM_NCLBUTTONUP:
                    {
                        IsNCMouseDown = false;
                        frmBase.Cursor = Cursors.Default;

                        Point pt = Form.MousePosition;
                        pt.X -= frmBase.Left;
                        pt.Y -= frmBase.Top;

                        if (SystemButtonRects[0].Contains(pt) || SystemButtonRects[1].Contains(pt) || SystemButtonRects[2].Contains(pt))
                        {
                            DrawSystemButton();

                            //如果在系统按钮矩形中    
                            int tmpSystemButtonIndex = -1;
                            for (int i = 0; i <= SystemButtonRects.Length - 1; i++)
                            {
                                if (SystemButtonRects[i].Contains(pt))
                                {
                                    //按下和弹起时必须是同一系统按钮才作处理
                                    if (MouseDownSystemButtonIndex == i)
                                    {
                                        tmpSystemButtonIndex = i;
                                    }
                                    else
                                    {
                                        tmpSystemButtonIndex = -1;
                                    }
                                    break;
                                }
                            }

                            if (frmBase.ControlBox)
                            {
                                switch (tmpSystemButtonIndex)
                                {
                                    case 0:
                                        {
                                            //最小化
                                            frmBase.WindowState = FormWindowState.Minimized;
                                            break;
                                        }
                                    case 1:
                                        {
                                            //最大化或还原
                                            switch (frmBase.WindowState)
                                            {
                                                case FormWindowState.Normal:
                                                    {
                                                        frmBase.WindowState = FormWindowState.Maximized;
                                                        break;
                                                    }
                                                case FormWindowState.Maximized:
                                                    {
                                                        frmBase.WindowState = FormWindowState.Normal;
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            //关闭
                                            frmBase.Close();
                                            break;
                                        }
                                }
                            }
                        }

                        break;
                    }
                #endregion

                #region 退出调整大小时
                case NativeConsts.WindowMessage.WM_EXITSIZEMOVE:
                    {
                        frmBase.Cursor = Cursors.Default;
                        break;
                    }
                #endregion
            }
            base.WndProc(ref m);
        }
        #endregion

        #region 画标题和边框
        private void DrawWindowBorderAndTitle()
        {
            //Debug.Print("MyForm:DrawWindowBorderAndTitle");
            //如果窗体没有边框
            if (frmBase.FormBorderStyle == FormBorderStyle.None)
            {
                return;
            }
            //先画到图片上
            Graphics g = Graphics.FromImage(bmpBuffer);
            
            int imageIndex = 0;

            //标题栏
            GraphicHelper.DrawImageWithoutBorder(g, TrueTopicImages[imageIndex], FormRects[4]);

            #region 图标和文字
            Rectangle TopicRect;
            if (frmBase.WindowState == FormWindowState.Maximized)
            {
                TopicRect = new Rectangle(SystemBorder, TopBorderHeight, SystemButtonRects[0].Left - LeftBorderWidth, TopicHeight);
            }
            else
            {
                TopicRect = new Rectangle(LeftBorderWidth, 0, SystemButtonRects[0].Left - LeftBorderWidth, TopicHeight + TopBorderHeight);
            }

            //文字矩形
            Rectangle rectTitleText = TopicRect;

            //画图标
            if (frmBase.ShowIcon &&
                (frmBase.FormBorderStyle != FormBorderStyle.FixedToolWindow &&
                frmBase.FormBorderStyle != FormBorderStyle.SizableToolWindow &&
                frmBase.FormBorderStyle != FormBorderStyle.FixedDialog))
            {
                Rectangle rectIcon = new Rectangle(TopicRect.Left, TopicRect.Top + (TopicRect.Height - IconSize.Height) / 2, IconSize.Width, IconSize.Height);
                g.DrawImage(frmBase.Icon.ToBitmap(), rectIcon);

                rectTitleText.X += rectIcon.Width + 2;
                rectTitleText.Width -= rectIcon.Width + 2;
            }

            //画标题文字
            StringFormat sf = new StringFormat();
            sf.Alignment = TitleTextAlign;
            sf.LineAlignment = StringAlignment.Center;

            SolidBrush brush2 = new SolidBrush(TitleTextColor);
            string displayText = GraphicHelper.GetAppropriateString(g, frmBase.Text, TitleTextFont, rectTitleText);
            g.DrawString(displayText, TitleTextFont, brush2, rectTitleText, sf);
            #endregion


            //边框线
            GraphicHelper.DrawImageWithoutBorder(g, TrueTopImages[imageIndex], FormRects[1]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftTopicImages[imageIndex], FormRects[3]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightTopicImages[imageIndex], FormRects[5]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftImages[imageIndex], FormRects[6]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightImages[imageIndex], FormRects[8]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueBottomImages[imageIndex], FormRects[10]);

            //边框四个边角
            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftTopImages[imageIndex], FormRects[0]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightTopImages[imageIndex], FormRects[2]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftBottomImages[imageIndex], FormRects[9]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightBottomImages[imageIndex], FormRects[11]);

            //画系统按钮
            DrawSystemButton(g);

            g = Graphics.FromHdc(Microsoft.Win32.NativeMethods.GetWindowDC(frmBase.Handle));
            g.DrawImage(bmpBuffer, 0, 0);
        }
        #endregion

        #region 画系统按钮
        private void DrawSystemButton(Graphics g)
        {
            //Debug.Print("MyForm:DrawSystemButton");
            //如果窗体没有边框
            if (frmBase.FormBorderStyle == FormBorderStyle.None)
            {
                return;
            }
            if (!frmBase.ControlBox)
            {
                return;
            }
            //画系统按钮
            Point pt = Form.MousePosition;
            pt.X -= frmBase.Left;
            pt.Y -= frmBase.Top;

            int[] indexs = new int[3];

            for (int i = 0; i <= indexs.Length - 1; i++)
            {
                indexs[i] = 0;
                if (SystemButtonRects[i].Contains(pt))
                {
                    if (IsNCMouseDown)
                    {
                        indexs[i] = 2;
                    }
                    else
                    {
                        indexs[i] = 1;
                    }
                }
            }

            g.DrawImage(TrueSystemButtonImages[0, indexs[0]], SystemButtonRects[0]);
            if (frmBase.WindowState == FormWindowState.Normal)
            {
                g.DrawImage(TrueSystemButtonImages[1, indexs[1]], SystemButtonRects[1]);
            }
            else
            {
                g.DrawImage(TrueSystemButtonImages[2, indexs[1]], SystemButtonRects[1]);
            }
            g.DrawImage(TrueSystemButtonImages[3, indexs[2]], SystemButtonRects[2]);
        }

        private void DrawSystemButton()
        {
            //如果窗体没有边框
            if (frmBase.FormBorderStyle == FormBorderStyle.None)
            {
                return;
            }
            if (!frmBase.ControlBox)
            {
                return;
            }
            Graphics g = Graphics.FromHdc(Microsoft.Win32.NativeMethods.GetWindowDC(frmBase.Handle));
            DrawSystemButton(g);
        }
        #endregion

        #region IMyContainer 成员

        public void InvokePaintBackground(Component c, PaintEventArgs e)
        {
            SolidBrush sb = new SolidBrush(skinEng.BackColor);
            e.Graphics.FillRectangle(sb, new Rectangle(new Point(0, 0), frmBase.ClientSize));
        }

        public void InvokePaint(Component c, PaintEventArgs e)
        {
            //frmBase_Paint(c, e);
        }

        #endregion
    }
}
