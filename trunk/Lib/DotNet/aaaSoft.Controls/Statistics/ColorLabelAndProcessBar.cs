using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls.Statistics
{
    public partial class ColorLabelAndProcessBar : UserControl
    {
        private Single _Process = 0;
        private String _DisplayText;
        private Int32 _PercentNumberOfDecimalPlaces = 2;
        private Color _ProcessBackColor = Color.FromArgb(238, 238, 238);
        private Boolean _IsHorizon = true;

        private Dictionary<Single, Color> _ForeColorDict;

        /// <summary>
        /// 进度
        /// </summary>
        public Single Process
        {
            get { return _Process; }
            set { _Process = value; SetData(); }
        }
        
        /// <summary>
        /// 显示文本(如果有值则显示文本，否则显示进度)
        /// </summary>
        public String DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; SetData(); }
        }

        /// <summary>
        /// 百分比显示小数位数
        /// </summary>
        public Int32 PercentNumberOfDecimalPlaces
        {
            get { return _PercentNumberOfDecimalPlaces; }
            set { _PercentNumberOfDecimalPlaces = value; SetData(); }
        }

        /// <summary>
        /// 进度背景颜色
        /// </summary>
        public Color ProcessBackColor
        {
            get { return _ProcessBackColor; }
            set { _ProcessBackColor = value; SetData(); }
        }

        /// <summary>
        /// 进度条长度
        /// </summary>
        public Int32 ProcessBarWidth
        {
            get { return pnlProcessBar.Width; }
            set { pnlProcessBar.Width = value; }
        }

        /// <summary>
        /// 是否水平放置控件
        /// </summary>
        public Boolean IsHorizon
        {
            get { return _IsHorizon; }
            set 
            {
                _IsHorizon = value;
                if (value)
                {
                    lblProcessText.Location = new Point(0, 0);
                    pnlProcessBar.Location = new Point(lblProcessText.Right + 5, 0);
                }
                else
                {
                    lblProcessText.Location = new Point(0, 0);
                    pnlProcessBar.Location = new Point(0, lblProcessText.Bottom + 2);
                }
            }
        }

        /*
        /// <summary>
        /// 前端颜色字典
        /// </summary>
        public Dictionary<Single, Color> ForeColorDict
        {
            get { return _ForeColorDict; }
            set
            {
                _ForeColorDict = value;
                SetData();
            }
        }
        */

        public ColorLabelAndProcessBar()
        {
            InitializeComponent();

            _ForeColorDict = new Dictionary<float, Color>();
            _ForeColorDict.Add(0.0F, Color.FromArgb(40, 171, 23));
            _ForeColorDict.Add(0.7F, Color.FromArgb(255, 153, 0));
            _ForeColorDict.Add(0.9F, Color.FromArgb(255, 0, 0));
        }

        private void SetData()
        {
            if (String.IsNullOrEmpty(DisplayText))
                lblProcessText.Text = (Process * 100).ToString("N" + PercentNumberOfDecimalPlaces) + "%";
            else
                lblProcessText.Text = DisplayText;

            lblProcessText.ForeColor = GetProcessColor();
            if (IsHorizon)
                pnlProcessBar.Left = lblProcessText.Right + 5;
            pnlProcessBar.BackColor = ProcessBackColor;
            this.Refresh();
        }

        //根据进度得到进度颜色
        private Color GetProcessColor()
        {
            Color rtnColor = Color.FromArgb(40, 171, 23);
            List<Single> processLevelList = new List<float>();
            foreach(var item in _ForeColorDict.Keys)
            {
                processLevelList.Add(item);
            }

            processLevelList.Sort();

            foreach (Single key in processLevelList)
            {
                if (Process < key)
                    break;
                rtnColor = _ForeColorDict[key];
            }
            return rtnColor;
        }

        private void ColorLabelAndProcessBar_Load(object sender, EventArgs e)
        {
            SetData();
        }

        private void pnlProcessBar_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            RectangleF rect = new RectangleF(0, 0, pnlProcessBar.ClientSize.Width * Process, pnlProcessBar.ClientSize.Height);
            var brush = new SolidBrush(GetProcessColor());
            g.FillRectangle(brush, rect);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0 - base.Left, 0 - base.Top);
            base.InvokePaintBackground(this.Parent, e);
            base.InvokePaint(this.Parent, e);
        }
    }
}
