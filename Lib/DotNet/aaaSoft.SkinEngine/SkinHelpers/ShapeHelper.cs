using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace aaaSoft.SkinEngine.SkinHelpers
{
    class ShapeHelper
    {
        #region 判断点是否在矩形中
        /// <summary>
        /// 判断点是否在矩形中
        /// </summary>
        /// <param name="pt">点</param>
        /// <param name="rect">矩形</param>
        /// <returns></returns>
        public static bool PointInRect(Point pt, Rectangle rect)
        {
            return ((((pt.X >= rect.Left) & (pt.X < rect.Right)) & (pt.Y >= rect.Top)) & (pt.Y < rect.Bottom));
        }
        #endregion

        #region 得到九块矩形区域(按钮用)
        public static Rectangle[] GetRectangles(Rectangle srcRect, int LeftBorderWidth, int RightBorderWidth, int TopBorderHeight, int BottomBorderHeight)
        {
            Rectangle[] rtnRects = new Rectangle[9];
            /*      0 1       2 3
             *      | |       | |
             *      ┌-----------┐-0
             *      |           |-1
             *      |           |-2
             *      └-----------┘-3
             *  水平、垂直四条线
             */
            int[] LineX = new int[4];
            int[] LineY = new int[4];

            LineX[0] = srcRect.Left;
            LineX[1] = srcRect.Left + LeftBorderWidth;
            LineX[2] = srcRect.Right - RightBorderWidth;
            LineX[3] = srcRect.Right;

            LineY[0] = srcRect.Top;
            LineY[1] = srcRect.Top + TopBorderHeight;
            LineY[2] = srcRect.Bottom - BottomBorderHeight;
            LineY[3] = srcRect.Bottom;


            rtnRects[0] = new Rectangle(LineX[0], LineY[0], LineX[1] - LineX[0], LineY[1] - LineY[0]);
            rtnRects[1] = new Rectangle(LineX[1], LineY[0], LineX[2] - LineX[1], LineY[1] - LineY[0]);
            rtnRects[2] = new Rectangle(LineX[2], LineY[0], LineX[3] - LineX[2], LineY[1] - LineY[0]);

            rtnRects[3] = new Rectangle(LineX[0], LineY[1], LineX[1] - LineX[0], LineY[2] - LineY[1]);
            rtnRects[4] = new Rectangle(LineX[1], LineY[1], LineX[2] - LineX[1], LineY[2] - LineY[1]);
            rtnRects[5] = new Rectangle(LineX[2], LineY[1], LineX[3] - LineX[2], LineY[2] - LineY[1]);

            rtnRects[6] = new Rectangle(LineX[0], LineY[2], LineX[1] - LineX[0], LineY[3] - LineY[2]);
            rtnRects[7] = new Rectangle(LineX[1], LineY[2], LineX[2] - LineX[1], LineY[3] - LineY[2]);
            rtnRects[8] = new Rectangle(LineX[2], LineY[2], LineX[3] - LineX[2], LineY[3] - LineY[2]);

            return rtnRects;
        }
        #endregion

        #region 得到十二块矩形区域(窗体用)
        public static Rectangle[] GetRectangles(Size srcSize, int LeftBorderWidth, int RightBorderWidth, int TopBorderHeight, int BottomBorderHeight, int TopicHeight)
        {
            Rectangle[] rtnRects = new Rectangle[12];
            /*
             * 
             *      0 1                         2 3
             *      | |                         | |
             *      ┌-----------------------------┐-0
             *      |-|-------------------------|-|-1
             *      |_|_________________________|_|-2
             *      | |                         | |
             *      | |                         | |
             *      | |                         | |
             *      | |                         | |
             *      | |                         | |
             *      | |                         | |
             *      |_|_________________________|_|-3
             *      └-|-------------------------|-┘-4   
             * 
             * 
             * 
             *  水平五条线、垂直四条线
             */
            int[] LineX = new int[4];
            int[] LineY = new int[5];

            LineX[0] = 0;
            LineX[1] = LeftBorderWidth;
            LineX[2] = srcSize.Width - RightBorderWidth;
            LineX[3] = srcSize.Width;

            LineY[0] = 0;
            LineY[1] = TopBorderHeight;
            LineY[2] = TopBorderHeight + TopicHeight;
            LineY[3] = srcSize.Height - BottomBorderHeight;
            LineY[4] = srcSize.Height;


            rtnRects[0] = new Rectangle(LineX[0], LineY[0], LineX[1] - LineX[0], LineY[1] - LineY[0]);
            rtnRects[1] = new Rectangle(LineX[1], LineY[0], LineX[2] - LineX[1], LineY[1] - LineY[0]);
            rtnRects[2] = new Rectangle(LineX[2], LineY[0], LineX[3] - LineX[2], LineY[1] - LineY[0]);

            rtnRects[3] = new Rectangle(LineX[0], LineY[1], LineX[1] - LineX[0], LineY[2] - LineY[1]);
            rtnRects[4] = new Rectangle(LineX[1], LineY[1], LineX[2] - LineX[1], LineY[2] - LineY[1]);
            rtnRects[5] = new Rectangle(LineX[2], LineY[1], LineX[3] - LineX[2], LineY[2] - LineY[1]);

            rtnRects[6] = new Rectangle(LineX[0], LineY[2], LineX[1] - LineX[0], LineY[3] - LineY[2]);
            rtnRects[7] = new Rectangle(LineX[1], LineY[2], LineX[2] - LineX[1], LineY[3] - LineY[2]);
            rtnRects[8] = new Rectangle(LineX[2], LineY[2], LineX[3] - LineX[2], LineY[3] - LineY[2]);

            rtnRects[9] = new Rectangle(LineX[0], LineY[3], LineX[1] - LineX[0], LineY[4] - LineY[3]);
            rtnRects[10] = new Rectangle(LineX[1], LineY[3], LineX[2] - LineX[1], LineY[4] - LineY[3]);
            rtnRects[11] = new Rectangle(LineX[2], LineY[3], LineX[3] - LineX[2], LineY[4] - LineY[3]);
            return rtnRects;
        }
        #endregion
    }
}
