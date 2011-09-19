using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls
{
    public partial class AutoRefreshUserControl : UserControl
    {
        /// <summary>
        /// 是否开启自动刷新
        /// </summary>
        public Boolean IsAutoRefresh
        {
            get { return tmrRefresh.Enabled; }
            set { tmrRefresh.Enabled = value; }
        }

        /// <summary>
        /// 刷新间隔(单位:毫秒)
        /// </summary>
        public Int32 RefreshInterval
        {
            get { return tmrRefresh.Interval; }
            set { tmrRefresh.Interval = value; }
        }

        public AutoRefreshUserControl()
        {
            InitializeComponent();
        }


        private void AutoRefreshUserControl_Load(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 刷新控件(数据)
        /// </summary>
        public virtual void RefreshControl()
        {
            this.Refresh();
        }
    }
}
