using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.SkinEngine.MyControls
{
    /// <summary>
    /// 控件接口
    /// </summary>
    interface IMyControl
    {
        /// <summary>
        /// 开始控件皮肤
        /// </summary>
        void StartControlSkin();
        /// <summary>
        /// 结束控件皮肤
        /// </summary>
        void StopControlSkin();
    }
}
