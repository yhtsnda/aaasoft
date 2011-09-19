using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls.Statistics
{
    public partial class LineChart : UserControl
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

        private DataTable _Data;
        /// <summary>
        /// 数据
        /// </summary>
        public DataTable Data
        {
            get { return _Data; }
            set { _Data = value; }
        }



        public LineChart()
        {
            InitializeComponent();
        }

        private void pnlDrawPaper_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
