using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DatabaseKeeperCore;
using Spring.Context;
using Spring.Context.Support;
using aaaSoft.Helpers;
using System.Threading;
using System.Collections;
using System.IO;

namespace DatabaseKeeper
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Spring加载的应用程序上下文
        /// </summary>
        private static IApplicationContext applicationContext;

        private KeeperProviderController keeperProviderController;
        private IDatabaseKeeperProvider currentProvider;

        //当前备份线程
        private Thread currentBackupThread = null;
        //当前还原线程
        private Thread currentRestoreThread = null;
        //当前定时备份线程
        private Thread currentBackupTimerThread = null;
        //上次备份时间(定时备份时用)
        private DateTime lastBackupTime = DateTime.Now;

        public MainForm()
        {
            InitializeComponent();

            //初始化
            applicationContext = ContextRegistry.GetContext();
            keeperProviderController = (KeeperProviderController)applicationContext.GetObject("keeperProviderController");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var name in keeperProviderController.GetProvidersNameArray())
            {
                cbDatabaseType.Items.Add(name);
            }
            cbDatabaseType.SelectedIndex = 0;
        }

        private DatabaseConnectionInfo GetDatabaseConnectionInfo()
        {
            DatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo();
            databaseConnectionInfo.Host = txtHost.Text.Trim();

            Int32 tmpPort = -1;
            Int32.TryParse(txtPort.Text.Trim(), out tmpPort);
            databaseConnectionInfo.Port = tmpPort;

            databaseConnectionInfo.UserName = txtUserName.Text.Trim();
            databaseConnectionInfo.Password = txtPassword.Text.Trim();
            return databaseConnectionInfo;
        }

        private String GetConnectionString()
        {
            return txtConnectionString.Text.Trim();
        }

        private void cbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentProvider = keeperProviderController.GetProvider(cbDatabaseType.SelectedItem.ToString());
            txtConnectionString.Text = currentProvider.GenerateConnectionString(GetDatabaseConnectionInfo());
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            txtConnectionString.Text = currentProvider.GenerateConnectionString(GetDatabaseConnectionInfo());
        }

        private void btnRefreshDatabaseName_Click(object sender, EventArgs e)
        {
            clbDatabaseName.Items.Clear();
            String[] databaseNameArray = null;
            try
            {
                databaseNameArray = currentProvider.GetDatabaseNameArray(GetConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取数据库名称数组失败，原因：" + ex.Message);
                return;
            }
            if (databaseNameArray == null)
            {
                MessageBox.Show("获取数据库名称数组失败！");
                return;
            }

            foreach (var name in databaseNameArray)
            {
                clbDatabaseName.Items.Add(name);
            }
            clbDatabaseName.Enabled = true;

            btnAutoBackup.Enabled = true;
            btnManualBackup.Enabled = true;
        }

        private void btnManualBackup_Click(object sender, EventArgs e)
        {
            if (clbDatabaseName.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选中任何数据库！");
                return;
            }

            List<String> selectedDatabaseNameList = new List<String>();
            foreach (String databaseName in clbDatabaseName.CheckedItems)
            {
                selectedDatabaseNameList.Add(databaseName);
            }
            startBackupThread(selectedDatabaseNameList, txtRemoteFolder.Text.Trim());
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            String timeString = InputForm.GetInput("添加时间", "请输入时间（格式：15:31或15:31:21）：", null);
            if (String.IsNullOrEmpty(timeString))
                return;

            TimeSpanConverter tsc = new TimeSpanConverter();
            Nullable<TimeSpan> ts;
            try
            {
                ts = tsc.ConvertFromString(timeString) as Nullable<TimeSpan>;
            }
            catch (Exception ex)
            {
                MessageBox.Show("时间格式不正确！" + ex.Message);
                return;
            }

            lbEveryDayBackupTime.Items.Add(ts.Value);
        }

        private void btnRemoveTime_Click(object sender, EventArgs e)
        {
            List<Object> selectedItemList = new List<object>();
            foreach (var item in lbEveryDayBackupTime.SelectedItems)
            {
                selectedItemList.Add(item);
            }
            foreach (var item in selectedItemList)
            {
                lbEveryDayBackupTime.Items.Remove(item);
            }
        }

        private void lbEveryDayBackupTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveTime.Enabled = lbEveryDayBackupTime.SelectedItem != null;
        }

        private void PushLog(String log)
        {
            this.Invoke(new aaaSoft.Helpers.ThreadHelper.UnnamedDelegate(delegate
                {
                    var lvi = lvLog.Items.Add(DateTimeHelper.GetNowDateTimeString());
                    lvi.SubItems.Add(log);
                    lvi.EnsureVisible();
                }));
        }

        private void btnAutoBackup_Click(object sender, EventArgs e)
        {
            if (currentBackupTimerThread == null)
            {
                List<String> selectedDatabaseNameList = new List<String>();
                foreach (String databaseName in clbDatabaseName.CheckedItems)
                {
                    selectedDatabaseNameList.Add(databaseName);
                }

                Hashtable ht = new Hashtable();
                ht.Add("databaseNameList", selectedDatabaseNameList);
                ht.Add("remoteFolderName", txtRemoteFolder.Text.Trim());

                List<TimeSpan> autoBackupTimeList = new List<TimeSpan>();
                foreach (TimeSpan ts in lbEveryDayBackupTime.Items)
                {
                    autoBackupTimeList.Add(ts);
                }
                ht.Add("autoBackupTimeList", autoBackupTimeList);
                currentBackupTimerThread = new Thread(backupTimerThreadFunction);
                currentBackupTimerThread.Start(ht);

                PushLog("已开启定时备份线程。");
                btnAutoBackup.Text = "停止自动备份线程";
                gbAutoBackupSettin.Enabled = false;
                gbBackupFileSetting.Enabled = false;
                gbChooseDatabaseName.Enabled = false;
                gbDatabaseConnection.Enabled = false;
            }
            else
            {
                currentBackupTimerThread = null;

                PushLog("已关闭定时备份线程。");
                btnAutoBackup.Text = "开始自动备份线程";
                gbAutoBackupSettin.Enabled = true;
                gbBackupFileSetting.Enabled = true;
                gbChooseDatabaseName.Enabled = true;
                gbDatabaseConnection.Enabled = true;
            }
        }

        //定时备份线程函数
        private void backupTimerThreadFunction(Object obj)
        {
            try
            {
                Hashtable ht = obj as Hashtable;
                if (ht == null)
                {
                    PushLog("未找到参数！");
                    return;
                }

                List<String> databaseNameList = (List<String>)ht["databaseNameList"];
                String remoteFolderName = ht["remoteFolderName"].ToString();
                List<TimeSpan> autoBackupTimeList = ht["autoBackupTimeList"] as List<TimeSpan>;

                while (true)
                {
                    if (currentBackupTimerThread == null || currentBackupTimerThread != Thread.CurrentThread)
                    {
                        break;
                    }

                    foreach (TimeSpan ts in autoBackupTimeList)
                    {
                        DateTime nowTime = DateTime.Now;
                        DateTime showBackupTime = DateTime.Parse(String.Format("{0} {1}:{2}:{3}", DateTimeHelper.GetNowDateString(), ts.Hours, ts.Minutes, ts.Seconds));
                        if (showBackupTime <= nowTime && showBackupTime > lastBackupTime)
                        {
                            lastBackupTime = nowTime;
                            startBackupThread(databaseNameList, remoteFolderName);
                            break;
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                PushLog("备份数据库失败，原因：" + ex.Message);
            }
            finally
            {
                currentBackupThread = null;
            }
        }

        private void startBackupThread(List<String> databaseNameList, String remoteFolderPath)
        {
            if (currentBackupThread != null)
            {
                PushLog("当前备份正在进行中，不能进行新的备份！");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("databaseNameList", databaseNameList);
            ht.Add("remoteFolderPath", remoteFolderPath);

            currentBackupThread = new Thread(backupThreadFunction);
            currentBackupThread.Start(ht);
        }

        //备份线程函数
        private void backupThreadFunction(Object obj)
        {
            try
            {
                Hashtable ht = obj as Hashtable;
                if (ht == null)
                {
                    PushLog("未找到参数！");
                    return;
                }

                List<String> databaseNameList = (List<String>)ht["databaseNameList"];
                String remoteFolderPath = ht["remoteFolderPath"].ToString();

                foreach (String databaseName in databaseNameList)
                {
                    PushLog(String.Format("开始备份数据库[{0}]。", databaseName));
                    currentProvider.BackupDatabase(GetConnectionString(), databaseName, remoteFolderPath + getAutoGenBackupFileName(databaseName));
                }
                PushLog("备份数据库成功。");
            }
            catch (Exception ex)
            {
                PushLog("备份数据库失败，原因：" + ex.Message);
            }
            finally
            {
                currentBackupThread = null;
            }
        }

        //得到自动生成的备份文件名
        private String getAutoGenBackupFileName(String databaseName)
        {
            return String.Format("{0} - {1}.bak", databaseName, DateTimeHelper.GetNowDateTimeStringNo());
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }



        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            niMain.Visible = false;
        }

        private void niMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            显示ToolStripMenuItem_Click(sender, e);
        }


        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.BringToFront();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            niMain.Visible = false;
            Environment.Exit(0);
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "xml文件(*.xml)|*.xml";
                var dr = ofd.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                    return;

                String xml = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                XmlTreeNode rootNode = XmlTreeNode.FromXml(xml);
                txtHost.Text = rootNode.GetItemValue("host");
                txtPort.Text = rootNode.GetItemValue("port");
                txtUserName.Text = rootNode.GetItemValue("userName");
                txtPassword.Text = rootNode.GetItemValue("password");
                txtConnectionString.Text = rootNode.GetItemValue("connectionString");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载配置时出错，原因：" + ex.Message);
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml文件(*.xml)|*.xml";
            var dr = sfd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return;

            XmlTreeNode rootNode = new XmlTreeNode("DatabaseBackupSetting");
            rootNode.AddItem("host", txtHost.Text.Trim());
            rootNode.AddItem("port", txtPort.Text.Trim());
            rootNode.AddItem("userName", txtUserName.Text.Trim());
            rootNode.AddItem("password", txtPassword.Text.Trim());
            rootNode.AddItem("connectionString", txtConnectionString.Text.Trim());

            File.WriteAllText(sfd.FileName, rootNode.ToXml(), Encoding.UTF8);
            MessageBox.Show("保存成功！");
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
        }

        private void btnSelectBackupFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "数据库备份文件(*.bak;*.dat;*.bin;...)|*.*";
            var dr = ofd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return;
            txtBackupFileName.Text = ofd.FileName;
        }

        private void btnManualRestore_Click(object sender, EventArgs e)
        {
            if (clbDatabaseName.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选中任何数据库！");
                return;
            }
            else if (clbDatabaseName.CheckedItems.Count > 1)
            {
                MessageBox.Show("只能选择一个数据库进行还原！");
                return;
            }
            startRestoreThread(clbDatabaseName.SelectedItem.ToString(), txtBackupFileName.Text.Trim());
        }

        private void startRestoreThread(String databaseName, String backupFileName)
        {
            if (currentRestoreThread != null)
            {
                PushLog("当前还原正在进行中，不能进行新的还原！");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("databaseName", databaseName);
            ht.Add("backupFileName", backupFileName);

            currentRestoreThread = new Thread(restoreThreadFunction);
            currentRestoreThread.Start(ht);
        }

        //还原线程函数
        private void restoreThreadFunction(Object obj)
        {
            try
            {
                Hashtable ht = obj as Hashtable;
                if (ht == null)
                {
                    PushLog("未找到参数！");
                    return;
                }

                String databaseName = (String)ht["databaseName"];
                String backupFileName = ht["backupFileName"].ToString();

                PushLog(String.Format("开始还原数据库[{0}]。", databaseName));
                currentProvider.RestoreDatabase(GetConnectionString(), databaseName, backupFileName);

                PushLog("还原数据库成功。");
            }
            catch (Exception ex)
            {
                PushLog("还原数据库失败，原因：" + ex.Message);
            }
            finally
            {
                currentRestoreThread = null;
            }
        }
    }
}
