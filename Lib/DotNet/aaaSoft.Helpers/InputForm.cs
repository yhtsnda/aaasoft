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
        public Boolean IsPassword
        {
            get { return txtInput.PasswordChar != Char.MinValue; }
            set 
            {
                if (value)
                    txtInput.PasswordChar = '●';
                else
                    txtInput.PasswordChar = Char.MinValue;
            }
        }

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
            return GetInput(Title, Description, DefaultValue, false);
        }

        public static String GetInput(String Title, String Description, String DefaultValue, Boolean isPassword)
        {
            var form = new InputForm(Title, Description, DefaultValue);
            form.IsPassword = isPassword;
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
