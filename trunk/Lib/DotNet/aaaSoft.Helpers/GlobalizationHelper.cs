using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Drawing;

namespace aaaSoft.Helpers
{
    /// <summary>
    /// 国际化辅助类
    /// </summary>
    public class GlobalizationHelper
    {
        private List<Control> managedControlList;
        private CultureInfo currentCulture;

        /// <summary>
        /// 设置或获取默认区域与文化
        /// </summary>
        public CultureInfo DefaultCulture { get; set; }
        /// <summary>
        /// 设置或获取当前区域与文化
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set { currentCulture = changeCulture(value); }
        }
        /// <summary>
        /// 设置或获取受管理的控件列表
        /// </summary>
        public List<Control> ManagedControlList
        {
            get { return managedControlList; }
            set { managedControlList = value; }
        }
        /// <summary>
        /// 获取或设置资源文件的路径(注意：不是文件系统的路径)
        /// </summary>
        public String ResourceFilePath { get; set; }
        /// <summary>
        /// 获取或设置资源文件所在的程序集(如不设置则默认为调用此类中方法的程序集)
        /// </summary>
        public Assembly ResourceFileAssembly { get; set; }

        private ResourceManager resourceManager;

        public GlobalizationHelper()
        {
            DefaultCulture = CultureInfo.CurrentCulture;
            ResourceFileAssembly = System.Reflection.Assembly.GetCallingAssembly();
            ManagedControlList = new List<Control>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            resourceManager = new System.Resources.ResourceManager(ResourceFilePath, ResourceFileAssembly);
            CurrentCulture = DefaultCulture;
        }

        /// <summary>
        /// 得到资源字符串
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public String GetString(String resourceName)
        {
            return GetString(resourceName, CurrentCulture);
        }

        /// <summary>
        /// 得到资源字符串
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public String GetString(String resourceName, CultureInfo culture)
        {
            String resourceValue;
            try
            {
                resourceValue = resourceManager.GetString(resourceName, culture);
            }
            catch
            {
                resourceValue = String.Format("{{RESOURCE：{0}}}", resourceName);
            }
            return resourceValue;
        }
        /// <summary>
        /// 得到资源图片
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public Image GetImage(String resourceName)
        {
            return GetImage(resourceName, CurrentCulture);
        }
        /// <summary>
        /// 得到资源图片
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public Image GetImage(String resourceName, CultureInfo culture)
        {
            Image resourceValue;
            try
            {
                resourceValue = (Image)resourceManager.GetObject(resourceName, culture);
            }
            catch
            {
                resourceValue = new Bitmap(400, 300);
                Graphics g = Graphics.FromImage(resourceValue);
                Font font = new Font(FontFamily.GenericMonospace, 16);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString("", font, Brushes.Black, new Rectangle(new Point(0, 0), resourceValue.Size), sf);
            }
            return resourceValue;
        }

        /// <summary>
        /// 改变区域与文化
        /// </summary>
        /// <param name="culture"></param>
        private CultureInfo changeCulture(CultureInfo culture)
        {
            try
            {
                foreach (var control in ManagedControlList)
                {
                    changeCultureCore(control, culture);
                }
                return culture;
            }
            catch
            {
                return DefaultCulture;
            }
        }

        private void changeCultureCore(Control control, CultureInfo culture)
        {
            //递归子控件
            foreach (Control subControl in control.Controls)
            {
                changeCultureCore(subControl, culture);
            }

            //如果Tag属性中没有包括资源占位符
            if (control.Tag == null
                || !(control.Tag is String)
                || String.IsNullOrEmpty(control.Tag as String)
                || !control.Tag.ToString().Contains("${"))
            {
                return;
            }

            //替换资源名称占位符为资源值
            StringBuilder sb = new StringBuilder(control.Tag.ToString());
            while (true)
            {
                String currentText = sb.ToString();
                if (currentText.Contains("${"))
                {
                    String resourceName = StringHelper.GetMiddleString(sb.ToString(), "${", "}", false);
                    String resourceValue = this.GetString(resourceName, culture);
                    sb.Replace("${" + resourceName + "}", resourceValue);
                }
                else
                {
                    break;
                }
            }
            //===============
            //设置资源
            //===============
            if (control is Form
                || control is Label)
            {
                control.Text = sb.ToString();
            }
        }
    }
}
