using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace aaaSoft.Controls.Globalization
{
    public partial class LanguageSwitchBar : UserControl
    {
        //事件
        public event EventHandler LanguageChanged;
        //属性
        private CultureInfo currentCulture;

        /// <summary>
        /// 默认文化与区域
        /// </summary>
        public CultureInfo DefaultCulture { get; set; }
        /// <summary>
        /// 当前文化与区域
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                if (value == null)
                    return;
                cbLanguageSwitch.SelectedItem = value;
                currentCulture = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LanguageSwitchBar()
        {
            InitializeComponent();
        }

        public void InitLanguageList()
        {
            InitLanguageList(Application.StartupPath);
        }

        public void InitLanguageList(String applicationStartupPath)
        {
            DirectoryInfo di = new DirectoryInfo(applicationStartupPath);
            DirectoryInfo[] resourcesFolderArray = di.GetDirectories("*-*");
            List<String> cultureInfoNameList = new List<string>();
            foreach (var resourcesFolder in resourcesFolderArray)
            {
                cultureInfoNameList.Add(resourcesFolder.Name);
            }
            InitLanguageList(cultureInfoNameList.ToArray());
        }

        public void InitLanguageList(String[] cultureInfoNameArray)
        {
            List<CultureInfo> cultureInfoList = new List<CultureInfo>();
            foreach (var cultureInfoName in cultureInfoNameArray)
            {
                try
                {
                    CultureInfo selectedCulture = new CultureInfo(cultureInfoName);
                    cultureInfoList.Add(selectedCulture);
                }
                catch { }
            }
            InitLanguageList(cultureInfoList.ToArray());
        }

        public void InitLanguageList(CultureInfo[] cultureInfoArray)
        {
            cbLanguageSwitch.Items.Clear();
            cbLanguageSwitch.Items.AddRange(cultureInfoArray);
            cbLanguageSwitch.SelectedItem = DefaultCulture;
        }

        private void cbLanguageSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentCulture = (CultureInfo)cbLanguageSwitch.SelectedItem;
            if (LanguageChanged != null)
                LanguageChanged(sender, e);
        }
    }
}
