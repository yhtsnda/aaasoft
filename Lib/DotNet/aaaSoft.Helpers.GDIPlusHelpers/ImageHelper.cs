using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace aaaSoft.Helpers.GDIPlusHelpers
{
    public class ImageHelper
    {
        #region 裁剪指定位置和大小的图片
        public static Image GetImage(Image srcImage, Point location, Size size)
        {
            Bitmap newBmp = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(newBmp);
            g.DrawImage(srcImage, (0 - location.X), (0 - location.Y));
            return newBmp;
        }

        public static Image GetImage(Image srcImage, int left, int top, int right, int bottom)
        {
            return GetImage(srcImage, new Point(left, top), new Size(right - left, bottom - top));
        }

        public static Image GetImage(Image srcImage, Rectangle rect)
        {
            return GetImage(srcImage, rect.Location, rect.Size);
        }
        #endregion

        #region 缩放图片为指定大小
        public static Image ScaleImage(Image srcImage, Size size)
        {
            if (srcImage == null) return null;
            if (size.Width <= 0 || size.Height <= 0)
            {
                return srcImage;
            }

            //原图片
            Bitmap srcBmp = new Bitmap(srcImage, srcImage.Size);
            //目地图片
            Bitmap newBmp = new Bitmap(size.Width, size.Height);

            for (int i = 0; i <= newBmp.Width - 1; i++)
            {
                int x = i * srcBmp.Width / newBmp.Width;
                for (int j = 0; j <= newBmp.Height - 1; j++)
                {
                    int y = j * srcBmp.Height / newBmp.Height;
                    newBmp.SetPixel(i, j, srcBmp.GetPixel(x, y));
                }
            }
            return newBmp;
        }
        #endregion

        #region 二值化
        public static Image TwoValueImage(Image srcImage, int middleValue)
        {
            Bitmap bmp = (Bitmap)srcImage.Clone();
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    if (c.R >= middleValue)
                        bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return bmp;
        }
        #endregion

        #region 等分切分图片
        /// <summary>
        /// 等分切分图片
        /// </summary>
        /// <param name="srcImage">源图片</param>
        /// <param name="count">切分数量</param>
        /// <param name="IsHorizon">是否水平切分(是则水平切分，否则垂直切分)</param>
        /// <returns></returns>
        public static Image[] CutImage(Image srcImage, int count, bool IsHorizon)
        {
            srcImage = new Bitmap(srcImage, srcImage.Width, srcImage.Height);

            Image[] imageArray = new Image[count];
            if (IsHorizon)
            {
                Size picSize = new Size(srcImage.Width, srcImage.Height / count);
                for (int i = 0; i <= count - 1; i++)
                {
                    imageArray[i] = GetImage(srcImage, new Point(0, i * picSize.Height), picSize);
                }
            }
            else
            {
                Size picSize = new Size(srcImage.Width / count, srcImage.Height);
                for (int i = 0; i <= count - 1; i++)
                {
                    imageArray[i] = GetImage(srcImage, new Point(i * picSize.Width, 0), picSize);
                }
            }
            return imageArray;
        }
        #endregion

        #region 去除右侧图片周围空白
        public static Image RemoveRightWhiteSpace(Image srcImage,Color whiteColor)
        {
            int left = 0, top = 0, right =srcImage.Width, bottom = srcImage.Height;

            for (int i = 0; i <= srcImage.Width - 1; i++)
            {
                for (int j = 0; j <= srcImage.Height - 1; j++)
                {
                    Color c = ((Bitmap)srcImage).GetPixel(i, j);
                    if (c == whiteColor)
                    {
                        if (right > i) right = i;
                    }
                }
            }
            
            return GetImage(srcImage, new Point(left, top), new Size(right - left + 1, bottom - top + 1));
        }
        #endregion

        #region 去除图片中颜色，得到灰度图片
        public static Image RemoveColor(Image srcImage)
        {
            if (srcImage == null) return null;
            Bitmap bmp = new Bitmap(srcImage, srcImage.Size);
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);//转换灰度的算法
                    bmp.SetPixel(i, j, Color.FromArgb(c.A, luma, luma, luma));
                }
            }
            return bmp;
        }
        #endregion

        #region 替换图片中某一颜色为指定颜色
        public static Image ReplaceColor(Image srcImage, Color srcColor, Color desColor)
        {
            Bitmap bmp = new Bitmap(srcImage, srcImage.Size);
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    if (c.Equals(srcColor))
                    {
                        bmp.SetPixel(i, j, desColor);
                    }
                }
            }
            return bmp;
        }
        #endregion

        #region 替换图片中的色彩为指定色彩
        public static Image ReplaceColor(Image srcImage, Color desColor)
        {
            if (srcImage == null) return null;
            Bitmap bmp = (Bitmap)RemoveColor(srcImage);

            int hue,sat,bri;
            ColorHelper.RGB2HSB(desColor, out hue, out sat, out bri);

            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    Color tmpColor = bmp.GetPixel(i, j);
                    //透明度
                    int A = tmpColor.A;
                    //如果此点完全透明，则不处理
                    if (A == 0)
                    {
                        continue;
                    }
                    //灰度百分比
                    float h = tmpColor.R * 1.0F / 148;

                    Color newColor = ColorHelper.HSB2RGBColor(hue, sat, (int)(bri * h));
                    bmp.SetPixel(i, j, Color.FromArgb(A, newColor));
                }
            }
            return bmp;
        }
        #endregion

        #region 将图片周围加边框(因为GDI+的BUG)
        public static Image AddBorder(Image srcImage)
        {
            return AddBorder(srcImage, 1);
        }
        public static Image AddBorder(Image srcImage, int borderWidth)
        {
            Bitmap bmp = new Bitmap(srcImage.Width + borderWidth, srcImage.Height + borderWidth);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(srcImage, 0, 0);
            g.DrawImage(srcImage, new Rectangle(bmp.Width - 1, 0, 1, bmp.Height));
            g.DrawImage(srcImage, new Rectangle(0, bmp.Height - 1, bmp.Width, 1));
            return bmp;
        }
        #endregion
    }
}
