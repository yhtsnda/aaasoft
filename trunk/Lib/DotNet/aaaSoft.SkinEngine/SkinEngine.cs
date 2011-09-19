using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using aaaSoft.SkinEngine.MyControls;
using aaaSoft.SkinEngine.SkinHelpers;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using aaaSoft.Helpers;

namespace aaaSoft.SkinEngine
{
    public partial class SkinEngine : Component
    {
        public static SkinEngine MainSkinEngine = null;

        /// <summary>
        /// 控件与IMyControl对应字典
        /// </summary>
        Dictionary<Control, IMyControl> dictIMyControls = new Dictionary<Control, IMyControl>();
        /// <summary>
        /// 消息钩子
        /// </summary>
        MessageHook mh = new MessageHook();

        #region 构造函数
        public SkinEngine()
        {
            if (SkinEngine.MainSkinEngine == null)
            {

                SkinEngine.MainSkinEngine = this;
            }
            InitializeComponent();
        }
        public SkinEngine(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        #region 当产生消息时
        /// <summary>
        /// 当产生消息时
        /// </summary>
        /// <param name="msg">消息</param>
        void mh_MessageOccurred(ref Message msg)
        {
            NativeConsts.WindowMessage WindowsMessage = (NativeConsts.WindowMessage)msg.Msg;
            switch (WindowsMessage)
            {
                case NativeConsts.WindowMessage.WM_CREATE:
                    {
                        try
                        {
                            Control ctl = Control.FromHandle(msg.HWnd);
                            if (ctl is Form)
                            {
                                BindEvent(ctl);
                            }
                        }
                        catch { }
                        break;
                    }
            }
        }
        #endregion
        
        #region 根据控件获取对应的接口
        /// <summary>
        /// 根据控件获取对应的接口
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal IMyControl GetInterface(Control c)
        {
            if (dictIMyControls.ContainsKey(c))
                return dictIMyControls[c];
            else
                return null;
        }
        #endregion

        #region 属性 - 是否牌设计模式
        /// <summary>
        /// 是否处于设计模式
        /// </summary>
        public bool IsDesignMode
        {
            get
            {
                return this.DesignMode;
            }
        }
        #endregion

        #region 是否开启皮肤引擎
        private Boolean _IsUseSkin = false;
        /// <summary>
        /// 是否开启皮肤引擎
        /// </summary>
        public Boolean IsUseSkin
        {
            get
            {
                return _IsUseSkin;
            }
            set
            {
                if (_IsUseSkin == value)
                {
                    return;
                }
                _IsUseSkin = value;
                
                if (value)
                {
                    StartEngine();
                }
                else
                {
                    StopEngine();
                    if (this.DesignMode)
                    {
                        StopEngine();
                    }
                }
                if (ContainerControl != null)
                {
                    ContainerControl.Refresh();
                }
            }
        }
        #endregion

        #region 当前主题
        private String _CurrentTheme = "默认主题";

        /// <summary>
        /// 当前主题
        /// </summary>
        public String CurrentTheme
        {
            get
            {
                return _CurrentTheme;
            }
            set
            {
                if (_CurrentTheme != value)
                {
                    _CurrentTheme = value;
                    switch (value)
                    {
                        case "无":
                            this.IsUseSkin = false;
                            break;
                        case "默认":
                            LoadDefaultSkin();
                            break;
                        default:
                            LoadSkin(new System.IO.FileInfo(String.Format("Skin/{0}.zip", value)).FullName);
                            break;
                    }                    
                }
            }
        }
        #endregion

        #region 皮肤背景色
        private Color _BackColor = Color.FromArgb(96, 147, 193);
        /// <summary>
        /// 皮肤背景色
        /// </summary>
        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                if (_BackColor == value)
                {
                    return;
                }
                _BackColor = value;
                if (ContainerControl != null)
                {
                    ContainerControl.Refresh();
                }
                if (BackColorChanged != null)
                {
                    BackColorChanged(this, new EventArgs());
                }
            }
        }
        #endregion

