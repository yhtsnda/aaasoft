using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using aaaSoft.Ftp;
using aaaSoft.Helpers;

namespace MultiserverFtpClient.Forms
{
    public partial class SiteManageForm : Form
    {
        //父窗体
        public MainForm faForm;
        //当前选中的TreeNode对象
        private TreeNode CurrentSelectedNode = null;

        public SiteManageForm()
        {
            InitializeComponent();
        }

        #region 窗体加载时
        private void SiteManageForm_Load(object sender, EventArgs e)
        {
            BindData();
            tvSites.Nodes[0].Expand();
            btnApply.Enabled = false;

            foreach (TabPage page in tcSiteEdit.TabPages)
                page.Enabled = false;
        }
        #endregion

        #region 绑定站点数据
        private void BindData()
        {
            tvSites.Nodes.Clear();
            _BindData(MainForm.RootSiteGroup, tvSites.Nodes.Add(""));
        }

        private void _BindData(FtpSiteDataGroup group, TreeNode node)
        {
            node.Name = group.Name;
            node.Text = group.Name;
            node.ImageKey = "Group";
            node.Tag = group;

            foreach (var subGroup in group.Groups)
            {
                _BindData(subGroup, node.Nodes.Add(""));
            }

            foreach (var site in group.Sites)
            {
                var tmpNode = node.Nodes.Add(site.Name, site.Name, "Site", "Site");
                tmpNode.Tag = site;
            }
        }
        #endregion

        private void GetCurrentGroupNode(out FtpSiteDataGroup group, out TreeNode groupNode)
        {
            group = null;
            groupNode = null;

            if (tvSites.SelectedNode != null)
            {
                var obj = tvSites.SelectedNode.Tag;
                if (obj is FtpSiteDataGroup)
                {
                    group = obj as FtpSiteDataGroup;
                    groupNode = tvSites.SelectedNode;
                }
                else if (obj is FtpSiteData)
                {
                    group = (obj as FtpSiteData).Group;
                    groupNode = tvSites.SelectedNode.Parent;
                }
            }
            if (group == null)
                group = MainForm.RootSiteGroup;
            if (groupNode == null)
                groupNode = tvSites.Nodes[0];
        }

        #region 点击"新建站点"按钮
        private void btnAddSite_Click(object sender, EventArgs e)
        {
            if (CheckSiteSettingChanged(sender) == System.Windows.Forms.DialogResult.Cancel)
                return;

            FtpSiteDataGroup group = null;
            TreeNode groupNode = null;
            GetCurrentGroupNode(out group, out groupNode);

            //用户输入新建站点名称
            var tmpSiteName = InputForm.GetInput("新建站点", "站点名称", "");
            if (String.IsNullOrEmpty(tmpSiteName))
                return;
            //创建FtpSiteData对象
            var tmpSiteData = new FtpSiteData()
            {
                GUID = Guid.NewGuid().ToString(),
                Name = tmpSiteName,
                Port = 21,
                Group = group
            };
            //在树形控件中创建节点
            var tmpNode = groupNode.Nodes.Add(tmpSiteName, tmpSiteName, "Site", "Site");
            tmpNode.Tag = tmpSiteData;
            //添加到组的站点列表中
            group.Sites.Add(tmpSiteData);
            //设置选中的节点为新建节点
            tvSites.SelectedNode = tmpNode;
        }
        #endregion

        #region 点击"新建组"按钮
        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (CheckSiteSettingChanged(sender) == System.Windows.Forms.DialogResult.Cancel)
                return;

            FtpSiteDataGroup group = MainForm.RootSiteGroup;
            TreeNode groupNode = tvSites.Nodes[0];
            //GetCurrentGroupNode(out group, out groupNode);

            //用户输入新建站点名称
            var tmpGroupName = InputForm.GetInput("新建组", "组名称", "");
            if (String.IsNullOrEmpty(tmpGroupName))
                return;
            //创建FtpSiteDataGroup对象
            var tmpGroup = new FtpSiteDataGroup()
            {
                GUID = Guid.NewGuid().ToString(),
                Name = tmpGroupName,
                Group = group
            };
            //在树形控件中创建节点
            var tmpNode = groupNode.Nodes.Add(tmpGroupName, tmpGroupName, "Group", "Group");
            tmpNode.Tag = tmpGroup;
            //添加到组的组列表中
            group.Groups.Add(tmpGroup);
            //设置选中的节点为新建节点
            tvSites.SelectedNode = tmpNode;
        }
        #endregion

