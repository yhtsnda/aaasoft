using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace aaaSoft.SkinEngine.SkinControls
{
    public partial class ThemeForm : Form
    {
        public ThemeForm()
        {
            InitializeComponent();
        }

        private void ThemeForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThemeForm_Load(object sender, EventArgs e)
        {
            InitSkinList();
        }

        private void InitSkinList()
        {
            lbSkin.Items.Add("无");
            lbSkin.Items.Add("默认");

            var di = new DirectoryInfo("Skin");
            if (di.Exists)
            {
                var files = di.GetFiles("*.zip");
                foreach (var file in files)
                {
                    lbSkin.Items.Add(Path.GetFileNameWithoutExtension(file.FullName));
                }
            }
            lbSkin.SelectedItem = SkinEngine.MainSkinEngine.CurrentTheme;
        }

        private void lbSkin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSkin.SelectedItem == null) return;
            SkinEngine.MainSkinEngine.CurrentTheme = lbSkin.SelectedItem.ToString();
        }
    }
}
