using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers
{
    public class NumberHelper
    {
        //int转为字节数组
        public static byte[] intToByte(int i)
        {
            byte[] bt = new byte[4];
            bt[0] = (byte)(0xff & i);
            bt[1] = (byte)((0xff00 & i) >> 8);
            bt[2] = (byte)((0xff0000 & i) >> 16);
            bt[3] = (byte)((0xff000000 & i) >> 24);
            return bt;
        }

        //字节数组转为int
        public static int bytesToInt(byte[] bytes)
        {
            int num = bytes[0] & 0xFF;
            num |= ((bytes[1] << 8) & 0xFF00);
            num |= ((bytes[2] << 16) & 0xFF0000);
            num |= Convert.ToInt32(((bytes[3] << 24) & 0xFF000000));
            return num;
        }
    }
}
