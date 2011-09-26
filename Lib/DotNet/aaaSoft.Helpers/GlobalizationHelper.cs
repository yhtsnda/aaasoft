using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

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
            currentCulture = DefaultCulture;
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
                    String resourceValue = "{NeedResource}";
                    try
                    {
                        resourceValue = resourceManager.GetString(resourceName, culture);
                    }
                    catch { }
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
