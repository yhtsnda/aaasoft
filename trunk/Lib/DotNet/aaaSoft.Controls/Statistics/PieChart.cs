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
    public partial class PieChart : UserControl
    {
        /// <summary>
        /// 标题
        /// </summary>
        public String Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        private String _Unit;
        /// <summary>
        /// 单位
        /// </summary>
        public String Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        private Int32 _PercentNumberOfDecimalPlaces = 2;
        /// <summary>
        /// 百分比显示小数位数
        /// </summary>
        public Int32 PercentNumberOfDecimalPlaces
        {
            get { return _PercentNumberOfDecimalPlaces; }
            set { _PercentNumberOfDecimalPlaces = value; }
        }
        private Int32 _DataNumberOfDecimalPlaces = 2;
        /// <summary>
        /// 数据显示小数位数
        /// </summary>
        public Int32 DataNumberOfDecimalPlaces
        {
            get { return _DataNumberOfDecimalPlaces; }
            set { _DataNumberOfDecimalPlaces = value; }
        }

        private PieChartItem[] _PieChartItemArray;
        /// <summary>
        /// 饼形图数据项数组
        /// </summary>
        public PieChartItem[] PieChartItemArray
        {
            get { return _PieChartItemArray; }
            set
            {
                _PieChartItemArray = value;
                //计算和
                _TotalDataValue = 0;
                if (PieChartItemArray != null)
                {
                    foreach (PieChartItem tmpItem in PieChartItemArray)
                    {
                        _TotalDataValue += tmpItem.Value;
                    }                    
                }
                GenPieCharts();
            }
        }

        [Serializable()]
        /// <summary>
        /// 饼形图数据项
        /// </summary>
        public class PieChartItem
        {
            private String _Description = "描述";
            /// <summary>
            /// 数据项描述
            /// </summary>
            public String Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            private Double _Value;
            /// <summary>
            /// 数据项数值
            /// </summary>
            public Double Value
            {
                get { return _Value; }
                set { _Value = value; }
            }

            private Color _Color = aaaSoft.Helpers.ColorHelper.GetRandomColor();
            /// <summary>
            /// 数据项颜色
            /// </summary>
            public Color Color
            {
                get { return _Color; }
                set { _Color = value; }
            }
        }

        public PieChart()
        {
            InitializeComponent();
        }

        private Double _TotalDataValue;
        //对象的的数据之和
        public Double TotalDataValue
        {
            get { return _TotalDataValue; }
        }
        /// <summary>
        /// 得到对象数据所占的百分比
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Double GetItemPercent(PieChartItem item)
        {
            return item.Value * 100 / TotalDataValue;
        }

        /// <summary>
        /// 得到对象数据所占的比例
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Double GetItemScale(PieChartItem item)
        {
            return item.Value / TotalDataValue;
        }

        private void PieCharts_Load(object sender, EventArgs e)
        {
            GenPieCharts();
        }

        //生成
        private void GenPieCharts()
        {
            pnlDrawPaper.Controls.Clear();
            flpDesc.Controls.Clear();

#warning 使用Region.IsVisible(Point)函数

            if (PieChartItemArray == null || PieChartItemArray.Length == 0)
                return;

            foreach (PieChartItem item in PieChartItemArray)
            {
                if (item == null)
                    return;

                ColorAndLabel calNew = new ColorAndLabel();
                calNew.Color = item.Color;
                calNew.BackColor = this.BackColor;
                calNew.ShowText = item.Description;
                calNew.Tag = item;

                calNew.MouseEnter += new EventHandler(pieLabel_MouseEnter);
                calNew.MouseMove += new MouseEventHandler(pieLabel_MouseMove);
                calNew.MouseLeave += new EventHandler(pieLabel_MouseLeave);

                flpDesc.Controls.Add(calNew);
            }


            pnlDrawPaper.Height = this.Height - lblTitle.Height - flpDesc.Height - 10;

            //圆直径
            var roundDiameter = Math.Min(pnlDrawPaper.Width, pnlDrawPaper.Height);
            if (roundDiameter <= 0) return;

            //圆所占的矩形区域
            var roundRect = new Rectangle(
                0,
                0,
                roundDiameter, roundDiameter);
            //圆心坐标
            Point roundCenterPoint = new Point(roundRect.Width / 2 + roundRect.Left, roundRect.Height / 2 + roundRect.Top);
            //下次的起始角度
            Single NextStartAngle = -90;
            
            foreach (PieChartItem item in PieChartItemArray)
            {
                Label pieLabel = new Label();
                pieLabel.AutoSize = false;
                pieLabel.Location = new Point((pnlDrawPaper.Width - roundDiameter) / 2, (pnlDrawPaper.Height - roundDiameter) / 2);
                pieLabel.Size = roundRect.Size;
                pieLabel.BackColor = item.Color;
                pieLabel.Tag = item;

                pieLabel.MouseEnter += new EventHandler(pieLabel_MouseEnter);
                pieLabel.MouseMove += new MouseEventHandler(pieLabel_MouseMove);
                pieLabel.MouseLeave += new EventHandler(pieLabel_MouseLeave);
                GraphicsPath gp = new GraphicsPath();

                Single TAngle = Convert.ToSingle(GetItemScale(item) * 360);
                gp.AddPie(roundRect, NextStartAngle, TAngle);
                NextStartAngle += TAngle;

                Region region = new Region(gp);
                pieLabel.Region = region;
                pnlDrawPaper.Controls.Add(pieLabel);
            }
        }

        void pieLabel_MouseEnter(object sender, EventArgs e)
        {
            var ctl = (Control)sender;
            var item = ctl.Tag as PieChartItem;

            String TextTemplate = String.Empty;
            if (String.IsNullOrEmpty(Unit))
            {
                TextTemplate = "\n   {0}:{1}%   \n ";
            }
            else
            {
                TextTemplate = "\n   {0}:{2}{3}({1}%)   \n ";
            }

            lblTip.Text = String.Format(
                TextTemplate
                , item.Description                
                , GetItemPercent(item).ToString("N" + PercentNumberOfDecimalPlaces)
                , item.Value.ToString("N" + DataNumberOfDecimalPlaces)
                , this.Unit);

            lblTip.BackColor = aaaSoft.Helpers.ColorHelper.GetDeeperColor(item.Color, 0.8F);
            lblTip.ForeColor = Color.White;
            lblTip.BringToFront();
            lblTip.Show();
        }

        void pieLabel_MouseLeave(object sender, EventArgs e)
        {
            lblTip.Hide();
        }


        void pieLabel_MouseMove(object sender, MouseEventArgs e)
        {
            var ctl = (Control)sender;
            var item = ctl.Tag as PieChartItem;

            Point showLocation = lblTip.Parent.PointToClient(Form.MousePosition);

            showLocation.X = showLocation.X - lblTip.Width / 2;
            Int32 Sp = 10;
            if (showLocation.Y < lblTip.Parent.Height / 2)
            {
                showLocation.Y += Sp;
            }
            else
            {
                showLocation.Y -= lblTip.Height + Sp;
            }
            

            if (showLocation.X < 0) showLocation.X = 0;
            if (showLocation.X + lblTip.Width > lblTip.Parent.Width)
            {
                showLocation.X = lblTip.Parent.Width - lblTip.Width;
            }
            if (showLocation.Y < 0) showLocation.Y = 0;
            if (showLocation.Y + lblTip.Height > lblTip.Parent.Height)
            {
                showLocation.Y = lblTip.Parent.Height - lblTip.Height;
            }

            lblTip.Location = showLocation;
        }

        private void PieCharts_SizeChanged(object sender, EventArgs e)
        {
            GenPieCharts();
        }
    }
}
