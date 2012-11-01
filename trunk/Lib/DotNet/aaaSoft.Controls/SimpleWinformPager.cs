using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls
{
    public partial class SimpleWinformPager : UserControl
    {
        public SimpleWinformPager()
        {
            InitializeComponent();
        }

        private Int32 _PageIndex = 1;
        /// <summary>
        /// 获取或设置当前页码(从1开始)
        /// </summary>
        public Int32 PageIndex
        {
            get { return _PageIndex; }
            set
            {
                //如果超出范围
                if (value < 1)
                    value = 1;

                _PageIndex = value;

                txtPageIndex.Text = _PageIndex.ToString();
                //触发页码改变事件
                if (PageIndexChanged != null)
                    PageIndexChanged(this, new EventArgs());
                CheckPageChangeButton();

                if (PageIndex > PageCount && PageIndex > 1)
                {
                    PageIndex = PageCount;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            PageIndex = PageIndex;
        }

        private Int32 _PageSize = 20;
        /// <summary>
        /// 每页显示的数据条数
        /// </summary>
        public Int32 PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        public Int32 PageCount
        {
            get
            {
                var tmpPageCount = (RecordCount / PageSize);
                if (RecordCount % PageSize > 0)
                {
                    tmpPageCount += 1;
                }
                return tmpPageCount;
            }
        }

        private Int32 _RecordCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public Int32 RecordCount
        {
            get { return _RecordCount; }
            set
            {
                if (_RecordCount != value)
                {
                    _RecordCount = value;
                    txtPageIndex.Text = _PageIndex.ToString();
                    lblTotalPageCount.Text = PageCountStringFormat
                        .Replace("{pageCount}", PageCount.ToString())
                        .Replace("{recordCount}", RecordCount.ToString());
                    if (PageIndex > PageCount && PageIndex > 1)
                        PageIndex = PageCount;
                }
            }
        }

        private String _PageCountStringFormat = "页/{pageCount}页，共{recordCount}条记录";
        /// <summary>
        /// 总页数显示格式
        /// </summary>
        public String PageCountStringFormat
        {
            get { return _PageCountStringFormat; }
            set
            {
                _PageCountStringFormat = value;
                RecordCount = RecordCount;
            }
        }

        /// <summary>
        /// 在页码改变时发生
        /// </summary>
        public event EventHandler PageIndexChanged;

        private void CheckPageChangeButton()
        {
            btnFirstPage.Enabled = false;
            btnPrePage.Enabled = false;
            btnNextPage.Enabled = false;
            btnLastPage.Enabled = false;
            txtPageIndex.Enabled = false;

            if (PageCount <= 0) return;
            txtPageIndex.Enabled = true;

            if (PageIndex > 1)
            {
                btnFirstPage.Enabled = true;
                btnPrePage.Enabled = true;
            }
            if (PageIndex < PageCount)
            {
                btnNextPage.Enabled = true;
                btnLastPage.Enabled = true;
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageIndex = 1;
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            if (PageIndex <= 1)
            {
                aaaSoft.Helpers.FormHelper.ShowWarningDialog("当前已经是第一页！");
                return;
            }
            PageIndex -= 1;
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (PageIndex >= PageCount)
            {
                aaaSoft.Helpers.FormHelper.ShowWarningDialog("当前已经是最后一页！");
                return;
            }
            PageIndex += 1;
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageIndex = PageCount;
        }

        private void txtPageIndex_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    int InputPageIndex;
                    if (Int32.TryParse(txtPageIndex.Text.Trim(), out InputPageIndex))
                    {
                        if (InputPageIndex < 1 || InputPageIndex > PageCount)
                        {
                            aaaSoft.Helpers.FormHelper.ShowWarningDialog("请输入正确的页码！");
                            return;
                        }
                        PageIndex = InputPageIndex;
                    }
                    else
                    {
                        aaaSoft.Helpers.FormHelper.ShowWarningDialog("请输入有效的数字！");
                    }
                }
            }
        }
    }
}
