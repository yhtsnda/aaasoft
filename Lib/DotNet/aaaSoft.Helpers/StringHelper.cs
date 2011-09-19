using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers
{
    public class StringHelper
    {
        public static String GetLeftString(String source, String strTail)
        {
            return GetLeftString(source, strTail, false);
        }
        public static String GetLeftString(String source, String strTail, bool KeepHeadAndTail)
        {
            return GetMiddleString(source, "", strTail, KeepHeadAndTail);
        }

        public static String GetRightString(String source, String strHead)
        {
            return GetRightString(source, strHead, false);
        }

        public static String GetRightString(String source, String strHead, bool KeepHeadAndTail)
        {
            return GetMiddleString(source, strHead, "", KeepHeadAndTail);
        }

        public static String GetMiddleString(String source, String strHead, String strTail)
        {
            return GetMiddleString(source, strHead, strTail, false);
        }
        public static String GetMiddleString(String source, String strHead, String strTail, bool KeepHeadAndTail)
        {
            try
            {
                int indexHead, indexTail;

                if (String.IsNullOrEmpty(strHead))
                {
                    indexHead = 0;
                }
                else
                {
                    indexHead = source.IndexOf(strHead);
                }

                if (String.IsNullOrEmpty(strTail))
                {
                    indexTail = source.Length;
                }
                else
                {
                    indexTail = source.IndexOf(strTail, indexHead + strHead.Length);
                }
                if (indexTail < 0)
                {
                    indexTail = source.Length;
                }

                string rtnStr = string.Empty;
                if ((indexHead >= 0) && (indexTail >= 0))
                {
                    if (KeepHeadAndTail)
                    {
                        int rtnLength = indexTail - indexHead + strTail.Length;
                        rtnStr = source.Substring(indexHead, rtnLength);
                    }
                    else
                    {
                        int rtnLength = indexTail - indexHead - strHead.Length;
                        rtnStr = source.Substring(indexHead + strHead.Length, rtnLength);
                    }
                }
                return rtnStr;
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}