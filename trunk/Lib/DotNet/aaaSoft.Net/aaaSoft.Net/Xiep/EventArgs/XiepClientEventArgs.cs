using System;
using System.Collections.Generic;
using System.Text;
using aaaSoft.Net.Xiep.Packages;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class XiepClientEventArgs : System.EventArgs
    {
        private EventPackage eventPackage;

        //获取事件包
        public EventPackage getEventPackage()
        {
            return eventPackage;
        }

        //构造函数
        public XiepClientEventArgs(EventPackage eventPackage)
        {
            this.eventPackage = eventPackage;
        }
    }
}
