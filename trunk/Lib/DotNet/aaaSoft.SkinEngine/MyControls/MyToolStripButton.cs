using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Helpers;
using aaaSoft.SkinEngine.SkinHelpers;
using Microsoft.Win32;
using System.Drawing.Drawing2D;

namespace aaaSoft.SkinEngine.MyControls
{
    public class MyToolStripButton : IMyControl
    {
        ToolStripButton tsbBase;
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
        public MyToolStripButton(ToolStripButton btn)
        {
            tsbBase = btn;
            tsbBase_SizeChanged(tsbBase, null);
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
            ButtonRects = ShapeHelper.GetRectangles(new Rectangle(new Point(0, 0), tsbBase.Size), LeftBorderWidth, RightBorderWidth, TopBorderHeight, BottomBorderHeight);
        }
        #endregion

        #region 启用皮肤时
        public void StartControlSkin()
        {
            tsbBase.Paint += new PaintEventHandler(tsbBase_Paint);
            tsbBase.MouseLeave += new EventHandler(tsbBase_MouseLeave);
            tsbBase.MouseMove += new MouseEventHandler(tsbBase_MouseMove);
            tsbBase.MouseDown += new MouseEventHandler(tsbBase_MouseDown);
            tsbBase.MouseUp += new MouseEventHandler(tsbBase_MouseUp);
            tsbBase.EnabledChanged += new EventHandler(tsbBase_EnabledChanged);
        }
        #endregion

        #region 停用皮肤时
        public void StopControlSkin()
        {
            tsbBase.Paint -= new PaintEventHandler(tsbBase_Paint);
            tsbBase.MouseLeave -= new EventHandler(tsbBase_MouseLeave);
            tsbBase.MouseMove -= new MouseEventHandler(tsbBase_MouseMove);
            tsbBase.MouseDown -= new MouseEventHandler(tsbBase_MouseDown);
            tsbBase.MouseUp -= new MouseEventHandler(tsbBase_MouseUp);
            tsbBase.EnabledChanged -= new EventHandler(tsbBase_EnabledChanged);

            //停止时鼠标状态回复初始化
            IsMouseDown = false;
            IsMouseOn = false;
        }

        #endregion

        #region 相关事件发生时

        //大小改变时
        void tsbBase_SizeChanged(object sender, EventArgs e)
        {
            //计算矩形
            CalcRect();
        }

        //鼠标移动时
        void tsbBase_MouseMove(object sender, MouseEventArgs e)
        {
            IsMouseOn = true;
            tsbBase.Invalidate();
        }

        //鼠标按下时
        void tsbBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IsMouseDown = true;
                tsbBase.Invalidate();
            }
        }

        //鼠标弹起时
        void tsbBase_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            tsbBase.Invalidate();
        }

        //鼠标离开时
        void tsbBase_MouseLeave(object sender, EventArgs e)
        {
            IsMouseOn = false;
            tsbBase.Invalidate();
        }
        
        void tsbBase_EnabledChanged(object sender, EventArgs e)
        {
            IsMouseDown = false;
            tsbBase.Invalidate();
        }

        //画图
        public void tsbBase_Paint(object sender, PaintEventArgs e)
        {
            //先画背景
            IMyContainer imctl = (IMyContainer)skinEng.GetInterface(tsbBase.Owner);
            imctl.InvokePaintBackground(tsbBase, e);

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
            }

            if (!tsbBase.Enabled)
            {
                imageIndex = 0;
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

            //准备工作
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.NoWrap;

            SolidBrush sb = null;
            Color textColor;

            Rectangle rect = new Rectangle(new Point(0, 0), tsbBase.Size);
            RectangleF rectF = new RectangleF(new Point(0, 0), tsbBase.Size);

            string displayText = GraphicHelper.GetAppropriateString(g, tsbBase.Text.Replace("&", string.Empty), tsbBase.Font, rectF);
            //文字大小
            SizeF stringSize = g.MeasureString(displayText, tsbBase.Font);


            Image toDrawImage;
            if (tsbBase.Enabled)
            {
                int hue, sat, bri;
                ColorHelper.RGB2HSB(skinEng.BackColor, out hue, out sat, out bri);
                textColor = ColorHelper.HSB2RGBColor(hue, sat, 49);
                toDrawImage = tsbBase.Image;
            }
            else
            {
                imageIndex = 0;
                
                textColor = Color.DarkGray;
                toDrawImage = ImageHelper.RemoveColor(tsbBase.Image);
            }
            //画背景图片
            if (tsbBase.BackgroundImage != null)
            {
                GraphicHelper.DrawImage(g, tsbBase.BackgroundImage, rect, tsbBase.BackgroundImageLayout);
            }

            sb = new SolidBrush(textColor);
            //画前景
            switch (tsbBase.DisplayStyle)
            {
                case ToolStripItemDisplayStyle.Image:
                    {
                        if (tsbBase.Image != null)
                        {
                            GraphicHelper.DrawImage(g, toDrawImage, rect, ImageLayout.Center);
                        }
                        break;
                    }
                case ToolStripItemDisplayStyle.Text:
                    {
                        g.DrawString(displayText, tsbBase.Font, sb, rectF, sf);
                        break;
                    }
                case ToolStripItemDisplayStyle.None:
                    {
                        break;
                    }
                case ToolStripItemDisplayStyle.ImageAndText:
                    {
                        if (tsbBase.Image == null)
                        {
                            g.DrawString(displayText, tsbBase.Font, sb, rectF, sf);
                        }
                        else
                        {
                            int totalWidth = (int)stringSize.Width + tsbBase.Image.Width;
                            Rectangle rectImage = new Rectangle((tsbBase.Width - totalWidth) / 2, (tsbBase.Height - tsbBase.Image.Height) / 2, tsbBase.Image.Width, tsbBase.Image.Height);
                            RectangleF rectText = new RectangleF(rectImage.Right, (tsbBase.Height - stringSize.Height) / 2, stringSize.Width, stringSize.Height);
                            g.DrawImage(toDrawImage, rectImage);
                            g.DrawString(displayText, tsbBase.Font, sb, rectText, sf);
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
