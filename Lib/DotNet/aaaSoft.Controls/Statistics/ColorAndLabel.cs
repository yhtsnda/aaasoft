using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls.Statistics
{
    public partial class ColorAndLabel : UserControl
    {
        public Color Color
        {
            get { return lblColor.BackColor; }
            set { lblColor.BackColor = value; }
        }

        public String ShowText
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        public ColorAndLabel()
        {
            InitializeComponent();

            lblColor.MouseEnter += new EventHandler(lblText_MouseEnter);
            lblColor.MouseMove += new MouseEventHandler(lblText_MouseMove);
            lblText.MouseEnter += new EventHandler(lblText_MouseEnter);
            lblText.MouseMove += new MouseEventHandler(lblText_MouseMove);
        }

        void lblText_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        void lblText_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }
    }
}
