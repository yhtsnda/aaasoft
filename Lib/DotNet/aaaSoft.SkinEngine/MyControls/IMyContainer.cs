using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace aaaSoft.SkinEngine.MyControls
{
    /// <summary>
    /// 容器接口
    /// </summary>
    interface IMyContainer
    {
        /// <summary>
        /// 委托画背景
        /// </summary>
        /// <param name="c">控件</param>
        /// <param name="e">绘画参数</param>
        void InvokePaintBackground(Component c, PaintEventArgs e);
        /// <summary>
        /// 委托画前景
        /// </summary>
        /// <param name="c">控件</param>
        /// <param name="e">绘画参数</param>
        void InvokePaint(Component c, PaintEventArgs e);
    }
}
