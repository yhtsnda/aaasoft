using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace aaaSoft.Helpers
{
    public class GraphicHelper
    {
        #region 根据矩形区域，获取合适的显示字符串
        /// <summary>
        /// 根据矩形区域，获取合适的显示字符串
        /// </summary>
        /// <param name="srcStr">源字符串</param>
        /// <param name="rect">矩形区域</param>
        /// <returns></returns>
        public static string GetAppropriateString(Graphics g, string srcStr, Font font, RectangleF rect)
        {
            if (string.IsNullOrEmpty(srcStr)) return srcStr;
            if (rect.Width <= 0 || rect.Height <= 0) return srcStr;

            SizeF strSize = g.MeasureString(srcStr, font);
            if (strSize.Width <= rect.Width)
            {
                return srcStr;
            }
            else
            {
                byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(srcStr);

                int newLength = (int)(srcBytes.Length * rect.Width / strSize.Width);
                string justStr = Encoding.UTF8.GetString(srcBytes, 0, newLength); //srcStr.Substring(0, newLength);

                string rtnStr = string.Empty;

                for (int i = justStr.Length - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        rtnStr = justStr.Substring(0, 1) + "...";
                        break;
                    }
                    string tmpStr = justStr.Substring(0, i) + "...";
                    strSize = g.MeasureString(tmpStr, font);
                    if (strSize.Width <= rect.Width)
                    {
                        rtnStr = tmpStr;
                        break;
                    }
                }
                return rtnStr;
            }
        }
        #endregion

        #region 获取圆角矩形区域
        /// <summary>
        /// 获取圆角矩形区域
        /// </summary>
        /// <param name="nLeftRect"></param>
        /// <param name="nTopRect"></param>
        /// <param name="nRightRect"></param>
        /// <param name="nBottomRect"></param>
        /// <param name="nWidthEllipse"></param>
        /// <param name="nHeightEllipse"></param>
        /// <returns></returns>
        public static Region CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
        {
            GraphicsPath gp = CreateRoundRectPath(new Rectangle(nLeftRect, nTopRect, nRightRect - nLeftRect, nBottomRect - nTopRect), nWidthEllipse, nHeightEllipse);
            Region rgn = new Region(gp);
            return rgn;
        }
        #endregion

        #region 获取圆角矩形路径
        /// <summary>
        /// 获取圆角矩形路径
        /// </summary>
        /// <param name="roundRect"></param>
        /// <param name="roundWidth"></param>
        /// <param name="roundHeight"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundRectPath(Rectangle roundRect, int roundWidth, int roundHeight)
        {
            GraphicsPath gpRoundRect = new GraphicsPath();
            gpRoundRect.AddArc(new Rectangle(roundRect.Right - roundWidth - 1, roundRect.Top, roundWidth, roundWidth), -90, 90);
            gpRoundRect.AddArc(new Rectangle(roundRect.Right - roundWidth - 1, roundRect.Bottom - roundHeight - 1, roundWidth, roundWidth), 0, 90);
            gpRoundRect.AddArc(new Rectangle(roundRect.Left, roundRect.Bottom - roundWidth - 1, roundWidth, roundWidth), 90, 90);
            gpRoundRect.AddArc(new Rectangle(roundRect.Left, roundRect.Top, roundWidth, roundWidth), -180, 90);
            gpRoundRect.CloseAllFigures();
            return gpRoundRect;
        }
        #endregion

        #region 画图片
        public static void DrawImage(Graphics g, Image srcImage, Rectangle rect, ImageLayout layout)
        {
            switch (layout)
            {
                case ImageLayout.None:
                    {
                        g.DrawImage(srcImage, 0, 0);
                        break;
                    }
                case ImageLayout.Center:
                    {
                        g.DrawImage(srcImage, (rect.Width - srcImage.Width) / 2, (rect.Height - srcImage.Height) / 2);
                        //居中
                        break;
                    }
                case ImageLayout.Stretch:
                    {
                        //拉伸
                        g.DrawImage(srcImage, rect);
                        break;
                    }
                case ImageLayout.Tile:
                    {
                        //平铺
                        int x = rect.Width / srcImage.Width + 1;
                        int y = rect.Height / srcImage.Height + 1;
                        for (int i = 0; i <= x - 1; i++)
                        {
                            for (int j = 0; j <= y - 1; j++)
                            {
                                g.DrawImageUnscaled(srcImage, srcImage.Width * x, srcImage.Height * y);
                            }
                        }
                        break;
                    }
                case ImageLayout.Zoom:
                    {
                        //按比例缩放
                        
                        //图片宽高比
                        float imageRatio;
                        //容器宽高比
                        float ctlRatio;

                        imageRatio = srcImage.Width * 1F / srcImage.Height;
                        ctlRatio = rect.Width * 1F / rect.Height;

                        Rectangle imageRect = new Rectangle();
                        if (imageRatio >= ctlRatio)
                        {
                            imageRect.X = rect.Left;
                            imageRect.Width = rect.Width;
                            imageRect.Height = Convert.ToInt32(imageRect.Width / imageRatio);
                            imageRect.Y = (rect.Height - imageRect.Height) / 2 + rect.Top;
                        }
                        else
                        {
                            imageRect.Y = rect.Top;
                            imageRect.Height = rect.Height;
                            imageRect.Width = Convert.ToInt32(rect.Height * imageRatio);
                            imageRect.X = (rect.Width - imageRect.Width) / 2 + rect.Left;
                        }
                        g.DrawImage(srcImage, imageRect);
                        break;
                    }
            }
        }

        public static void DrawImage(Graphics g, Image srcImage, Rectangle rect, ContentAlignment align)
        {
            Point location = new Point();
            //水平方向对齐方式
            StringAlignment xAlign = StringAlignment.Center;
            //垂直方向对方方式
            StringAlignment yAlign = StringAlignment.Center;

            switch (align)
            {
                case ContentAlignment.TopLeft:
                    {
                        xAlign = StringAlignment.Near;
                        yAlign = StringAlignment.Near;
                        break;
                    }
                case ContentAlignment.TopCenter:
                    {
                        xAlign = StringAlignment.Center;
                        yAlign = StringAlignment.Near;
                        break;
                    }
                case ContentAlignment.TopRight:
                    {
                        xAlign = StringAlignment.Far;
                        yAlign = StringAlignment.Near;
                        break;
                    }
                case ContentAlignment.MiddleLeft:
                    {
                        xAlign = StringAlignment.Near;
                        yAlign = StringAlignment.Center;
                        break;
                    }
                case ContentAlignment.MiddleCenter:
                    {
                        xAlign = StringAlignment.Center;
                        yAlign = StringAlignment.Center;
                        break;
                    }
                case ContentAlignment.MiddleRight:
                    {
                        xAlign = StringAlignment.Far;
                        yAlign = StringAlignment.Center;
                        break;
                    }
                case ContentAlignment.BottomLeft:
                    {
                        xAlign = StringAlignment.Near;
                        yAlign = StringAlignment.Far;
                        break;
                    }
                case ContentAlignment.BottomCenter:
                    {
                        xAlign = StringAlignment.Center;
                        yAlign = StringAlignment.Far;
                        break;
                    }
                case ContentAlignment.BottomRight:
                    {
                        xAlign = StringAlignment.Far;
                        yAlign = StringAlignment.Far;
                        break;
                    }
            }

            switch (xAlign)
            {
                case StringAlignment.Near:
                    {
                        location.X = 0;
                        break;
                    }
                case StringAlignment.Center:
                    {
                        location.X = (rect.Width - srcImage.Width) / 2;
                        break;
                    }
                case StringAlignment.Far:
                    {
                        location.X = rect.Width - srcImage.Width;
                        break;
                    }
            }

            switch (yAlign)
            {
                case StringAlignment.Near:
                    {
                        location.Y = 0;
                        break;
                    }
                case StringAlignment.Center:
                    {
                        location.Y = (rect.Height - srcImage.Height) / 2;
                        break;
                    }
                case StringAlignment.Far:
                    {
                        location.Y = rect.Height - srcImage.Height;
                        break;
                    }
            }
            location.X += rect.Left;
            location.Y += rect.Top;
            g.DrawImage(srcImage, location);
        }
        #endregion

        #region 画图片(不画图片的边框的拉伸)
        public static void DrawImageWithoutBorder(Graphics g, Image srcImage, Rectangle rect)
        {
            DrawImageWithoutBorder(g, srcImage, rect, 1);
        }
        public static void DrawImageWithoutBorder(Graphics g, Image srcImage, Rectangle rect, int BorderWidth)
        {
            Rectangle srcRect = new Rectangle();
            srcRect.Width = srcImage.Width - BorderWidth;
            srcRect.Height = srcImage.Height - BorderWidth;

            g.DrawImage(srcImage, rect, srcRect, GraphicsUnit.Pixel);
        }
        #endregion
    }
}
