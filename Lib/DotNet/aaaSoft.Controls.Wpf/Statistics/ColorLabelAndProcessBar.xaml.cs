using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;


namespace aaaSoft.Controls.WPF.Statistics
{
    /// <summary>
    /// ColorLabelAndProcessBar.xaml 的交互逻辑
    /// </summary>
    public partial class ColorLabelAndProcessBar : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 成员
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        #endregion

        //private Single _Process = 0;
        private Int32 _PercentNumberOfDecimalPlaces = 2;
        private Color _ProcessBackColor = Color.FromRgb(238, 238, 238);
        private Dictionary<Single, Color> _ForeColorDict;

        public static readonly DependencyProperty ProcessProperty;

        //通知属性值改变
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 进度
        /// </summary>
        public Single Process
        {
            get { return (Single)GetValue(ProcessProperty); }
            set
            {
                SetValue(ProcessProperty, value);
                /*
                _Process = value;
                NotifyPropertyChanged("Process");
                 */
            }
        }

        /// <summary>
        /// 百分比显示小数位数
        /// </summary>
        public Int32 PercentNumberOfDecimalPlaces
        {
            get { return _PercentNumberOfDecimalPlaces; }
            set
            {
                _PercentNumberOfDecimalPlaces = value;
                NotifyPropertyChanged("Process");
            }
        }


        /// <summary>
        /// 进度背景颜色
        /// </summary>
        public Color ProcessBackColor
        {
            get { return _ProcessBackColor; }
            set
            {
                _ProcessBackColor = value;
                NotifyPropertyChanged("ProcessBackColor");
            }
        }

        /// <summary>
        /// 前端颜色字典
        /// </summary>
        public Dictionary<Single, Color> ForeColorDict
        {
            get { return _ForeColorDict; }
            set
            {
                _ForeColorDict = value;
                NotifyPropertyChanged("Process");
            }
        }

        /// <summary>
        /// 根据进度得到进度颜色
        /// </summary>
        /// <returns></returns>
        public Color GetProcessColor()
        {
            Color rtnColor = Color.FromRgb(40, 171, 23);
            List<Single> processLevelList = _ForeColorDict.Keys.ToList();
            processLevelList.Sort();

            foreach (Single key in processLevelList)
            {
                if (Process < key)
                    break;
                rtnColor = _ForeColorDict[key];
            }
            return rtnColor;
        }

        //构造函数
        public ColorLabelAndProcessBar()
        {
            InitializeComponent();

            _ForeColorDict = new Dictionary<float, Color>();
            _ForeColorDict.Add(0.0F, Color.FromRgb(40, 171, 23));
            _ForeColorDict.Add(0.7F, Color.FromRgb(255, 153, 0));
            _ForeColorDict.Add(0.9F, Color.FromRgb(255, 0, 0));
        }

        static ColorLabelAndProcessBar()
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.DefaultValue = 0F;
            metadata.PropertyChangedCallback += OnProcessPropertyChanged;
            ProcessProperty = DependencyProperty.Register("Process", typeof(Single), typeof(ColorLabelAndProcessBar), metadata, ValidateProcessValue);
        }

        //验证Process属性值
        private static bool ValidateProcessValue(Object obj)
        {
            Single value = (Single)obj;
            return value >= 0 && value <= 1;
        }

        //Process属性改变时
        private static void OnProcessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        //控件加载时
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //绑定进度文本显示
            Binding processTextBinding = new Binding("Process");
            processTextBinding.Converter = new ProcessTextConverter();
            processTextBinding.Source = this;
            processTextBinding.ConverterParameter = this;
            processTextBinding.Mode = BindingMode.OneWay;
            lblProcessText.SetBinding(TextBlock.TextProperty, processTextBinding);

            //绑定进度条显示
            Binding processGridWidthBinding = new Binding("Process");
            processGridWidthBinding.Converter = new ProcessWidthConverter();
            processGridWidthBinding.Source = this;
            processGridWidthBinding.ConverterParameter = gridBack;
            processGridWidthBinding.Mode = BindingMode.OneWay;
            gridFront.SetBinding(Grid.WidthProperty, processGridWidthBinding);

            //绑定进度背景颜色
            Binding processBackColorBinding = new Binding("ProcessBackColor");
            processBackColorBinding.Converter = new ColorBackgroundBrushConverter();
            processBackColorBinding.Source = this;
            processBackColorBinding.Mode = BindingMode.OneWay;
            gridBack.SetBinding(Grid.BackgroundProperty, processBackColorBinding);


            //绑定进度前景颜色
            Binding processForeColorBinding = new Binding("Process");
            processForeColorBinding.Converter = new ProcessForegroundBrushConverter();
            processForeColorBinding.Source = this;
            processForeColorBinding.ConverterParameter = this;
            processForeColorBinding.Mode = BindingMode.OneWay;
            gridFront.SetBinding(Grid.BackgroundProperty, processForeColorBinding);
        }


        //控件大小改变时
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyPropertyChanged("Process");
        }

        //进度数字-文本转换器
        public class ProcessTextConverter : IValueConverter
        {

            #region IValueConverter 成员

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value is Single)
                {
                    Single process = (Single)value;
                    ColorLabelAndProcessBar control = (ColorLabelAndProcessBar)parameter;
                    Int32 percentNumberOfDecimalPlaces = control.PercentNumberOfDecimalPlaces;
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

        //进度数字-宽度转换器
        public class ProcessWidthConverter : IValueConverter
        {

            #region IValueConverter 成员

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value is Single)
                {
                    Single process = (Single)value;
                    Grid control = (Grid)parameter;
                    Double fullWidth = control.ActualWidth;
                    return process * fullWidth;
                }
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        //进度数字-前端笔刷转换器
        public class ProcessForegroundBrushConverter : IValueConverter
        {

            #region IValueConverter 成员

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value is Single)
                {
                    ColorLabelAndProcessBar control = (ColorLabelAndProcessBar)parameter;
                    Color color = control.GetProcessColor();
                    return new SolidColorBrush(color);
                }
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        //颜色-后端笔刷转换器
        public class ColorBackgroundBrushConverter : IValueConverter
        {
            #region IValueConverter 成员

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value is Color)
                {
                    Color color = (Color)value;
                    return new SolidColorBrush(color);
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
}
