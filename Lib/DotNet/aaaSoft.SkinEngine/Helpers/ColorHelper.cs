using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace aaaSoft.Helpers
{
    public class ColorHelper
    {
        #region HSB转RGB
        public static Color HSB2RGBColor(int hue, int sat, int bri)
        {
            int r, g, b;
            if (hue > 239)
            {
                hue = 239;
            }
            else
            {
                if (hue < 0)
                {
                    hue = 0;
                }
            }

            if (sat > 240)
            {
                sat = 240;
            }
            else
            {
                if (sat < 0)
                {
                    sat = 0;
                }
            }

            if (bri > 240)
            {
                bri = 240;
            }
            else
            {
                if (bri < 0)
                {
                    bri = 0;
                }
            }

            float H = hue / 239.0f;
            float S = sat / 240.0f;
            float L = bri / 240.0f;

            float red = 0, green = 0, blue = 0;
            float d1, d2;

            if (L == 0)
            {
                red = green = blue = 0;
            }
            else
            {
                if (S == 0)
                {
                    red = green = blue = L;
                }
                else
                {
                    d2 = (L <= 0.5f) ? L * (1.0f + S) : L + S - (L * S);
                    d1 = 2.0f * L - d2;

                    float[] d3 = new float[] { H + 1.0f / 3.0f, H, H - 1.0f / 3.0f };
                    float[] rgb = new float[] { 0, 0, 0 };

                    for (int i = 0; i < 3; i++)
                    {
                        if (d3[i] < 0)
                        {
                            d3[i] += 1.0f;
                        }

                        if (d3[i] > 1.0f)
                        {
                            d3[i] -= 1.0f;
                        }

                        if (6.0f * d3[i] < 1.0f)
                        {
                            rgb[i] = d1 + (d2 - d1) * d3[i] * 6.0f;
                        }
                        else
                        {
                            if (2.0f * d3[i] < 1.0f)
                            {
                                rgb[i] = d2;
                            }
                            else
                            {
                                if (3.0f * d3[i] < 2.0f)
                                {
                                    rgb[i] = (d1 + (d2 - d1) * ((2.0f / 3.0f) - d3[i]) * 6.0f);
                                }
                                else
                                {
                                    rgb[i] = d1;
                                }
                            }
                        }
                    }

                    red = rgb[0];
                    green = rgb[1];
                    blue = rgb[2];
                }
            }

            red = 255.0f * red;
            green = 255.0f * green;
            blue = 255.0f * blue;

            if (red < 1)
            {
                red = 0.0f;
            }
            else
            {
                if (red > 255.0f)
                {
                    red = 255.0f;
                }
            }

            if (green < 1)
            {
                green = 0.0f;
            }
            else
            {
                if (green > 255.0f)
                {
                    green = 255.0f;
                }
            }

            if (blue < 1)
            {
                blue = 0.0f;
            }
            else
            {
                if (blue > 255.0f)
                {
                    blue = 255.0f;
                }
            }

            r = (int)(red + 0.5);
            g = (int)(green + 0.5);
            b = (int)(blue + 0.5);
            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region RGB转HSB
        public static void RGB2HSB(Color cor, out int hue, out int sat, out int bri)
        {
            RGB2HSB(cor.R, cor.G, cor.B, out hue, out sat, out bri);
        }

        public static void RGB2HSB(int r, int g, int b, out float hue, out float sat, out float bri)
        {
            int minval = Math.Min(r, Math.Min(g, b));
            int maxval = Math.Max(r, Math.Max(g, b));

            //bri 
            bri = (float)(maxval + minval) / 510;

            //sat 
            if (maxval == minval)
            {
                sat = 0.0f;
            }
            else
            {
                int sum = maxval + minval;

                if (sum > 255)
                {
                    sum = 510 - sum;
                }

                sat = (float)(maxval - minval) / sum;
            }

            //hue 
            if (maxval == minval)
            {
                hue = 0.0f;
            }
            else
            {
                float diff = (float)(maxval - minval);
                float rnorm = (maxval - r) / diff;
                float gnorm = (maxval - g) / diff;
                float bnorm = (maxval - b) / diff;

                hue = 0.0f;

                if (r == maxval)
                {
                    hue = 60.0f * (6.0f + bnorm - gnorm);
                }

                if (g == maxval)
                {
                    hue = 60.0f * (2.0f + rnorm - bnorm);
                }

                if (b == maxval)
                {
                    hue = 60.0f * (4.0f + gnorm - rnorm);
                }

                if (hue > 360.0f)
                {
                    hue = hue - 360.0f;
                }
            }
        }
        
        public static void RGB2HSB(int r, int g, int b, out int hue, out int sat, out int bri)
        {
            float fHue, fSat, fBri;

            RGB2HSB(r, g, b, out fHue, out fSat, out fBri);

            hue = (int)((fHue / 360.0f) * 240 + 0.5);
            sat = (int)(fSat * 241 + 0.5);
            bri = (int)(fBri * 241 + 0.5);

            if (hue > 239)
            {
                hue = 239;
            }

            if (sat > 240)
            {
                sat = 240;
            }

            if (bri > 240)
            {
                bri = 240;
            }
        } 
        #endregion

        #region 获取随机颜色
        public static Color GetRandomColor()
        {
            Random rnd = new Random();
            return Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        }
        #endregion

        #region 得到灰度颜色
        public static Color RemoveColor(Color srcColor)
        {
            Color c = srcColor;
            int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);//转换灰度的算法
            return Color.FromArgb(c.A, luma, luma, luma);
        }
        #endregion

        #region 替换颜色
        public static Color ReplaceColor(Color srcColor, Color desColor)
        {
            int hue, sat, bri;
            ColorHelper.RGB2HSB(desColor, out hue, out sat, out bri);

            Color tmpColor = RemoveColor(srcColor);
            //透明度
            int A = tmpColor.A;
            //灰度百分比
            float h = tmpColor.R * 1.0F / 148;

            Color newColor = ColorHelper.HSB2RGBColor(hue, sat, (int)(bri * h));
            return Color.FromArgb(A, newColor);
        }
        #endregion
    }
}
