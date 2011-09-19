using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.Controls.Helpers;
using System.Drawing.Drawing2D;
using System.Diagnostics;


namespace aaaSoft.Controls
{
    public class ListView : System.Windows.Forms.ListView
    {
        #region 是否启用浮动
        private bool _IsUseFloating = false;
        public bool IsUseFloating
        {
            get { return _IsUseFloating; }
            set { _IsUseFloating = value; }
        }
        #endregion

        #region 浮动列序号
        private Int32 _FloatingColumnIndex = -1;
        /// <summary>
        /// 浮动列序号(如果为-1,则所有列都浮动)
        /// </summary>
        public Int32 FloatingColumnIndex
        {
            get { return _FloatingColumnIndex; }
            set { _FloatingColumnIndex = value; }
        }
        #endregion

        #region 内部变量
        //自动调整宽度时的锁
        private  Object lockObj = new Object();
        //是否正在自动调整列宽度
        private bool IsColumnAutoChangingSize = false;
        //列宽度列表
        private List<Int32> ColumnWidthList = new List<int>();
        //列宽度之和
        private Int32 ColumnWidthSum
        {
            get
            {
                Int32 rtn = 0;
                foreach (var width in ColumnWidthList)
                {
                    rtn += width;
                }
                return rtn;
            }
        }
        #endregion

        #region OnCreateControl
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!this.OwnerDraw)
                this.OwnerDraw = true;

            if (IsUseFloating)
                GetColumnWidthData();

            this.ListViewItemSorter = new aaaSoft.Controls.Helpers.ListViewColumnSorter();
            this.ColumnClick += new ColumnClickEventHandler(aaaSoft.Controls.Helpers.ListViewHelper.ListView_ColumnClick);

            OnSizeChanged(new EventArgs());
        }
        #endregion

        #region OnColumnWidthChanged
        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            if (!IsColumnAutoChangingSize && IsUseFloating)
                GetColumnWidthData();
            base.OnColumnWidthChanged(e);
        }
        #endregion

        private Size preClientSize;
        #region OnSizeChanged
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (preClientSize != this.Size)
            {
                if (this.IsHandleCreated && !this.DesignMode && IsUseFloating)
                {
                    Int32 AfterColumnWidthSum = 0;
                    if (this.ClientSize.Width > ColumnWidthSum)
                    {
                        do
                        {
                            AfterColumnWidthSum = this.ClientSize.Width;
                            lock (lockObj)
                            {
                                IsColumnAutoChangingSize = true;
                                AutoSizeColumnWidth();
                                IsColumnAutoChangingSize = false;
                            }
                        } while (AfterColumnWidthSum != this.ClientSize.Width);
                    }
                }
                preClientSize = this.Size;
            }
            this.UpdateExtendedStyles();
        }
        #endregion

        #region 采集列宽
        private void GetColumnWidthData()
        {
            ColumnWidthList.Clear();
            for (Int32 i = 0; i <= this.Columns.Count - 1; i++)
            {
                ColumnWidthList.Add(this.Columns[i].Width);
            }
        }
        #endregion

        #region 根据比例调整列宽
        private void AutoSizeColumnWidth()
        {
            if (FloatingColumnIndex >= this.Columns.Count)
            {
                FloatingColumnIndex = -1;
            }

            Int32 AfterColumnWidthSum = this.ClientSize.Width;
            //如果是所有列都浮动
            if (FloatingColumnIndex == -1)
            {
                //比例
                Double scale = AfterColumnWidthSum * 1D / ColumnWidthSum;
                for (int i = 0; i <= this.Columns.Count - 1; i++)
                {
                    this.Columns[i].Width = (Int32)(ColumnWidthList[i] * scale);
                }
            }
            else
            {
                this.Columns[this.FloatingColumnIndex].Width = ColumnWidthList[this.FloatingColumnIndex] + AfterColumnWidthSum - ColumnWidthSum;
            }
        }
        #endregion





        #region 画排序小三角部分
        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);
            var sorter = this.ListViewItemSorter as ListViewColumnSorter;
            if (sorter == null)
                return;
            if (sorter.Order != SortOrder.None)
            {
                this.Columns[sorter.SortColumn].Text = this.Columns[sorter.SortColumn].Text;
            }            
        }
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawItem(e);
        }
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawSubItem(e);
        }
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawColumnHeader(e);

            var sorter = this.ListViewItemSorter as ListViewColumnSorter;
            if (sorter == null)
                return;

            if (sorter.Order != SortOrder.None && sorter.SortColumn == e.ColumnIndex)
            {
                var g = e.Graphics;

                Int32 trangleWidth = 8;
                Int32 trangleHeight = 4;
                //与边缘的间隔
                Int32 sp = 4;

                //准备图形路径
                GraphicsPath gp = new GraphicsPath();
                if (sorter.Order == SortOrder.Ascending)
                {
                    //递增小三角
                    gp.AddLines(new Point[]
                    {
                        new Point(trangleWidth / 2, 0),
                        new Point(0,trangleHeight),
                        new Point(trangleWidth, trangleHeight),
                    });
                }
                else
                {
                    //递减小三角
                    gp.AddLines(new Point[]
                    {
                        new Point(0, 0),
                        new Point(trangleWidth,0),
                        new Point(trangleWidth / 2, trangleHeight),
                    });
                }
                gp.CloseFigure();
                //准备小三角的位置
                Point pt;
                if (this.Columns[e.ColumnIndex].TextAlign == HorizontalAlignment.Right)
                {
                    pt = new Point(e.Bounds.Left + sp, (e.Bounds.Height - trangleHeight) / 2 - e.Bounds.Top);
                }
                else
                {
                    pt = new Point(e.Bounds.Right - trangleWidth - sp, (e.Bounds.Height - trangleHeight) / 2 - e.Bounds.Top);
                }
                Region rgn = new System.Drawing.Region(gp);
                Brush brs = new SolidBrush(e.ForeColor);
                Pen pen = new Pen(e.ForeColor);

                g.TranslateTransform(pt.X, pt.Y);
                g.DrawPath(pen, gp);
                g.FillRegion(brs, rgn);
                g.ResetTransform();
            }
        }
        #endregion
    }
}