        #region 点击"删除"按钮
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckSiteSettingChanged(sender) == System.Windows.Forms.DialogResult.Cancel)
                return;

            var currentNode = tvSites.SelectedNode;
            if (currentNode == null) return;
            var obj = currentNode.Tag;
            if (obj == null) return;

            if (obj is FtpSiteDataGroup)
            {
                var group = obj as FtpSiteDataGroup;
                if (group.Group == null) return;
                var dr = MessageBox.Show(String.Format("您确定要删除 \"{0}\" 组?\n所有嵌套站点都将被删除。\n您不能撤销此操作!", group.Name), "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.No) return;
                group.Group.Groups.Remove(group);
                currentNode.Parent.Nodes.Remove(currentNode);
            }
            else if (obj is FtpSiteData)
            {
                var site = obj as FtpSiteData;
                var dr = MessageBox.Show(String.Format("您确定要删除 \"{0}\" 站点?\n您不能撤销此操作!", site.Name), "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.No) return;
                site.Group.Sites.Remove(site);
                currentNode.Parent.Nodes.Remove(currentNode);
            }
        }
        #endregion

        #region 点击"应用"按钮
        private void btnApply_Click(object sender, EventArgs e)
        {
            var site = CurrentSelectedNode.Tag as FtpSiteData;
            if (site == null) return;

            if (!String.IsNullOrEmpty(txtSiteName.Text.Trim()))
                site.Name = txtSiteName.Text.Trim();
            site.HostName = txtHostName.Text.Trim();

            Int32 tmpPort = 21;
            Int32.TryParse(txtPort.Text.Trim(), out tmpPort);
            site.Port = tmpPort;

            site.UserName = txtUserName.Text.Trim();
            site.IsAnonymous = cbIsAnonymous.Checked;
            site.Password = txtPassword.Text.Trim();
            site.RemotePath = txtRemotePath.Text.Trim();
            site.LocalPath = txtLocalPath.Text.Trim();
            site.Tip = txtTip.Text.Trim();

            site.IsShowHidenFile = cbIsShowHidenFile.Checked;
            site.IsUseMlsdToListFolder = cbIsUseMlsdToListFolder.Checked;
            site.IsNotSupportFEAT = cbIsNotSupportFEAT.Checked;
            site.StringEncoding = cbStringEncoding.Text;
            site.BufferSize = Convert.ToInt32(nudBufferSize.Value);

            CurrentSelectedNode.Text = site.Name;
            btnApply.Enabled = false;
        }
        #endregion

        #region 点击"连接"按钮
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (CheckSiteSettingChanged(sender) == System.Windows.Forms.DialogResult.Cancel)
                return;
            if (!(CurrentSelectedNode.Tag is FtpSiteData))
                return;

            faForm.ConnectToSite(CurrentSelectedNode.Tag as FtpSiteData);
            this.Close();
        }
        #endregion

        #region 点击"关闭"按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (CheckSiteSettingChanged(sender) == System.Windows.Forms.DialogResult.Cancel)
                return;

            this.Close();
        }
        #endregion

        #region 检查站点配置是否更改
        private System.Windows.Forms.DialogResult CheckSiteSettingChanged(Object sender)
        {
            EventArgs e = new EventArgs();
            var dr = System.Windows.Forms.DialogResult.Yes;

            if (btnApply.Enabled)
            {
                dr = MessageBox.Show(String.Format("保存改变到 \"{0}\"?", CurrentSelectedNode.Text), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                    btnApply_Click(sender, e);
                else if (dr == System.Windows.Forms.DialogResult.No)
                    btnApply.Enabled = false;
                else if (dr == System.Windows.Forms.DialogResult.Cancel) { }
            }
            return dr;
        }
        #endregion

        #region tvSites鼠标双击
        private void tvSites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnConnect_Click(sender, e);
        }
        #endregion

        #region tvSites选中对象前
        private void tvSites_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (btnApply.Enabled)
            {
                var dr = CheckSiteSettingChanged(sender);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
        #endregion

        #region tvSites选中对象过后
        private void tvSites_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var obj = e.Node.Tag;
            if (obj == null) return;

            var tcd = new ControlHelper.TraversalControlDelegate(UnbindControlValueChangedEvent);
            ControlHelper.TraversalControl(tcSiteEdit, tcd);

            if (obj is FtpSiteDataGroup)
            {
                foreach (TabPage page in tcSiteEdit.TabPages)
                    page.Enabled = false;

                tcd = new ControlHelper.TraversalControlDelegate(SetControlDefaultValue);
                ControlHelper.TraversalControl(tcSiteEdit, tcd);


            }
            else if (obj is FtpSiteData)
            {
                foreach (TabPage page in tcSiteEdit.TabPages)
                    page.Enabled = true;

                var siteData = obj as FtpSiteData;
                //常规选项卡
                txtSiteName.Text = siteData.Name;
                txtHostName.Text = siteData.HostName;
                txtPort.Text = siteData.Port.ToString();
                txtUserName.Text = siteData.UserName;
                cbIsAnonymous.Checked = siteData.IsAnonymous;
                txtPassword.Text = siteData.Password;
                txtRemotePath.Text = siteData.RemotePath;
                txtLocalPath.Text = siteData.LocalPath;
                txtTip.Text = siteData.Tip;
                //选项选项卡
                cbIsShowHidenFile.Checked = siteData.IsShowHidenFile;
                cbIsUseMlsdToListFolder.Checked = siteData.IsUseMlsdToListFolder;
                cbIsNotSupportFEAT.Checked = siteData.IsNotSupportFEAT;
                //如果没有下面两行代码，则nudBufferSize控件会显示为空白
                nudBufferSize.Value = nudBufferSize.Maximum;
                nudBufferSize.Value = nudBufferSize.Minimum;
                nudBufferSize.Value = siteData.BufferSize;
                foreach (var encoding in Encoding.GetEncodings())
                    cbStringEncoding.Items.Add(encoding.Name);
                cbStringEncoding.Text = siteData.StringEncoding;

                tcd = new ControlHelper.TraversalControlDelegate(BindControlValueChangedEvent);
                ControlHelper.TraversalControl(tcSiteEdit, tcd);
            }
            CurrentSelectedNode = e.Node;
            btnConnect.Enabled = CurrentSelectedNode.Tag is FtpSiteData;
        }

        #region 设置控件的默认值
        private void SetControlDefaultValue(Control ctl)
        {
            if (ctl is TextBox)
                ctl.Text = "";
            else if (ctl is CheckBox)
                (ctl as CheckBox).Checked = false;
            else if (ctl is ComboBox)
            {
                (ctl as ComboBox).Items.Clear();
                (ctl as ComboBox).Text = "";
            }
            else if (ctl is NumericUpDown)
            {
                (ctl as NumericUpDown).Value = (ctl as NumericUpDown).Minimum;
            }
        }
        #endregion

        #region 绑定控件值改变事件
        private void BindControlValueChangedEvent(Control ctl)
        {
            if (ctl is TextBox)
                ctl.TextChanged += new EventHandler(SiteSettingChanged);
            else if (ctl is CheckBox)
                (ctl as CheckBox).CheckedChanged += new EventHandler(SiteSettingChanged);
            else if (ctl is ComboBox)
                (ctl as ComboBox).TextChanged += new EventHandler(SiteSettingChanged);
            else if (ctl is NumericUpDown)
                (ctl as NumericUpDown).ValueChanged += new EventHandler(SiteSettingChanged);
        }
        #endregion

        #region 取消绑定控件值改变事件
        private void UnbindControlValueChangedEvent(Control ctl)
        {
            if (ctl is TextBox)
                ctl.TextChanged -= new EventHandler(SiteSettingChanged);
            else if (ctl is CheckBox)
                (ctl as CheckBox).CheckedChanged -= new EventHandler(SiteSettingChanged);
            else if (ctl is ComboBox)
                (ctl as ComboBox).TextChanged -= new EventHandler(SiteSettingChanged);
            else if (ctl is NumericUpDown)
                (ctl as NumericUpDown).ValueChanged -= new EventHandler(SiteSettingChanged);
        }
        #endregion

        #region 站点设置改变事件
        private void SiteSettingChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = true;
        }
        #endregion

        #region 选择本地路径
        private void btnSelectLocalPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtLocalPath.Text;
            var dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return;
            txtLocalPath.Text = fbd.SelectedPath;
        }
        #endregion

        #endregion
    }
}
