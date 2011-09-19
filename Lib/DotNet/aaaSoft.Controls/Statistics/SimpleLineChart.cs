using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace aaaSoft.Controls.Statistics
{
    public partial class SimpleLineChart : UserControl
    {
        public SimpleLineChart()
        {
            InitializeComponent();
        }

        private Single _MaxValue = 100;
        /// <summary>
        /// 最大值
        /// </summary>
        public Single MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; this.Refresh(); }
        }

        private Single _MinValue = 0;
        /// <summary>
        /// 最小值
        /// </summary>
        public Single MinValue
        {
            get { return _MinValue; }
            set { _MinValue = value; this.Refresh(); }
        }

        private Single[] _DataArray;
        /// <summary>
        /// 数据
        /// </summary>
        public Single[] DataArray
        {
            get { return _DataArray; }
            set { _DataArray = value; this.Refresh(); }
        }

        private Color _LineColor = Color.FromArgb(0, 119, 204);
        /// <summary>
        /// 线条颜色
        /// </summary>
        public Color LineColor
        {
            get { return _LineColor; }
            set { _LineColor = value; this.Refresh(); }
        }

        private Single _LineWidth = 1.6F;
        /// <summary>
        /// 线条宽度
        /// </summary>
        public Single LineWidth
        {
            get { return _LineWidth; }
            set { _LineWidth = value; this.Refresh(); }
        }

        private Color _SurfaceColor = Color.FromArgb(229, 241, 249);
        /// <summary>
        /// 面颜色
        /// </summary>
        public Color SurfaceColor
        {
            get { return _SurfaceColor; }
            set { _SurfaceColor = value; this.Refresh(); }
        }

        private void DrawToGraphics(Graphics g)
        {
            if (DataArray == null || DataArray.Length <= 1)
                return;
            if (MaxValue <= MinValue)
            {
                MaxValue = MinValue + 1;
                return;
            }

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Pen linePen = new Pen(LineColor, LineWidth);


            var XUnit = this.ClientSize.Width * 1F / (DataArray.Length - 1);
            var YUnit = (this.ClientSize.Height - LineWidth) * 1F / (MaxValue - MinValue);

            for (int i = 0; i <= DataArray.Length - 1 - 1; i++)
            {
                var CurrentData = DataArray[i];
                var NextData = DataArray[i + 1];

                if (CurrentData < MinValue)
                    continue;

                PointF CurrentDataLocation = new PointF(i * XUnit, ((MaxValue - CurrentData) * YUnit + LineWidth / 2));
                if (NextData < MinValue)
                {
                    continue;
                }
                else
                {
                    PointF NextDataLocation = new PointF((i + 1) * XUnit, ((MaxValue - NextData) * YUnit + LineWidth / 2));
                    PointF LeftBottomPoint = new PointF(CurrentDataLocation.X,(MaxValue - 0) * YUnit + LineWidth / 2);
                    PointF RightBottomPoint = new PointF(NextDataLocation.X,(MaxValue - 0) * YUnit + LineWidth / 2);
                    //填充背景
                    var surfacePath = new GraphicsPath();
                    surfacePath.AddLines(new PointF[]
                    {
                        LeftBottomPoint
                        ,CurrentDataLocation
                        ,NextDataLocation
                        ,RightBottomPoint
                        ,LeftBottomPoint
                    });
                    g.DrawLine(new Pen(SurfaceColor), NextDataLocation, RightBottomPoint);
                    g.FillPath(new SolidBrush(SurfaceColor), surfacePath);
                    //画线
                    g.DrawLine(linePen, CurrentDataLocation, NextDataLocation);
                    
                }
            }
        }

        public Bitmap DrawToBitmap()
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(bmp);
            DrawToGraphics(g);
            return bmp;
        }

        private void SimpleLineChart_Paint(object sender, PaintEventArgs e)
        {
            DrawToGraphics(e.Graphics);
        }

        private void SimpleLineChart_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
