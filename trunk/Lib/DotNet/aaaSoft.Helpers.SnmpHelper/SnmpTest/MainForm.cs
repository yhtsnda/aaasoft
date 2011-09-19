using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using aaaSoft.Helpers;

namespace SnmpTest
{
    public partial class MainForm : Form
    {
        SnmpHelper sh;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            sh = new SnmpHelper(txtHostName.Text, Int32.Parse(txtPort.Text), txtCommunity.Text, 2);

            txtHostName.Enabled = false;
            txtPort.Enabled = false;
            txtCommunity.Enabled = false;

            foreach (Control ctl in this.Controls)
            {
                if (ctl is Button)
                    ctl.Enabled = true;
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            txtResult.Text = sh.GetString(txtOid.Text);
        }

        private void btnWalk_Click(object sender, EventArgs e)
        {
            dgvResult.DataSource = sh.WalkDataTable(txtOid.Text);
        }

        private void btnWalkStoargeInfo_Click(object sender, EventArgs e)
        {
            dgvResult.DataSource = sh.GetStorageTable();
        }

        private void btnGetInterfaceInfo_Click(object sender, EventArgs e)
        {
            dgvResult.DataSource = sh.GetInterfaceTable();
        }
    }
}
