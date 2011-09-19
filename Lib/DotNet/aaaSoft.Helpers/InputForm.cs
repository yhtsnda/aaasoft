using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Helpers
{
    public partial class InputForm : Form
    {
        public InputForm(String Title, String Description, String DefaultValue)
        {
            InitializeComponent();
            this.Text = Title;
            lblDescription.Text = Description;
            txtInput.Text = DefaultValue;
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public String Value
        {
            get
            {
                return txtInput.Text;
            }
        }

        public static String GetInput(String Title, String Description, String DefaultValue)
        {
            var form = new InputForm(Title, Description, DefaultValue);
            var dr = form.ShowDialog();
            if (dr == DialogResult.Cancel) return null;
            return form.Value;
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                btnCancel_Click(sender, e);
        }
    }
}
