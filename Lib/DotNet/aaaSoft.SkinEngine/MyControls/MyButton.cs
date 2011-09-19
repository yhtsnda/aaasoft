using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using aaaSoft.SkinEngine.SkinHelpers;
using aaaSoft.Helpers;
using System.Diagnostics;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyButton : IMyControl
    {
        Button btnBase;
        public static SkinEngine skinEng
        {
            get
            {
                return SkinEngine.MainSkinEngine;
            }
        }

        //是否鼠标已按下
        private bool IsMouseDown = false;
        //是否鼠标在按钮上
        private bool IsMouseOn = false;

        //原始图片
        #region 原始图片
        private static Image _ButtonImage;
        public static Image ButtonImage
        {
            get
            {
                return _ButtonImage;
            }
            set
            {
                _ButtonImage = value;
                if (_ButtonImage != null)
                {
                    _ButtonImage = ImageHelper.ScaleImage(value, value.Size);
                    InitButtonImage();
                    ChangeControlColor();
                }
            }
        }
        #endregion

        //按钮文字颜色
        public static Color ButtonTextColor = Color.Black;
        //按钮不可用时文字的颜色
        public static Color ButtonDisableTextColor = Color.DarkGray;

        #region 图片数组
        //原始颜色图片
        //左
        public static Image[] SrcLeftImages = new Image[4];
        //右
        public static Image[] SrcRightImages = new Image[4];
        //上
        public static Image[] SrcTopImages = new Image[4];
        //下
        public static Image[] SrcBottomImages = new Image[4];
        //左上
        public static Image[] SrcLeftTopImages = new Image[4];
        //右上
        public static Image[] SrcRightTopImages = new Image[4];
        //左下
        public static Image[] SrcLeftBottomImages = new Image[4];
        //右下
        public static Image[] SrcRightBottomImages = new Image[4];
        //中间
        public static Image[] SrcCenterImages = new Image[4];


        //真实颜色图片
        //左
        public static Image[] TrueLeftImages = new Image[4];
        //右
        public static Image[] TrueRightImages = new Image[4];
        //上
        public static Image[] TrueTopImages = new Image[4];
        //下
        public static Image[] TrueBottomImages = new Image[4];
        //左上
        public static Image[] TrueLeftTopImages = new Image[4];
        //右上
        public static Image[] TrueRightTopImages = new Image[4];
        //左下
        public static Image[] TrueLeftBottomImages = new Image[4];
        //右下
        public static Image[] TrueRightBottomImages = new Image[4];
        //中间
        public static Image[] TrueCenterImages = new Image[4];

        #endregion

        //按钮的矩形划分
        Rectangle[] ButtonRects;

        #region 按钮边框设置
        //左侧边框宽度
        public static int LeftBorderWidth = 3;
        //右侧边框宽度
        public static int RightBorderWidth = 3;
        //上方边框高度
        public static int TopBorderHeight = 3;
        //下方边框高度
        public static int BottomBorderHeight = 3;
        #endregion

        #region 构造函数
        public MyButton(Button btn)
        {
            btnBase = btn;
            btnBase_SizeChanged(btnBase, null);
        }
        #endregion

        #region 初始化按钮图片
        public static void InitButtonImage()
        {
            Image tmpButtonImage = (Bitmap)ButtonImage.Clone();
            //设置透明
            tmpButtonImage = ImageHelper.ReplaceColor(tmpButtonImage, Color.FromArgb(255, 0, 255), Color.Transparent);

            //切分图片
            Image[] SrcButtonImages = ImageHelper.CutImage(tmpButtonImage, 3, true);

            if (SrcButtonImages.Length == 3)
            {
                Image[] tmpImages = new Image[4];
                SrcButtonImages.CopyTo(tmpImages, 0);
                tmpImages[3] = ImageHelper.ReplaceColor(SrcButtonImages[0], Color.DarkGray);
                SrcButtonImages = tmpImages;
            }

            //原始矩形划分
            Rectangle[] tmpRects = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), SrcButtonImages[0].Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
            
            for (int i = 0; i <= SrcButtonImages.Length - 1; i++)
            {
                Image tmpImage = SrcButtonImages[i];

                //切分出九个部分的图片
                SrcLeftTopImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[0]);
                SrcTopImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[1]);
                SrcRightTopImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[2]);

                SrcLeftImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[3]);
                SrcCenterImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[4]);
                SrcRightImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[5]);

                SrcLeftBottomImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[6]);
                SrcBottomImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[7]);
                SrcRightBottomImages[i] = ImageHelper.GetImage(tmpImage, tmpRects[8]);

                //加边框
                SrcLeftTopImages[i] = ImageHelper.AddBorder(SrcLeftTopImages[i]);
                SrcTopImages[i] = ImageHelper.AddBorder(SrcTopImages[i]);
                SrcRightTopImages[i] = ImageHelper.AddBorder(SrcRightTopImages[i]);

                SrcLeftImages[i] = ImageHelper.AddBorder(SrcLeftImages[i]);
                SrcCenterImages[i] = ImageHelper.AddBorder(SrcCenterImages[i]);
                SrcRightImages[i] = ImageHelper.AddBorder(SrcRightImages[i]);

                SrcLeftBottomImages[i] = ImageHelper.AddBorder(SrcLeftBottomImages[i]);
                SrcBottomImages[i] = ImageHelper.AddBorder(SrcBottomImages[i]);
                SrcRightBottomImages[i] = ImageHelper.AddBorder(SrcRightBottomImages[i]);
            }
        }
        #endregion

        #region 改变控件颜色(关键字：颜色)
        public static void ChangeControlColor()
        {
            for (int i = 0; i <= 4 - 1; i++)
            {
                TrueLeftTopImages[i] = ImageHelper.ReplaceColor(SrcLeftTopImages[i], skinEng.BackColor);
                TrueTopImages[i] = ImageHelper.ReplaceColor(SrcTopImages[i], skinEng.BackColor);
                TrueRightTopImages[i] = ImageHelper.ReplaceColor(SrcRightTopImages[i], skinEng.BackColor);

                TrueLeftImages[i] = ImageHelper.ReplaceColor(SrcLeftImages[i], skinEng.BackColor);
                TrueCenterImages[i] = ImageHelper.ReplaceColor(SrcCenterImages[i], skinEng.BackColor);
                TrueRightImages[i] = ImageHelper.ReplaceColor(SrcRightImages[i], skinEng.BackColor);

                TrueLeftBottomImages[i] = ImageHelper.ReplaceColor(SrcLeftBottomImages[i], skinEng.BackColor);
                TrueBottomImages[i] = ImageHelper.ReplaceColor(SrcBottomImages[i], skinEng.BackColor);
                TrueRightBottomImages[i] = ImageHelper.ReplaceColor(SrcRightBottomImages[i], skinEng.BackColor);
            }
        }
        #endregion

        #region 计算矩形
        private void CalcRect()
        {
            //真实按钮的矩形划分
            ButtonRects = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), btnBase.Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
        }
        #endregion

        #region 启用皮肤时
        public void StartControlSkin()
        {
            btnBase.Paint += new PaintEventHandler(btnBase_Paint);
            btnBase.MouseLeave += new EventHandler(btnBase_MouseLeave);
            btnBase.MouseMove += new MouseEventHandler(btnBase_MouseMove);
            btnBase.MouseDown += new MouseEventHandler(btnBase_MouseDown);
            btnBase.MouseUp += new MouseEventHandler(btnBase_MouseUp);
            btnBase.SizeChanged += new EventHandler(btnBase_SizeChanged);
            btnBase.EnabledChanged += new EventHandler(btnBase_EnabledChanged);
        }
        #endregion

        #region 停用皮肤时
        public void StopControlSkin()
        {
            btnBase.Paint -= new PaintEventHandler(btnBase_Paint);
            btnBase.MouseLeave -= new EventHandler(btnBase_MouseLeave);
            btnBase.MouseMove -= new MouseEventHandler(btnBase_MouseMove);
            btnBase.MouseDown -= new MouseEventHandler(btnBase_MouseDown);
            btnBase.MouseUp -= new MouseEventHandler(btnBase_MouseUp);
            btnBase.SizeChanged -= new EventHandler(btnBase_SizeChanged);
            btnBase.EnabledChanged -= new EventHandler(btnBase_EnabledChanged);

            //停止时鼠标状态回复初始化
            IsMouseDown = false;
            IsMouseOn = false;
        }
        #endregion

        #region 相关事件发生时

        //大小改变时
        void btnBase_SizeChanged(object sender, EventArgs e)
        {
            //计算矩形
            CalcRect();

            //设置按钮区域
            Region rgn = new Region(new Rectangle(new Point(0, 0), btnBase.Size));
            GraphicsPath gp = GetUndrawGraphicsPath();
            rgn.Exclude(gp);
            btnBase.Region = rgn;
        }

        #region 得到透明部分图形路线
        private GraphicsPath GetUndrawGraphicsPath()
        {
            GraphicsPath gp = new GraphicsPath();
            Bitmap tmpBmp = (Bitmap)ButtonImage;
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
                        gp.AddRectangle(new Rectangle(i + ButtonRects[2].Left - (tmpBmp.Width - RightBorderWidth), j, 1, 1));
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
                        gp.AddRectangle(new Rectangle(i, j + ButtonRects[6].Top - (tmpBmp.Height - BottomBorderHeight), 1, 1));
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
                        gp.AddRectangle(new Rectangle(i + ButtonRects[8].Left - (tmpBmp.Width - RightBorderWidth), j + ButtonRects[8].Top - (tmpBmp.Height - BottomBorderHeight), 1, 1));
                    }
                }
            }
            
            return gp;
        }
        #endregion

        //鼠标移动时
        void btnBase_MouseMove(object sender, MouseEventArgs e)
        {
            IsMouseOn = true;
        }

        //鼠标按下时
        void btnBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsMouseDown = true;
                btnBase.Invalidate();
            }
        }

        //鼠标弹起时
        void btnBase_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            btnBase.Invalidate();
        }

        //鼠标离开时
        void btnBase_MouseLeave(object sender, EventArgs e)
        {
            IsMouseOn = false;
            btnBase.Invalidate();
        }
        
        void btnBase_EnabledChanged(object sender, EventArgs e)
        {
            IsMouseDown = false;
            btnBase.Invalidate();
        }

        //画图
        void btnBase_Paint(object sender, PaintEventArgs e)
        {
            //Debug.Print("btnBase_Paint,ButtonText:" + btnBase.Text);
            Graphics g = e.Graphics;
            int imageIndex = 0;

            if (IsMouseOn)
            {
                if (IsMouseDown)
                {
                    imageIndex = 2;
                }
                else
                {
                    imageIndex = 1;
                }
            }
            else
            {
                imageIndex = 0;

#warning 当按钮得到焦点时
                //当按钮得到焦点时
                if (btnBase.Focused)
                {
                    imageIndex = 1;
                }
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            SolidBrush sb = null;

            Rectangle rect = ButtonRects[4];
            RectangleF rectF = ButtonRects[4];

            string displayText = GraphicHelper.GetAppropriateString(g, btnBase.Text.Replace("&", string.Empty), btnBase.Font, rectF);

            Color textColor;
            if (btnBase.Enabled)
            {
                textColor = ButtonTextColor;
            }
            else
            {
                imageIndex = 3;
                textColor = ButtonDisableTextColor;
            }
            //画按钮图片
            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftTopImages[imageIndex], ButtonRects[0]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueTopImages[imageIndex], ButtonRects[1]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightTopImages[imageIndex], ButtonRects[2]);

            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftImages[imageIndex], ButtonRects[3]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueCenterImages[imageIndex], ButtonRects[4]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightImages[imageIndex], ButtonRects[5]);

            GraphicHelper.DrawImageWithoutBorder(g, TrueLeftBottomImages[imageIndex], ButtonRects[6]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueBottomImages[imageIndex], ButtonRects[7]);
            GraphicHelper.DrawImageWithoutBorder(g, TrueRightBottomImages[imageIndex], ButtonRects[8]);

            //画背景图片
            if (btnBase.BackgroundImage != null)
            {
                GraphicHelper.DrawImage(g, btnBase.BackgroundImage, rect, btnBase.BackgroundImageLayout);
            }
            //画前景图片
            if (btnBase.Image != null)
            {
                GraphicHelper.DrawImage(g, btnBase.Image, rect, btnBase.ImageAlign);
            }
            sb = new SolidBrush(textColor);
            g.DrawString(displayText, btnBase.Font, sb, rectF, sf);
        }
        #endregion
    }
}
