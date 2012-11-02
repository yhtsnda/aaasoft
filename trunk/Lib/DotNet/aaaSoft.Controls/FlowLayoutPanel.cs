using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace aaaSoft.Controls
{
    public class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel
    {
        public FlowLayoutPanel()
        {
            AutoScroll = true;
            HScroll = false;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ResizeAllControl();
            this.AutoScroll = !this.AutoScroll;
            this.AutoScroll = !this.AutoScroll;
            base.OnSizeChanged(e);
        }

        private void ResizeAllControl()
        {
            foreach (Control control in this.Controls)
                ResizeControl(control);
        }
        private void ResizeControl(Control control)
        {
            control.Width = this.ClientSize.Width - control.Margin.Size.Width;
        }

        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            ResizeControl(e.Control);
        }
    }
}