        public bool LoadDefaultSkin()
        {
            try
            {
                Bitmap MyForm_Form = Properties.Resources.Form;
                Bitmap MyForm_SystemButtonImage = Properties.Resources.SystemButton;
                Bitmap MyButton_ButtonImage = Properties.Resources.Button;
                Bitmap MyHScrollBar_HScrollbarImage = Properties.Resources.HScollbar;
                Bitmap MyVScrollBar_VScrollbarImage = Properties.Resources.VScollbar;
                Bitmap MyToolStrip_ToolbarImage = Properties.Resources.toolbar;
                Bitmap MyToolStripButton_ButtonImage = Properties.Resources.toolbutton;
                Bitmap MyToolStripSeparator_ToolStripSeparatorImage = Properties.Resources.toolbar_sp;
                Bitmap MyStatusStrip_StatusBarImage = Properties.Resources.StatusBar;

                //基本

                //皮肤引擎背景色
                this.BackColor = Color.FromArgb(96, 147, 193);

                //窗体

                //左边框宽度
                MyForm.LeftBorderWidth = 3;
                //右边框宽度
                MyForm.RightBorderWidth = 3;
                //上边框高度
                MyForm.TopBorderHeight = 3;
                //下边框高度
                MyForm.BottomBorderHeight = 3;
                //标题栏高度
                MyForm.TopicHeight = 27;
                //最大化时系统按钮偏移量
                MyForm.SystemButtonOffsetWhenMaxmize = 3;
                //窗体背景颜色
                MyForm.FormBackColor = Color.FromArgb(248, 249, 253);
                //窗体状态数量
                MyForm.FormStateCount = 1;
                //窗体图标大小
                MyForm.IconSize = new Size(16, 16);
                //标题栏文字颜色
                MyForm.TitleTextColor = Color.White;
                //标题栏文字对齐方式
                MyForm.TitleTextAlign = StringAlignment.Near;
                //标题栏文字字体
                MyForm.TitleTextFont = new Font("微软雅黑",9);

                //按钮
                //左边框宽度
                MyButton.LeftBorderWidth = 3;
                //右边框宽度
                MyButton.RightBorderWidth = 3;
                //上边框高度
                MyButton.TopBorderHeight = 3;
                //下边框高度
                MyButton.BottomBorderHeight = 3;
                //按钮文字颜色
                MyButton.ButtonTextColor = Color.Black;
                //按钮不可用时文字颜色
                MyButton.ButtonDisableTextColor = Color.DarkGray;

                //水平滚动条
                //左边框宽度
                MyHScrollBar.LeftBorderWidth = 2;
                //右边框宽度
                MyHScrollBar.RightBorderWidth = 2;
                //上边框高度
                MyHScrollBar.TopBorderHeight = 2;
                //下边框高度
                MyHScrollBar.BottomBorderHeight = 2;
                //滑块最小宽度
                MyHScrollBar.ScrollButtonMinWidth = 15;

                //垂直滚动条
                MyVScrollBar.LeftBorderWidth = 2;
                //右边框宽度
                MyVScrollBar.RightBorderWidth = 2;
                //上边框高度
                MyVScrollBar.TopBorderHeight = 2;
                //下边框高度
                MyVScrollBar.BottomBorderHeight = 2;
                //滑块最小高度
                MyVScrollBar.ScrollButtonMinHeight = 15;

                //==================
                //　读取图片
                //==================

                //窗体
                MyForm.FormImage = MyForm_Form;
                //窗体-系统按钮
                MyForm.SystemButtonImage = MyForm_SystemButtonImage;
                //按钮
                MyButton.ButtonImage = MyButton_ButtonImage;
                //水平滚动条
                MyHScrollBar.HScrollbarImage = MyHScrollBar_HScrollbarImage;
                //垂直滚动条
                MyVScrollBar.VScrollbarImage = MyVScrollBar_VScrollbarImage;
                //工具条
                MyToolStrip.ToolbarImage = MyToolStrip_ToolbarImage;
                //工具条-按钮
                MyToolStripButton.ButtonImage = MyToolStripButton_ButtonImage;
                //工具条-分隔符
                MyToolStripSeparator.ToolStripSeparatorImage = MyToolStripSeparator_ToolStripSeparatorImage;
                //状态条
                MyStatusStrip.StatusBarImage = MyStatusStrip_StatusBarImage;


                IsUseSkin = false;
                IsUseSkin = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LoadSkin(String skinFilePath)
        {
            //如果皮肤文件不存在
            if (!System.IO.File.Exists(skinFilePath))
            {
                return false;
            }

            var zipDllBytes = CompressHelper.DecompressBytes(Properties.Resources.Ionic_Zip_dll);
            var ass = System.Reflection.Assembly.Load(zipDllBytes);
            var ZipFileType = ass.GetType("Ionic.Zip.ZipFile");
            var constructorInfo = ZipFileType.GetConstructor(new Type[] { typeof(String) });
            var ZipFileObject = constructorInfo.Invoke(new object[] { skinFilePath });

            var ExtractAllMethod = ZipFileType.GetMethod("ExtractAll", new Type[] { typeof(String) });

            //临时皮肤目录
            String skinDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());
            //解压皮肤文件包中的所有文件
            ExtractAllMethod.Invoke(ZipFileObject, new object[] { skinDir });
            //加载皮肤
            bool rtnBool = LoadSkin(new System.IO.DirectoryInfo(skinDir));
            //删除临时目录及其中的文件
            System.IO.Directory.Delete(skinDir, true);

            //释放ZipFile对象
            var DisposeMethod = ZipFileType.GetMethod("Dispose");
            DisposeMethod.Invoke(ZipFileObject, new object[] { });

            return rtnBool;
        }

        #region 从文件得到Bitmap对象并关闭文件
        private Bitmap GetBitmapFormFile(String pictureFilePath)
        {
            var fStream = new System.IO.FileStream(pictureFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Bitmap rtnBmp = new Bitmap(fStream);
            fStream.Close();
            return rtnBmp;
        }
        #endregion

        public bool LoadSkin(System.IO.DirectoryInfo skinDir)
        {
            try
            {
                Bitmap MyForm_Form = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "Form.png"));
                Bitmap MyForm_SystemButtonImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "SystemButton.png"));
                Bitmap MyButton_ButtonImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "Button.png"));
                Bitmap MyHScrollBar_HScrollbarImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "HScrollbar.png"));
                Bitmap MyVScrollBar_VScrollbarImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "VScrollbar.png"));
                Bitmap MyToolStrip_ToolbarImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "Toolbar.png"));
                Bitmap MyToolStripButton_ButtonImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "toolbutton.png"));
                Bitmap MyToolStripSeparator_ToolStripSeparatorImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "toolbar_sp.png"));
                Bitmap MyStatusStrip_StatusBarImage = GetBitmapFormFile(System.IO.Path.Combine(skinDir.FullName, "StatusBar.png"));

                //==================
                //　读取设置
                //==================
                XmlDocument doc = new XmlDocument();
                doc.Load(System.IO.Path.Combine(skinDir.FullName, "Skin.xml"));

                //基本

                //皮肤引擎背景色
                Color tmpColor = this.BackColor;
                XmlHelper.ReadColor(doc, "Setting/Base/BackColor", ref tmpColor);
                if (tmpColor != this.BackColor) this.BackColor = tmpColor;

                //窗体

                //左边框宽度
                XmlHelper.ReadInt(doc, "Setting/Form/LeftBorderWidth", ref MyForm.LeftBorderWidth);
                //右边框宽度
                XmlHelper.ReadInt(doc, "Setting/Form/RightBorderWidth", ref MyForm.RightBorderWidth);
                //上边框高度
                XmlHelper.ReadInt(doc, "Setting/Form/TopBorderHeight", ref MyForm.TopBorderHeight);
                //下边框高度
                XmlHelper.ReadInt(doc, "Setting/Form/BottomBorderHeight", ref MyForm.BottomBorderHeight);
                //标题栏高度
                XmlHelper.ReadInt(doc, "Setting/Form/TopicHeight", ref MyForm.TopicHeight);
                //最大化时系统按钮偏移量
                XmlHelper.ReadInt(doc, "Setting/Form/SystemButtonOffsetWhenMaxmize", ref MyForm.SystemButtonOffsetWhenMaxmize);
                //窗体背景颜色
                XmlHelper.ReadColor(doc, "Setting/Form/FormBackColor", ref MyForm.FormBackColor);
                //窗体状态数量
                XmlHelper.ReadInt(doc, "Setting/Form/FormStateCount", ref MyForm.FormStateCount);
                //窗体图标大小
                XmlHelper.ReadSize(doc, "Setting/Form/IconSize", ref MyForm.IconSize);
                //标题栏文字颜色
                XmlHelper.ReadColor(doc, "Setting/Form/TitleTextColor", ref MyForm.TitleTextColor);
                //标题栏文字对齐方式
                XmlHelper.ReadStringAlignment(doc, "Setting/Form/TitleTextAlign", ref MyForm.TitleTextAlign);
                //标题栏文字字体
                XmlHelper.ReadFont(doc, "Setting/Form/TitleTextFont", ref MyForm.TitleTextFont);

                //按钮
                //左边框宽度
                XmlHelper.ReadInt(doc, "Setting/Button/LeftBorderWidth", ref MyButton.LeftBorderWidth);
                //右边框宽度
                XmlHelper.ReadInt(doc, "Setting/Button/RightBorderWidth", ref MyButton.RightBorderWidth);
                //上边框高度
                XmlHelper.ReadInt(doc, "Setting/Button/TopBorderHeight", ref MyButton.TopBorderHeight);
                //下边框高度
                XmlHelper.ReadInt(doc, "Setting/Button/BottomBorderHeight", ref MyButton.BottomBorderHeight);
                //按钮文字颜色
                XmlHelper.ReadColor(doc, "Setting/Button/ButtonTextColor", ref MyButton.ButtonTextColor);
                //按钮不可用时文字颜色
                XmlHelper.ReadColor(doc, "Setting/Button/ButtonDisableTextColor", ref MyButton.ButtonDisableTextColor);

                //水平滚动条
                //左边框宽度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/LeftBorderWidth", ref MyHScrollBar.LeftBorderWidth);
                //右边框宽度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/RightBorderWidth", ref MyHScrollBar.RightBorderWidth);
                //上边框高度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/TopBorderHeight", ref MyHScrollBar.TopBorderHeight);
                //下边框高度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/BottomBorderHeight", ref MyHScrollBar.BottomBorderHeight);
                //滑块最小宽度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/ScrollButtonMinWidth", ref MyHScrollBar.ScrollButtonMinWidth);

                //垂直滚动条
                XmlHelper.ReadInt(doc, "Setting/VScrollBar/LeftBorderWidth", ref MyVScrollBar.LeftBorderWidth);
                //右边框宽度
                XmlHelper.ReadInt(doc, "Setting/VScrollBar/RightBorderWidth", ref MyVScrollBar.RightBorderWidth);
                //上边框高度
                XmlHelper.ReadInt(doc, "Setting/VScrollBar/TopBorderHeight", ref MyVScrollBar.TopBorderHeight);
                //下边框高度
                XmlHelper.ReadInt(doc, "Setting/VScrollBar/BottomBorderHeight", ref MyVScrollBar.BottomBorderHeight);
                //滑块最小高度
                XmlHelper.ReadInt(doc, "Setting/HScrollBar/ScrollButtonMinHeight", ref MyVScrollBar.ScrollButtonMinHeight);

                //==================
                //　读取图片
                //==================

                //窗体
                MyForm.FormImage = MyForm_Form;
                //窗体-系统按钮
                MyForm.SystemButtonImage = MyForm_SystemButtonImage;
                //按钮
                MyButton.ButtonImage = MyButton_ButtonImage;
                //水平滚动条
                MyHScrollBar.HScrollbarImage = MyHScrollBar_HScrollbarImage;
                //垂直滚动条
                MyVScrollBar.VScrollbarImage = MyVScrollBar_VScrollbarImage;
                //工具条
                MyToolStrip.ToolbarImage = MyToolStrip_ToolbarImage;
                //工具条-按钮
                MyToolStripButton.ButtonImage = MyToolStripButton_ButtonImage;
                //工具条-分隔符
                MyToolStripSeparator.ToolStripSeparatorImage = MyToolStripSeparator_ToolStripSeparatorImage;
                //状态条
                MyStatusStrip.StatusBarImage = MyStatusStrip_StatusBarImage;

                IsUseSkin = false;
                IsUseSkin = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 初始化皮肤引擎
        /// <summary>
        /// 初始化皮肤引擎
        /// </summary>
        private void StartEngine()
        {
            if (SkinEngine.MainSkinEngine != this) return;

            //绑定事件
            List<Form> FormList = new List<Form>();
            foreach (Form form in Application.OpenForms)
            {
                FormList.Add(form);
            }
            foreach (Form form in FormList)
            {
                BindEvent(form);
            }
            //Application.AddMessageFilter(new MyMessageFilter());
            mh.MessageOccurred += new MessageHook.MessageCallback(mh_MessageOccurred);
            mh.Start();
        }
        #endregion

        #region 停止皮肤引擎
        /// <summary>
        /// 停止皮肤引擎
        /// </summary>
        private void StopEngine()
        {
            if (SkinEngine.MainSkinEngine != this) return;

            List<Form> FormList = new List<Form>();
            foreach (Form form in Application.OpenForms)
            {
                FormList.Add(form);
            }
            foreach (Form form in FormList)
            {
                UnbindEvent(form);
            }
            //Application.RemoveMessageFilter(new MyMessageFilter());
            mh.MessageOccurred -= new MessageHook.MessageCallback(mh_MessageOccurred);
            mh.Stop();
            //清空字典
            dictIMyControls.Clear();
        }
        #endregion

        #region 递归绑定事件
        /// <summary>
        /// 递归绑定事件
        /// </summary>
        /// <param name="ctl"></param>
        private void BindEvent(Control ctl)
        {
            ctl.ControlAdded += new ControlEventHandler(Control_ControlAdded);
            ctl.ControlRemoved += new ControlEventHandler(Control_ControlRemoved);

            MyControls.IMyControl ICtl = GetIMyControl(ctl);
            //开始控件皮肤
            if (ICtl != null)
            {
                ICtl.StartControlSkin();
            }

            //递归
            List<Control> ctls = new List<Control>();
            foreach (Control tmpctl in ctl.Controls)
            {
                ctls.Add(tmpctl);
            }
            foreach (Control tmpctl in ctls)
            {
                BindEvent(tmpctl);
            }
        }
        #endregion

        #region 递归取消绑定相关事件
        /// <summary>
        /// 递归取消绑定相关事件
        /// </summary>
        private void UnbindEvent(Control ctl)
        {
            ctl.ControlAdded -= new ControlEventHandler(Control_ControlAdded);
            ctl.ControlRemoved -= new ControlEventHandler(Control_ControlRemoved);

            MyControls.IMyControl ICtl = GetIMyControl(ctl);
            //停止控件皮肤
            if (ICtl != null)
            {
                ICtl.StopControlSkin();
            }

            //递归
            List<Control> ctls = new List<Control>();

            foreach (Control tmpctl in ctl.Controls)
            {
                ctls.Add(tmpctl);
            }

            foreach (Control tmpctl in ctls)
            {
                UnbindEvent(tmpctl);
            }
        }
        #endregion

        #region 根据Control返回IMyControl接口
        /// <summary>
        /// 根据Control返回IMyControl接口
        /// </summary>
        /// <param name="ctl">控件</param>
        /// <returns></returns>
        private MyControls.IMyControl GetIMyControl(Control ctl)
        {
            MyControls.IMyControl ICtl = null;
            if (dictIMyControls.ContainsKey(ctl))
            {
                //如果已经存在Hash表中
                ICtl = dictIMyControls[ctl] as MyControls.IMyControl;
            }
            else
            {
                //如果不存在Hash表中

                if (ctl is Form)
                {
                    //如果是窗体
                    ICtl = new MyControls.MyForm(ctl as Form);
                }
                else if (ctl is Button)
                {
                    //如果是按钮

                    //如果是StartButton
                    //if (ctl.Tag != null && ctl.Tag is string && ctl.Tag.ToString().Equals("StartButton"))
                        //ICtl = new MyControls.MyStartButton(ctl as Button, this);
                    //else
                        ICtl = new MyControls.MyButton(ctl as Button);
                }
                else if (ctl is StatusStrip)
                {
                    //如果是状态条
                    ICtl = new MyStatusStrip(ctl as StatusStrip);
                }
                else if (ctl is ToolStrip)
                {
                    //如果是工具条
                    ICtl = new MyToolStrip(ctl as ToolStrip);
                }
                else if (ctl is DataGridView)
                {
                    //如果是DataGridView
                    ICtl = new MyDataGridView(ctl as DataGridView);
                }
                else if (ctl is HScrollBar)
                {
                    //如果是水平滚动条
                    ICtl = new MyHScrollBar(ctl as HScrollBar);
                }
                else if (ctl is VScrollBar)
                {
                    //如果是垂直滚动条
                    ICtl = new MyVScrollBar(ctl as VScrollBar);
                }
                /*
                else if (ctl is ProgressBar)
                {
                    //如果是进度条
                    ICtl = new MyControls.MyProgressBar(ctl as ProgressBar, this);
                }
                else if (ctl is ListView)
                {
                    //如果是列表
                    ICtl = new MyControls.MyListView(ctl as ListView, this);
                }
                else if (ctl is Label)
                {
                    //如果是Label标签
                    ICtl = new MyControls.MyLabel(ctl as Label, this);
                }
                else if (ctl is GroupBox)
                {
                    //如果是GroupBox
                    ICtl = new MyControls.MyGroupBox(ctl as GroupBox, this);
                }
                else if (ctl is TabControl)
                {
                    //如果是TabControl
                    ICtl = new MyControls.MyTabControl(ctl as TabControl, this);
                }
                else if (ctl is TrackBar)
                {
                    //如果是TrackBar
                    ICtl = new MyControls.MyTrackBar(ctl as TrackBar, this);
                }
                else if (ctl is Panel)
                {
                    //如果是面板
                    ICtl = new MyControls.MyPanel(ctl as Panel, this);
                }
                else if (ctl is CheckBox)
                {
                    //如果是勾选项
                    ICtl = new MyControls.MyCheckBox(ctl as CheckBox, this);
                }
                */

                //添加到Hash表中
                if (ICtl != null)
                {
                    dictIMyControls.Add(ctl, ICtl);
                }
            }
            return ICtl;
        }
        #endregion

        #region 控件添加或移除子控件时
        private void Control_ControlAdded(object sender, ControlEventArgs e)
        {
            BindEvent(e.Control);
        }
        
        private void Control_ControlRemoved(object sender, ControlEventArgs e)
        {
            UnbindEvent(e.Control);
        }
        #endregion

        #region 皮肤引擎事件定义
        /// <summary>
        /// 皮肤引擎背景色改变事件
        /// </summary>
        public event EventHandler BackColorChanged;
        #endregion

        #region 皮肤引擎事件处理
        void SkinEngine_BackColorChanged(object sender, System.EventArgs e)
        {
            MyForm.ChangeControlColor();
            MyForm.ChangeSystemButtonColor();

            MyButton.ChangeControlColor();
            MyHScrollBar.ChangeControlColor();
            MyVScrollBar.ChangeControlColor();
            MyToolStrip.ChangeControlColor();
            MyToolStripButton.ChangeControlColor();
            MyToolStripSeparator.ChangeControlColor();
            MyStatusStrip.ChangeControlColor();

            foreach (Form form in Application.OpenForms)
            {
                form.Refresh();
            }
        }
        #endregion
    }
}
