using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls
{
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
            
        }

        private void SearchBox_Load(object sender, EventArgs e)
        {
            txtKey.Text = GrayText;
        }

        /// <summary>
        /// 搜索框没有文本时显示的文字
        /// </summary>
        string _GrayText = "搜索";
        public String GrayText
        {
            get
            {
                return _GrayText;
            }
            set
            {
                _GrayText = value;
            }
        }

        //搜索框中的文本
        string _Text = String.Empty;
        public override String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
            }
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            if (txtKey.ForeColor == Color.Black)
            {
                _Text = txtKey.Text;
            }
        }

        private void txtKey_Enter(object sender, EventArgs e)
        {
            txtKey.ForeColor = Color.Black;
            Font newFont = new Font(txtKey.Font, FontStyle.Regular);
            txtKey.Font = newFont;
            if (String.IsNullOrEmpty(Text))
            {
                txtKey.Text = String.Empty;
            }
        }

        private void txtKey_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Text))
            {
                txtKey.ForeColor = Color.DarkGray;
                Font newFont = new Font(txtKey.Font, FontStyle.Italic);
                txtKey.Font = newFont;
                txtKey.Text = GrayText;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.OnClick(e);
            }
        }

        private void SearchBox_FontChanged(object sender, EventArgs e)
        {
            txtKey.Font = this.Font;
        }
    }
}
