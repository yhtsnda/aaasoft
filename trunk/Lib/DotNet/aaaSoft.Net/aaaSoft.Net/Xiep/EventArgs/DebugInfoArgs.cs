using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Net.Xiep.EventArgs
{
    public class DebugInfoArgs : System.EventArgs
    {

        private String debugText;

        //获取调试文本
        public String getDebugText()
        {
            return debugText;
        }

        //设置调试文本
        public void setDebugText(String value)
        {
            debugText = value;
        }
    }
}