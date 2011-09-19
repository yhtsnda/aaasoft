using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace aaaSoft.SkinEngine.SkinControls
{
    public class ThemeButton : Button
    {
        public new string Text
        {
            get
            {
                return "";
            }
        }

        public ThemeButton()
        {
            this.Size = new Size(16, 16);
            this.Image = Properties.Resources.Theme;

            this.Click += new EventHandler(ThemeButton_Click);
        }

        void ThemeButton_Click(object sender, EventArgs e)
        {
            var frmTheme = new ThemeForm();
            Point FormLocation = new Point();

            FormLocation = this.Parent.PointToScreen(this.Location);
            FormLocation.X = FormLocation.X - frmTheme.Width + this.Width;
            FormLocation.Y = FormLocation.Y + this.Height;

            frmTheme.Location = FormLocation;
            frmTheme.Show();
        }
    }
}
