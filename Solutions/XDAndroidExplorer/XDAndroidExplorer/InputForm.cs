using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XDAndroidExplorer
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
            InputForm form = new InputForm(Title, Description, DefaultValue);
            DialogResult dr = form.ShowDialog();
            if (dr == DialogResult.Cancel) return null;
            return form.Value;
        }
    }
}
