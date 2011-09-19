using System.ComponentModel.Design;
using System.Windows.Forms;
using System.ComponentModel;
using aaaSoft.SkinEngine.MyControls;
namespace aaaSoft.SkinEngine
{
    partial class SkinEngine
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (ContainerControl != null)
                {
                    StopEngine();
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 以下代码保证能从组件中访问父容器
        /// <summary>
        /// 父容器
        /// </summary>
        public ContainerControl ContainerControl
        {
            get
            {
                return _containerControl;
            }
            set
            {
                _containerControl = value;
                if (IsUseSkin)
                {
                    StartEngine();
                }
            }
        }
        private ContainerControl _containerControl = null;
        public override ISite Site
        {
            set
            {
                base.Site = value;
                if (value != null)
                {
                    IDesignerHost service = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (service != null)
                    {
                        IComponent rootComponent = service.RootComponent;
                        if (rootComponent is ContainerControl)
                        {
                            this.ContainerControl = (ContainerControl)rootComponent;
                        }
                    }
                }
            }
        }
        #endregion

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BackColorChanged += new System.EventHandler(SkinEngine_BackColorChanged);
            components = new System.ComponentModel.Container();
            MyForm.FormImage = Properties.Resources.Form;
            MyForm.SystemButtonImage = Properties.Resources.SystemButton;

            MyButton.ButtonImage = Properties.Resources.Button;
            MyHScrollBar.HScrollbarImage = Properties.Resources.HScollbar;
            MyVScrollBar.VScrollbarImage = Properties.Resources.VScollbar;
            MyToolStrip.ToolbarImage = Properties.Resources.toolbar;
            MyToolStripButton.ButtonImage = Properties.Resources.toolbutton;
            MyToolStripSeparator.ToolStripSeparatorImage = Properties.Resources.toolbar_sp;
            MyStatusStrip.StatusBarImage = Properties.Resources.StatusBar;
        }

        #endregion
    }
}
