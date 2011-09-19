using System;
using System.Collections.Generic;
using System.Text;

namespace aaaSoft.Helpers
{
    public class UrlHelper
    {
        public static String Combin(String Url1, String Url2)
        {
            return Combin(new String[] { Url1, Url2 });
        }

        public static String Combin(String[] UrlArray)
        {
            Uri CurrentUri = new Uri(UrlArray[0]);
            for (int i = 1; i <= UrlArray.Length - 1; i++)
            {
                if (!Uri.TryCreate(CurrentUri, UrlArray[i], out CurrentUri))
                {
                    return String.Empty;
                }
            }
            return CurrentUri.ToString();
        }
    }
}
