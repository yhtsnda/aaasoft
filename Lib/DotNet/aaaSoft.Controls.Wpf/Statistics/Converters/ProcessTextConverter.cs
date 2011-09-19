using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace aaaSoft.Controls.Wpf.Statistics.Converters
{
    public class ProcessTextConverter : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Single)
            {
                Single process = (Single)value;
                Int32 percentNumberOfDecimalPlaces = (Int32)parameter;
                return String.Format("{0}%", (process * 100).ToString("N" + percentNumberOfDecimalPlaces));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
